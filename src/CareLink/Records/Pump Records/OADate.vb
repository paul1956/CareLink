' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class OADate
    Implements IComparable(Of OADate)
    Private ReadOnly _oADate As Double

    Public Sub New(oADate As Double)
        _oADate = oADate
    End Sub

    Public Sub New(oADate As Date)
        _oADate = oADate.ToOADate()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Function GetDebuggerDisplay() As String
        Return Date.FromOADate(_oADate).ToShortDateTimeString
    End Function

    Public Shared Operator -(v1 As OADate, v2 As OADate) As OADate
        Return New OADate(v1._oADate - v2._oADate)
    End Operator

    Public Shared Operator +(v1 As OADate, v2 As OADate) As OADate
        Return New OADate(v1._oADate + v2._oADate)
    End Operator

    Public Shared Operator <(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate < v2._oADate
    End Operator

    Public Shared Operator <=(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate <= v2._oADate
    End Operator

    Public Shared Operator <>(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate <> v2._oADate
    End Operator

    Public Shared Operator =(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate = v2._oADate
    End Operator

    Public Shared Operator >(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate > v2._oADate
    End Operator

    Public Shared Operator >=(v1 As OADate, v2 As OADate) As Boolean
        Return v1._oADate >= v2._oADate
    End Operator

    Public Shared Widening Operator CType(d As OADate) As Double
        Return d._oADate
    End Operator

    Public Shared Narrowing Operator CType(b As Byte) As OADate
        Return New OADate(b)
    End Operator

    Public Function CompareTo(other As OADate) As Integer Implements IComparable(Of OADate).CompareTo
        Return _oADate.CompareTo(other._oADate)
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim oA As OADate = TryCast(obj, OADate)
        Return oA IsNot Nothing AndAlso _oADate = oA._oADate
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return _oADate.ToString("F37", CurrentUICulture)
    End Function

    Public Function Within10Minutes(currentOADate As OADate) As Boolean

        If ((Me + s_sixMinuteOADate) <= currentOADate) OrElse currentOADate < Me Then
            Return True
        End If
        Return False
    End Function

End Class
