Partial Class DataSet1
    Partial Public Class tbl_detailjualDataTable
        Private Sub tbl_detailjualDataTable_tbl_detailjualRowChanging(sender As Object, e As tbl_detailjualRowChangeEvent) Handles Me.tbl_detailjualRowChanging

        End Sub

    End Class

    Partial Public Class tbl_cetakDataTable
        Private Sub tbl_cetakDataTable_tbl_cetakRowChanging(sender As Object, e As tbl_cetakRowChangeEvent) Handles Me.tbl_cetakRowChanging

        End Sub

    End Class

    Partial Public Class tbl_pelangganDataTable
        Private Sub tbl_pelangganDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.namapelangganColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class
End Class
