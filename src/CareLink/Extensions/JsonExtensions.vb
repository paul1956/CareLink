' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.RegularExpressions

Public Module JsonExtensions

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for deserialization.
    '''  - Allows reading numbers that are encoded as JSON strings (backwards compatibility with older files).
    '''  - Uses case-insensitive property name matching.
    '''  - Disallows unmapped members to catch unexpected JSON fields.
    ''' </summary>
    Public ReadOnly Property DeserializationOptions As New JsonSerializerOptions() With {
        .NumberHandling = JsonNumberHandling.AllowReadingFromString,
        .PropertyNameCaseInsensitive = True,
        .UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow}

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for serialization with indented output.
    '''  Note: we keep default numeric handling so numbers are written as native JSON numbers
    '''  (not as quoted strings) to support round-trip serialization.
    ''' </summary>
    Public ReadOnly Property SerializerOptions As New JsonSerializerOptions With {
        .WriteIndented = True}

    ''' <summary>
    '''   Handles extended information in a JSON key-value pair and
    '''   adds it to the result dictionary.
    ''' </summary>
    ''' <param name="item">The JSON key-value pair to process.</param>
    ''' <param name="resultDictionary">
    '''  The dictionary to add the processed information to.
    ''' </param>
    Private Sub HandleExtendedInfo(item As KeyValuePair(Of String,
                                   JsonElement), resultDictionary As Dictionary(Of String, String))
        If item.Value.IsEmpty() Then
            Return
        End If
        Select Case item.Value.ValueKind
            Case JsonValueKind.Array
                Stop
            Case JsonValueKind.Object
                Dim jsonItem As String = item.Value.ElementToString()
                Dim extendedInfo As Dictionary(Of String, JsonElement)
                Try
                    extendedInfo = jsonItem.FromJson(Of Dictionary(Of String, JsonElement))()
                Catch ex As Exception
                    Return
                End Try
                For Each kvp As KeyValuePair(Of String, JsonElement) In extendedInfo
                    resultDictionary.Add(key:=$"{item.Key}:{kvp.Key}", value:=kvp.Value.ElementToString())
                Next
            Case JsonValueKind.Undefined
                Stop
                resultDictionary.Add(key:=$"{item.Key}", value:=Nothing)
            Case JsonValueKind.String
                resultDictionary.Add(key:=$"{item.Key}", value:=item.Value.ElementToString())
            Case JsonValueKind.Number
                resultDictionary.Add(key:=$"{item.Key}", value:=item.Value.ElementToString())
            Case JsonValueKind.True
                resultDictionary.Add(key:=$"{item.Key}", value:=item.Value.ElementToString())
            Case JsonValueKind.False
                resultDictionary.Add(key:=$"{item.Key}", value:=item.Value.ElementToString())
            Case JsonValueKind.Null
                Stop
                Exit Select
        End Select
    End Sub

    ''' <summary>
    '''  Converts Dictionary(Of String, JsonElement) to Dictionary(Of String, String)
    ''' </summary>
    ''' <Property name="source">The source dictionary to convert.</Property>
    ''' <returns>A new dictionary with string values.</returns>
    Friend Function ConvertJsonElementDict(source As Dictionary(Of String, JsonElement)) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)(Comparer)

        If source Is Nothing Then
            Return result
        End If

        For Each kvp As KeyValuePair(Of String, JsonElement) In source
            Try
                If kvp.Value.ValueKind = JsonValueKind.String Then
                    result(kvp.Key) = kvp.Value.GetString()
                ElseIf kvp.Value.IsEmpty Then
                    result(kvp.Key) = Nothing
                Else
                    result(kvp.Key) = kvp.Value.ToString()
                End If
            Catch ex As Exception
                ' Handle unexpected conversion issues
                result(kvp.Key) = Nothing
            End Try
        Next

        Return result
    End Function

    ''' <summary>
    ''' Centralized conversion of a JsonElement to a String.
    ''' - Returns String.Empty for Null/Undefined/Empty elements.
    ''' - Returns unwrapped string for JSON strings.
    ''' - Returns raw JSON text for numbers, booleans, objects and arrays.
    ''' - Booleans are returned as "True"/"False" (capitalized) for compatibility with existing callers.
    ''' </summary>
    <Extension>
    Public Function ElementToString(element As JsonElement) As String
        If element.IsEmpty Then
            Return String.Empty
        End If

        Select Case element.ValueKind
            Case JsonValueKind.String
                Return element.GetString()
            Case JsonValueKind.True
                Return "True"
            Case JsonValueKind.False
                Return "False"
            Case JsonValueKind.Number
                Return element.GetRawText()
            Case JsonValueKind.Object, JsonValueKind.Array
                Return element.GetRawText()
            Case Else
                Return element.GetRawText()
        End Select
    End Function

    ''' <summary>
    '''  Converts (Deserializes) a JSON string  to an object of type <typeparamref name="T"/>
    '''  using the provided <see cref="JsonSerializerOptions"/>.
    ''' </summary>
    ''' <typeparam name="T">The type of the object to deserialize.</typeparam>
    ''' <param name="json">The JSON string to deserialize.</param>
    ''' <returns>The deserialized object.</returns>
    ''' <param name="DeserializationOptions"></param>
    <Extension>
    Public Function FromJson(Of T)(json As String) As T

        Try
            Return JsonSerializer.Deserialize(Of T)(json, options:=DeserializationOptions)
        Catch ex As JsonException
            Stop
            LogMessage(message:=$"ERROR: failed deserializing JSON string: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    '''  Converts (Deserializes) a JsonElement to an object of type <typeparamref name="T"/>
    '''  using the module-level <see cref="DeserializationOptions"/>.
    ''' </summary>
    ''' <typeparam name="T">The type of the object to deserialize.</typeparam>
    ''' <param name="element">The JSON string to deserialize.</param>
    ''' <returns>The deserialized object.</returns>
    <Extension>
    Public Function FromJson(Of T)(element As JsonElement) As T
        Try
            Dim elem As T = JsonSerializer.Deserialize(Of T)(element, options:=DeserializationOptions)
            Return elem
        Catch ex As JsonException
            Stop
            LogMessage(message:=$"ERROR: failed deserializing JSON element: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    '''   Checks if a <see cref="JsonElement"/> is empty, null, or undefined.
    ''' </summary>
    ''' <param name="element"> The JsonElement to check. </param>
    ''' <returns>
    '''  True if the element is empty, null, or undefined;
    '''  otherwise, False.
    ''' </returns>
    <Extension>
    Public Function IsEmpty(element As JsonElement) As Boolean
        Select Case element.ValueKind
            Case JsonValueKind.Null,
                 JsonValueKind.Undefined
                Return True
            Case JsonValueKind.Object
                Return Not element.EnumerateObject().Any()
            Case JsonValueKind.Array
                Return element.GetArrayLength() = 0
            Case JsonValueKind.String
                Dim value As String = element.GetString()
                Return String.IsNullOrEmpty(value)
            Case JsonValueKind.Number,
                 JsonValueKind.True,
                 JsonValueKind.False
                Return False
        End Select
        Return False
    End Function

    ''' <summary>
    ''' Checks if a string is valid JSON using System.Text.Json
    ''' </summary>
    ''' <param name="value">The JSON string to validate</param>
    ''' <returns>True if valid JSON, otherwise False</returns>
    <Extension>
    Public Function IsValidJson(value As String) As Boolean
        ' Null or empty strings are not valid JSON
        If String.IsNullOrWhiteSpace(value) Then
            Return False
        End If

        ' Fast pre-check to avoid noisy JsonReaderException first-chance
        ' when the string clearly does not start with a JSON token.
        Dim s As String = value.TrimStart()
        If s.Length = 0 Then
            Return False
        End If
        Dim first As Char = s(index:=0)
        If Not (first = "{"c OrElse
                first = "["c OrElse
                first = """"c OrElse
                first = "-"c OrElse
                Char.IsDigit(c:=first) OrElse
                first = "t"c OrElse
                first = "f"c OrElse
                first = "n"c) Then
            ' Starts with a character that cannot begin valid JSON -> avoid parsing
            Return False
        End If
        If first = "-"c OrElse
            Char.IsDigit(c:=first) Then

            ' Could be a JSON number. First reject common date/time text values
            ' that start with digits (for example: 10/4/2026 7:26:20 AM).
            Dim dt As Date
            If Date.TryParse(s,
                             provider:=Globalization.CultureInfo.CurrentCulture,
                             styles:=Globalization.DateTimeStyles.None,
                             result:=dt) Then
                Return False
            End If

            ' If not a date/time, require strict JSON number syntax.
            Dim numberPattern As String = "^-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?$"
            If Not Regex.IsMatch(input:=s, pattern:=numberPattern) Then
                Return False
            End If
        End If

        Try
            ' Attempt to parse the JSON
            Using doc As JsonDocument = JsonDocument.Parse(value)
                ' If parsing succeeds, it's valid JSON
                Return True
            End Using
        Catch jex As JsonException
            ' JSON is invalid - log the offending input to help diagnose noisy parser errors
            Try
                LogMessage(message:=$"DEBUG: IsValidJson failed parsing input: '{value}'. Error: {jex.Message}")
            Catch
                ' Ensure logging failures do not propagate
            End Try
            Return False
        Catch ex As Exception
            ' Other unexpected errors (e.g., OutOfMemoryException)
            Return False
        End Try
    End Function

    ''' <summary>
    '''  Converts a <paramref name="json"/> object to a
    '''  <see cref="Dictionary(Of String, Object)"/>,
    '''  recursively handling nested objects and arrays.
    ''' </summary>
    ''' <param name="json">The JsonElement representing a JSON object.</param>
    ''' <returns>A dictionary representing the JSON object.</returns>
    ''' <summary>
    ''' Converts a JsonElement (Object) into a Dictionary(Of String, JsonElement)
    ''' </summary>
    <Extension>
    Public Function JsonElementToDictionary(element As JsonElement) As Dictionary(Of String, JsonElement)
        Dim result As New Dictionary(Of String, JsonElement)(Comparer)

        ' Ensure the element is an object
        If element.ValueKind <> JsonValueKind.Object Then
            Throw New ArgumentException(message:="JsonElement must be an object to convert to Dictionary.")
        End If

        ' Enumerate properties and add to dictionary
        For Each prop As JsonProperty In element.EnumerateObject()
            result(key:=prop.Name) = prop.Value
        Next

        Return result
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
    <Extension>
    Public Function JsonToDictionary(json As String) As Dictionary(Of String, String)
        Dim resultDictionary As New Dictionary(Of String, String)(Comparer)
        If IsNullOrWhiteSpace(value:=json) Then
            Return resultDictionary
        End If
        Dim item As KeyValuePair(Of String, JsonElement)
        Dim rawJsonData As List(Of KeyValuePair(Of String, JsonElement)) = Nothing
        Dim tmpDict As Dictionary(Of String, JsonElement) = Nothing
        Try
            tmpDict = json.FromJson(Of Dictionary(Of String, JsonElement))()
        Catch ex As Exception
            Return resultDictionary
        End Try
        rawJsonData = tmpDict.ToList()

        For Each item In rawJsonData
            If item.Value.ValueKind = JsonValueKind.Null Then
                resultDictionary.Add(item.Key, value:=Nothing)
                Continue For
            End If
            Try
                Select Case item.Key
                    Case "activeNotifications", "clearedNotifications"
                        If item.Value.IsEmpty() Then
                            resultDictionary.Add(item.Key, value:=Nothing)
                        Else
                            resultDictionary.Add(item.Key, value:=item.Value.ToJson)
                        End If
                    Case NameOf(ServerDataEnum.clientTimeZoneName)
                        If s_useLocalTimeZone Then
                            PumpTimeZoneInfo = TimeZoneInfo.Local
                        Else
                            PumpTimeZoneInfo = CalculateTimeZone(timeZoneName:=item.Value.ElementToString())
                            Dim text As String
                            Dim messageButtons As MessageBoxButtons
                            If PumpTimeZoneInfo Is Nothing Then
                                Dim value As String = item.Value.ElementToString()
                                If IsNullOrWhiteSpace(value) Then
                                    text = "Your pump appears To be off-line, some " &
                                           "values will be wrong do you want to continue? " &
                                           $"If you select OK '{TimeZoneInfo.Local.Id}' " &
                                           "will be used as you local time and you will " &
                                           "not be prompted further. Cancel will Exit."
                                    messageButtons = MessageBoxButtons.OKCancel
                                Else
                                    text = $"Your pump TimeZone '{item.Value.ElementToString()}' " &
                                           "is not recognized, do you want to exit? " &
                                           "If you select No permanently use " &
                                           $"'{TimeZoneInfo.Local.Id}''? If you select " &
                                           $"Yes '{TimeZoneInfo.Local.Id}' " &
                                           "will be used and you will not be prompted further. No will use " &
                                           $"'{TimeZoneInfo.Local.Id}' until you restart " &
                                           "program. Cancel will exit program. " &
                                           "Please open an issue and provide the name " &
                                           $"'{item.Value.ElementToString()}'. After selecting 'Yes' " &
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
                        resultDictionary.Add(item.Key, value:=item.Value.ElementToString())
                    Case "Sg",
                         "sg",
                         NameOf(ServerDataEnum.averageSG),
                         NameOf(ServerDataEnum.sgBelowLimit),
                         NameOf(ServerDataEnum.averageSGFloat)

                        resultDictionary.Add(item.Key, value:=item.ScaleSg())
                    Case Else
                        If item.Value.ValueKind = JsonValueKind.String Then
                            resultDictionary.Add(item.Key, value:=item.Value.ElementToString())
                        Else
                            HandleExtendedInfo(item, resultDictionary)
                        End If
                End Select
            Catch ex As Exception
                Stop
                'Throw
            End Try
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
    Public Function JsonToListOfDictionary(json As String) As List(Of Dictionary(Of String, String))
        Dim resultListOfDictionary As New List(Of Dictionary(Of String, String))
        If IsNullOrWhiteSpace(value:=json) Then
            Return resultListOfDictionary
        End If

        Dim jsonList As List(Of Dictionary(Of String, JsonElement))
        Try
            jsonList = json.FromJson(Of List(Of Dictionary(Of String, JsonElement)))()
        Catch ex As Exception
            Return resultListOfDictionary
        End Try

        For Each e As IndexClass(Of Dictionary(Of String, JsonElement)) In jsonList.WithIndex
            Dim item As New Dictionary(Of String, String)(Comparer)
            Dim defaultTime As Date = PumpNow() - Eleven55PmSpan
            Dim index As Integer = -1
            For Each e1 As IndexClass(Of KeyValuePair(Of String, JsonElement)) In e.Value.WithIndex
                If e1.Value.Value.ValueKind = JsonValueKind.Null Then
                    item.Add(e1.Value.Key, value:=Nothing)
                ElseIf e1.Value.Key = "index" Then
                    index = CInt(e1.Value.Value.ElementToString())
                    item.Add(e1.Value.Key, value:=e1.Value.Value.ElementToString())
                ElseIf e1.Value.Key = "sg" Then
                    item.Add(e1.Value.Key, value:=e1.Value.ScaleSg)
                ElseIf e1.Value.Key = "dateTime" Then
                    Dim dateValue As Date = e1.Value.Value.GetDateTime()

                    ' Prevent Crash but not valid data
                    If dateValue.Year <= 2001 AndAlso index >= 0 Then
                        item.Add(e1.Value.Key,
                        value:=s_sgRecords(index).Timestamp.ToStringExact)
                    Else
                        item.Add(e1.Value.Key, value:=dateValue.ToShortDateTime())
                    End If
                Else
                    item.Add(e1.Value.Key, value:=e1.Value.Value.ElementToString())
                End If
            Next

            resultListOfDictionary.Add(item)
        Next
        Return resultListOfDictionary
    End Function

    ''' <summary>
    '''  Serializes an object of type <typeparamref name="T"/> to a JSON string
    '''  using the module-level <see cref="SerializerOptions"/>.
    '''  Returns an empty string if serialization fails (debugger will break when running under a debugger).
    ''' </summary>
    ''' <typeparam name="T">The type of the object to serialize.</typeparam>
    ''' <param name="value">The object to serialize.</param>
    ''' <returns>The JSON string representing the object.</returns>
    <Extension>
    Public Function ToJson(Of T)(value As T) As String
        Dim json As String
        Try
            json = JsonSerializer.Serialize(value, options:=SerializerOptions)
        Catch ex As Exception
            Stop
            LogMessage(message:=$"ERROR: failed serializing object to JSON: {ex.Message}")
            Throw
        End Try
        Return json
    End Function

    ''' <summary>
    '''  Convert a <see langword="String"/> into a <see cref="jsonElement"/>
    ''' </summary>
    ''' <param name="value">The String to0 be converted</param>
    <Extension>
    Public Function ToJsonElement(value As String) As JsonElement
        Dim json As String = value.ToJson()
        Using doc As JsonDocument = JsonDocument.Parse(json)
            Return doc.RootElement.Clone
        End Using
    End Function

    ''' <summary>
    '''  Converts a JSON string to a result of strings.
    ''' </summary>
    ''' <param name="Json">The JSON string to convert.</param>
    ''' <returns>
    '''  A result containing the key-element pairs from the JSON string.
    ''' </returns>
    <Extension>
    Public Function ToStringDictionary(Json As String) As Dictionary(Of String, String)
        Dim raw As Dictionary(Of String, JsonElement) = Nothing
        Try
            raw = Json.FromJson(Of Dictionary(Of String, JsonElement))()
        Catch ex As Exception
            Return New Dictionary(Of String, String)(Comparer)
        End Try

        Dim keySelector As Func(Of KeyValuePair(Of String, JsonElement), String) =
                Function(kvp As KeyValuePair(Of String, JsonElement)) As String
                    Return kvp.Key
                End Function

        Dim elementSelector As Func(Of KeyValuePair(Of String, JsonElement), String) =
                Function(kvp As KeyValuePair(Of String, JsonElement)) As String
                    Return kvp.Value.ElementToString()
                End Function
        Dim result As Dictionary(Of String, String) = Nothing
        Try
            result = raw.ToDictionary(keySelector, elementSelector)
        Catch ex As Exception
            Stop
        End Try
        Return result
    End Function

    ''' <summary>
    '''  Converts a <see cref="JsonElement"/> object to a
    '''  <see cref="Dictionary(Of String, JsonElement)"/>,
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
        Dim result As New Dictionary(Of String, String)(Comparer)

        For Each prop As JsonProperty In jsonElement.EnumerateObject()
            Dim v As JsonElement = prop.Value
            Dim s As String = Nothing

            Select Case v.ValueKind
                Case JsonValueKind.String
                    s = v.GetString()
                Case JsonValueKind.Number, JsonValueKind.True, JsonValueKind.False
                    s = v.GetRawText()
                Case JsonValueKind.Object, JsonValueKind.Array
                    s = v.GetRawText() ' or recursively flatten
                Case JsonValueKind.Null, JsonValueKind.Undefined
                    s = Nothing
            End Select

            If s IsNot Nothing Then
                result(key:=prop.Name) = s
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' Try to get a string property from a JsonElement safely.
    ''' </summary>
    <Extension>
    Public Function TryGetStringProperty(element As JsonElement,
                                         propertyName As String,
                                         ByRef value As String) As Boolean
        value = Nothing
        If element.IsEmpty Then
            Return False
        End If
        Dim prop As JsonElement
        If element.TryGetProperty(propertyName, value:=prop) AndAlso Not prop.IsEmpty Then
            value = prop.GetString()
            Return True
        End If
        Return False
    End Function

End Module
