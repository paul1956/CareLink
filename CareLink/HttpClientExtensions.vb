Imports System.Net.Http
Imports System.Runtime.CompilerServices
Imports System.Text

Module HttpClientExtensions

    <Extension>
    Public Function [Get](client As HttpClient, url As String, Optional headers As Dictionary(Of String, String) = Nothing, Optional params As Dictionary(Of String, String) = Nothing, Optional data As Dictionary(Of String, String) = Nothing) As HttpResponseMessage
        If headers IsNot Nothing Then
            client.DefaultRequestHeaders.Clear()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next
        End If
        If params IsNot Nothing Then
            url &= "?"
            For Each header As KeyValuePair(Of String, String) In params
                url &= $"{header.Key}={header.Value}&"
            Next
            url = url.TrimEnd("&"c)
        End If
        If data Is Nothing Then
            Return client.GetAsync(url).Result
        End If
        Dim formContent As HttpContent = New FormUrlEncodedContent(params.ToList())
        Return client.PostAsync(url, formContent).Result
    End Function

    <Extension>
    Public Function Post(client As HttpClient, url As String, Optional _headers As Dictionary(Of String, String) = Nothing, Optional params As Dictionary(Of String, String) = Nothing, Optional data As Dictionary(Of String, String) = Nothing) As HttpResponseMessage
        client.DefaultRequestHeaders.Clear()
        If params IsNot Nothing Then
            url &= "?"
            For Each header As KeyValuePair(Of String, String) In params
                url &= $"{header.Key}={header.Value}&"
            Next
            url = url.TrimEnd("&"c)
        End If
        Dim FormData As FormUrlEncodedContent = New FormUrlEncodedContent(data.ToList())
        For Each header As KeyValuePair(Of String, String) In _headers
            If header.Key = "Content-Type" Then
                'FormData.Headers.Add(header.Key,header.Value})
            Else
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value)
            End If
        Next
        Return client.PostAsync(url, FormData).Result
    End Function

    <Extension>
    Public Function Text(client As HttpResponseMessage) As String
        Return client.Content.ReadAsStringAsync().Result
    End Function

End Module
