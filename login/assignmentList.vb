Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.Office.Interop


Public Class assignmentList
    Public DBConnection As New MySqlConnection
    Public adp As SqlDataAdapter
    Dim exlFile As Excel.Application

    Public Sub Connect_to_DB()

        Dim DBConnectionString As String = "server=localhost;user id=root; port=3306; password=admin; database=activity"
        With DBConnection
            Try
                If .State = ConnectionState.Open Then .Close()
                .ConnectionString = DBConnectionString
                .Open()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Connection Error")
                Call Disconnect_to_DB()


            End Try
        End With
    End Sub
    Public Sub Disconnect_to_DB()
        With DBConnection
            .Close()
            .Dispose()
        End With

    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim strsql As String = "INSERT INTO assignments VALUES('" & Me.txtID.Text & "','" & Me.txtCourseID.Text & "','" & Me.txtName.Text & "','" & Me.txtDescription.Text & "','" & Me.txtDeadline.Text & "', '" & Me.txtPoints.Text & "' )"

        Connect_to_DB()
        Dim myCommand As New MySqlCommand


        Try
            myCommand.Connection = DBConnection
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            MsgBox("Successfully Added", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
        Disconnect_to_DB()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim strsql As String = "UPDATE assignments SET course_id = " & Me.txtCourseID.Text & ", name = '" & Me.txtName.Text & "', description = '" & Me.txtDescription.Text & "', deadline = '" & Me.txtDeadline.Text & "', points = " & Me.txtPoints.Text & " WHERE id = " & Me.txtID.Text


        '" & Me.txtID.Text & "','" & Me.txtName.Text & "','" & Me.txtEmail.Text & "','" & Me.txtAge.Text & "','" & Me.txtMajor.Text & "' )"

        Connect_to_DB()
        Dim myCommand As New MySqlCommand

        Try
            myCommand.Connection = DBConnection
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            MsgBox("Successfully Updated", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
        Disconnect_to_DB()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim strsql As String = "delete from assignments where id = '" & Me.txtID.Text & "'"

        Connect_to_DB()
        Dim myCommand As New MySqlCommand


        Try
            myCommand.Connection = DBConnection
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            MsgBox("Successfully Deleted", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
        Disconnect_to_DB()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim DBConnectionString As String = "server=localhost;user id=root; port=3306; password=admin; database=activity"
        Dim strsql As String = "SELECT * FROM assignments"
        Dim dt As New DataTable()

        Using conn As New MySqlConnection(DBConnectionString)
            conn.Open()

            Using cmd As New MySqlCommand(strsql, conn)

                Using adp As New MySqlDataAdapter(cmd)
                    adp.Fill(dt)
                End Using

            End Using

            conn.Close()
        End Using

        DataGridView1.DataSource = dt

    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim nameToSearch As String = Me.txtSearch.Text
        Dim DBConnectionString As String = "server=localhost;user id=root; port=3306; password=admin; database=activity"
        Dim strsql As String = "SELECT * FROM assignments WHERE name LIKE '%" & nameToSearch & "%'"
        Dim dt As New DataTable()

        Using conn As New MySqlConnection(DBConnectionString)
            conn.Open()

            Using cmd As New MySqlCommand(strsql, conn)

                Using adp As New MySqlDataAdapter(cmd)
                    adp.Fill(dt)
                End Using

            End Using

            conn.Close()
        End Using

        DataGridView1.DataSource = dt

    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        dashboard.Show()
        Me.Close()
    End Sub

    Private csvFilePath As String = ""
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        If openFileDialog.ShowDialog() = DialogResult.OK Then

            Dim dataTable As New DataTable()

            Using parser As New TextFieldParser(openFileDialog.FileName)
                parser.TextFieldType = FieldType.Delimited
                parser.SetDelimiters(",")
                While Not parser.EndOfData
                    Dim fields As String() = parser.ReadFields()
                    If dataTable.Columns.Count = 0 Then
                        For i As Integer = 0 To fields.Length - 1
                            dataTable.Columns.Add(New DataColumn("Column " & (i + 1)))
                        Next
                    End If
                    dataTable.Rows.Add(fields)
                End While
            End Using
            csvFilePath = openFileDialog.FileName

            DataGridView1.DataSource = dataTable
        End If
    End Sub

    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        Try
            If Not String.IsNullOrEmpty(csvFilePath) Then

                Dim backupFilePath As String = Path.Combine(Path.GetDirectoryName(csvFilePath), "assignmentBackup.bat")
                File.Copy(csvFilePath, backupFilePath, True)

                MessageBox.Show("CSV file backed up successfully!")
            Else
                MessageBox.Show("Please select a CSV file to backup.")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            exlFile = New Excel.Application
            exlFile.Workbooks.Open("C:\Users\Mark Louie Manrique\source\repos\login\template\assignmentTemplate.xlsx")

            Dim rowCount As Integer = DataGridView1.Rows.Count
            For i As Integer = 0 To rowCount - 2
                For j As Integer = 0 To DataGridView1.Columns.Count - 1
                    exlFile.Cells(i + 6, j + 1).Value = DataGridView1.Rows(i).Cells(j).Value
                Next
            Next

            exlFile.Cells(4, 5) = "Date: " & FormatDateTime(Now(), DateFormat.LongDate)

            Dim filename As String = "assignmentRecord_" & DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") & ".xlsx"
            Dim path As String = "C:\Users\Mark Louie Manrique\source\repos\login\sampleOutput\" & filename
            exlFile.ActiveWorkbook.SaveAs(path)

            MessageBox.Show("File saved successfully: " & filename, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            exlFile.Visible = True
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            exlFile = Nothing
        End Try
    End Sub
End Class