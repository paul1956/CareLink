' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class CarbRatioRecord
    Implements IEquatable(Of CarbRatioRecord)

    <DisplayName(NameOf(StartTime))>
    <Column(Order:=0, TypeName:=NameOf(TimeOnly))>
    Public Property StartTime As TimeOnly

    <DisplayName(NameOf(EndTime))>
    <Column(Order:=1, TypeName:=NameOf(TimeOnly))>
    Public Property EndTime As TimeOnly

    <Column(Order:=2, TypeName:=NameOf(System.Single))>
    Public Property CarbRatio As Single

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, CarbRatioRecord))
    End Function

    Public Overloads Function Equals(other As CarbRatioRecord) As Boolean Implements IEquatable(Of CarbRatioRecord).Equals
        Return other IsNot Nothing AndAlso
               Me.StartTime.Equals(other.StartTime) AndAlso
               Me.EndTime.Equals(other.EndTime) AndAlso
               Me.CarbRatio = other.CarbRatio
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Me.StartTime, Me.EndTime, Me.CarbRatio)
    End Function

End Class
