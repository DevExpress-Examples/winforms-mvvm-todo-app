Imports DevExpress.XtraEditors
Imports TodoApp.ViewModels

Namespace TodoApp

    Public Partial Class MainView
        Inherits XtraForm

        Public Sub New()
            InitializeComponent()
            ' Initializing bindings only at runtime  
            If Not mvvmContext.IsDesignMode Then
                InitializeNavigation()
                InitializeBindings()
            End If
        End Sub

        Private Sub InitializeNavigation()
            ' creating the NavigationFrame as INavigationService
            Dim navigationService = DevExpress.Utils.MVVM.Services.NavigationService.Create(navigationFrame)
            ' registering the service instance
            mvvmContext.RegisterService(navigationService)
            ' Initialize the Fluent API
            Dim fluent = mvvmContext.OfType(Of AppViewModel)()
            ' Bind the OnShown command to the Shown event
            fluent.WithEvent(CObj(Me), CStr("Shown")).EventToCommand(Function(x) AddressOf x.OnShown)
        End Sub

        Private Sub InitializeBindings()
            ' Initialize the Fluent API
            Dim fluent = mvvmContext.OfType(Of AppViewModel)()
            ' Bind the Title property to the Text
            fluent.SetBinding(Me, Function(view) view.Text, Function(x) x.Title)
        End Sub
    End Class
End Namespace
