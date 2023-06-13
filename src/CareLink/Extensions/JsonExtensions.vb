' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
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
        If cleanRecentData Is Nothing Then Return ""
        cleanRecentData("firstName") = "First"
        cleanRecentData("lastName") = "Last"
        cleanRecentData("medicalDeviceSerialNumber") = "NG1234567H"
        Return JsonSerializer.Serialize(cleanRecentData, New JsonSerializerOptions)
    End Function

    Public Function LoadList(value As String) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        If String.IsNullOrWhiteSpace(value) Then
            Return resultDictionaryArray
        End If

        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        For Each e As IndexClass(Of Dictionary(Of String, Object)) In JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, options).WithIndex
            Dim resultDictionary As New Dictionary(Of String, String)
            For Each item As KeyValuePair(Of String, Object) In e.Value
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                ElseIf item.Key = "sg" Then
                    resultDictionary.Add(item.Key, item.ScaleSgToString)
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
        If String.IsNullOrWhiteSpace(value) Then
            Return resultDictionary
        End If
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}
        Dim item As KeyValuePair(Of String, Object)
        If Not String.IsNullOrWhiteSpace(value) Then
            Dim rawJsonData As List(Of KeyValuePair(Of String, Object)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(value, options).ToList()
            For Each item In rawJsonData
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                    Continue For
                End If
                Try
                    Select Case item.Key
                        Case NameOf(ItemIndexes.bgUnits)
                            Dim bgUnits As String = item.Value.ToString()
                            If Not s_unitsStrings.TryGetValue(bgUnits, BgUnitsNativeString) Then
                                Dim averageSGFloatAsString As String = rawJsonData(ItemIndexes.averageSGFloat).jsonItemAsString
                                If averageSGFloatAsString.ParseSingle(1) > 40 Then
                                    BgUnitsNativeString = "mg/dl"
                                    BgUnitsAlternativeString = "mmol/L"
                                Else
                                    BgUnitsNativeString = "mmol/L"
                                    BgUnitsAlternativeString = "mg/dL"
                                End If
                            End If
                            nativeMmolL = BgUnitsNativeString <> "mg/dl"
                            resultDictionary.Add(item.Key, item.jsonItemAsString)
                        Case NameOf(ItemIndexes.clientTimeZoneName)
                            If s_useLocalTimeZone Then
                                PumpTimeZoneInfo = TimeZoneInfo.Local
                            Else
                                PumpTimeZoneInfo = CalculateTimeZone(item.Value.ToString)
                                Dim message As String
                                Dim messageButtons As MessageBoxButtons
                                If PumpTimeZoneInfo Is Nothing Then
                                    If String.IsNullOrWhiteSpace(item.Value.ToString) Then
                                        message = $"Your pump appears to be off-line, some values will be wrong do you want to continue? If you select OK '{TimeZoneInfo.Local.Id}' will be used as you local time and you will not be prompted further. Cancel will Exit."
                                        messageButtons = MessageBoxButtons.OKCancel
                                    Else
                                        message = $"Your pump TimeZone '{item.Value}' is not recognized, do you want to exit? If you select No permanently use '{TimeZoneInfo.Local.Id}''? If you select Yes '{TimeZoneInfo.Local.Id}' will be used and you will not be prompted further. No will use '{TimeZoneInfo.Local.Id}' until you restart program. Cancel will exit program. Please open an issue and provide the name '{item.Value}'. After selecting 'Yes' you can change the behavior under the Options Menu."
                                        messageButtons = MessageBoxButtons.YesNoCancel
                                    End If
                                    Dim result As DialogResult = MessageBox.Show(message, "TimeZone Unknown",
                                                                                 messageButtons,
                                                                                 MessageBoxIcon.Question)
                                    s_useLocalTimeZone = True
                                    PumpTimeZoneInfo = TimeZoneInfo.Local
                                    Select Case result
                                        Case DialogResult.Yes
                                            My.Settings.UseLocalTimeZone = True
                                        Case DialogResult.Cancel
                                            Form1.Close()
                                    End Select
                                End If
                            End If
                            resultDictionary.Add(item.Key, item.jsonItemAsString)
                        Case NameOf(ItemIndexes.timeFormat)
                            Dim internalTimeFormat As String = item.Value.ToString
                            s_timeWithMinuteFormat = If(internalTimeFormat = "HR_12", TimeFormatTwelveHourWithMinutes, TimeFormatMilitaryWithMinutes)
                            s_timeWithoutMinuteFormat = If(internalTimeFormat = "HR_12", TimeFormatTwelveHourWithoutMinutes, TimeFormatMilitaryWithoutMinutes)
                            resultDictionary.Add(item.Key, item.jsonItemAsString)
                        Case "Sg", "sg", NameOf(ItemIndexes.averageSGFloat), NameOf(ItemIndexes.averageSG), NameOf(ItemIndexes.sgBelowLimit)
                            resultDictionary.Add(item.Key, item.ScaleSgToString())
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
