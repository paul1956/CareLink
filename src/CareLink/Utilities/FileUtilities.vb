' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net.Http
Imports System.Text.Json

Friend Module FileUtilities

    Private ReadOnly s_requiredFields() As String = {
        "access_token",
        "refresh_token",
        "scope",
        "client_id",
        "client_secret",
        "mag-identifier"}

    Public Const DEFAULT_FILENAME As String = "logindata.json"

    Public Sub ByteArrayToFile(fileName As String, byteArray() As Byte)
        Try
            Using fs As New FileStream(fileName, FileMode.Create, FileAccess.Write)
                fs.Write(byteArray, 0, byteArray.Length)
            End Using
        Catch ex As Exception
            Stop
        End Try
    End Sub

    Public Function GetLoginDataFileName(userName As String, tokenBaseFileName As String) As String
        If String.IsNullOrWhiteSpace(tokenBaseFileName) Then
            Throw New ArgumentException($"'{NameOf(tokenBaseFileName)}' cannot be null or whitespace.", NameOf(tokenBaseFileName))
        End If

        Return If(tokenBaseFileName.Equals(DEFAULT_FILENAME, StringComparison.InvariantCultureIgnoreCase),
            Path.Join(Directory.GetParent(SettingsDirectory).FullName, $"{userName}{DEFAULT_FILENAME.Substring(0, 1).ToUpper}{DEFAULT_FILENAME.Substring(1)}"),
            tokenBaseFileName)
    End Function

    Public Function ReadTokenDataFile(userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME) As TokenData
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        If File.Exists(fileWithPath) Then
            Try
                Dim jsonAsText As String = File.ReadAllText(fileWithPath)
                Dim tempData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(jsonAsText)
                For Each field As String In s_requiredFields
                    If Not tempData.TryGetProperty(field, Nothing) Then
                        Debug.WriteLine($"Field {field} is missing from data file")
                        Return Nothing
                    End If
                Next
                Return JsonSerializer.Deserialize(Of TokenData)(jsonAsText)
            Catch ex As JsonException
                Debug.WriteLine("Failed parsing JSON")
            End Try
        End If
        Return Nothing
    End Function

    Public Function ReadTokenFile(userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME) As JsonElement
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Debug.WriteLine(NameOf(fileWithPath))
        If File.Exists(fileWithPath) Then
            Try
                Dim jsonAsText As String = File.ReadAllText(fileWithPath)
                Dim tokenData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(jsonAsText)
                For Each field As String In s_requiredFields
                    If Not tokenData.TryGetProperty(field, Nothing) Then
                        Debug.WriteLine($"ERROR: field {field} is missing from token file")
                        Return Nothing
                    End If
                Next
                Return tokenData
            Catch ex As JsonException
                Debug.WriteLine($"ERROR: failed parsing token file {fileWithPath}")
            End Try
        Else
            Debug.WriteLine($"ERROR: token file {fileWithPath} not found")
        End If
        Return Nothing
    End Function

    Public Sub WriteTokenDataFile(tokenData As TokenData, userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME)
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Debug.WriteLine("Wrote data file")
        File.WriteAllText(fileWithPath, JsonSerializer.Serialize(tokenData, s_jsonSerializerOptions))
    End Sub

    Public Sub WriteTokenFile(tokenData As JsonElement, userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME)
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Debug.WriteLine(NameOf(WriteTokenFile))
        Dim serializedTokenData As String = JsonSerializer.Serialize(tokenData, s_jsonSerializerOptions)
        File.WriteAllText(fileWithPath, serializedTokenData)
    End Sub

End Module
