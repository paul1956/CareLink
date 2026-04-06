' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text

Public Module LoginHelper
    Public Sub GetLoginData(isUsRegion As Boolean, tokenData As TokenData)
        If tokenData Is Nothing Then
            ' Get the embedded EXE as a byte array
            Dim buffer() As Byte = My.Resources.carelink_carepartner_api_login
            ' Create a temporary file for the EXE
            Dim exePath As String = $"{Path.GetTempFileName()}.exe"
            ' Write the EXE to the temporary file
            Using fs As New FileStream(path:=exePath, mode:=FileMode.Create)
                fs.Write(buffer, offset:=0, count:=buffer.Length)
                fs.Flush()
            End Using

            Dim isUsRegionStr As String = If(isUsRegion,
                                             "--us",
                                             String.Empty)

            ' Create a temporary file for the JSON output
            Dim sourceFileName As String = $"{Path.GetTempFileName()}.json"
            Dim startInfo As New ProcessStartInfo With {
                    .FileName = exePath,
                    .Arguments = $"{If(isUsRegion, "--us ", String.Empty)} --output {sourceFileName}",
                    .RedirectStandardOutput = True,
                    .RedirectStandardError = True,
                    .UseShellExecute = False}

            Dim process As New Process With {.StartInfo = startInfo}

            Dim outputBuilder As New StringBuilder()
            Dim errorBuilder As New System.Text.StringBuilder()

            Dim outHandler As System.Diagnostics.DataReceivedEventHandler = Nothing
            Dim errHandler As System.Diagnostics.DataReceivedEventHandler = Nothing
            outHandler = Sub(sender2 As Object, args2 As DataReceivedEventArgs)
                             If args2.Data IsNot Nothing Then
                                 Debug.WriteLine(args2.Data)
                                 outputBuilder.AppendLine(args2.Data)
                             End If
                         End Sub
            errHandler = Sub(sender2 As Object, args2 As DataReceivedEventArgs)
                             If args2.Data IsNot Nothing Then
                                 Debug.WriteLine($"ERR: {args2.Data}")
                                 errorBuilder.AppendLine(args2.Data)
                             End If
                         End Sub

            AddHandler process.OutputDataReceived, outHandler
            AddHandler process.ErrorDataReceived, errHandler

            Try
                process.Start()
                process.BeginOutputReadLine()
                process.BeginErrorReadLine()

                ' Wait until either the process exits or the source file is created.
                While Not process.HasExited AndAlso Not File.Exists(sourceFileName)
                    Threading.Thread.Sleep(200)
                    Application.DoEvents()
                End While

                Dim outputText As String = outputBuilder.ToString()
                Dim standardError As String = errorBuilder.ToString()

                If File.Exists(sourceFileName) Then
                    ' If the helper created the file, stop the process and proceed to move the file.
                    Try
                        If Not process.HasExited Then
                            process.Kill()
                            process.WaitForExit()
                        End If
                    Catch ex As Exception
                        Debug.WriteLine($"Failed to kill process: {ex.Message}")
                    End Try

                    Dim destinationFileName As String = GetLoginDataFileName(s_userName)
                    SafeDeleteFile(path:=destinationFileName)
                    File.Move(sourceFileName, destinationFileName)
                Else
                    ' Process exited without creating the file — nothing to do.
                    Try
                        If Not process.HasExited Then
                            process.WaitForExit()
                        End If
                    Catch
                    End Try
                End If
            Finally
                ' Remove handlers and stop async reads to avoid leaks
                Try
                    process.CancelOutputRead()
                Catch
                End Try
                Try
                    process.CancelErrorRead()
                Catch
                End Try
                Try
                    RemoveHandler process.OutputDataReceived, outHandler
                Catch
                End Try
                Try
                    RemoveHandler process.ErrorDataReceived, errHandler
                Catch
                End Try
                Try
                    process.Dispose()
                Catch
                End Try
            End Try
            SafeDeleteFile(exePath)
        End If
    End Sub
End Module
