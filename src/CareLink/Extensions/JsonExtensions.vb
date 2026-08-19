' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Nodes
Imports System.Text.Json.Serialization
Imports DocumentFormat.OpenXml.Spreadsheet

Public Module JsonExtensions

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for serialization with indented output.
    ''' </summary>
    Private ReadOnly Property SerializerOptions As New JsonSerializerOptions With
        {.WriteIndented = True}

    ''' <summary>
    '''  Default <see cref="JsonSerializerOptions"/> for deserialization.
    '''  Ignores null values, writes numbers as strings,
    '''  uses case-insensitive property names, and disallows unmapped members.
    ''' </summary>
    Public ReadOnly Property DeserializationOptions As New JsonSerializerOptions() With
        {.NumberHandling = JsonNumberHandling.WriteAsString,
         .PropertyNameCaseInsensitive = True,
         .UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow}

    Private Sub HandleExtendedInfo(item As KeyValuePair(Of String,
                                   JsonElement), resultDictionary As Dictionary(Of String, String))
        If item.Value.IsEmpty() Then
            Return
        End If
        Select Case item.Value.ValueKind
            Case JsonValueKind.Array
                Stop
            Case JsonValueKind.Object
                Dim jsonItem As String = item.DeserializeJsonAsString
                Dim extendedInfo As Dictionary(Of String, JsonElement) =
                    jsonItem.FromJson(Of Dictionary(Of String, JsonElement))(DeserializationOptions)
                For Each kvp As KeyValuePair(Of String, JsonElement) In extendedInfo
                    resultDictionary.Add(key:=$"{item.Key}:{kvp.Key}", value:=kvp.Value.ToString)
                Next
            Case JsonValueKind.Undefined
                Stop
                resultDictionary.Add(key:=$"{item.Key}", value:=Nothing)
            Case JsonValueKind.String
                resultDictionary.Add(key:=$"{item.Key}", value:=item.Value.ToString)
            Case JsonValueKind.Number
                resultDictionary.Add(key:=$"{item.Key}", value:=item.Value.ToString)
            Case JsonValueKind.True
                resultDictionary.Add(key:=$"{item.Key}", value:="True")
            Case JsonValueKind.False
                resultDictionary.Add(key:=$"{item.Key}", value:="False")
            Case JsonValueKind.Null
                Stop
                Exit Select
        End Select
    End Sub

    ''' <summary>
    '''  Converts a JSON item (key-value pair) to its <see langword="String"/> representation.
    ''' </summary>
    ''' <param name="item">The key-value pair to convert.</param>
    ''' <returns>The <see langword="String"/> representation of the item's value.</returns>
    <Extension>
    Public Function DeserializeJsonAsString(item As KeyValuePair(Of String, JsonElement)) As String
        Select Case item.Value.ValueKind
            Case JsonValueKind.String
                Return item.Value.GetString()

            Case JsonValueKind.Number
                Return item.Value.ToString() ' Keeps numeric formatting

            Case JsonValueKind.True
                Return "True"

            Case JsonValueKind.False
                Return "False"

            Case JsonValueKind.Null
                Return String.Empty

            Case Else
                ' For objects, arrays, or other kinds, return raw JSON text
                Return item.Value.GetRawText()
        End Select
    End Function

    ''' <summary>
    '''  Converts a <see cref="JsonElement"/> to its <see langword="String"/> representation.
    '''  If the jsonElement is null or undefined, returns an empty string.
    '''  If the jsonElement is a string, returns its value; otherwise, returns the raw JSON text.
    ''' </summary>
    ''' <param name="value">The <see cref="JsonElement"/> to convert.</param>
    ''' <returns>The <see langword="String"/> representation of the jsonElement.</returns>
    <Extension>
    Public Function ElementToJson(value As JsonElement) As String
        Return If(value.IsEmpty,
                  String.Empty,
                  If(value.ValueKind = JsonValueKind.String,
                     value.GetString(),
                     value.GetRawText()))
    End Function

    ''' <summary>
    '''  Converts (Deserializes) a JSON string  to an object of type <typeparamref name="T"/>
    '''  using the default <see cref="JsonSerializerOptions"/>.
    ''' </summary>
    ''' <typeparam name="T">The type of the object to deserialize.</typeparam>
    ''' <param name="json">The JSON string to deserialize.</param>
    ''' <returns>The deserialized object.</returns>
    ''' <param name="DeserializationOptions"></param>
    <Extension>
    Public Function FromJson(Of T)(json As String, DeserializationOptions As JsonSerializerOptions) As T
        Try
            Return JsonSerializer.Deserialize(Of T)(json, options:=DeserializationOptions)
        Catch ex As JsonException
            Stop
            Debug.WriteLine(message:=$"ERROR: failed deserializing JSON string: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Converts (Deserializes) a JSON string to an object of type <typeparamref name="T"/>
    '''  using the default <see cref="JsonSerializerOptions"/>.
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
            Debug.WriteLine(message:=$"ERROR: failed deserializing JSON string: {ex.Message}")
            Return Nothing
        End Try
    End Function

    <Extension>
    Public Function IsEmpty(element As JsonElement) As Boolean
        Select Case element.ValueKind
            Case JsonValueKind.Null
                Return True
            Case JsonValueKind.Undefined
                Return True
            Case JsonValueKind.Object
                Return Not element.EnumerateObject().Any()
            Case JsonValueKind.Array
                Return element.GetArrayLength() = 0
            Case JsonValueKind.String
                Return element.ToString.Length = 0
            Case JsonValueKind.Number
                Return False
            Case JsonValueKind.True
                Return False
            Case JsonValueKind.False
                Return False
        End Select
        Return False
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
        Dim result As New Dictionary(Of String, JsonElement)(StringComparer.OrdinalIgnoreCase)

        ' Ensure the element is an object
        If element.ValueKind <> JsonValueKind.Object Then
            Throw New ArgumentException(message:="JsonElement must be an object to convert to Dictionary.")
        End If

        ' Enumerate properties and add to dictionary
        For Each prop As JsonProperty In element.EnumerateObject()
            result(prop.Name) = prop.Value
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
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim resultDictionary As New Dictionary(Of String, String)(comparer)
        If IsNullOrWhiteSpace(value:=json) Then
            Return resultDictionary
        End If
        Dim item As KeyValuePair(Of String, JsonElement)
        Dim rawJsonData As List(Of KeyValuePair(Of String, JsonElement)) =
            json.FromJson(Of Dictionary(Of String, JsonElement))(DeserializationOptions).ToList()

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
                            PumpTimeZoneInfo = CalculateTimeZone(timeZoneName:=item.Value.ToString)
                            Dim text As String
                            Dim messageButtons As MessageBoxButtons
                            If PumpTimeZoneInfo Is Nothing Then
                                Dim value As String = item.Value.ToString
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
                        If item.Value.ValueKind = JsonValueKind.String Then
                            resultDictionary.Add(item.Key, value:=item.DeserializeJsonAsString)
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

        Dim jsonList As List(Of Dictionary(Of String, JsonElement)) =
            json.FromJson(Of List(Of Dictionary(Of String, JsonElement)))(DeserializationOptions)

        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase

        For Each e As IndexClass(Of Dictionary(Of String, JsonElement)) In jsonList.WithIndex
            Dim item As New Dictionary(Of String, String)(comparer)
            Dim defaultTime As Date = PumpNow() - Eleven55Span
            Dim index As Integer = -1
            For Each e1 As IndexClass(Of KeyValuePair(Of String, JsonElement)) In e.Value.WithIndex
                If e1.Value.Value.ValueKind = JsonValueKind.Null Then
                    item.Add(e1.Value.Key, value:=Nothing)
                ElseIf e1.Value.Key = "index" Then
                    index = CInt(e1.Value.DeserializeJsonAsString)
                    item.Add(e1.Value.Key, value:=e1.Value.DeserializeJsonAsString)
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
                    item.Add(e1.Value.Key, value:=e1.Value.DeserializeJsonAsString())
                End If
            Next

            resultListOfDictionary.Add(item)
        Next
        Return resultListOfDictionary
    End Function

    ''' <summary>
    '''  Serializes an object of type <typeparamref name="T"/> to a JSON string
    '''  using the default <see cref="JsonSerializerOptions"/>.
    ''' </summary>
    ''' <typeparam name="T">The type of the object to serialize.</typeparam>
    ''' <param name="value">The object to serialize.</param>
    ''' <returns>The JSON string representing the object.</returns>
    <Extension>
    Public Function ToJson(Of T)(value As T) As String
        Dim json As String = String.Empty
        Try
            json = JsonSerializer.Serialize(value, options:=SerializerOptions)
        Catch ex As Exception
            Stop
        End Try
        Return json
    End Function

    ''' <summary>
    '''  Convert a <see langword="String"/> into a <see cref="jsonElement"/>
    ''' </summary>
    ''' <param name="value">The String to0 be converted</param>
    <Extension>
    Public Function ToJsonElement(value As String) As JsonElement
        Dim json As String = JsonSerializer.Serialize(value)
        Using doc As JsonDocument = JsonDocument.Parse(json)
            Return doc.RootElement.Clone
        End Using
    End Function

    ''' <summary>
    '''  Converts a <paramref name="jsonArray"/> array to a <see cref="List"/> of objects,
    '''  recursively handling nested arrays and objects.
    ''' </summary>
    ''' <param name="jsonArray">The JsonElement representing a JSON array.</param>
    ''' <returns>A list of objects representing the array elements.</returns>
    <Extension>
    Public Function ToList(jsonArray As JsonElement) As List(Of JsonElement)
        Dim result As New List(Of JsonElement)()
        For Each jsonElement As JsonElement In jsonArray.EnumerateArray()
            Select Case jsonElement.ValueKind
                Case JsonValueKind.Object
                    result.Add(item:=jsonElement)
                Case JsonValueKind.Array
                    result.Add(item:=jsonElement)
                Case Else
                    result.Add(item:=jsonElement)
            End Select
        Next
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
        Dim result As New Dictionary(Of String, String)(comparer:=StringComparer.OrdinalIgnoreCase)

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
    Public Function TryGetStringProperty(element As JsonElement, propertyName As String, ByRef value As String) As Boolean
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
