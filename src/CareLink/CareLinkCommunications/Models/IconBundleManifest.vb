' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text

Friend Class IconBundleManifest
    Public Property extracted As Boolean
    Public Property extractedPath As String
    Public Property extractedAtUtc As Date
    Public Property serverTimestamp As Date
    Public Property source As String
    Public Property files As String()

    Public Sub New()
    End Sub

    Public Sub SaveToFile(path As String)
        Dim contents As String = Me.ToJson()
        File.WriteAllText(path, contents, encoding:=Encoding.UTF8)
    End Sub

    Public Shared Function TryLoadFromFile(path As String, ByRef manifest As IconBundleManifest) As Boolean
        manifest = Nothing
        Try
            If Not File.Exists(path) Then
                Return False
            End If
            Dim text As String =
                File.ReadAllText(path, encoding:=Encoding.UTF8)
            manifest = text.FromJson(Of IconBundleManifest)()
            Return manifest IsNot Nothing
        Catch
            manifest = Nothing
            Return False
        End Try
    End Function

End Class
