' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.Json

Friend Module HttpClientExtensions

    <Extension>
    Friend Async Function GetRequestAsync(httpClient As HttpClient, authorizeUrl As String, authParams As Dictionary(Of String, String)) As Task(Of JsonElement)
        Dim response As HttpResponseMessage = httpClient.GetAsync(authorizeUrl & "?" & String.Join("&", authParams.Select(Function(kvp) kvp.Key & "=" & Uri.EscapeDataString(kvp.Value)))).Result
        ' This will handle the redirect automatically
        Dim responseContent As String = Await response.Content.ReadAsStringAsync()
        Dim providers As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(responseContent)
        Return providers
    End Function

    ''' <summary>
    '''  Sets the default request headers for the specified <see cref="HttpClient"/> instance using
    '''  the common headers defined in <c>s_common_Headers</c>.
    ''' </summary>
    ''' <param name="httpClient">The <see cref="HttpClient"/> instance to configure.</param>

    <Extension>
    Friend Sub SetDefaultRequestHeaders(ByRef httpClient As HttpClient)
        httpClient.DefaultRequestHeaders.Clear()
        For Each header As KeyValuePair(Of String, String) In s_common_Headers.Sort
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next
    End Sub

    <Extension>
    Public Function [Get](httpClient As HttpClient, requestUri As StringBuilder, ByRef lastError As String, Optional queryParams As Dictionary(Of String, String) = Nothing, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As HttpResponseMessage
        httpClient.SetDefaultRequestHeaders()
        If queryParams IsNot Nothing Then
            requestUri.Append("?"c)
            For Each param As KeyValuePair(Of String, String) In queryParams
                requestUri.Append($"{param.Key}={param.Value}&")
            Next
            requestUri = requestUri.TrimEnd("&"c)
        End If

        Try
            lastError = Nothing
            Dim requestUriString As String = requestUri.ToString
            DebugPrint($"uri={requestUriString} from {memberName}, line {sourceLineNumber}.")
            Return httpClient.GetAsync(requestUriString).Result
        Catch ex As Exception
            lastError = ex.DecodeException()
            Return New HttpResponseMessage(Net.HttpStatusCode.NotImplemented)
        End Try
    End Function

    <Extension>
    Public Function Post(httpClient As HttpClient, url As StringBuilder, Optional headers As Dictionary(Of String, String) = Nothing, Optional params As Dictionary(Of String, String) = Nothing, Optional data As Dictionary(Of String, String) = Nothing) As HttpResponseMessage
        If params IsNot Nothing Then
            url.Append("?"c)
            For Each header As KeyValuePair(Of String, String) In params
                url.Append($"{header.Key}={header.Value}&")
            Next
            url = url.TrimEnd("&"c)
        End If
        Dim formData As FormUrlEncodedContent = Nothing
        If data IsNot Nothing Then
            formData = New FormUrlEncodedContent(data.ToList())
        End If
        If headers IsNot Nothing Then
            httpClient.DefaultRequestHeaders.Clear()
            For Each header As KeyValuePair(Of String, String) In headers
                If header.Key = "Content-Type" AndAlso formData IsNot Nothing Then
                    formData.Headers.ContentType.MediaType = header.Value
                Else
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                End If
            Next
        End If
        Return httpClient.PostAsync(url.ToString, formData).Result
    End Function

    <Extension>
    Public Function PostRequest(ByRef httpClient As HttpClient, url As String, data As Dictionary(Of String, String), headers As Dictionary(Of String, String)) As String
        For Each header As KeyValuePair(Of String, String) In headers
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next

        Dim content As New FormUrlEncodedContent(data)
        Dim response As HttpResponseMessage = httpClient.PostAsync(url, content).Result
        Return response.Content.ReadAsStringAsync().Result
    End Function

    <Extension>
    Public Function ResultText(client As HttpResponseMessage) As String
        Return client.Content.ReadAsStringAsync().Result
    End Function

End Module
