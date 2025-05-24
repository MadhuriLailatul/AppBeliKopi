Imports Microsoft.Reporting.WinForms

Public Class StrukCetak
    Private Sub StrukCetak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ReportViewer1.RefreshReport()

        Me.ReportViewer1.LocalReport.ReportPath = Application.StartupPath & "\ReportStruk.rdlc"
    End Sub

    Private Sub ReportViewer1_Load(sender As Object, e As EventArgs) Handles ReportViewer1.Load
        Me.ReportViewer1.RefreshReport()

    End Sub
End Class