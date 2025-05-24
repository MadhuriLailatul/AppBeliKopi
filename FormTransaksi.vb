Imports System.Data.Common
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar
Imports Microsoft.Reporting.WinForms
Imports Microsoft.ReportingServices.Diagnostics.Internal

Public Class FormTransaksi
    Dim TglMySQL As String
    Sub KondisiAwal()
        LBLNamaPelanggan.Text = ""
        LBLTanggal.Text = Today
        LBLAdmin.Text = FormMenuUtama.STLabel2.Text
        LBLKembali.Text = ""
        LBLItem.Text = ""
        Call MunculKodePelanggan()
        Call NomorOtomatis()
        Label5.Text = "0"
        TextBox4.Text = ""
    End Sub

    Private Sub FormTransaksi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Sub MunculKodePelanggan()
        Call Koneksi()
        ComboBox1.Items.Clear()
        cmd = New OdbcCommand("Select * from tbl_pelanggan", Conn)
        Rd = cmd.ExecuteReader
        Do While Rd.Read
            ComboBox1.Items.Add(Rd.Item(0))
        Loop
    End Sub

    Sub RumusCariItem()
        Dim HitungItem As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            HitungItem = HitungItem + DataGridView1.Rows(i).Cells(4).Value
            LBLItem.Text = HitungItem
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        LBLJam.Text = TimeOfDay
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Call Koneksi()
        cmd = New OdbcCommand("Select * from tbl_pelanggan where kodepelanggan ='" & ComboBox1.Text & "'", Conn)
        Rd = cmd.ExecuteReader
        Rd.Read()
        If Rd.HasRows Then
            LBLNamaPelanggan.Text = Rd!NamaPelanggan
        End If
        Call TampilkanMenu(ComboBox1.Text)
        Call RumusCariItem()
    End Sub

    Sub TampilkanMenu(ByVal kodePelanggan As String)
        DataGridView1.Rows.Clear()
        Call Koneksi()
        Dim cmdMenu As New OdbcCommand("SELECT * FROM tbl_pelanggan WHERE kodepelanggan = '" & kodePelanggan & "'", Conn)
        Dim rdMenu As OdbcDataReader = cmdMenu.ExecuteReader
        While rdMenu.Read()
            Dim namaPelanggan As String = rdMenu("namapelanggan").ToString()
            Dim namaMenu As String = rdMenu("namamenu").ToString()
            Dim harga As Integer = Convert.ToInt32(rdMenu("harga"))
            Dim jumlah As Integer = Convert.ToInt32(rdMenu("jumlah"))
            Dim subTotal As Integer = harga * jumlah
            DataGridView1.Rows.Add(New String() {kodePelanggan, namaPelanggan, namaMenu, harga.ToString(), jumlah.ToString(), subTotal.ToString()})
        End While
        rdMenu.Close()
        Call RumusSubTotal()
    End Sub

    Sub NomorOtomatis()
        Call Koneksi()
        Dim UrutanKode As String
        Dim Hitung As Integer

        ' Ambil NoJual dengan nilai maksimal berdasarkan tanggal hari ini
        cmd = New OdbcCommand("SELECT MAX(nojual) FROM tbl_jual WHERE nojual LIKE 'J" & Format(Now, "yyMMdd") & "%'", Conn)
        Rd = cmd.ExecuteReader
        Rd.Read()

        ' Jika tidak ada data, mulai dengan 001
        If Not Rd.HasRows OrElse IsDBNull(Rd(0)) Then
            UrutanKode = "J" & Format(Now, "yyMMdd") & "001"
        Else
            ' Ambil angka urutan dari NoJual terakhir
            Dim LastNo As String = Rd.GetString(0)
            Hitung = CInt(Microsoft.VisualBasic.Right(LastNo, 3)) + 1
            UrutanKode = "J" & Format(Now, "yyMMdd") & Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If

        ' Set hasil ke label
        LBLNoJual.Text = UrutanKode
    End Sub

    Sub RumusSubTotal()
        Dim hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            hitung = hitung + DataGridView1.Rows(i).Cells(5).Value
            Label5.Text = hitung
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If e.KeyChar = Chr(13) Then
            If Val(TextBox4.Text) < Val(Label5.Text) Then
                MsgBox("Pembayaran Kurang!")
            ElseIf Val(TextBox4.Text) = Val(Label5.Text) Then
                LBLKembali.Text = 0
            ElseIf Val(TextBox4.Text) > Val(Label5.Text) Then
                LBLKembali.Text = Val(TextBox4.Text) - Val(Label5.Text)
                Button1.Focus()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If LBLKembali.Text = "" Or LBLNamaPelanggan.Text = "" Or Label5.Text = "" Then
            MsgBox("Transaksi Tidak Ada, Silahkan Lakukan Transaksi Terlebih Dahulu")
        Else
            Dim waktu As String = Format(TimeOfDay, "HH:mm:ss")
            Dim TglMySQL As String = Format(Today, "yyyy-MM-dd")
            Dim SimpanJual As String = "INSERT INTO tbl_jual(nojual, tgljual, jamjual, itemjual, totaljual, dibayar, kembali, kodepelanggan, kodeadmin) " &
                           "VALUES ('" & LBLNoJual.Text & "', '" & TglMySQL & "', '" & waktu & "', '" & LBLItem.Text & "', '" & Label5.Text & "', '" & TextBox4.Text & "', '" & LBLKembali.Text & "', '" & ComboBox1.Text & "', '" & LBLAdmin.Text & "')"
            cmd = New OdbcCommand(SimpanJual, Conn)
            cmd.ExecuteNonQuery()

            For Baris As Integer = 0 To DataGridView1.Rows.Count - 2
                Dim SimpanDetail As String = "INSERT INTO tbl_detailjual (nojual, namamenu, hargajual, jumlahjual, subtotal) VALUES ('" & LBLNoJual.Text & "', '" & DataGridView1.Rows(Baris).Cells(2).Value & "', '" & DataGridView1.Rows(Baris).Cells(3).Value & "', '" & DataGridView1.Rows(Baris).Cells(4).Value & "', '" & DataGridView1.Rows(Baris).Cells(5).Value & "')"
                cmd = New OdbcCommand(SimpanDetail, Conn)
                cmd.ExecuteNonQuery()

                Dim namaMenu As String = DataGridView1.Rows(Baris).Cells(2).Value.ToString()
                Dim jumlahTerjual As Integer = Convert.ToInt32(DataGridView1.Rows(Baris).Cells(4).Value)

                Dim queryStok As String = "SELECT JumlahStok FROM tbl_menu WHERE namamenu = '" & namaMenu & "'"
                Dim cmdStok As New OdbcCommand(queryStok, Conn)
                Dim Rd As OdbcDataReader = cmdStok.ExecuteReader()

                If Rd.HasRows Then
                    Rd.Read()
                    Dim stokSaatIni As Integer = Convert.ToInt32(Rd("JumlahStok"))
                    Dim stokBaru As Integer = stokSaatIni - jumlahTerjual

                    If stokBaru < 0 Then
                        MessageBox.Show("Stok untuk " & namaMenu & " tidak mencukupi.")
                    Else
                        Dim KurangiStok As String = "UPDATE tbl_menu SET JumlahStok = '" & stokBaru & "' WHERE NamaMenu = '" & namaMenu & "'"
                        Dim cmdKurangiStok As New OdbcCommand(KurangiStok, Conn)
                        cmdKurangiStok.ExecuteNonQuery()
                    End If
                Else
                    MessageBox.Show("Item " & namaMenu & " tidak ditemukan di tabel menu.")
                End If
                Rd.Close()
            Next
            DataGridView1.Rows.Clear()
            Call KondisiAwal()
            MsgBox("Transaksi Telah Berhasil Disimpan")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "Tutup" Then
            Me.Close()
        Else
            Call KondisiAwal()
        End If
    End Sub

    Private Sub ButtonCetak_Click(sender As Object, e As EventArgs) Handles ButtonCetak.Click
        Dim ds As New DataSet5
        Dim dt As New DataTable
        dt = ds.Tables("kopi")
        For i = 0 To DataGridView1.Rows.Count - 1
            dt.Rows.Add(DataGridView1.Rows(i).Cells(0).Value, DataGridView1.Rows(i).Cells(1).Value,
             DataGridView1.Rows(i).Cells(2).Value, DataGridView1.Rows(i).Cells(3).Value,
             DataGridView1.Rows(i).Cells(4).Value, DataGridView1.Rows(i).Cells(5).Value)
        Next
        With StrukCetak.ReportViewer1.LocalReport
            .ReportPath = "C:\Users\ASUS\source\repos\AppBeliKopi\ReportStruk.rdlc"
            .DataSources.Clear()
            .DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet5", dt))

        End With
        StrukCetak.Show()
        StrukCetak.ReportViewer1.RefreshReport()

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class