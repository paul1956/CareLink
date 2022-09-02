' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class OADate
    Private ReadOnly _oADate As Double

    Public Sub New(oADate As Double)
        _oADate = oADate
    End Sub

    Public Sub New(oADate As Date)
        _oADate = oADate.ToOADate()
    End Sub

    Private Function GetDebuggerDisplay() As String
        Return Date.FromOADate(_oADate).ToShortDateTimeString
    End Function

    Public Shared Operator -(v1 As OADate, v2 As OADate) As OADate
        Return New OADate(v1.AsDouble - v2.AsDouble)
    End Operator

    Public Shared Operator +(v1 As OADate, v2 As OADate) As OADate
        Return New OADate(v1.AsDouble + v2.AsDouble)
    End Operator

    Public Shared Operator <(v1 As OADate, v2 As OADate) As Boolean
        Return v1.AsDouble < v2.AsDouble
    End Operator

    Public Shared Operator <=(v1 As OADate, v2 As OADate) As Boolean
        Return v1.AsDouble <= v2.AsDouble
    End Operator

    Public Shared Operator <>(v1 As OADate, v2 As OADate) As Boolean
        Return v1.AsDouble <> v2.AsDouble
    End Operator

    Public Shared Operator =(v1 As OADate, v2 As OADate) As Boolean
        Return v1.AsDouble = v2.AsDouble
    End Operator

    Public Shared Operator >(v1 As OADate, v2 As OADate) As Boolean
        Return v1.AsDouble > v2.AsDouble
    End Operator

    Public Shared Operator >=(v1 As OADate, v2 As OADate) As Boolean
        Return v1.AsDouble >= v2.AsDouble
    End Operator

    Public Function AsDouble() As Double
        Return _oADate
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim oA As OADate = TryCast(obj, OADate)
        Return oA IsNot Nothing AndAlso _oADate = oA.AsDouble
    End Function

    Public Function Within10Minutes(currentValue As OADate) As Boolean

        If Me + s_sixMinuteOADate <= currentValue OrElse currentValue < Me Then
            Return True
        End If
        Return False
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return MyBase.ToString()
    End Function
End Class
