' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Module JsonExtensions

    <Extension>
    Private Function jsonItemAsString(item As KeyValuePair(Of String, Object)) As String
        Dim itemValue As JsonElement = CType(item.Value, JsonElement)
        Dim valueAsString As String = itemValue.ToString
        Select Case itemValue.ValueKind
            Case JsonValueKind.False
                Return "False"
            Case JsonValueKind.Null
                Return ""
            Case JsonValueKind.Number
                Return valueAsString
            Case JsonValueKind.True
                Return "True"
            Case JsonValueKind.String
        End Select
        Return valueAsString
    End Function

    <Extension>
    Public Function CleanUserData(cleanRecentData As Dictionary(Of String, String)) As String
        cleanRecentData("firstName") = "First"
        cleanRecentData("lastName") = "Last"
        cleanRecentData("medicalDeviceSerialNumber") = "NG1234567H"
        Return JsonSerializer.Serialize(cleanRecentData, New JsonSerializerOptions)
    End Function

    Public Function LoadList(value As String) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        For Each e As IndexClass(Of Dictionary(Of String, Object)) In JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, options).WithIndex
            Dim resultDictionary As New Dictionary(Of String, String)
            For Each item As KeyValuePair(Of String, Object) In e.Value
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                ElseIf item.Key = "sg" Then
                    resultDictionary.Add(item.Key, CStr((item.jsonItemAsString.ParseDouble / scaleUnitsDivisor).RoundDouble(2)))
                Else
                    resultDictionary.Add(item.Key, item.jsonItemAsString)
                End If
            Next

            resultDictionaryArray.Add(resultDictionary)
        Next
        Return resultDictionaryArray
    End Function

    Public Function Loads(value As String) As Dictionary(Of String, String)
        Dim resultDictionary As New Dictionary(Of String, String)
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}
        Dim item As KeyValuePair(Of String, Object)
        Try
            For Each item In JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(value, options).ToList()
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                    Continue For
                End If
                Select Case item.Key
                    Case NameOf(ItemIndexs.bgUnits)
                        BgUnitsString = s_unitsStrings(item.Value.ToString)
                        If BgUnitsString = "mg/dl" Then
                            scaleUnitsDivisor = 1
                            s_markerRow = 400
                            s_limitHigh = 180
                            s_limitLow = 70
                            s_insulinRow = 50
                            s_criticalLow = 50
                        Else
                            scaleUnitsDivisor = 18
                            s_markerRow = CSng(Math.Round(400 / 18, 0, MidpointRounding.AwayFromZero))
                            s_limitHigh = (180 / scaleUnitsDivisor).RoundSingle(1)
                            s_limitLow = (70 / scaleUnitsDivisor).RoundSingle(1)
                            s_insulinRow = CSng(Math.Round(50 / scaleUnitsDivisor, 0, MidpointRounding.ToZero))
                            s_criticalLow = s_insulinRow
                        End If
                        resultDictionary.Add(item.Key, CStr((item.jsonItemAsString.ParseDouble / scaleUnitsDivisor).RoundDouble(2)))
                    Case NameOf(ItemIndexs.clientTimeZoneName)
                        If My.Settings.UseLocalTimeZone Then
                            s_clientTimeZone = TimeZoneInfo.Local
                        Else
                            s_clientTimeZoneName = item.Value.ToString
                            s_clientTimeZone = CalculateTimeZone()
                            If s_clientTimeZone Is Nothing Then
                                Dim message As String = $"Your pump timezone '{s_clientTimeZoneName}' is not recognized. If you select Yes '{TimeZoneInfo.Local.Id}' will be used and you will not be prompted further. No will exit program. Please open an issue and provide the name '{s_clientTimeZoneName}'. After selecting yes you can change the behavior under Options Menu."
                                Dim result As DialogResult = MessageBox.Show(message, "Timezone Unknown",
                                                                             MessageBoxButtons.YesNo,
                                                                             MessageBoxIcon.Question)
                                Select Case result
                                    Case DialogResult.Yes
                                        My.Settings.UseLocalTimeZone = True
                                        s_clientTimeZone = TimeZoneInfo.Local
                                    Case DialogResult.No
                                        Form1.Close()
                                End Select
                            End If
                        End If
                        resultDictionary.Add(item.Key, CStr((item.jsonItemAsString.ParseDouble / scaleUnitsDivisor).RoundDouble(2)))
                    Case NameOf(ItemIndexs.timeFormat)
                        Dim internaltimeFormat As String = item.Value.ToString
                        s_timeWithMinuteFormat = If(internaltimeFormat = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
                        s_timeWithoutMinuteFormat = If(internaltimeFormat = "HR_12", TwelveHourTimeWithoutMinuteFormat, MilitaryTimeWithoutMinuteFormat)
                        resultDictionary.Add(item.Key, item.jsonItemAsString)
                    Case "Sg", "sg", NameOf(ItemIndexs.averageSGFloat), NameOf(ItemIndexs.sgBelowLimit)
                        resultDictionary.Add(item.Key, CStr((item.jsonItemAsString.ParseDouble / scaleUnitsDivisor).RoundDouble(2)))
                    Case Else
                        resultDictionary.Add(item.Key, item.jsonItemAsString)
                End Select
            Next
            Return resultDictionary
        Catch ex As Exception
            Throw
        End Try
        Stop
    End Function

End Module
