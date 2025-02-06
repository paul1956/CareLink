' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text.Json

Friend Module FileUtilities

    Public Sub ByteArrayToFile(fileName As String, byteArray() As Byte)
        Try
            Using fs As New FileStream(fileName, FileMode.Create, FileAccess.Write)
                fs.Write(byteArray, 0, byteArray.Length)
            End Using
        Catch ex As Exception
            Stop
        End Try
    End Sub

    Public Function ReadTokenDataFile(fileWithPath As String) As JsonElement?
        If File.Exists(fileWithPath) Then
            Try
                Dim tokenData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(File.ReadAllText(fileWithPath))
                Dim requiredFields() As String = {
                    "access_token",
                    "refresh_token",
                    "scope",
                    "client_id",
                    "client_secret",
                    "mag-identifier"}

                For Each field As String In requiredFields
                    If Not tokenData.TryGetProperty(field, Nothing) Then
                        Console.WriteLine($"Field {field} is missing from data file")
                        Return Nothing
                    End If
                Next
                Return tokenData
            Catch ex As JsonException
                Console.WriteLine("Failed parsing JSON")
            End Try
        End If
        Return Nothing
    End Function

    Public Sub WriteTokenDataFile(obj As Object, filename As String)
        Console.WriteLine("Wrote data file")
        File.WriteAllText(filename, JsonSerializer.Serialize(obj, s_jsonSerializerOptions))
    End Sub

End Module
