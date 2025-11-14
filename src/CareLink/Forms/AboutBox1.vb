' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public NotInheritable Class AboutBox1

    Private Sub AboutBox1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim applicationTitle As String = If(My.Application.Info.Title = "",
                                            IO.Path.GetFileNameWithoutExtension(path:=My.Application.Info.AssemblyName),
                                            My.Application.Info.Title)

        Me.Text = $"About {applicationTitle}"
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text =
            $"CareLink™ Program Version {My.Application.Info.Version}, .NET Version {Environment.Version}"
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    ''' <summary>
    '''  Overrides the OnHandleCreated method to enable dark mode
    '''  for the dialog when its handle is created.
    ''' </summary>
    ''' <param name="e">The event data.</param>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        EnableDarkMode(hwnd:=Me.Handle)
    End Sub

End Class
