' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetAmountRecord
    Private ReadOnly _units As String
    Private ReadOnly _rate As Single
    Private ReadOnly _percent As Single
    Private ReadOnly _typeIsRate As Boolean

    Public Sub New(s As String)
        If s.Contains("%"c) Then
            _typeIsRate = False
            _percent = Integer.Parse(s.Trim("%"c).Trim)
        Else
            _typeIsRate = True
            Dim sSplit As String() = s.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            _rate = Single.Parse(sSplit(0))
            _units = sSplit(1)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return If(_typeIsRate, $"Rate:  {vbTab}{_rate:F1} {_units}", $"Percent:{vbTab}{_percent}%")
    End Function

End Class
