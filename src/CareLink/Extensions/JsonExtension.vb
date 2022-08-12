' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Module JsonExtensions
    Friend s_timeZoneList As List(Of TimeZoneInfo)
    Friend s_useLocalTimeZone As Boolean

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
        If value IsNot Nothing Then

            Dim rawJsonData As List(Of KeyValuePair(Of String, Object)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(value, options).ToList()
            For Each item In rawJsonData
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                    Continue For
                End If
                Try
                    Select Case item.Key
                        Case NameOf(ItemIndexs.bgUnits)
                            BgUnits = item.Value.ToString()
                            If Not s_unitsStrings.TryGetValue(BgUnits, BgUnitsString) Then
                                Dim averageSGFloatAsString As String = rawJsonData(ItemIndexs.averageSGFloat).jsonItemAsString
                                If averageSGFloatAsString.ParseSingle() > 40 Then
                                    BgUnitsString = "mg/dl"
                                    BgUnits = "MG_DL"
                                Else
                                    BgUnitsString = "mmol/L"
                                    BgUnits = "MMOL_L"
                                End If
                            End If
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
                            resultDictionary.Add(item.Key, item.jsonItemAsString)
                        Case NameOf(ItemIndexs.clientTimeZoneName)
                            If s_useLocalTimeZone Then
                                s_clientTimeZone = TimeZoneInfo.Local
                            Else
                                s_clientTimeZoneName = item.Value.ToString
                                s_clientTimeZone = CalculateTimeZone()
                                Dim message As String
                                Dim messageButtons As MessageBoxButtons
                                If s_clientTimeZone Is Nothing Then
                                    If s_clientTimeZoneName = "NaN" Then
                                        message = $"Your pump appears to be off-line, some values will be wrong do you want to continue? If you select OK '{TimeZoneInfo.Local.Id}' will be used as you local time and you will not be prompted further. Cancel will Exit."
                                        messageButtons = MessageBoxButtons.OKCancel
                                    Else
                                        message = $"Your pump timezone '{s_clientTimeZoneName}' is not recognized, do you want to exit? If you select No permanently use '{TimeZoneInfo.Local.Id}''? If you select Yes '{TimeZoneInfo.Local.Id}' will be used and you will not be prompted further. No will use '{TimeZoneInfo.Local.Id}' until you restart program. Cancel will exit program. Please open an issue and provide the name '{s_clientTimeZoneName}'. After selecting 'Yes' you can change the behavior under the Options Menu."
                                        messageButtons = MessageBoxButtons.YesNoCancel
                                    End If
                                    Dim result As DialogResult = MessageBox.Show(message, "Timezone Unknown",
                                                                                 messageButtons,
                                                                                 MessageBoxIcon.Question)
                                    s_useLocalTimeZone = True
                                    s_clientTimeZone = TimeZoneInfo.Local
                                    Select Case result
                                        Case DialogResult.Yes
                                            My.Settings.UseLocalTimeZone = True
                                        Case DialogResult.Cancel
                                            Form1.Close()
                                    End Select
                                End If
                            End If
                            resultDictionary.Add(item.Key, item.jsonItemAsString)
                        Case NameOf(ItemIndexs.timeFormat)
                            Dim internaltimeFormat As String = item.Value.ToString
                            s_timeWithMinuteFormat = If(internaltimeFormat = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
                            s_timeWithoutMinuteFormat = If(internaltimeFormat = "HR_12", TwelveHourTimeWithoutMinuteFormat, MilitaryTimeWithoutMinuteFormat)
                            resultDictionary.Add(item.Key, item.jsonItemAsString)
                        Case "Sg", "sg", NameOf(ItemIndexs.averageSGFloat), NameOf(ItemIndexs.averageSG), NameOf(ItemIndexs.sgBelowLimit)
                            resultDictionary.Add(item.Key, CStr((item.jsonItemAsString.ParseDouble / scaleUnitsDivisor).RoundDouble(2)))
                        Case Else
                            resultDictionary.Add(item.Key, item.jsonItemAsString)
                    End Select
                Catch ex As Exception
                    Stop
                    'Throw
                End Try
            Next
        End If
            Return resultDictionary
    End Function

End Module
