Imports System.Data.Odbc
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Public Class FormStokBarang
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        ComboBox1.Enabled = False
        ComboBox2.Enabled = False

        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True

        Button1.Text = "Input"
        Button2.Text = "Edit"
        Button3.Text = "Hapus"
        Button4.Text = "Tutup"
        Call Koneksi()
        Da = New OdbcDataAdapter("Select * From tbl_menu", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tbl_menu")
        DataGridView1.DataSource = Ds.Tables("tbl_menu")
        DataGridView1.ReadOnly = True
    End Sub

    Private namaMenu As New Dictionary(Of String, String) From {
    {"Ice Hazelnut Latte", "kopi, sirup hazelnut, susu"},
    {"Ice Kopi Gula Aren", "gula aren, kopi, susu"},
    {"Ice Kopi Gula Bandung", "kopi, gula bandung, susu"},
    {"Ice Kopi Hitam", "kopi hitam"},
    {"Ice Caramel Macchiato", "espresso, susu, caramel"},
    {"Ice Kopi Nusantara", "kopi nusantara, gula aren, susu"}
}

    Sub SiapIsi()
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        ComboBox1.Enabled = True
        ComboBox2.Enabled = True

        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()

        For Each menu As String In namaMenu.Keys
            ComboBox1.Items.Add(menu)
        Next
        ComboBox2.Items.Add("PCS Takar")
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim selectedMenu As String = ComboBox1.SelectedItem.ToString()
        If namaMenu.ContainsKey(selectedMenu) Then
            TextBox1.Text = namaMenu(selectedMenu)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call SiapIsi()

        Else
            If ComboBox1.Text = "" Or TextBox1.Text = "" Or TextBox2.Text = "" Or ComboBox2.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim InputData As String = "insert into tbl_menu values('" & ComboBox1.Text & "', '" & TextBox1.Text & "','" & TextBox2.Text & "','" & ComboBox2.Text & "')"
                cmd = New OdbcCommand(InputData, Conn)
                cmd.ExecuteNonQuery()
                MsgBox("Input Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "Edit" Then
            Button2.Text = "Simpan"
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call SiapIsi()

        Else
            If ComboBox1.Text = "" Or TextBox1.Text = "" Or TextBox2.Text = "" Or ComboBox2.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim UpdateData As String = "UPDATE tbl_menu SET komposisi='" & TextBox1.Text & "', jumlahstok='" & TextBox2.Text & "', satuanbarang='" & ComboBox2.Text & "'  WHERE namamenu='" & ComboBox1.Text & "'"
                cmd = New OdbcCommand(UpdateData, Conn)
                cmd.ExecuteNonQuery()
                MsgBox("Update Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub
    Private Sub FormStokBarang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button4.Text = "Tutup" Then
            Me.Close()
        Else
            Call KondisiAwal()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Hapus" Then
            Button3.Text = "Delete"
            Button1.Enabled = False
            Button2.Enabled = False
            Button4.Text = "Batal"
            Call SiapIsi()

        Else
            If ComboBox1.Text = "" Or TextBox1.Text = "" Or TextBox2.Text = "" Or ComboBox2.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                Dim HapusData As String = "Delete from tbl_menu WHERE namamenu='" & ComboBox1.Text & "'"
                cmd = New OdbcCommand(HapusData, Conn)
                cmd.ExecuteNonQuery()
                MsgBox("Hapus Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub




End Class