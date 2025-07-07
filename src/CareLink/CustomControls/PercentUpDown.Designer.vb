' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class PercentUpDown
    Inherits NumericUpDown

    Protected Overrides Sub UpdateEditText()
        Me.Text = Me.Value.ToString() & " %"
    End Sub

    Protected Overrides Sub ValidateEditText()
        Dim text As String = Me.Text.Replace(" %", "")
        Dim value As Decimal
        If Decimal.TryParse(text, value) Then
            Me.Value = value
        End If
        MyBase.ValidateEditText()
    End Sub

End Class
