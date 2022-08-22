' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PumpData
    Public BolusRow As Single
    Public InsulinRow As Single
    Public MealRow As Single
    Public JsonData As Dictionary(Of String, String) = Nothing

    Public Sub New(bolusRow As Single, insulinRow As Single, mealRow As Single, jsonData As Dictionary(Of String, String))
        Me.BolusRow = bolusRow
        Me.InsulinRow = insulinRow
        Me.MealRow = mealRow
        Me.JsonData = jsonData
    End Sub

    Public Function ExtractPumpData(ByRef bolusRow As Single, ByRef insulinRow As Single, ByRef mealRow As Single) As Dictionary(Of String, String)
        If Me.BolusRow = 0 Then
            Stop
        End If
        bolusRow = Me.BolusRow
        insulinRow = Me.InsulinRow
        mealRow = Me.MealRow
        Return JsonData
    End Function

    Friend Function ExtractJsonData() As Dictionary(Of String, String)
        Return JsonData
    End Function
End Class
