' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

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

End Module
