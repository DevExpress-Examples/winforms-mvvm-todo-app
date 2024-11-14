Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.ViewInfo
Imports TodoApp.Model
Imports TodoApp.ViewModels

Namespace TodoApp.Views

    Public Partial Class ItemsView
        Inherits XtraUserControl

        Public Sub New()
            InitializeComponent()
            ' Initializing bindings only at runtime    
            If Not mvvmContext.IsDesignMode Then InitializeBindings()
        End Sub

        Private Sub InitializeBindings()
            ' Initialize the Fluent API
            Dim fluent = mvvmContext.OfType(Of ItemsViewModel)()
            ' Bind the Title property to the label Text
            fluent.SetBinding(titleLabel, Function(lbl) lbl.Text, Function(x) x.Title)
            ' Bind the ShowAllItems option to the toggle switch
            fluent.SetBinding(toggleShowAll, Function(tgl) tgl.EditValue, Function(x) x.ShowAllItems)
            ' Bind data-items to listbox
            fluent.SetBinding(todoItemBindingSource, Function(bs) bs.DataSource, Function(x) x.Items)
            fluent.SetBinding(listBoxControl, Function(lb) lb.SelectedValue, Function(x) x.SelectedItem)
            ' Bind the Complete command to the context button click (with args propagation)
            fluent.WithEvent(Of ContextItemClickEventArgs)(listBoxControl, "ContextButtonClick").EventToCommand(Function(x) AddressOf x.Complete, Function(args) TryCast(args.DataItem, TodoItem))
            ' Bind the Complete command to the item double click
            fluent.WithEvent(CObj(listBoxControl), CStr("DoubleClick")).EventToCommand(Function(x) AddressOf x.Edit)
            ' Bind the New command
            fluent.BindCommand(btnNew, Function(x) AddressOf x.[New])
        End Sub

        ' Hide check-mark image from completed tasks
        Private Sub listBoxControl1_CustomizeItem(ByVal sender As Object, ByVal e As CustomizeTemplatedItemEventArgs)
            Dim item = TryCast(e.DataItem, TodoItem)
            If Not item.IsCompleted Then e.TemplatedItem.Elements(CStr(NameOf(CBool(TodoItem.IsCompleted)))).ImageOptions.SvgImage = Nothing
        End Sub

        ' Check-mark button visibility customization
        Private Sub ListBoxControl1_CustomizeContextItem(ByVal sender As Object, ByVal e As ListBoxControlContextButtonCustomizeEventArgs)
            Dim item = TryCast(e.Item, TodoItem)
            e.ContextItem.Visibility = If(item.IsCompleted, ContextItemVisibility.Hidden, ContextItemVisibility.Auto)
        End Sub
    End Class
End Namespace
