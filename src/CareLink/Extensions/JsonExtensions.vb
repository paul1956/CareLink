' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Module JsonExtensions
    Private Const Format As String = "yyyy-MM-ddTHH:mm:ss"

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for deserialization. Ignores null values,
    '''  writes numbers as strings, uses case-insensitive property names, and disallows unmapped members.
    ''' </summary>
    Public ReadOnly s_jsonDeserializerOptions As New JsonSerializerOptions() With {
        .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        .NumberHandling = JsonNumberHandling.WriteAsString,
        .PropertyNameCaseInsensitive = True,
        .UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow}

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for serialization with indented output.
    ''' </summary>
    Public ReadOnly s_jsonSerializerOptions As New JsonSerializerOptions With {
        .WriteIndented = True}

    ''' <summary>
    '''  Converts a list of dictionaries representing JSON objects to a list of <see cref="SG"/> objects.
    ''' </summary>
    ''' <param name="json">The list of dictionaries to convert.</param>
    ''' <returns>A list of <see cref="SG"/> objects.</returns>
    <Extension>
    Private Function ToSgList(json As List(Of Dictionary(Of String, String))) As List(Of SG)
        Dim sGs As New List(Of SG)
        Dim yesterday As Date =
            PatientData.LastConduitUpdateServerDateTime.Epoch2PumpDateTime - Eleven55Span
        For index As Integer = 0 To json.Count - 1
            sGs.Add(item:=New SG(json:=json(index), index))
            If sGs.Last.Timestamp.Equals(value:=New DateTime) Then
                sGs.Last.TimestampAsString =
                    If(index = 0,
                       yesterday.RoundDownToMinute().ToStringExact(),
                       (sGs(index:=0).Timestamp + (FiveMinuteSpan * index)).ToStringExact())
            End If
        Next
        Return sGs
    End Function

    ''' <summary>
    '''  Converts a <see cref="Date"/> to a <see langword="String"/> with the specified format.
    '''  Defaults to "yyyy-MM-ddTHH:mm:ss" if no format is provided.
    ''' </summary>
    ''' <param name="d">The date to convert.</param>
    ''' <returns>The formatted date as a string.</returns>
    <Extension>
    Private Function ToStringExact(d As Date) As String
        Return d.ToString(Format, provider:=CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    '''  Converts a <paramref name="jsonArray"/> array to a <see cref="List"/> of objects,
    '''  recursively handling nested arrays and objects.
    ''' </summary>
    ''' <param name="jsonArray">The JsonElement representing a JSON array.</param>
    ''' <returns>A list of objects representing the array elements.</returns>
    <Extension>
    Public Function ConvertJsonArrayToList(jsonArray As JsonElement) As List(Of Object)
        Dim result As New List(Of Object)()
        For Each jsonElement As JsonElement In jsonArray.EnumerateArray()
            Select Case jsonElement.ValueKind
                Case JsonValueKind.Object
                    result.Add(item:=ConvertElementToDictionary(jsonElement))
                Case JsonValueKind.Array
                    result.Add(item:=ConvertJsonArrayToList(jsonArray:=jsonElement))
                Case Else
                    result.Add(item:=ConvertJsonValue(jsonElement))
            End Select
        Next

        Return result
    End Function

    ''' <summary>
    '''  Converts a <paramref name="json"/> object to a
    '''  <see cref="Dictionary(Of String, Object)"/>,
    '''  recursively handling nested objects and arrays.
    ''' </summary>
    ''' <param name="json">The JsonElement representing a JSON object.</param>
    ''' <returns>A dictionary representing the JSON object.</returns>
    <Extension>
    Public Function ConvertElementToDictionary(json As JsonElement) _
        As Dictionary(Of String, Object)

        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim result As New Dictionary(Of String, Object)(comparer)

        If json.ValueKind = JsonValueKind.Object Then
            For Each [property] As JsonProperty In json.EnumerateObject()
                Dim key As String = [property].Name
                Select Case [property].Value.ValueKind
                    Case JsonValueKind.Object
                        result.Add(
                            key,
                            value:=ConvertElementToDictionary(json:=[property].Value))
                    Case JsonValueKind.Array
                        result.Add(
                            key,
                            value:=ConvertJsonArrayToList(jsonArray:=[property].Value))
                    Case Else
                        result.Add(
                            key,
                            value:=ConvertJsonValue(jsonElement:=[property].Value))
                End Select
            Next
        End If

        Return result
    End Function

    ''' <summary>
    '''  Converts a <see cref="JsonElement"/> object to a
    '''  <see cref="Dictionary(Of String, Object)"/>,
    '''  recursively handling nested objects and arrays.
    ''' </summary>
    ''' <param name="jsonElement">
    '''  The <see cref="JsonElement"/> representing a JSON object.
    ''' </param>
    ''' <returns>
    '''  A <see cref="Dictionary(Of String, String)"/> representing the JSON object.
    ''' </returns>
    <Extension>
    Public Function ConvertJsonElementToStringDictionary(jsonElement As JsonElement) _
        As Dictionary(Of String, String)

        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim result As New Dictionary(Of String, String)(comparer)

        If jsonElement.ValueKind = JsonValueKind.Object Then
            For Each [property] As JsonProperty In jsonElement.EnumerateObject()
                Dim key As String = [property].Name
                Dim value As String = [property].Value.ToString

                Select Case [property].Value.ValueKind
                    Case JsonValueKind.String
                        result.Add(key, value)
                    Case JsonValueKind.Object
                        result.Add(key, value)
                    Case JsonValueKind.Array
                        result.Add(key, value)
                    Case JsonValueKind.Null
                        result.Add(key, value:="")
                    Case JsonValueKind.Undefined
                        Stop
                        Exit Select
                    Case JsonValueKind.Number
                        result.Add(key, value)
                    Case JsonValueKind.True
                        result.Add(key, value:="True")
                    Case JsonValueKind.False
                        result.Add(key, value:="False")
                    Case Else
                        result.Add(
                            key,
                            value:=ConvertJsonValue(jsonElement:=[property].Value).ToString)
                End Select
            Next
        End If

        Return result
    End Function

    ''' <summary>
    '''  Converts a <see cref="JsonElement"/> to a .NET object based on its value kind.
    ''' </summary>
    ''' <param name="jsonElement">The <see cref="JsonElement"/> to convert.</param>
    ''' <returns>The corresponding .NET object.</returns>
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

    ''' <summary>
    '''  Retrieves a <see langword="Boolean"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="Boolean"/> value if found;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    <Extension>
    Public Function GetBooleanFromJson(item As Marker, key As String) As Boolean
        Dim value As Object = Nothing
        Dim result As Boolean = False
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.True
                    Return True
                Case JsonValueKind.False
                    Return False
                Case Else
                    Stop
            End Select
        Else
            Return Nothing
        End If
        Return result
    End Function

    ''' <summary>
    '''  Retrieves a <see langword="Double"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="Double"/> value if found;
    '''  otherwise, <see cref="Double.NaN"/>.
    ''' </returns>
    <Extension>
    Public Function GetDoubleFromJson(item As Marker, key As String) As Double
        Dim result As Double = Double.NaN
        Dim value As Object = Nothing
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    result = element.GetString.ParseDoubleInvariant
                Case JsonValueKind.Number
                    result = element.GetDouble
                Case Else
                    Stop
            End Select
        Else
            Stop
        End If
        Return result
    End Function

    ''' <summary>
    '''  Retrieves an <see langword="Integer"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="Integer"/> value if found;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    <Extension>
    Public Function GetIntegerFromJson(item As Marker, key As String) As Integer
        Dim value As Object = Nothing
        Dim result As Integer = 0
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    result = Integer.Parse(element.GetString)
                Case JsonValueKind.Number
                    result = element.GetInt32
                Case Else
                    Stop
            End Select
        Else
            Return Nothing
        End If
        Return result
    End Function

    ''' <summary>
    '''  Retrieves a <see langword="Single"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    '''  Optionally rounds the value to a specified number of <paramref name="digits"/>.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <param name="digits">The number of decimal digits to round to. Use -1 for no rounding.</param>
    ''' <param name="considerValue">Whether to consider the value when rounding.</param>
    ''' <returns>
    '''  The <see langword="Single"/> value if found;
    '''  otherwise, <see cref="Single.NaN"/>.
    ''' </returns>
    <Extension>
    Public Function GetSingleFromJson(
        item As Marker,
        key As String,
        Optional digits As Integer = -1,
        Optional considerValue As Boolean = False) As Single

        Dim value As Object = Nothing
        Dim result As Single = Single.NaN
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Select Case True
                Case TypeOf value Is JsonElement
                    Dim element As JsonElement = CType(value, JsonElement)
                    Select Case element.ValueKind
                        Case JsonValueKind.String
                            result = element.GetString.ParseSingleInvariant
                        Case JsonValueKind.Number
                            result = element.GetSingle
                        Case Else
                            Stop
                            Return result
                    End Select
                Case TypeOf value Is String
                    result = CStr(value).ParseSingleInvariant
                Case Else
                    Stop
            End Select

            If digits = -1 Then Return result
            Return If(digits = 3,
                      result.RoundTo025(),
                      result.RoundToSingle(digits, considerValue))
        Else
            Return Single.NaN
        End If
    End Function

    ''' <summary>
    '''  Retrieves a <see langword="String"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="String"/> value if found;
    '''  otherwise, <see cref="String.Empty"/>.
    ''' </returns>
    <Extension>
    Public Function GetStringFromJson(item As Marker, key As String) As String
        Dim value As Object = Nothing
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    Return element.GetString
                Case JsonValueKind.Undefined
                    Stop
                    Return ""
                Case JsonValueKind.Object
                    Stop
                    Return ""
                Case JsonValueKind.Array
                    Stop
                    Return ""
                Case JsonValueKind.Number
                    Return element.ToString
                Case JsonValueKind.True
                    Return "True"
                Case JsonValueKind.False
                    Return "False"
                Case JsonValueKind.Null
                    Return ""
            End Select
        End If
        Return String.Empty
    End Function

    ''' <summary>
    '''  Determines whether a <see cref="JsonValueKind"/> is
    '''  <see cref="JsonValueKind.Null"/> or
    '''  <see cref="JsonValueKind.Undefined"/>.
    ''' </summary>
    ''' <param name="kind">The <see cref="JsonValueKind"/> to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the kind is Null or Undefined;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsNullOrUndefined(kind As JsonValueKind) As Boolean
        Return kind = JsonValueKind.Null OrElse kind = JsonValueKind.Undefined
    End Function

    ''' <summary>
    '''  Converts a JSON item (key-value pair) to its <see langword="String"/> representation.
    ''' </summary>
    ''' <param name="item">The key-value pair to convert.</param>
    ''' <returns>The <see langword="String"/> representation of the item's value.</returns>
    <Extension>
    Public Function jsonItemAsString(item As KeyValuePair(Of String, Object)) As String
        Dim itemAsElement As JsonElement = CType(item.Value, JsonElement)
        Dim valueAsString As String = itemAsElement.ToString
        Select Case itemAsElement.ValueKind
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

    ''' <summary>
    '''  Converts a JSON string to a <see cref="Dictionary(Of String, String)"/>.
    ''' </summary>
    ''' <param name="json">The JSON string to convert.</param>
    ''' <returns>
    '''  A <see cref="Dictionary"/> with string values representing the JSON object.
    ''' </returns>
    Public Function JsonToDictionary(json As String) As Dictionary(Of String, String)
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim resultDictionary As New Dictionary(Of String, String)(comparer)
        If String.IsNullOrWhiteSpace(value:=json) Then
            Return resultDictionary
        End If

        Dim item As KeyValuePair(Of String, Object)
        Dim rawJsonData As List(Of KeyValuePair(Of String, Object)) =
            JsonSerializer.Deserialize(Of Dictionary(Of String, Object)) _
                (json, options:=s_jsonDeserializerOptions).ToList()
        For Each item In rawJsonData
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, value:=Nothing)
                Continue For
            End If
            resultDictionary.Add(item.Key, value:=item.jsonItemAsString)
        Next
        Return resultDictionary
    End Function

    ''' <summary>
    '''  Converts a JSON string representing an array of objects
    '''  to a <see cref="List(Of Dictionary(Of String, String)"/>.
    ''' </summary>
    ''' <param name="json">The JSON string to convert.</param>
    ''' <returns>
    '''  A <see cref="List(Of Dictionary(Of String, String)"/> representing
    '''  the JSON objects.
    ''' </returns>
    Public Function JsonToDictionaryList(json As String) As List(Of Dictionary(Of String, String))
        Dim resultListOfDictionary As New List(Of Dictionary(Of String, String))
        If String.IsNullOrWhiteSpace(value:=json) Then
            Return resultListOfDictionary
        End If

        Dim jsonList As List(Of Dictionary(Of String, Object)) =
            JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object))) _
                (json, options:=s_jsonDeserializerOptions)

        For Each e As IndexClass(Of Dictionary(Of String, Object)) In jsonList.WithIndex
            Dim item As New Dictionary(Of String, String)(comparer:=StringComparer.OrdinalIgnoreCase)
            Dim defaultTime As Date = PumpNow() - Eleven55Span
            Dim index As Integer = -1
            For Each e1 As IndexClass(Of KeyValuePair(Of String, Object)) In e.Value.WithIndex
                If e1.Value.Value Is Nothing Then
                    item.Add(e1.Value.Key, value:=Nothing)
                ElseIf e1.Value.Key = "index" Then
                    index = CInt(e1.Value.jsonItemAsString)
                    item.Add(e1.Value.Key, value:=e1.Value.jsonItemAsString)
                ElseIf e1.Value.Key = "sg" Then
                    item.Add(e1.Value.Key, value:=e1.Value.ScaleSgToString)
                ElseIf e1.Value.Key = "dateTime" Then
                    Dim d As Date = CType(e1.Value.Value, JsonElement).GetDateTime()

                    ' Prevent Crash but not valid data
                    If d.Year <= 2001 AndAlso index >= 0 Then
                        item.Add(e1.Value.Key,
                        value:=s_sgRecords(index).Timestamp.ToStringExact)
                    Else
                        item.Add(e1.Value.Key, value:=ToShortDateString(d))
                    End If
                Else
                    item.Add(e1.Value.Key, value:=e1.Value.jsonItemAsString())
                End If
            Next

            resultListOfDictionary.Add(item)
        Next
        Return resultListOfDictionary
    End Function

    ''' <summary>
    '''  Converts a JSON string representing an array of objects to a <see cref="List(Of SG)"/>.
    ''' </summary>
    ''' <param name="json">The JSON string to convert.</param>
    ''' <returns>A <see cref="List"/> of <see cref="SG"/> objects.</returns>
    Public Function JsonToListOfSgs(json As String) As List(Of SG)
        Dim jsonList As List(Of Dictionary(Of String, Object)) =
            JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object))) _
                (json, options:=s_jsonDeserializerOptions)
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        For Each e As IndexClass(Of Dictionary(Of String, Object)) In jsonList.WithIndex
            Dim resultDictionary As New Dictionary(Of String, String)(comparer)
            For Each item As KeyValuePair(Of String, Object) In e.Value
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, value:=Nothing)
                ElseIf item.Key = "sg" Then
                    resultDictionary.Add(item.Key, value:=item.ScaleSgToString)
                Else
                    resultDictionary.Add(item.Key, value:=item.jsonItemAsString)
                End If
            Next

            resultDictionaryArray.Add(resultDictionary)
        Next
        Return resultDictionaryArray.ToSgList()
    End Function

    ''' <summary>
    '''  Loads indexed items from a JSON string into a <see cref="Dictionary(Of String, String)"/>.
    '''  Handles special cases for certain keys and manages time zone information.
    ''' </summary>
    ''' <param name="json">The JSON string to load.</param>
    ''' <returns>
    '''  A <see cref="Dictionary(Of String, String)"/> with
    '''  <see langword="String"/> values representing the indexed items.
    ''' </returns>
    Public Function LoadIndexedItems(json As String) As Dictionary(Of String, String)
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim resultDictionary As New Dictionary(Of String, String)(comparer)
        If String.IsNullOrWhiteSpace(value:=json) Then
            Return resultDictionary
        End If
        Dim item As KeyValuePair(Of String, Object)
        Dim options As JsonSerializerOptions = s_jsonDeserializerOptions
        Dim rawJsonData As List(Of KeyValuePair(Of String, Object)) =
            JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json, options).ToList()
        For Each item In rawJsonData
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, value:=Nothing)
                Continue For
            End If
            Try
                Select Case item.Key
                    Case "additionalInfo"
                        Dim additionalInfo As Dictionary(Of String, Object) =
                            JsonSerializer.Deserialize(Of Dictionary(Of String, Object)) _
                                (json:=item.jsonItemAsString, options)
                        For Each kvp As KeyValuePair(Of String, Object) In additionalInfo
                            resultDictionary.Add(kvp.Key, value:=kvp.Value.ToString)
                        Next
                    Case NameOf(ServerDataIndexes.clientTimeZoneName)
                        If s_useLocalTimeZone Then
                            PumpTimeZoneInfo = TimeZoneInfo.Local
                        Else
                            PumpTimeZoneInfo =
                                CalculateTimeZone(timeZoneName:=item.Value.ToString)
                            Dim text As String
                            Dim messageButtons As MessageBoxButtons
                            If PumpTimeZoneInfo Is Nothing Then
                                Dim value As String = item.Value?.ToString
                                If String.IsNullOrWhiteSpace(value) Then
                                    text =
                                        "Your pump appears To be off-line, some " &
                                        "values will be wrong do you want to continue?" &
                                        $" If you select OK '{TimeZoneInfo.Local.Id}'" &
                                        " will be used as you local time and you will" &
                                        " not be prompted further. Cancel will Exit."
                                    messageButtons = MessageBoxButtons.OKCancel
                                Else
                                    text = $"Your pump TimeZone '{item.Value}' " &
                                        "is not recognized, do you want to exit?" &
                                        " If you select No permanently use" &
                                        $" '{TimeZoneInfo.Local.Id}''? If you select" &
                                        $" Yes '{TimeZoneInfo.Local.Id}'" &
                                        $" will be used and you will not be prompted" &
                                        " further. No will use" &
                                        $" '{TimeZoneInfo.Local.Id}' until you restart" &
                                        " program. Cancel will exit program." &
                                        " Please open an issue and provide the name" &
                                        $" '{item.Value}'. After selecting 'Yes'" &
                                        " you can change the behavior under" &
                                        " the Options Menu."
                                    messageButtons = MessageBoxButtons.YesNoCancel
                                End If
                                Dim result As DialogResult = MessageBox.Show(
                                    text,
                                    caption:="TimeZone Unknown",
                                    buttons:=messageButtons,
                                    icon:=MessageBoxIcon.Question)

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
                        resultDictionary.Add(item.Key, value:=item.jsonItemAsString)
                    Case "Sg",
                         "sg",
                         NameOf(ServerDataIndexes.averageSG),
                         NameOf(ServerDataIndexes.sgBelowLimit),
                         NameOf(ServerDataIndexes.averageSGFloat)

                        resultDictionary.Add(item.Key, value:=item.ScaleSgToString())
                    Case Else
                        resultDictionary.Add(item.Key, value:=item.jsonItemAsString)
                End Select
            Catch ex As Exception
                Stop
                'Throw
            End Try
        Next
        Return resultDictionary
    End Function

    ''' <summary>
    '''  Converts a JsonElement to a string representation of the value,
    '''  scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="item">The JsonElement to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSgToString(item As JsonElement) As String
        Dim itemAsSingle As Single
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Select Case item.ValueKind
            Case JsonValueKind.String
                itemAsSingle = Single.Parse(item.GetString(), provider)
            Case JsonValueKind.Null
                Return String.Empty
            Case JsonValueKind.Undefined
                Return String.Empty
            Case JsonValueKind.Number
                itemAsSingle = item.GetSingle
            Case Else
                Stop
        End Select

        Dim s As Single =
            If(NativeMmolL,
               (itemAsSingle / MmolLUnitsDivisor).RoundToSingle(digits:=GetPrecisionDigits()),
               itemAsSingle)
        Return s.ToString(provider)
    End Function

End Module
