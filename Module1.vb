﻿Imports System.Data.Odbc
Module Module1
    Public Conn As New OdbcConnection
    Public Da As OdbcDataAdapter
    Public Ds As DataSet
    Public Rd As OdbcDataReader
    Public cmd As OdbcCommand
    Public MyDB As String
    Public Sub Koneksi()
        MyDB = "DSN=db_kasirkopi;Uid=root;Pwd=;"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub
End Module
