Partial Class DataSet5
    Partial Public Class kopiDataTable
        Private Sub kopiDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            ' Ganti "NamaKolom" dengan nama kolom yang benar dari DataSet Anda
            If (e.Column.ColumnName = "NamaKolom") Then
                ' Tambahkan logika atau kode yang ingin dijalankan saat kolom berubah
                ' Contoh: Validasi data
                If e.ProposedValue Is Nothing OrElse e.ProposedValue.ToString() = "" Then
                    Throw New ArgumentException("Nilai kolom tidak boleh kosong.")
                End If
            End If
        End Sub
    End Class
End Class
