﻿' Licensed to the .NET Foundation under one or more agreements.
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
    Private Const DEFAULT_FILENAME As String = "logindata.json"

    ''' <summary>
    '''  The list of required fields for token data validation.
    ''' </summary>
    Private ReadOnly s_requiredFields() As String = {
        "access_token",
        "refresh_token",
        "scope",
        "client_id",
        "client_secret",
        "mag-identifier"}

    ''' <summary>
    '''  Writes a byte array to a file.
    ''' </summary>
    ''' <param name="fileName">The name of the file to write to.</param>
    ''' <param name="byteArray">The byte array to write.</param>
    Friend Sub ByteArrayToFile(fileName As String, byteArray() As Byte)
        Try
            Using fs As New FileStream(fileName, FileMode.Create, FileAccess.Write)
                fs.Write(byteArray, 0, byteArray.Length)
            End Using
        Catch ex As Exception
            Stop
        End Try
    End Sub

    ''' <summary>
    '''  Gets the full path for the login data file based on the user name and base file name.
    ''' </summary>
    ''' <param name="userName">The user name.</param>
    ''' <param name="tokenBaseFileName">The base file name for the token data file.</param>
    ''' <returns>The full path to the login data file.</returns>
    ''' <exception cref="ArgumentException">
    '''  Thrown if <paramref name="tokenBaseFileName"/> is null or whitespace.
    ''' </exception>
    Friend Function GetLoginDataFileName(userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME) As String
        If String.IsNullOrWhiteSpace(tokenBaseFileName) Then
            Throw New ArgumentException($"'{NameOf(tokenBaseFileName)}' cannot be null or whitespace.", NameOf(tokenBaseFileName))
        End If

        Return If(tokenBaseFileName.Equals(DEFAULT_FILENAME, StringComparison.InvariantCultureIgnoreCase),
            Path.Join(Directory.GetParent(SettingsDirectory).FullName, $"{userName}{DEFAULT_FILENAME.Substring(0, 1).ToUpper}{DEFAULT_FILENAME.Substring(1)}"),
            tokenBaseFileName)
    End Function

    ''' <summary>
    '''  Reads and validates the token data file for a user and returns a <see cref="TokenData"/> object.
    ''' </summary>
    ''' <param name="userName">The user name.</param>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the token data file. Defaults to <see cref="DEFAULT_FILENAME"/>.
    ''' </param>
    ''' <returns>
    '''  A <see cref="TokenData"/> object if the file exists and is valid; otherwise, <see langword="Nothing"/>.
    ''' </returns>
    Friend Function ReadTokenDataFile(userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME) As TokenData
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        If File.Exists(fileWithPath) Then
            Try
                Dim json As String = File.ReadAllText(fileWithPath)
                Dim jsonElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json)
                For Each propertyName As String In s_requiredFields
                    If Not jsonElement.TryGetProperty(propertyName, value:=Nothing) Then
                        Debug.WriteLine($"Field {propertyName} is missing from data file")
                        Return Nothing
                    End If
                Next
                Return JsonSerializer.Deserialize(Of TokenData)(json)
            Catch ex As JsonException
                Debug.WriteLine("Failed parsing JSON")
            End Try
        End If
        Return Nothing
    End Function

    ''' <summary>
    '''  Reads and validates the token file for a user and returns a <see cref="JsonElement"/>.
    ''' </summary>
    ''' <param name="userName">The user name.</param>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the token data file. Defaults to <see cref="DEFAULT_FILENAME"/>.
    ''' </param>
    ''' <returns>
    '''  A <see cref="JsonElement"/> if the file exists and is valid; otherwise, <see langword="Nothing"/>.
    ''' </returns>
    Friend Function ReadTokenFile(userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME) As JsonElement
        Dim path As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Debug.WriteLine(NameOf(path))
        If File.Exists(path) Then
            Try
                Dim jsonAsText As String = File.ReadAllText(path)
                Dim tokenData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(jsonAsText)
                For Each propertyName As String In s_requiredFields
                    If Not tokenData.TryGetProperty(propertyName, value:=Nothing) Then
                        Debug.WriteLine($"ERROR: field {propertyName} is missing from token file")
                        Return Nothing
                    End If
                Next
                Return tokenData
            Catch ex As JsonException
                Debug.WriteLine($"ERROR: failed parsing token file {path}")
            End Try
        Else
            Debug.WriteLine($"ERROR: token file {path} not found")
        End If
        Return Nothing
    End Function

    ''' <summary>
    '''  Writes the specified <see cref="TokenData"/> to a file for the given user.
    ''' </summary>
    ''' <param name="value">The token data to write.</param>
    ''' <param name="userName">The user name.</param>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the token data file. Defaults to <see cref="DEFAULT_FILENAME"/>.
    ''' </param>
    Public Sub WriteTokenDataFile(
            value As TokenData,
            userName As String,
            Optional tokenBaseFileName As String = DEFAULT_FILENAME)
        Dim path As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Debug.WriteLine("Wrote data file")
        Dim contents As String = JsonSerializer.Serialize(value, s_jsonSerializerOptions)
        File.WriteAllText(path, contents)
    End Sub

    ''' <summary>
    '''  Writes the specified <see cref="JsonElement"/> token data to a file for the given user.
    ''' </summary>
    ''' <param name="value">The token data to write.</param>
    ''' <param name="userName">The user name.</param>
    ''' <param name="tokenBaseFileName">
    '''  The base file name for the token data file. Defaults to <see cref="DEFAULT_FILENAME"/>.
    ''' </param>
    Public Sub WriteTokenFile(value As JsonElement,
            userName As String,
            Optional tokenBaseFileName As String = DEFAULT_FILENAME)
        Dim path As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Debug.WriteLine(NameOf(WriteTokenFile))
        Dim contents As String = JsonSerializer.Serialize(value, s_jsonSerializerOptions)
        File.WriteAllText(path, contents)
    End Sub

End Module
