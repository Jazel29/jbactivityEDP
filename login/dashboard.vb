Public Class dashboard
    Private Sub btnstudent_Click(sender As Object, e As EventArgs) Handles btnstudent.Click
        studentlist.Show()
        Me.Close()

    End Sub

    Private Sub btnteacher_Click(sender As Object, e As EventArgs) Handles btnteacher.Click
        teacherList.Show()
        Me.Close()
    End Sub

    Private Sub btnassignment_Click(sender As Object, e As EventArgs) Handles btnassignment.Click
        coursesList.Show()
        Me.Close()
    End Sub

    Private Sub btnenrollment_Click(sender As Object, e As EventArgs) Handles btnenrollment.Click
        enrollmentList.Show()
        Me.Close()
    End Sub

    Private Sub btnass_Click(sender As Object, e As EventArgs) Handles btnass.Click
        assignmentList.Show()
        Me.Close()
    End Sub

End Class