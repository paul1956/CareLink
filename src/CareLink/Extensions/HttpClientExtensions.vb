' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Runtime.CompilerServices
Imports System.Text

Friend Module HttpClientExtensions

    <Extension>
    Public Function [Get](client As HttpClient, url As StringBuilder, ByRef lastError As String, Optional headers As Dictionary(Of String, String) = Nothing, Optional params As Dictionary(Of String, String) = Nothing) As HttpResponseMessage
        If headers IsNot Nothing Then
            client.DefaultRequestHeaders.Clear()
            For Each header As KeyValuePair(Of String, String) In headers
                If header.Key <> "Content-Type" Then
                    client.DefaultRequestHeaders.Add(header.Key, header.Value)
                End If
            Next
        End If
        If params IsNot Nothing Then
            url.Append("?"c)
            For Each param As KeyValuePair(Of String, String) In params
                url.Append($"{param.Key}={param.Value}&")
            Next
            url = url.TrimEnd("&"c)
        End If

        Try
            lastError = Nothing
            Return client.GetAsync(url.ToString).Result
        Catch ex As Exception
            lastError = ex.DecodeException()
            Return New HttpResponseMessage(Net.HttpStatusCode.NotImplemented)
        End Try
    End Function

    <Extension>
    Public Function Post(client As HttpClient, url As StringBuilder, Optional headers As Dictionary(Of String, String) = Nothing, Optional params As Dictionary(Of String, String) = Nothing, Optional data As Dictionary(Of String, String) = Nothing) As HttpResponseMessage
        If params IsNot Nothing Then
            url.Append("?"c)
            For Each header As KeyValuePair(Of String, String) In params
                url.Append($"{header.Key}={header.Value}&")
            Next
            url = url.TrimEnd("&"c)
        End If
        Dim formData As New FormUrlEncodedContent(data.ToList())
        If headers IsNot Nothing Then
            client.DefaultRequestHeaders.Clear()
            For Each header As KeyValuePair(Of String, String) In headers
                If header.Key = "Content-Type" Then
                    formData.Headers.ContentType.MediaType = header.Value
                Else
                    client.DefaultRequestHeaders.Add(header.Key, header.Value)
                End If
            Next
        End If
        Return client.PostAsync(url.ToString, formData).Result
    End Function

    <Extension>
    Public Function ResultText(client As HttpResponseMessage) As String
        Return client.Content.ReadAsStringAsync().Result
    End Function

End Module
