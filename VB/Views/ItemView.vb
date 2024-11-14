Imports DevExpress.XtraEditors
Imports TodoApp.ViewModels

Namespace TodoApp.Views

    Public Partial Class ItemView
        Inherits XtraUserControl

        Public Sub New()
            InitializeComponent()
            ' Initializing bindings only at runtime    
            If Not mvvmContext.IsDesignMode Then InitializeBindings()
        End Sub

        Private Sub InitializeBindings()
            ' Initialize the Fluent API
            Dim fluent = mvvmContext.OfType(Of ItemViewModel)()
            ' Bind the Title property to the label Text
            fluent.SetBinding(titleLabel, Function(lbl) lbl.Text, Function(x) x.Title)
            ' Bind commands to buttons
            fluent.BindCommand(btnBack, Function(x) AddressOf x.Close)
            fluent.BindCommand(btnSave, Function(x) AddressOf x.Save)
            fluent.BindCommand(btnDelete, Function(x) AddressOf x.Delete)
            ' Bind datasource to editors
            fluent.SetObjectDataSourceBinding(todoItemBindingSource, Function(x) x.Item, Function(x) AddressOf x.Update)
        End Sub
    End Class
End Namespace
