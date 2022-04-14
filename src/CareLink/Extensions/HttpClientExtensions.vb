' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Runtime.CompilerServices

Module HttpClientExtensions

    <Extension>
    Public Function [Get](client As HttpClient, url As String, Optional headers As Dictionary(Of String, String) = Nothing, Optional params As Dictionary(Of String, String) = Nothing, Optional data As Dictionary(Of String, String) = Nothing) As HttpResponseMessage
        Dim formContent As HttpContent = Nothing
        If data IsNot Nothing Then
            formContent = New FormUrlEncodedContent(data.ToList())
        End If
        If headers IsNot Nothing Then
            client.DefaultRequestHeaders.Clear()
            For Each header As KeyValuePair(Of String, String) In headers
                If header.Key = "Content-Type" Then
                    If formContent IsNot Nothing Then
                        formContent.Headers.ContentType.MediaType = header.Value
                    End If
                Else
                    client.DefaultRequestHeaders.Add(header.Key, header.Value)
                End If
            Next
        End If
        If params IsNot Nothing Then
            url &= "?"
            For Each param As KeyValuePair(Of String, String) In params
                url &= $"{param.Key}={param.Value}&"
            Next
            url = url.TrimEnd("&"c)
        End If
        If data Is Nothing Then
            Return client.GetAsync(url).Result
        End If
        Return client.PostAsync(url, formContent).Result
    End Function

    <Extension>
    Public Function Post(client As HttpClient, url As String, Optional headers As Dictionary(Of String, String) = Nothing, Optional params As Dictionary(Of String, String) = Nothing, Optional data As Dictionary(Of String, String) = Nothing) As HttpResponseMessage
        If params IsNot Nothing Then
            url &= "?"
            For Each header As KeyValuePair(Of String, String) In params
                url &= $"{header.Key}={header.Value}&"
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
        Return client.PostAsync(url, formData).Result
    End Function

    <Extension>
    Public Function Text(client As HttpResponseMessage) As String
        Dim result As String = client.Content.ReadAsStringAsync().Result
        Return result
    End Function

End Module
