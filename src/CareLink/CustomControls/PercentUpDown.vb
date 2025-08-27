' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PercentUpDown
    Inherits NumericUpDown

    Protected Overrides Sub UpdateEditText()
        Me.Text = $"{Me.Value} %"
    End Sub

    Protected Overrides Sub ValidateEditText()
        Dim s As String = Me.Text.Replace(oldValue:=" %", newValue:="")
        Dim result As Decimal
        If Decimal.TryParse(s, result) Then
            Me.Value = result
        End If
        MyBase.ValidateEditText()
    End Sub

End Class
