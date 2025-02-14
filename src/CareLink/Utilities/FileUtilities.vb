' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net.Http
Imports System.Text.Json

Friend Module FileUtilities
    Private Const DEFAULT_FILENAME As String = "logindata.json"

    Private ReadOnly s_requiredFields() As String = {
        "access_token",
        "refresh_token",
        "scope",
        "client_id",
        "client_secret",
        "mag-identifier"}

    Private Function GetLoginDataFileName(userName As String, tokenBaseFileName As String) As String
        Return If(tokenBaseFileName <> DEFAULT_FILENAME,
            tokenBaseFileName,
            Path.Combine(Directory.GetParent(SettingsDirectory).FullName, $"{userName}{DEFAULT_FILENAME}"))
    End Function

    Public Sub ByteArrayToFile(fileName As String, byteArray() As Byte)
        Try
            Using fs As New FileStream(fileName, FileMode.Create, FileAccess.Write)
                fs.Write(byteArray, 0, byteArray.Length)
            End Using
        Catch ex As Exception
            Stop
        End Try
    End Sub

    Public Function ReadTokenDataFile(userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME) As AccessToken
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        If File.Exists(fileWithPath) Then
            Try
                Dim jsonAsText As String = File.ReadAllText(fileWithPath)
                Dim tempData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(jsonAsText)
                For Each field As String In s_requiredFields
                    If Not tempData.TryGetProperty(field, Nothing) Then
                        Console.WriteLine($"Field {field} is missing from data file")
                        Return Nothing
                    End If
                Next
                Return JsonSerializer.Deserialize(Of AccessToken)(jsonAsText)
            Catch ex As JsonException
                Console.WriteLine("Failed parsing JSON")
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
                        Console.WriteLine($"ERROR: field {field} is missing from token file")
                        Return Nothing
                    End If
                Next
                Return tokenData
            Catch ex As JsonException
                Console.WriteLine($"ERROR: failed parsing token file {fileWithPath}")
            End Try
        Else
            Console.WriteLine($"ERROR: token file {fileWithPath} not found")
        End If
        Return Nothing
    End Function

    Public Sub WriteTokenDataFile(tokenData As AccessToken, userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME)
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Console.WriteLine("Wrote data file")
        File.WriteAllText(fileWithPath, JsonSerializer.Serialize(tokenData, s_jsonSerializerOptions))
    End Sub

    Public Sub WriteTokenFile(obj As JsonElement, userName As String, Optional tokenBaseFileName As String = DEFAULT_FILENAME)
        Dim fileWithPath As String = GetLoginDataFileName(userName, tokenBaseFileName)
        Console.WriteLine(NameOf(WriteTokenFile))
        File.WriteAllText(fileWithPath, JsonSerializer.Serialize(obj, s_jsonSerializerOptions))
    End Sub

End Module
