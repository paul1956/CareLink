' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports DocumentFormat.OpenXml

Public Module JsonExtensions

    Public ReadOnly s_jsonDeserializerOptions As New JsonSerializerOptions() With {
        .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        .NumberHandling = JsonNumberHandling.WriteAsString,
        .PropertyNameCaseInsensitive = True,
        .UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow}

    Public ReadOnly s_jsonSerializerOptions As New JsonSerializerOptions With {.WriteIndented = True}

    <Extension>
    Private Function ToSgList(innerJson As List(Of Dictionary(Of String, String))) As List(Of SG)
        Dim sGs As New List(Of SG)
        For i As Integer = 0 To innerJson.Count - 1
            sGs.Add(New SG(innerJson(i), i))
            With sGs.Last
                If .Timestamp.Equals(New DateTime) Then
                    .Timestamp = If(i = 0,
                                   (s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime - New TimeSpan(23, 55, 0)).RoundDownToMinute(),
                                   sGs(0).Timestamp + (s_05MinuteSpan * i)
                                  )
                End If
            End With
        Next
        Return sGs
    End Function

    <Extension>
    Public Function CleanUserData(cleanRecentData As Dictionary(Of String, Object)) As String
        If cleanRecentData Is Nothing Then Return ""
        cleanRecentData("firstName") = "First"
        cleanRecentData("lastName") = "Last"
        cleanRecentData("medicalDeviceSerialNumber") = "NG1234567H"
        Return JsonSerializer.Serialize(cleanRecentData, s_jsonSerializerOptions)
    End Function

    <Extension>
    Public Function ConvertJsonArrayToList(jsonElement As JsonElement) As List(Of Object)
        Dim result As New List(Of Object)()
        For Each element As JsonElement In jsonElement.EnumerateArray()
            Select Case element.ValueKind
                Case JsonValueKind.Object
                    result.Add(ConvertJsonElementToDictionary(element))
                Case JsonValueKind.Array
                    result.Add(ConvertJsonArrayToList(element))
                Case Else
                    result.Add(ConvertJsonValue(element))
            End Select
        Next

        Return result
    End Function

    <Extension>
    Public Function ConvertJsonElementToDictionary(jsonElement As JsonElement) As Dictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)

        If jsonElement.ValueKind = JsonValueKind.Object Then
            For Each [property] As JsonProperty In jsonElement.EnumerateObject()
                Select Case [property].Value.ValueKind
                    Case JsonValueKind.Object
                        result.Add([property].Name, ConvertJsonElementToDictionary([property].Value))
                    Case JsonValueKind.Array
                        result.Add([property].Name, ConvertJsonArrayToList([property].Value))
                    Case Else
                        result.Add([property].Name, ConvertJsonValue([property].Value))
                End Select
            Next
        End If

        Return result
    End Function

    <Extension>
    Public Function ConvertJsonElementToStringDictionary(jsonElement As JsonElement, expandSubElements As Boolean) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

        If jsonElement.ValueKind = JsonValueKind.Object Then
            For Each [property] As JsonProperty In jsonElement.EnumerateObject()
                Select Case [property].Value.ValueKind
                    Case JsonValueKind.String
                        result.Add([property].Name, [property].Value.ToString)
                    Case JsonValueKind.Object
                        If expandSubElements Then
                            result.Add([property].Name, ConvertJsonElementToDictionary([property].Value).ToString)
                        Else
                            result.Add([property].Name, [property].Value.ToString)
                        End If
                    Case JsonValueKind.Array
                        If expandSubElements Then
                            result.Add([property].Name, ConvertJsonArrayToList([property].Value).ToString)
                        Else
                            result.Add([property].Name, [property].Value.ToString)
                        End If
                    Case JsonValueKind.Null
                        result.Add([property].Name, "")
                    Case JsonValueKind.Undefined
                        Stop
                        Exit Select
                    Case JsonValueKind.Number
                        result.Add([property].Name, [property].Value.ToString)
                    Case JsonValueKind.True
                        result.Add([property].Name, "True")
                    Case JsonValueKind.False
                        result.Add([property].Name, "False")
                    Case Else
                        result.Add([property].Name, ConvertJsonValue([property].Value).ToString)
                End Select
            Next
        End If

        Return result
    End Function

    <Extension>
    Public Function ConvertJsonValue(jsonElement As JsonElement) As Object
        Select Case jsonElement.ValueKind
            Case JsonValueKind.String
                Return jsonElement.GetString()
            Case JsonValueKind.Number
                Return jsonElement.GetDecimal()
            Case JsonValueKind.True
                Return True
            Case JsonValueKind.False
                Return False
            Case JsonValueKind.Null
                Return Nothing
            Case Else
                Return jsonElement.GetRawText()
        End Select
    End Function

    <Extension>
    Public Function GetBooleanValueFromJson(markerEntry As Marker, fieldName As String) As Boolean
        Dim obj As Object = Nothing
        Dim value As Boolean = False
        If markerEntry.Data.DataValues.TryGetValue(fieldName, obj) Then
            Dim element As JsonElement = CType(obj, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.True
                    Return True
                Case JsonValueKind.False
                    Return False
                Case Else
                    Stop
            End Select
        End If
        Return value
    End Function

    <Extension>
    Public Function GetDoubleValueFromJson(markerEntry As Marker, fieldName As String) As Double
        Dim obj As Object = Nothing
        Dim value As Double = Double.NaN
        If markerEntry.Data.DataValues.TryGetValue(fieldName, obj) Then
            Dim element As JsonElement = CType(obj, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    value = Double.Parse(element.GetString)
                Case JsonValueKind.Number
                    value = element.GetDouble
                Case Else
                    Stop
            End Select
        End If
        Return value
    End Function

    <Extension>
    Public Function GetIntegerValueFromJson(markerEntry As Marker, fieldName As String) As Integer
        Dim obj As Object = Nothing
        Dim value As Integer = 0
        If markerEntry.Data.DataValues.TryGetValue(fieldName, obj) Then
            Dim element As JsonElement = CType(obj, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    value = Integer.Parse(element.GetString)
                Case JsonValueKind.Number
                    value = element.GetInt32
                Case Else
                    Stop
            End Select
        End If
        Return value
    End Function

    <Extension>
    Public Function GetSingleValueFromJson(markerEntry As Marker, fieldName As String, Optional decimalDigits As Integer = -1) As Single
        Dim obj As Object = Nothing
        Dim value As Single = Single.NaN
        If markerEntry.Data.DataValues.TryGetValue(fieldName, obj) Then
            Dim element As JsonElement = CType(obj, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    value = Single.Parse(element.GetString)
                Case JsonValueKind.Number
                    value = element.GetSingle
                Case Else
                    Stop
                    Return value
            End Select
            Return If(decimalDigits = 3, value.RoundTo025, value.RoundSingle(decimalDigits, False))
        Else
            Return Single.NaN
        End If
    End Function

    <Extension>
    Public Function GetStringValueFromJson(markerEntry As Marker, fieldName As String) As String
        Dim obj As Object = Nothing
        If markerEntry.Data.DataValues.TryGetValue(fieldName, obj) Then
            Dim element As JsonElement = CType(obj, JsonElement)
            If element.ValueKind = JsonValueKind.String Then
                Return element.GetString
            Else
                Stop
            End If
        End If
        Return String.Empty
    End Function

    <Extension>
    Public Function IsNullOrUndefined(kind As JsonValueKind) As Boolean
        Return kind = JsonValueKind.Null OrElse kind = JsonValueKind.Undefined
    End Function

    <Extension>
    Public Function jsonItemAsString(item As KeyValuePair(Of String, Object)) As String
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

    Public Function JsonToDictionary(jsonString As String) As Dictionary(Of String, String)
        Dim resultDictionary As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        If String.IsNullOrWhiteSpace(jsonString) Then
            Return resultDictionary
        End If

        Dim item As KeyValuePair(Of String, Object)
        Dim rawJsonData As List(Of KeyValuePair(Of String, Object)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString, s_jsonDeserializerOptions).ToList()
        For Each item In rawJsonData
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, Nothing)
                Continue For
            End If
            resultDictionary.Add(item.Key, item.jsonItemAsString)
        Next
        Return resultDictionary
    End Function

    Public Function JsonToLisOfDictionary(value As String) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        If String.IsNullOrWhiteSpace(value) Then
            Return resultDictionaryArray
        End If

        Dim jsonList As List(Of Dictionary(Of String, Object)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, s_jsonDeserializerOptions)
        For Each e As IndexClass(Of Dictionary(Of String, Object)) In jsonList.WithIndex
            Dim resultDictionary As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            Dim defaultTime As Date = PumpNow() - New TimeSpan(23, 55, 0)
            Dim recordIndex As Integer = -1
            For Each e1 As IndexClass(Of KeyValuePair(Of String, Object)) In e.Value.WithIndex
                Dim item As KeyValuePair(Of String, Object) = e1.Value
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                ElseIf item.Key = "index" Then
                    recordIndex = CInt(item.jsonItemAsString)
                    resultDictionary.Add(item.Key, item.jsonItemAsString)
                ElseIf item.Key = "sg" Then
                    resultDictionary.Add(item.Key, item.ScaleSgToString)
                ElseIf item.Key = "dateTime" Then
                    Dim d As Date = item.Value.ToString.ParseDate(item.Key)

                    ' Prevent Crash but not valid data
                    If d.Year <= 2001 AndAlso recordIndex >= 0 Then
                        resultDictionary.Add(item.Key, s_listOfSgRecords(recordIndex).Timestamp.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
                    Else
                        resultDictionary.Add(item.Key, item.jsonItemAsString)
                    End If
                Else
                    resultDictionary.Add(item.Key, item.jsonItemAsString)
                End If
            Next

            resultDictionaryArray.Add(resultDictionary)
        Next
        Return resultDictionaryArray
    End Function

    Public Function JsonToLisOfSgs(value As String) As List(Of SG)
        If String.IsNullOrWhiteSpace(value) Then
            Return New List(Of SG)
        End If

        Dim jsonList As List(Of Dictionary(Of String, Object)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, s_jsonDeserializerOptions)
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        For Each e As IndexClass(Of Dictionary(Of String, Object)) In jsonList.WithIndex
            Dim resultDictionary As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            Dim defaultTime As Date = PumpNow() - New TimeSpan(23, 55, 0)
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
        Return resultDictionaryArray.ToSgList()
    End Function

    Public Function LoadIndexedItems(jsonString As String) As Dictionary(Of String, String)
        Dim resultDictionary As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        If String.IsNullOrWhiteSpace(jsonString) Then
            Return resultDictionary
        End If
        Dim item As KeyValuePair(Of String, Object)
        Dim rawJsonData As List(Of KeyValuePair(Of String, Object)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString, s_jsonDeserializerOptions).ToList()
        For Each item In rawJsonData
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, Nothing)
                Continue For
            End If
            Try
                Select Case item.Key
                    Case NameOf(ItemIndexes.bgUnits)
                        If Not UnitsStrings.TryGetValue(item.Value.ToString(), SgUnitsNativeString) Then
                            Dim averageSGFloatAsString As String = rawJsonData(ItemIndexes.averageSGFloat).jsonItemAsString
                            SgUnitsNativeString = If(averageSGFloatAsString.ParseSingle(1) > 40,
                                                    "mg/dl",
                                                    "mmol/L"
                                                    )
                        End If
                        NativeMmolL = SgUnitsNativeString <> "mg/dl"
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
                                    message = $"Your pump appears To be off-line, some values will be wrong do you want to continue? If you select OK '{TimeZoneInfo.Local.Id}' will be used as you local time and you will not be prompted further. Cancel will Exit."
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
                        s_timeFormat = item.Value.ToString
                        s_timeWithMinuteFormat = If(s_timeFormat = "HR_12", TimeFormatTwelveHourWithMinutes, TimeFormatMilitaryWithMinutes)
                        s_timeWithoutMinuteFormat = If(s_timeFormat = "HR_12", TimeFormatTwelveHourWithoutMinutes, TimeFormatMilitaryWithoutMinutes)
                        resultDictionary.Add(item.Key, item.jsonItemAsString)
                    Case "Sg", "sg", NameOf(ItemIndexes.averageSG), NameOf(ItemIndexes.sgBelowLimit), NameOf(ItemIndexes.averageSGFloat)
                        resultDictionary.Add(item.Key, item.ScaleSgToString())
                    Case Else
                        resultDictionary.Add(item.Key, item.jsonItemAsString)
                End Select
            Catch ex As Exception
                Stop
                'Throw
            End Try
        Next
        Return resultDictionary
    End Function

End Module
