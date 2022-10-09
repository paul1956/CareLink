' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Structure FileNameStruct
    Public FullFileNameWithPath As String
    Public FileNameWithExtension As String

    Public Sub New(fullFileNameWithPath As String, fileNameWithExtension As String)
        Me.FullFileNameWithPath = fullFileNameWithPath
        Me.FileNameWithExtension = fileNameWithExtension
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf obj Is FileNameStruct) Then
            Return False
        End If

        Dim other As FileNameStruct = DirectCast(obj, FileNameStruct)
        Return FullFileNameWithPath = other.FullFileNameWithPath AndAlso
               FileNameWithExtension = other.FileNameWithExtension
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(FullFileNameWithPath, FileNameWithExtension)
    End Function

    Public Sub Deconstruct(ByRef fullFileNameWithPath As String, ByRef fileNameWithExtension As String)
        fullFileNameWithPath = Me.FullFileNameWithPath
        fileNameWithExtension = Me.FileNameWithExtension
    End Sub

    Public Shared Widening Operator CType(value As FileNameStruct) As (FullFileNameWithPath As String, FileNameWithExtension As String)
        Return (value.FullFileNameWithPath, value.FileNameWithExtension)
    End Operator

    Public Shared Widening Operator CType(value As (FullFileNameWithPath As String, FileNameWithExtension As String)) As FileNameStruct
        Return New FileNameStruct(value.FullFileNameWithPath, value.FileNameWithExtension)
    End Operator
End Structure
