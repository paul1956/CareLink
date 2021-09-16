' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports Xunit

Public Class UnitTest1
    Private Const ActiveInsulinIncrements As Integer = 27

    <Fact>
    Sub TestSub()
        Dim remainingInsulinList As New List(Of Insulin)
        Dim getSgDateTime As Date = Now - (288 * Form1._FiveMinutes)
        Dim oaTime As Double = getSgDateTime.RoundDateDown(RoundDateTo.Minute).ToOADate()
        remainingInsulinList.Add(New Insulin(oaTime, 10, ActiveInsulinIncrements))
        For i = 1 To 287
            oaTime = (getSgDateTime + (Form1._FiveMinutes * i)).RoundDateDown(RoundDateTo.Minute).ToOADate()
            remainingInsulinList.Add(New Insulin(oaTime, 0, ActiveInsulinIncrements))
        Next
        For i As Integer = 0 To remainingInsulinList.Count - 1
            If i < ActiveInsulinIncrements Then
                If i > 0 Then
                    remainingInsulinList.Adjustlist(0, i)
                End If
                Continue For
            End If
            Dim startIndex As Integer = i - ActiveInsulinIncrements + 1
            remainingInsulinList.Adjustlist(startIndex, ActiveInsulinIncrements)
        Next

    End Sub
End Class
