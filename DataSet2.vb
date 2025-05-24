Partial Class DataSet2
    Partial Public Class tbl_pelangganDataTable
        Private Sub tbl_pelangganDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.hargaColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

    Partial Public Class DataTable1DataTable
        Private Sub DataTable1DataTable_DataTable1RowChanging(sender As Object, e As DataTable1RowChangeEvent) Handles Me.DataTable1RowChanging

        End Sub

    End Class
End Class
