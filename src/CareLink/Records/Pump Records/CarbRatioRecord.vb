' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

''' <summary>
'''  Represents a carbohydrate ratio record, including start and end times and the
'''  carb ratio value. Used for configuring insulin pump carbohydrate ratio schedules.
''' </summary>
Public Class CarbRatioRecord
    Implements IEquatable(Of CarbRatioRecord)

    ''' <summary>
    '''  Backing field for the <see cref="EndTime"/> property.
    ''' </summary>
    Private _endTime As TimeOnly

    ''' <summary>
    '''  Gets or sets the start time for this carb ratio period.
    ''' </summary>
    <DisplayName("Start Time")>
    <Column(Order:=0, TypeName:=NameOf(TimeOnly))>
    Public Property StartTime As TimeOnly

    ''' <summary>
    '''  Gets or sets the end time for this carb ratio period.
    '''  If set to <see cref="Midnight"/>, it is replaced with <see cref="Eleven59"/>.
    ''' </summary>
    <DisplayName("End Time")>
    <Column(Order:=1, TypeName:=NameOf(TimeOnly))>
    Public Property EndTime As TimeOnly
        Get
            Return _endTime
        End Get
        Set
            _endTime = If(Value = Midnight,
                          Eleven59,
                          Value)
        End Set
    End Property

    ''' <summary>
    '''  Gets or sets the carbohydrate ratio for this period.
    ''' </summary>
    <DisplayName("Carb Ratio")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public Property CarbRatio As Single

    ''' <summary>
    '''  Determines whether the specified object is equal to the current
    '''  <see cref="CarbRatioRecord"/>.
    ''' </summary>
    ''' <param name="obj">The object to compare with the current object.</param>
    ''' <returns>
    '''  <see langword="True"/> if the objects are equal;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, CarbRatioRecord))
    End Function

    ''' <summary>
    '''  Determines whether the specified <see cref="CarbRatioRecord"/> is equal
    '''  to the current <see cref="CarbRatioRecord"/>.
    ''' </summary>
    ''' <param name="other">
    '''  The <see cref="CarbRatioRecord"/> to compare with the current object.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if the records are equal;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Overloads Function Equals(other As CarbRatioRecord) As Boolean _
        Implements IEquatable(Of CarbRatioRecord).Equals

        Return other IsNot Nothing AndAlso
            Me.CarbRatio = other.CarbRatio AndAlso
            Me.StartTime.Equals(other.StartTime) AndAlso
            ((other.EndTime - Me.EndTime).Duration > Eleven30Span OrElse
            (other.EndTime - Me.EndTime).Duration < ThirtyMinuteSpan)
    End Function

    ''' <summary>
    '''  Returns a hash code for the current <see cref="CarbRatioRecord"/>.
    ''' </summary>
    ''' <returns>A hash code for the current object.</returns>
    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Me.StartTime, Me.EndTime, Me.CarbRatio)
    End Function

End Class
