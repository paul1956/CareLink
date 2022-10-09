' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Friend Module DataFileNameManagement

    ''' <summary>
    ''' Returns a unique file name in MyDocuments of the form baseName(CultureCode).Extension
    ''' given an filename and culture as a seed
    ''' </summary>
    ''' <param Name="baseName">The first part of the file name</param>
    ''' <param Name="cultureName">A valid Culture Name in the form of language-CountryCode</param>
    ''' <param Name="extension">The extension for the file</param>
    ''' <returns>
    ''' A unique file name valid in MyDocuments folder or
    ''' An empty file name on error.
    ''' </returns>
    ''' <param Name="MustBeUnique"></param>
    Public Function GetDataFileName(baseName As String, cultureName As String, extension As String, MustBeUnique As Boolean) As FileNameStruct
        If String.IsNullOrWhiteSpace(baseName) Then
            Throw New ArgumentException($"'{NameOf(baseName)}' cannot be null or whitespace.", NameOf(baseName))
        End If

        If String.IsNullOrWhiteSpace(cultureName) Then
            Throw New ArgumentException($"'{NameOf(cultureName)}' cannot be null or whitespace.", NameOf(cultureName))
        End If

        If String.IsNullOrWhiteSpace(extension) Then
            Throw New ArgumentException($"'{NameOf(extension)}' cannot be null or whitespace.", NameOf(extension))
        End If

        Try
            Dim fileNameWithCultureAndExtension As String = $"{baseName}({cultureName}).{extension}"
            Dim fileNameWithPath As String = Path.Combine(MyDocumentsPath, fileNameWithCultureAndExtension)

            If MustBeUnique AndAlso File.Exists(fileNameWithPath) Then
                'Get unique file name
                Dim lCount As Long
                Do
                    lCount += 1
                    fileNameWithPath = Path.Combine(MyDocumentsPath, $"{$"{baseName}({cultureName})"}{lCount}.{extension}")
                    fileNameWithCultureAndExtension = Path.GetFileName(fileNameWithPath)
                Loop While File.Exists(fileNameWithPath)
            End If

            Return New FileNameStruct(fileNameWithPath, fileNameWithCultureAndExtension)
        Catch ex As Exception
        End Try
        Return New FileNameStruct

    End Function

End Module
