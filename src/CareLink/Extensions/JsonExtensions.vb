' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Module JsonExtensions

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for deserialization.
    '''  Ignores null values, writes numbers as strings,
    '''  uses case-insensitive property names, and disallows unmapped members.
    ''' </summary>
    Public ReadOnly s_jsonDesterilizeOptions As New JsonSerializerOptions() With {
        .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        .NumberHandling = JsonNumberHandling.WriteAsString,
        .PropertyNameCaseInsensitive = True,
        .UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow}

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for serialization with indented output.
    ''' </summary>
    Public ReadOnly s_jsonSerializerOptions As New JsonSerializerOptions With {.WriteIndented = True}

    ''' <summary>
    '''  Converts a list of dictionaries representing JSON objects
    '''  to a list of <see cref="SG"/> objects.
    ''' </summary>
    ''' <param name="json">The list of dictionaries to convert.</param>
    ''' <returns>A list of <see cref="SG"/> objects.</returns>
    <Extension>
    Private Function ToSgList(json As List(Of Dictionary(Of String, String))) As List(Of SG)
        Dim sGs As New List(Of SG)
        Dim yesterday As Date = PatientData.LastConduitUpdateServerDateTime.Epoch2PumpDateTime - Eleven55Span
        For index As Integer = 0 To json.Count - 1
            sGs.Add(item:=New SG(json:=json(index), index))
            If sGs.Last.Timestamp.Equals(value:=New DateTime) Then
                sGs.Last.TimestampAsString = If(index = 0,
                                                yesterday.RoundDownToMinute().ToStringExact(),
                                                (sGs(index:=0).Timestamp + (FiveMinuteSpan * index)).ToStringExact())
            End If
        Next
        Return sGs
    End Function

    ''' <summary>
    '''  Loads indexed items from a JSON string
    '''  into a <see cref="Dictionary(Of String, String)"/>.
    '''  Handles special cases for certain keys and manages time zone information.
    ''' </summary>
    ''' <param name="json">The JSON string to load.</param>
    ''' <returns>
    '''  A <see cref="Dictionary(Of String, String)"/> with
    '''  <see langword="String"/> values representing the indexed items.
    ''' </returns>
    Public Function DeserializeJsonAsDictionary(json As String) As Dictionary(Of String, String)
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim resultDictionary As New Dictionary(Of String, String)(comparer)
        If IsNullOrWhiteSpace(value:=json) Then
            Return resultDictionary
        End If
        Dim item As KeyValuePair(Of String, Object)
        Dim options As JsonSerializerOptions = s_jsonDesterilizeOptions
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
                        Dim jsonItem As String = item.DeserializeJsonAsString
                        Dim additionalInfo As Dictionary(Of String, Object) =
                            JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json:=jsonItem, options)
                        For Each kvp As KeyValuePair(Of String, Object) In additionalInfo
                            resultDictionary.Add(kvp.Key, value:=kvp.Value.ToString)
                        Next
                    Case NameOf(ServerDataEnum.clientTimeZoneName)
                        If s_useLocalTimeZone Then
                            PumpTimeZoneInfo = TimeZoneInfo.Local
                        Else
                            PumpTimeZoneInfo = CalculateTimeZone(timeZoneName:=item.Value.ToString)
                            Dim text As String
                            Dim messageButtons As MessageBoxButtons
                            If PumpTimeZoneInfo Is Nothing Then
                                Dim value As String = item.Value?.ToString
                                If IsNullOrWhiteSpace(value) Then
                                    text = "Your pump appears To be off-line, some " &
                                           "values will be wrong do you want to continue? " &
                                           $"If you select OK '{TimeZoneInfo.Local.Id}' " &
                                           "will be used as you local time and you will " &
                                           "not be prompted further. Cancel will Exit."
                                    messageButtons = MessageBoxButtons.OKCancel
                                Else
                                    text = $"Your pump TimeZone '{item.Value}' " &
                                           "is not recognized, do you want to exit? " &
                                           "If you select No permanently use " &
                                           $"'{TimeZoneInfo.Local.Id}''? If you select " &
                                           $"Yes '{TimeZoneInfo.Local.Id}' " &
                                           "will be used and you will not be prompted further. No will use " &
                                           $"'{TimeZoneInfo.Local.Id}' until you restart " &
                                           "program. Cancel will exit program. " &
                                           "Please open an issue and provide the name " &
                                           $"'{item.Value}'. After selecting 'Yes' " &
                                           "you can change the behavior under the Options Menu."
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
                        resultDictionary.Add(item.Key, value:=item.DeserializeJsonAsString)
                    Case "Sg",
                         "sg",
                         NameOf(ServerDataEnum.averageSG),
                         NameOf(ServerDataEnum.sgBelowLimit),
                         NameOf(ServerDataEnum.averageSGFloat)

                        resultDictionary.Add(item.Key, value:=item.ScaleSg())
                    Case Else
                        resultDictionary.Add(item.Key, value:=item.DeserializeJsonAsString)
                End Select
            Catch ex As Exception
                Stop
                'Throw
            End Try
        Next
        Return resultDictionary
    End Function

    ''' <summary>
    '''  Converts a JSON item (key-value pair) to its <see langword="String"/> representation.
    ''' </summary>
    ''' <param name="item">The key-value pair to convert.</param>
    ''' <returns>The <see langword="String"/> representation of the item's value.</returns>
    <Extension>
    Public Function DeserializeJsonAsString(item As KeyValuePair(Of String, Object)) As String
        Dim itemAsElement As JsonElement = CType(item.Value, JsonElement)
        Dim valueAsString As String = itemAsElement.ToString
        Select Case itemAsElement.ValueKind
            Case JsonValueKind.False
                Return "False"
            Case JsonValueKind.Null
                Return String.Empty
            Case JsonValueKind.Number
                Return valueAsString
            Case JsonValueKind.True
                Return "True"
            Case JsonValueKind.String
        End Select
        Return valueAsString
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
    '''  Converts a JSON string to a <see cref="Dictionary(Of String, String)"/>.
    ''' </summary>
    ''' <param name="json">The JSON string to convert.</param>
    ''' <returns>
    '''  A <see cref="Dictionary"/> with string values representing the JSON object.
    ''' </returns>
    Public Function JsonToDictionary(json As String) As Dictionary(Of String, String)
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim resultDictionary As New Dictionary(Of String, String)(comparer)
        If IsNullOrWhiteSpace(value:=json) Then
            Return resultDictionary
        End If

        Dim item As KeyValuePair(Of String, Object)
        Dim options As JsonSerializerOptions = s_jsonDesterilizeOptions
        Dim rawJsonData As List(Of KeyValuePair(Of String, Object)) =
            JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json, options).ToList()

        For Each item In rawJsonData
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, value:=Nothing)
                Continue For
            End If
            resultDictionary.Add(item.Key, value:=item.DeserializeJsonAsString)
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
        If IsNullOrWhiteSpace(value:=json) Then
            Return resultListOfDictionary
        End If

        Dim options As JsonSerializerOptions = s_jsonDesterilizeOptions
        Dim jsonList As List(Of Dictionary(Of String, Object)) =
            JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(json, options)

        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase

        For Each e As IndexClass(Of Dictionary(Of String, Object)) In jsonList.WithIndex
            Dim item As New Dictionary(Of String, String)(comparer)
            Dim defaultTime As Date = PumpNow() - Eleven55Span
            Dim index As Integer = -1
            For Each e1 As IndexClass(Of KeyValuePair(Of String, Object)) In
                e.Value.WithIndex

                If e1.Value.Value Is Nothing Then
                    item.Add(e1.Value.Key, value:=Nothing)
                ElseIf e1.Value.Key = "index" Then
                    index = CInt(e1.Value.DeserializeJsonAsString)
                    item.Add(e1.Value.Key, value:=e1.Value.DeserializeJsonAsString)
                ElseIf e1.Value.Key = "sg" Then
                    item.Add(e1.Value.Key, value:=e1.Value.ScaleSg)
                ElseIf e1.Value.Key = "dateTime" Then
                    Dim dateValue As Date = CType(e1.Value.Value, JsonElement).GetDateTime()

                    ' Prevent Crash but not valid data
                    If dateValue.Year <= 2001 AndAlso index >= 0 Then
                        item.Add(e1.Value.Key,
                        value:=s_sgRecords(index).Timestamp.ToStringExact)
                    Else
                        item.Add(e1.Value.Key, value:=dateValue.ToShortDateTime())
                    End If
                Else
                    item.Add(e1.Value.Key, value:=e1.Value.DeserializeJsonAsString())
                End If
            Next

            resultListOfDictionary.Add(item)
        Next
        Return resultListOfDictionary
    End Function

    ''' <summary>
    '''  Converts a JSON string representing an array of objects to a
    '''  <see cref="List(Of SG)"/>.
    ''' </summary>
    ''' <param name="json">The JSON string to convert.</param>
    ''' <returns>A <see cref="List"/> of <see cref="SG"/> objects.</returns>
    Public Function JsonToListOfSgs(json As String) As List(Of SG)
        Dim options As JsonSerializerOptions = s_jsonDesterilizeOptions
        Dim jsonList As List(Of Dictionary(Of String, Object)) =
            JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(json, options)
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        For Each e As IndexClass(Of Dictionary(Of String, Object)) In jsonList.WithIndex
            Dim resultDictionary As New Dictionary(Of String, String)(comparer)
            For Each item As KeyValuePair(Of String, Object) In e.Value
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, value:=Nothing)
                ElseIf item.Key = "sg" Then
                    resultDictionary.Add(item.Key, value:=item.ScaleSg)
                Else
                    resultDictionary.Add(item.Key, value:=item.DeserializeJsonAsString)
                End If
            Next

            resultDictionaryArray.Add(resultDictionary)
        Next
        Return resultDictionaryArray.ToSgList()
    End Function

    ''' <summary>
    '''  Converts a <paramref name="jsonArray"/> array to a <see cref="List"/> of objects,
    '''  recursively handling nested arrays and objects.
    ''' </summary>
    ''' <param name="jsonArray">The JsonElement representing a JSON array.</param>
    ''' <returns>A list of objects representing the array elements.</returns>
    <Extension>
    Public Function ToList(jsonArray As JsonElement) As List(Of Object)
        Dim result As New List(Of Object)()
        For Each element As JsonElement In jsonArray.EnumerateArray()
            Select Case element.ValueKind
                Case JsonValueKind.Object
                    result.Add(item:=ToObjectDictionary(element))
                Case JsonValueKind.Array
                    result.Add(item:=ToList(jsonArray:=element))
                Case Else
                    result.Add(item:=ToObject(element))
            End Select
        Next
        Return result
    End Function

    ''' <summary>
    '''  Converts a <see cref="JsonElement"/> to a .NET object based on its value kind.
    ''' </summary>
    ''' <param name="jsonElement">The <see cref="JsonElement"/> to convert.</param>
    ''' <returns>The corresponding .NET object.</returns>
    <Extension>
    Public Function ToObject(jsonElement As JsonElement) As Object
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
    '''  Converts a <paramref name="json"/> object to a
    '''  <see cref="Dictionary(Of String, Object)"/>,
    '''  recursively handling nested objects and arrays.
    ''' </summary>
    ''' <param name="json">The JsonElement representing a JSON object.</param>
    ''' <returns>A dictionary representing the JSON object.</returns>
    <Extension>
    Public Function ToObjectDictionary(json As JsonElement) As Dictionary(Of String, Object)
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim result As New Dictionary(Of String, Object)(comparer)

        If json.ValueKind = JsonValueKind.Object Then
            For Each [property] As JsonProperty In json.EnumerateObject()
                Dim key As String = [property].Name
                Select Case [property].Value.ValueKind
                    Case JsonValueKind.Object
                        result.Add(key, value:=ToObjectDictionary(json:=[property].Value))
                    Case JsonValueKind.Array
                        result.Add(key, value:=ToList(jsonArray:=[property].Value))
                    Case Else
                        result.Add(key, value:=ToObject(jsonElement:=[property].Value))
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
    Public Function ToStringDictionary(jsonElement As JsonElement) As Dictionary(Of String, String)
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
                        result.Add(key, value:=String.Empty)
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
                        result.Add(key, value:=ToObject(jsonElement:=[property].Value).ToString)
                End Select
            Next
        End If

        Return result
    End Function

End Module
