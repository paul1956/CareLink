' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Friend Module UniqueFilename

    ''' <summary>
    ''' Returns a unique file name given an filename as a seed
    ''' </summary>
    ''' <param name="sFileName"></param>
    ''' The path and file name to be used as a seed.
    ''' <returns>
    ''' A unique indexed version of the filename from the input parameter "sFilename".
    ''' Returns an empty file name on error.
    ''' </returns>
    Public Function GetUniqueFileNameWithPath(sFileName As String) As String
        If String.IsNullOrEmpty(sFileName) Then
            Throw New ArgumentException($"'{NameOf(sFileName)}' cannot be null or empty.", NameOf(sFileName))
        End If

        Try
            Dim sFileNoExtension As String = Path.GetFileNameWithoutExtension(sFileName)
            Dim sExtension As String = Path.GetExtension(sFileName)
            If Not File.Exists(Path.Combine(MyDocumentsPath, $"{sFileNoExtension}.{sExtension}")) Then
                Return sFileName
            End If

            'Get unique file name
            Dim lCount As Long
            Do
                lCount += 1
            Loop While File.Exists($"{sFileNoExtension}{lCount}.{sExtension}")
            Return Path.Combine(MyDocumentsPath, $"{sFileNoExtension}{lCount}.{sExtension}")
        Catch ex As Exception
        End Try
        Return ""

    End Function

End Module
