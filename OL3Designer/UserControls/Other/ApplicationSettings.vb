Public Class ApplicationSettings

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        saveSettings()
        Me.Dispose()
    End Sub

    Sub loadSettings()
        'open map post export
        If My.Settings.OpenMap Then
            CheckBox1.Checked = True
        Else
            CheckBox1.Checked = False
        End If

        'add to key
        If My.Settings.AddLyrToKey Then
            CheckBox2.Checked = True
        Else
            CheckBox2.Checked = False
        End If

        'override
        If My.Settings.MapOverwrite Then
            CheckBox3.Checked = True
        Else
            CheckBox3.Checked = False
        End If

    End Sub

    Sub saveSettings()

        'open map post export
        If CheckBox1.Checked Then
            My.Settings.OpenMap = True
        Else
            My.Settings.OpenMap = False
        End If

        'add to key
        If CheckBox2.Checked Then
            My.Settings.AddLyrToKey = True
        Else
            My.Settings.AddLyrToKey = False
        End If

        'override
        If CheckBox3.Checked Then
            My.Settings.MapOverwrite = True
        Else
            My.Settings.MapOverwrite = False
        End If

    End Sub

    Private Sub ApplicationSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadSettings()
    End Sub
End Class