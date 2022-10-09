' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class RunningActiveInsulinRecord
    Private ReadOnly _dontAdjust As Boolean = False
    Private ReadOnly _incrementDownCount As Integer
    Private _adjustmentValue As Single
    Private _incrementUpCount As Integer

    Public Sub New(oaDateTime As OADate, initialInsulinLevel As Single, useAdvancedAITDecay As Boolean)
        Me.OaDateTime = oaDateTime
        Me.EventDate = Date.FromOADate(oaDateTime)
        Dim divisor As Double = If(useAdvancedAITDecay, 3.5, 3)
        _incrementUpCount = CInt(Math.Ceiling(s_activeInsulinIncrements * (1 / divisor)))
        _incrementDownCount = s_activeInsulinIncrements - _incrementUpCount
        _adjustmentValue = initialInsulinLevel / s_activeInsulinIncrements
        Me.CurrentInsulinLevel = _adjustmentValue * _incrementDownCount
    End Sub

    Public Property CurrentInsulinLevel As Single
    Public Property EventDate As Date

    Public Property OaDateTime As OADate

    Friend Function Adjust() As RunningActiveInsulinRecord
        If _dontAdjust Then Return Me
        If Me.CurrentInsulinLevel > 0 Then
            If _incrementUpCount > 0 Then
                _incrementUpCount -= 1
                Me.CurrentInsulinLevel += _adjustmentValue
                If _incrementUpCount = 0 Then
                    _adjustmentValue = Me.CurrentInsulinLevel / _incrementDownCount
                End If
            Else
                Me.CurrentInsulinLevel -= _adjustmentValue
            End If
        End If
        Return Me
    End Function

    Public Overrides Function ToString() As String
        Return $"{Me.EventDate.ToShortTimeString()} {Me.CurrentInsulinLevel.RoundSingle(3)}"
    End Function

End Class
