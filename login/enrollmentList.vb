Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Public Class enrollmentList
    Public DBConnection As New MySqlConnection
    Public adp As SqlDataAdapter

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
        Dim strsql As String = "INSERT INTO enrollment VALUES('" & Me.txtStudentID.Text & "','" & Me.txtCourseID.Text & "','" & Me.txtSemester.Text & "','" & Me.txtYear.Text & "','" & Me.txtGrade.Text & "' )"

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
        Dim strsql As String = "UPDATE enrollment SET course_id = " & Me.txtCourseID.Text & ", semester = '" & Me.txtSemester.Text & "', year = " & Me.txtYear.Text & ", grade = " & Me.txtGrade.Text & " WHERE student_id = " & Me.txtStudentID.Text

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
        Dim strsql As String = "delete from enrollment where student_id = '" & Me.txtStudentID.Text & "'"

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
        Dim strsql As String = "SELECT * FROM enrollment"
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
        Dim searchYear As Integer = Integer.Parse(txtSearch.Text)
        Dim DBConnectionString As String = "server=localhost;user id=root; port=3306; password=admin; database=activity"
        Dim strsql As String = "SELECT * FROM enrollment WHERE year = " & searchYear.ToString()
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

                Dim backupFilePath As String = Path.Combine(Path.GetDirectoryName(csvFilePath), "assignmentBackup.txt")
                File.Copy(csvFilePath, backupFilePath, True)

                MessageBox.Show("CSV file backed up successfully!")
            Else
                MessageBox.Show("Please select a CSV file to backup.")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub
End Class