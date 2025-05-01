' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class CarbRatioRecord
    Implements IEquatable(Of CarbRatioRecord)

    Private _endTime As TimeOnly

    <DisplayName("Start Time")>
    <Column(Order:=0, TypeName:=NameOf(TimeOnly))>
    Public Property StartTime As TimeOnly

    <DisplayName("End Time")>
    <Column(Order:=1, TypeName:=NameOf(TimeOnly))>
    Public Property EndTime As TimeOnly
        Get
            Return _endTime
        End Get
        Set
            _endTime = If(Value = Midnight, Eleven59, Value)
        End Set
    End Property

    <DisplayName("Carb Ratio")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public Property CarbRatio As Single

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, CarbRatioRecord))
    End Function

    Public Overloads Function Equals(other As CarbRatioRecord) As Boolean Implements IEquatable(Of CarbRatioRecord).Equals
        Return other IsNot Nothing AndAlso
               Me.CarbRatio = other.CarbRatio AndAlso
               Me.StartTime.Equals(other.StartTime) AndAlso
               ((other.EndTime - other.EndTime).Duration > New TimeSpan(23, 30, 0) OrElse
               (other.EndTime - other.EndTime).Duration < New TimeSpan(0, 30, 0))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Me.StartTime, Me.EndTime, Me.CarbRatio)
    End Function

End Class
