' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text.Json

''' <summary>
'''  Provides utility methods for file operations related to tokenDataElement data management.
''' </summary>
Friend Module FileUtilities

    ''' <summary>
    '''  The default filename for login data files.
    ''' </summary>
    Private Const LOGIN_DATA_FILENAME As String = "LoginData.json"

    ''' <summary>
    '''  The list of required fields for tokenDataElement data validation.
    ''' </summary>
    Private ReadOnly s_requiredFields() As String = {
        "access_token",
        "refresh_token",
        "scope",
        "client_id"}

    Private Function ReadAndValidateTokenJsonElement(
        Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME) As JsonElement

        Dim path As String = GetLoginDataFileName(tokenBaseFileName)
        Dim message As String
        Const startKey As String = "Reading tokenDataElement file: "
        message = $"{startKey}{path}"
        UpdateMessage(message, startKey)
        If Not File.Exists(path) Then
            message = $"ERROR: tokenDataElement file {path} not found"
            LogMessage(message)
            Return Nothing
        End If

        Try
            Dim json As String = File.ReadAllText(path)
            Dim tokenData As JsonElement
            Try
                tokenData = json.FromJson(Of JsonElement)()
            Catch ex As Exception
                message = $"ERROR: failed parsing tokenDataElement file {path}"
                Debug.WriteLine(message)
                Return Nothing
            End Try
            For Each propertyName As String In s_requiredFields
                Dim propElem As JsonElement = Nothing
                If Not tokenData.TryGetProperty(propertyName, value:=propElem) Then
                    message = $"ERROR: field {propertyName} is missing from tokenDataElement file"
                    LogMessage(message)
                    Return Nothing
                End If
            Next

            ' Strip unsupported fields (client_secret, mag-identifier) from the token JSON
            Try
                Dim dict As Dictionary(Of String, JsonElement) =
                    JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(json:=tokenData.GetRawText())
                If dict IsNot Nothing Then
                    ' Strip deprecated fields if present. These may appear in legacy files
                    ' but should not be part of the runtime token model.
                    dict.Remove(key:="client_secret")
                    dict.Remove(key:="mag-identifier")

                    Dim sanitizedJson As String = JsonSerializer.Serialize(dict, JsonExtensions.SerializerOptions)
                    Return JsonSerializer.Deserialize(Of JsonElement)(sanitizedJson)
                End If
            Catch
                ' If sanitization fails, return original tokenData for compatibility
            End Try

            Return tokenData
        Catch ex As JsonException
            message =
                $"ERROR: failed parsing tokenDataElement file {path}: {ex.Message}"
            LogMessage(message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Writes a byte array to a file.
    ''' </summary>
    ''' <param name="path">The name of the file to write to.</param>
    ''' <param name="buffer">The byte array to write.</param>
    Friend Sub ByteArrayToFile(path As String, buffer() As Byte)
        Try
            Const access As FileAccess = FileAccess.Write
            Using fs As New FileStream(path, mode:=FileMode.Create, access)
                fs.Write(buffer, offset:=0, count:=buffer.Length)
            End Using
        Catch ex As Exception
            Stop
        End Try
    End Sub

    ''' <summary>
    '''  Gets the full path for the login data file based on the user name
    '''  and base file name.
    ''' </summary>
    ''' <param name="userName">The user name.</param>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the tokenDataElement data file.
    ''' </param>
    ''' <returns>The full path to the login data file.</returns>
    ''' <exception cref="ArgumentException">
    '''  Thrown if <paramref name="tokenBaseFileName"/> is null or whitespace.
    ''' </exception>
    Friend Function GetLoginDataFileName(Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME) As String

        If IsNullOrWhiteSpace(value:=tokenBaseFileName) Then
            Throw New ArgumentException(
                message:=$"'{NameOf(tokenBaseFileName)}' cannot be null or whitespace.",
                paramName:=NameOf(tokenBaseFileName))
        End If

        If tokenBaseFileName.EqualsNoCase(LOGIN_DATA_FILENAME) Then
            Dim settingsPathParent As String =
                Directory.GetParent(path:=GetSettingsDirectory()).FullName

            Dim loginTokenFileName As String = $"{GetUserName()}{LOGIN_DATA_FILENAME}"
            Return Path.Join(settingsPathParent, loginTokenFileName)
        Else
            Return tokenBaseFileName
        End If
    End Function

    ''' <summary>
    '''  Reads a file and deserializes its contents into a <see cref="JsonElement"/>.
    '''  Returns Nothing on error.
    ''' </summary>
    Friend Function ReadJsonElementFromFile(path As String) As JsonElement
        If Not File.Exists(path) Then
            LogMessage(message:=$"ERROR: file {path} not found")
            Return Nothing
        End If

        Try
            Dim json As String = File.ReadAllText(path)
            Dim result As JsonElement
            Try
                result = json.FromJson(Of JsonElement)()
            Catch ex As Exception
                Return Nothing
            End Try
            Return result
        Catch ex As Exception
            Dim message As String =
                $"ERROR: failed reading file {path}: {ex.Message}"
            LogMessage(message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Reads and validates the tokenDataElement data file for a user and
    '''  returns a <see cref="TokenData"/> object.
    ''' </summary>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the tokenDataElement data file.
    '''  Defaults to <see cref="LOGIN_DATA_FILENAME"/>.
    ''' </param>
    ''' <returns>
    '''  A <see cref="TokenData"/> object if the file exists and is valid;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    Friend Function ReadTokenDataFile(
            Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME) As TokenData

        Dim tokenElement As JsonElement =
            ReadAndValidateTokenJsonElement(tokenBaseFileName)
        If tokenElement.IsEmpty Then
            Return Nothing
        End If

        Try
            Dim json As String = tokenElement.GetRawText()
            Dim td As TokenData = Nothing
            Try
                td = json.FromJson(Of TokenData)()
            Catch ex As Exception
                Return Nothing
            End Try
            Return td
        Catch ex As JsonException
            Dim message As String =
                $"Failed parsing tokenDataElement data to TokenData: {ex.Message}"
            LogMessage(message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Reads and validates the tokenDataElement file for a user and
    '''  returns a <see cref="JsonElement"/>.
    ''' </summary>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the tokenDataElement data file.
    '''  Defaults to <see cref="LOGIN_DATA_FILENAME"/>.
    ''' </param>
    ''' <returns>
    '''  A <see cref="JsonElement"/> if the file exists and is valid;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    Friend Function ReadTokenFile(
            Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME) As JsonElement
        Return ReadAndValidateTokenJsonElement(tokenBaseFileName)
    End Function

    ''' <summary>
    '''  Writes the specified <see cref="JsonElement"/> tokenDataElement data
    '''  to a file for the given user.
    ''' </summary>
    ''' <param name="tokenDataElement">The tokenDataElement data to write.</param>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the tokenDataElement data file.
    '''  Defaults to <see cref="LOGIN_DATA_FILENAME"/>.
    ''' </param>
    Public Sub WriteTokenFile(tokenDataElement As JsonElement,
        Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME)
        If tokenDataElement.IsEmpty Then
            Exit Sub
        End If
        Dim path As String = GetLoginDataFileName(tokenBaseFileName)
        Dim contents As String = String.Empty
        ' Remove unsupported fields (mag-identifier, client_secret) before writing
        Try
            Dim dict As Dictionary(Of String, JsonElement) =
                JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(tokenDataElement.GetRawText())
            If dict IsNot Nothing Then
                dict.Remove(key:="mag-identifier")
                dict.Remove(key:="client_secret")
                contents = JsonSerializer.Serialize(dict, JsonExtensions.SerializerOptions)
            Else
                If Not tokenDataElement.TryToJson(json:=contents) Then
                    LogMessage(message:=$"ERROR: failed serializing tokenDataElement for file {path}")
                    Return
                End If
            End If
        Catch ex As Exception
            ' Fallback to original serialization if manipulation fails
            If Not tokenDataElement.TryToJson(json:=contents) Then
                LogMessage(message:=$"ERROR: failed serializing tokenDataElement for file {path}")
                Return
            End If
        End Try
        ' Write sanitized contents to file
        Try
            File.WriteAllText(path, contents)
        Catch ex As Exception
            LogMessage(message:=$"ERROR: failed writing token file {path}: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    '''   Writes the specified tokenDataElement data of type <typeparamref name="T"/>
    '''  to a file at the given path.
    ''' </summary>
    ''' <typeparam name="T">The type of the tokenDataElement data to write.</typeparam>
    ''' <param name="token">The tokenDataElement data to write.</param>
    ''' <param name="path">The path to the file where the tokenDataElement data will be written.</param>
    Public Sub WriteTokenFile(Of T)(token As T, path As String)
        Dim contents As String = String.Empty
        If Not token.TryToJson(contents) Then
            LogMessage(message:=$"ERROR: failed serializing tokenDataElement to file {path}")
            Return
        End If
        File.WriteAllText(path, contents)
    End Sub

End Module
