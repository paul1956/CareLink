' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class RunningActiveInsulin
    Private ReadOnly _incrementDownCount As Integer
    Private _adjustmentValue As Single
    Private _incrementUpCount As Integer

    Public Sub New(firstNotSkippedOaTime As OADate, initialInsulinLevel As Single, currentUser As CurrentUserRecord)
        Me.OaDateTime = firstNotSkippedOaTime
        Me.EventDate = Date.FromOADate(firstNotSkippedOaTime)

        _incrementUpCount = s_insulinTypes(currentUser.InsulinTypeName).UpCount
        _incrementDownCount = currentUser.GetActiveInsulinIncrements - _incrementUpCount
        _adjustmentValue = initialInsulinLevel / currentUser.GetActiveInsulinIncrements
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
        Return $"{Me.EventDate.ToShortTimeString()} {Me.CurrentInsulinLevel.RoundSingle(digits:=3)}"
    End Function

End Class
