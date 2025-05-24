Imports System.Data.Odbc
Imports System.Data.SqlClient

Public Class FormRegister
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim email As String = TextBox1.Text.Trim()
        Dim password As String = TextBox2.Text.Trim()

        ' Validasi Input
        If String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(password) Then
            MessageBox.Show("Email dan Password tidak boleh kosong", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Buka koneksi
            Koneksi()

            ' Query untuk menambahkan data ke database
            Dim query As String = "INSERT INTO tbl_login (email, password) VALUES (?, ?)"
            Dim cmd As New OdbcCommand(query, Conn)
            cmd.Parameters.AddWithValue("?", email)
            cmd.Parameters.AddWithValue("?", password)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Registrasi berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Alihkan ke form login
            Me.Hide()
            FormLogin.Show()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Tutup koneksi
            If Conn IsNot Nothing AndAlso Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub
End Class