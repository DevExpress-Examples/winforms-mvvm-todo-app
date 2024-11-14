Imports System
Imports System.Windows.Forms

Namespace TodoApp

    Friend Module Program

        <STAThread>
        Sub Main()
            Call Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Call Application.Run(New MainView())
        End Sub
    End Module
End Namespace
