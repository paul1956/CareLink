' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http

Friend Module Network

    Public Sub DownloadFile(url As String, destinationPath As String, credentials As NetworkCredential, progress As IProgress(Of Double))

        Task.Run(Async Function()
                     Await DownloadFileAsync(url, destinationPath, credentials, progress)
                 End Function).Wait()

    End Sub

    Public Async Function DownloadFileAsync(url As String, destinationPath As String, credentials As NetworkCredential, progress As IProgress(Of Double)) As Task

        Using handler As New HttpClientHandler() With
        {
            .Credentials = credentials
        }

            Using httpClient As New HttpClient(handler)
                Using response As HttpResponseMessage = Await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)
                    Using responseStream As Stream = Await response.Content.ReadAsStreamAsync()
                        Using fileStream As New FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None)

                            Dim buffer(8191) As Byte
                            Dim totalBytesRead As Long = 0
                            Dim bytesRead As Integer

                            While (Await responseStream.ReadAsync(buffer)) > 0
                                Await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead))
                                totalBytesRead += bytesRead

                                Dim contentLength? As Long = response.Content.Headers.ContentLength
                                If Not contentLength.HasValue Then
                                    Continue While
                                End If

                                Dim percentage As Double = CDbl(totalBytesRead) / CDbl(contentLength.Value) * 100
                                progress.Report(percentage)

                            End While
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Function

End Module
