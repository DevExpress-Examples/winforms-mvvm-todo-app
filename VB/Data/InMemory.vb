Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports TodoApp.Model

Namespace TodoApp.Data

    Friend NotInheritable Class InMemoryRepository
        Implements TodoApp.Data.IRepository

        Private NotInheritable Class InMemoryTodoItem
            Inherits TodoApp.Model.TodoItem

            Public Sub New(ByVal id As Integer)
                MyBase.New(id)
            End Sub
        End Class

        '
        Private ReadOnly dataStorage As System.Collections.Generic.Dictionary(Of Integer, TodoApp.Model.TodoItem) = New System.Collections.Generic.Dictionary(Of Integer, TodoApp.Model.TodoItem)() From {{TodoApp.Model.AppModel.NewItemID, New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(TodoApp.Model.AppModel.NewItemID)}, {1, New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(1) With {.Title = "Create the MainView", .Description = "Create a Form with application icon and NavigationFrame control.", .IsCompleted = True}}, {2, New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(2) With {.Title = "Create the AppViewModel", .Description = "Create a class with the Title property.", .IsCompleted = True}}, {3, New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(3) With {.Title = "Bind the AppViewModel to the MainView.", .Description = "Bind the Title property to the Text. Register the NavigationFrame as a service.", .IsCompleted = True}}, {4, New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(4) With {.Title = "Create the ItemsView", .Description = "Create an UserControl with ListBoxControl and SearchBox."}}, {5, New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(5) With {.Title = "Create the ItemsViewModel", .Description = "Create a class with the Items property."}}, {6, New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(6) With {.Title = "Bind the ItemsViewModel to the ItemsView.", .Description = "Bind the Title property to the label. Bind the Items property to the bindingSource."}}}

        Private IdGeneratorSeed As Integer

        Public Sub New()
            Me.IdGeneratorSeed = Me.dataStorage.Count
        End Sub

        Private Function GenerateNewId() As Integer
            Return System.Math.Min(System.Threading.Interlocked.Increment(Me.IdGeneratorSeed), Me.IdGeneratorSeed - 1)
        End Function

        Private Function Count(Optional filter As System.Func(Of TodoApp.Model.TodoItem, Boolean) = Nothing) As Integer Implements Global.TodoApp.Data.IRepository.Count
            Dim result As Integer = 0
            For Each item In Me.dataStorage
                If item.Key <> TodoApp.Model.AppModel.NewItemID AndAlso (filter Is Nothing OrElse filter(item.Value)) Then result += 1
            Next

            Return result
        End Function

        Private Function LoadItems(Optional filter As System.Func(Of TodoApp.Model.TodoItem, Boolean) = Nothing) As IList(Of TodoApp.Model.TodoItem) Implements Global.TodoApp.Data.IRepository.LoadItems
            Dim items As System.ComponentModel.BindingList(Of TodoApp.Model.TodoItem) = New System.ComponentModel.BindingList(Of TodoApp.Model.TodoItem)()
            For Each item In Me.dataStorage
                If item.Key <> TodoApp.Model.AppModel.NewItemID AndAlso (filter Is Nothing OrElse filter(item.Value)) Then items.Add(TodoApp.Data.InMemoryRepository.MakeCopy(item.Value))
            Next

            Return items
        End Function

        Private Function LoadItem(ByVal itemId As Integer) As TodoItem Implements Global.TodoApp.Data.IRepository.LoadItem
            Dim item As TodoApp.Model.TodoItem
            If Me.dataStorage.TryGetValue(itemId, item) Then Return TodoApp.Data.InMemoryRepository.MakeCopy(item)
            Return Nothing
        End Function

        Private Function ReloadItem(ByVal items As System.Collections.Generic.IList(Of TodoApp.Model.TodoItem), ByVal id As Integer) As TodoItem Implements Global.TodoApp.Data.IRepository.ReloadItem
            Dim bindingList = CType(items, System.ComponentModel.BindingList(Of TodoApp.Model.TodoItem))
            bindingList.RaiseListChangedEvents = False
            Dim index As Integer = 0
            Dim item As TodoApp.Model.TodoItem = Nothing
            For i As Integer = 0 To items.Count - 1
                If items(CInt((i))).Id = id Then
                    item = CType(Me, TodoApp.Data.IRepository).LoadItem(id)
                    items(CSharpImpl.__Assign(index, i)) = item
                End If
            Next

            bindingList.RaiseListChangedEvents = True
            bindingList.ResetItem(index)
            Return item
        End Function

        Private Function Delete(ByVal id As Integer) As Boolean Implements Global.TodoApp.Data.IRepository.Delete
            Return(id <> TodoApp.Model.AppModel.NewItemID) AndAlso Me.dataStorage.Remove(id)
        End Function

        Private Function Save(ByVal item As TodoApp.Model.TodoItem) As Integer Implements Global.TodoApp.Data.IRepository.Save
            If item.Id = TodoApp.Model.AppModel.NewItemID Then
                Dim newId As Integer = Me.GenerateNewId()
                Me.dataStorage.Add(newId, TodoApp.Data.InMemoryRepository.MakeCopy(item, newId))
                Return newId
            Else
                Dim itemToUpdate = Me.dataStorage(item.Id)
                itemToUpdate.Title = item.Title
                itemToUpdate.Description = item.Description
                itemToUpdate.IsCompleted = item.IsCompleted
                Return item.Id
            End If
        End Function

        Private Function HasChanges(ByVal id As Integer, ByVal item As TodoApp.Model.TodoItem) As Boolean Implements Global.TodoApp.Data.IRepository.HasChanges
            If id = TodoApp.Model.AppModel.NewItemID Then
                Return Not String.IsNullOrEmpty(item.Title) AndAlso Not String.IsNullOrEmpty(item.Description)
            Else
                Dim itemToCheck = Me.dataStorage(id)
                Return(Not Equals(itemToCheck.Title, item.Title)) OrElse (Not Equals(itemToCheck.Description, item.Description)) OrElse (itemToCheck.IsCompleted <> item.IsCompleted)
            End If
        End Function

        Private Shared Function MakeCopy(ByVal item As TodoApp.Model.TodoItem, ByVal Optional id As Integer? = Nothing) As TodoItem
            Dim actualId As Integer = id.GetValueOrDefault(item.Id)
            Return New TodoApp.Data.InMemoryRepository.InMemoryTodoItem(actualId) With {.Title = item.Title, .Description = item.Description, .IsCompleted = item.IsCompleted}
        End Function

        Private Class CSharpImpl

            <System.Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
