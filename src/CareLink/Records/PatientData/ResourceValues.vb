' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Class ResourceValues
    Public Property LS_BloodGlucose As String()
    Public Property LS_BloodGlucoseEntered As String()
    Public Property LS_Bolus As String()
    Public Property LS_CalibrationAccepted As String()
    Public Property LS_CarbsWithValue As String()
    Public Property LS_Grams As String()
    Public Property LS_Insulin_Units As String()
    Public Property LS_Meal As String()
    Public Property LS_Mgdl As String()
    Public Property LS_RegularBolusCompletedDeliveryDesc As String()
    Public Property LS_RegularBolusCompletedDeliveryTitle As String()
    Public Property LS_SensorCalibrated As String()
    Public Property LS_SensorCalibratedWith As String()

    <JsonExtensionData>
    Public Property AdditionalProperties As Dictionary(Of String, JsonElement)

End Class
