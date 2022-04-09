' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class Insulin
    Private ReadOnly _incrementDownCount As Integer
    Private _adjustmentValue As Double
    Private _incrementUpCount As Integer

    Public Sub New(oaTime As Double, initialInsulinLevel As Double, activeInsulinIncrements As Integer)
        Me.OaTime = oaTime
        Me.EventDate = Date.FromOADate(oaTime)
        _incrementUpCount = CInt(Math.Ceiling(activeInsulinIncrements * (1 / 3)))
        _incrementDownCount = activeInsulinIncrements - _incrementUpCount
        _adjustmentValue = initialInsulinLevel / activeInsulinIncrements
        Me.CurrentInsulinLevel = _adjustmentValue * _incrementDownCount
    End Sub

    Public Property CurrentInsulinLevel As Double

    Public Property EventDate As Date

    Public Property OaTime As Double

    Friend Function Adjust() As Insulin
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
        Return $"{Me.EventDate.ToShortTimeString()} {Me.CurrentInsulinLevel.RoundDouble(3)}"
    End Function

End Class
