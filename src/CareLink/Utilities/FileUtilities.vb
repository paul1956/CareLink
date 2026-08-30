' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text.Json

''' <summary>
'''  Provides utility methods for file operations related to token data management.
''' </summary>
Friend Module FileUtilities

    ''' <summary>
    '''  The default filename for login data files.
    ''' </summary>
    Private Const LOGIN_DATA_FILENAME As String = "LoginData.json"

    ''' <summary>
    '''  The list of required fields for token data validation.
    ''' </summary>
    Private ReadOnly s_requiredFields() As String = {
        "access_token",
        "refresh_token",
        "scope",
        "client_id"}

    Private Function ReadAndValidateTokenJsonElement(
                Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME) As JsonElement

        Dim path As String = GetLoginDataFileName(tokenBaseFileName)
        Debug.WriteLine(message:=$"Reading token file: {path}")
        If Not File.Exists(path) Then
            Debug.WriteLine(message:=$"ERROR: token file {path} not found")
            Return Nothing
        End If

        Try
            Dim json As String = File.ReadAllText(path)
            Dim tokenData As JsonElement = json.FromJson(Of JsonElement)(DeserializationOptions)
            For Each propertyName As String In s_requiredFields
                Dim propElem As JsonElement = Nothing
                If Not tokenData.TryGetProperty(propertyName, value:=propElem) Then
                    Dim message As String = $"ERROR: field {propertyName} is missing from token file"
                    Debug.WriteLine(message)
                    Return Nothing
                End If
            Next

            Return tokenData
        Catch ex As JsonException
            Debug.WriteLine(message:=$"ERROR: failed parsing token file {path}: {ex.Message}")
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

    Friend Function DeleteTokenFile() As JsonElement
        Dim path As String = GetLoginDataFileName(tokenBaseFileName:=LOGIN_DATA_FILENAME)
        SafeDeleteFile(path)
    End Function

    ''' <summary>
    '''  Gets the full path for the login data file based on the user name
    '''  and base file name.
    ''' </summary>
    ''' <param name="userName">The user name.</param>
    ''' <param name="tokenBaseFileName">The base file name for the token data file.</param>
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
            Debug.WriteLine(message:=$"ERROR: file {path} not found")
            Return Nothing
        End If

        Try
            Dim json As String = File.ReadAllText(path)
            Return json.FromJson(Of JsonElement)(DeserializationOptions)
        Catch ex As Exception
            Debug.WriteLine(message:=$"ERROR: failed reading file {path}: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Reads and validates the token data file for a user and
    '''  returns a <see cref="TokenData"/> object.
    ''' </summary>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the token data file.
    '''  Defaults to <see cref="LOGIN_DATA_FILENAME"/>.
    ''' </param>
    ''' <returns>
    '''  A <see cref="TokenData"/> object if the file exists and is valid;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    Friend Function ReadTokenDataFile(
            Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME) As TokenData

        Dim tokenElement As JsonElement = ReadAndValidateTokenJsonElement(tokenBaseFileName)
        If tokenElement.IsEmpty Then
            Return Nothing
        End If

        Try
            Dim json As String = tokenElement.GetRawText()
            Return json.FromJson(Of TokenData)(DeserializationOptions)
        Catch ex As JsonException
            Debug.WriteLine(message:=$"Failed parsing token data to TokenData: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Reads and validates the token file for a user and
    '''  returns a <see cref="JsonElement"/>.
    ''' </summary>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the token data file.
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
    '''  Writes the specified <see cref="JsonElement"/> token data
    '''  to a file for the given user.
    ''' </summary>
    ''' <param name="token">The token data to write.</param>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the token data file.
    '''  Defaults to <see cref="LOGIN_DATA_FILENAME"/>.
    ''' </param>
    Public Sub WriteTokenFile(token As JsonElement,
        Optional tokenBaseFileName As String = LOGIN_DATA_FILENAME)
        Dim path As String = GetLoginDataFileName(tokenBaseFileName)
        Dim contents As String = token.ToJson()
        WriteTokenFile(Of JsonElement)(token, path)
    End Sub

    ''' <summary>
    '''   Writes the specified token data of type <typeparamref name="T"/>
    '''  to a file at the given path.
    ''' </summary>
    ''' <typeparam name="T">The type of the token data to write.</typeparam>
    ''' <param name="token">The token data to write.</param>
    ''' <param name="path">The path to the file where the token data will be written.</param>
    Public Sub WriteTokenFile(Of T)(token As T, path As String)
        Dim contents As String = token.ToJson()
        File.WriteAllText(path, contents)
    End Sub

End Module
