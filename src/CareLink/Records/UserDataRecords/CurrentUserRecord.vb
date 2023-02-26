' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CurrentUserRecord
    Implements IEquatable(Of CurrentUserRecord)

    Public Sub New(userName As String)
        Me.UserName = userName
        Me.UseAdvancedAitDecay = CheckState.Indeterminate
    End Sub

    Public Property Ait As TimeSpan?
    Public Property CarbRatios As New List(Of CarbRatioRecord)
    Public Property InsulinRealAit As TimeSpan
    Public Property InsulinTypeName As String
    Public Property UseAdvancedAitDecay As CheckState
    Public Property UserName As String

    Friend Function Clone() As CurrentUserRecord

        Return New CurrentUserRecord(Me.UserName) With {
            .Ait = Me.Ait,
            .CarbRatios = Me.CarbRatios,
            .InsulinRealAit = Me.InsulinRealAit,
            .InsulinTypeName = Me.InsulinTypeName,
            .UseAdvancedAitDecay = Me.UseAdvancedAitDecay
        }
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, CurrentUserRecord))
    End Function

    Public Overloads Function Equals(other As CurrentUserRecord) As Boolean Implements IEquatable(Of CurrentUserRecord).Equals
        Return other IsNot Nothing AndAlso
               EqualityComparer(Of TimeSpan?).Default.Equals(Me.Ait, other.Ait) AndAlso
               EqualityComparer(Of List(Of CarbRatioRecord)).Default.Equals(Me.CarbRatios, other.CarbRatios) AndAlso
               Me.InsulinRealAit.Equals(other.InsulinRealAit) AndAlso
               Me.InsulinTypeName = other.InsulinTypeName AndAlso
               Me.UseAdvancedAitDecay = other.UseAdvancedAitDecay AndAlso
               Me.UserName = other.UserName
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Me.Ait, Me.CarbRatios, Me.InsulinRealAit, Me.InsulinTypeName, Me.UseAdvancedAitDecay, Me.UserName)
    End Function

End Class
