' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization

''' <summary>
'''  Represents a date and time value stored as an OLE Automation date (OADate).
'''  Provides arithmetic, comparison, and conversion operators,
'''  as well as utility methods for working with OADate values.
''' </summary>
<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class OADate
    Implements IComparable(Of OADate)

    ''' <summary>
    '''  The underlying OLE Automation date value.
    ''' </summary>
    Private ReadOnly _oADate As Double

    ''' <summary>
    '''  Initializes a new instance of the <see cref="OADate"/> class
    '''  from a <see cref="Double"/> OADate value.
    ''' </summary>
    ''' <param name="oADateAsDouble">
    '''  The OLE Automation date as a <see cref="Double"/>.
    ''' </param>
    Public Sub New(oADateAsDouble As Double)
        _oADate = oADateAsDouble
    End Sub

    ''' <summary>
    '''  Initializes a new instance of the <see cref="OADate"/> class
    '''  from a <see cref="Date"/>.
    ''' </summary>
    ''' <param name="asDate">The <see cref="Date"/> to convert to OADate.</param>
    Public Sub New(asDate As Date)
        _oADate = asDate.ToOADate()
    End Sub

    ''' <summary>
    '''  Finalizer for the <see cref="OADate"/> class.
    ''' </summary>
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    ''' <summary>
    '''  Returns a string for debugger display, showing the date and time in short format.
    ''' </summary>
    ''' <returns>A string representation of the OADate value.</returns>
    Private Function GetDebuggerDisplay() As String
        Return Date.FromOADate(_oADate).ToShortDateTime
    End Function

    ''' <summary>
    '''  Subtracts one <see cref="OADate"/> from another.
    ''' </summary>
    ''' <param name="v1">The minuend.</param>
    ''' <param name="v2">The subtrahend.</param>
    ''' <returns>A new <see cref="OADate"/> representing the difference.</returns>
    Public Shared Operator -(v1 As OADate, v2 As OADate) As OADate
        Return New OADate(oADateAsDouble:=v1._oADate - v2._oADate)
    End Operator

    ''' <summary>
    '''  Adds two <see cref="OADate"/> values.
    ''' </summary>
    ''' <param name="v1">The first operand.</param>
    ''' <param name="v2">The second operand.</param>
    ''' <returns>A new <see cref="OADate"/> representing the sum.</returns>
    Public Shared Operator +(v1 As OADate, v2 As OADate) As OADate
        Return New OADate(oADateAsDouble:=v1._oADate + v2._oADate)
    End Operator

    ''' <summary>
    '''  Determines whether one <see cref="OADate"/> is less than another.
    ''' </summary>
    Public Shared Operator <(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate < v2._oADate
    End Operator

    ''' <summary>
    '''  Determines whether one <see cref="OADate"/> is less than or equal to another.
    ''' </summary>
    Public Shared Operator <=(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate <= v2._oADate
    End Operator

    ''' <summary>
    '''  Determines whether two <see cref="OADate"/> values are not equal.
    ''' </summary>
    Public Shared Operator <>(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate <> v2._oADate
    End Operator

    ''' <summary>
    '''  Determines whether two <see cref="OADate"/> values are equal.
    ''' </summary>
    Public Shared Operator =(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate = v2._oADate
    End Operator

    ''' <summary>
    '''  Determines whether one <see cref="OADate"/> is greater than another.
    ''' </summary>
    Public Shared Operator >(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate > v2._oADate
    End Operator

    ''' <summary>
    '''  Determines whether one <see cref="OADate"/> is greater than or equal to another.
    ''' </summary>
    Public Shared Operator >=(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate >= v2._oADate
    End Operator

    ''' <summary>
    '''  Converts an <see cref="OADate"/> to a <see cref="Double"/>.
    ''' </summary>
    ''' <param name="d">The <see cref="OADate"/> to convert.</param>
    ''' <returns>The underlying <see cref="Double"/> value.</returns>
    Public Shared Widening Operator CType(d As OADate) As Double
        Return d._oADate
    End Operator

    ''' <summary>
    '''  Compares the current <see cref="OADate"/> with another <see cref="OADate"/>.
    ''' </summary>
    ''' <param name="other">The other <see cref="OADate"/> to compare to.</param>
    ''' <returns>
    '''  A value less than zero if this instance is less than <paramref name="other"/>,
    '''  zero if they are equal, or greater than zero if this instance is greater.
    ''' </returns>
    Public Function CompareTo(other As OADate) As Integer Implements IComparable(Of OADate).CompareTo
        Return _oADate.CompareTo(value:=other._oADate)
    End Function

    ''' <summary>
    '''  Determines whether the specified object is equal to the current <see cref="OADate"/>.
    ''' </summary>
    ''' <param name="obj">The object to compare with the current instance.</param>
    ''' <returns><see langword="True"/> if the objects are equal;
    ''' otherwise, <see langword="False"/>.</returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        Dim oA As OADate = TryCast(obj, OADate)
        Return oA IsNot Nothing AndAlso _oADate = oA._oADate
    End Function

    ''' <summary>
    '''  Returns a hash code for the current <see cref="OADate"/>.
    ''' </summary>
    ''' <returns>A hash code for the current object.</returns>
    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode()
    End Function

    ''' <summary>
    '''  Returns a string representation of the OADate value.
    ''' </summary>
    ''' <returns>A string representation of the OADate value.</returns>
    Public Overrides Function ToString() As String
        Return _oADate.ToString(format:="F37", provider:=CultureInfo.CurrentUICulture)
    End Function

    ''' <summary>
    '''  Determines whether the current <see cref="OADate"/> is within 6 minutes
    '''  of the specified <see cref="OADate"/>.
    ''' </summary>
    ''' <param name="xValue">The <see cref="OADate"/> to compare to.</param>
    ''' <returns>
    '''  <see langword="True"/> if within 6 minutes;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Function Within6Min(xValue As OADate) As Boolean
        Return ((Me + SixMinuteOADate) <= xValue) OrElse xValue < Me
    End Function


End Class
