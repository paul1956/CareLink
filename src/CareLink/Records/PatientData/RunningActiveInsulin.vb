' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class RunningActiveInsulin
    Private ReadOnly _incrementDownCount As Integer
    Private _adjustmentValue As Single
    Private _incrementUpCount As Integer

    Public Sub New(
        firstValidOaTime As OADate,
        initialInsulinLevel As Single,
        insulinIncrements As Integer,
        upCount As Integer)

        Me.OaDateTime = firstValidOaTime
        Me.EventDate = Date.FromOADate(firstValidOaTime)

        _incrementUpCount = upCount
        _incrementDownCount = insulinIncrements - upCount
        _adjustmentValue = initialInsulinLevel / insulinIncrements
        Me.CurrentInsulinLevel = _adjustmentValue * _incrementDownCount
    End Sub

    Public Property EventDate As Date
    Public Property OaDateTime As OADate
    Public Property CurrentInsulinLevel As Single

    Friend Function Adjust() As RunningActiveInsulin
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
        Return $"{Me.EventDate:t} {Me.CurrentInsulinLevel.RoundToSingle(digits:=3)}"
    End Function

End Class
