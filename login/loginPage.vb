Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn.ConnectionString = "server=localhost;user id=root; port=3306; password=admin; database=activity"

    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        sql = "SELECT * FROM login WHERE username = '" & txtUserName.Text & "' AND password = '" & txtPassword.Text & "' "
        connect()

        If dr.Read Then
            dashboard.Show()

        Else
            MsgBox("Invalid Username or Password")
        End If
        conn.Close()


    End Sub


End Class
