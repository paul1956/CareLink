﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Structure TabLocation
    Public page As Integer
    Public tab As Integer

    Public Sub New(page As Integer, tab As Integer)
        Me.page = page
        Me.tab = tab
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf obj Is TabLocation) Then
            Return False
        End If

        Dim other As TabLocation = DirectCast(obj, TabLocation)
        Return page = other.page AndAlso
               tab = other.tab
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(page, tab)
    End Function

    Public Sub Deconstruct(ByRef page As Integer, ByRef tab As Integer)
        page = Me.page
        tab = Me.tab
    End Sub

    Public Shared Widening Operator CType(value As TabLocation) As (page As Integer, tab As Integer)
        Return (value.page, value.tab)
    End Operator

    Public Shared Widening Operator CType(value As (page As Integer, tab As Integer)) As TabLocation
        Return New TabLocation(value.page, value.tab)
    End Operator

End Structure
