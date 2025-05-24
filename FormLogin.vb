Imports System.Data.Odbc
Imports System.Data.SqlClient

Public Class FormLogin
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Sub Terbuka()
        FormMenuUtama.LoginToolStripMenuItem.Enabled = False
        FormMenuUtama.LogoutToolStripMenuItem.Enabled = True
        FormMenuUtama.MasterToolStripMenuItem.Enabled = True
        FormMenuUtama.TransaksiToolStripMenuItem.Enabled = True
    End Sub

    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call Terbuka()
        FormMenuUtama.STLabel2.Text = TextBox1.Text
        FormMenuUtama.STLabel4.Text = TextBox2.Text

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

            ' Query untuk memeriksa apakah data login sesuai
            Dim query As String = "SELECT * FROM tbl_login WHERE email = ? AND password = ?"
            Dim cmd As New OdbcCommand(query, Conn)
            cmd.Parameters.AddWithValue("?", email)
            cmd.Parameters.AddWithValue("?", password)
            Dim reader As OdbcDataReader = cmd.ExecuteReader()

            If reader.HasRows Then
                MessageBox.Show("Login berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Arahkan ke halaman utama
                Me.Hide()
                ' Tambahkan form berikutnya jika ada, contoh:
                ' FormUtama.Show()
            Else
                MessageBox.Show("Email atau Password salah.", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Tutup koneksi
            If Conn IsNot Nothing AndAlso Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub


    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        FormRegister.ShowDialog()
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub
End Class