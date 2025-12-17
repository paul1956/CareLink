' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CurrentUserRecord
    Implements IEquatable(Of CurrentUserRecord)

    Public Sub New(userName As String, useAdvancedAitDecay As CheckState)
        Me.UserName = userName
        Me.UseAdvancedAitDecay = useAdvancedAitDecay
        Me.CurrentTarget = If(NativeMmolL,
                              Target6_7mmol,
                              Target120mgDl)
    End Sub

    Public Property CarbRatios As New List(Of CarbRatioRecord)
    Public Property CurrentTarget As Single
    Public Property InsulinRealAit As Single
    Public Property InsulinTypeName As String
    Public Property PumpAit As Single
    Public Property UseAdvancedAitDecay As CheckState
    Public ReadOnly Property UserName As String

    Friend Function Clone() As CurrentUserRecord

        Return New CurrentUserRecord(Me.UserName, Me.UseAdvancedAitDecay) With {
                .CarbRatios = Me.CarbRatios,
                .CurrentTarget = Me.CurrentTarget,
                .InsulinRealAit = Me.InsulinRealAit,
                .InsulinTypeName = Me.InsulinTypeName,
                .PumpAit = Me.PumpAit
                }
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(other:=TryCast(obj, CurrentUserRecord))
    End Function

    Public Overloads Function Equals(other As CurrentUserRecord) As Boolean _
        Implements IEquatable(Of CurrentUserRecord).Equals

        If other Is Nothing Then
            Return False
        End If
        Dim x As List(Of CarbRatioRecord) = Me.CarbRatios
        Dim y As List(Of CarbRatioRecord) = other.CarbRatios
        Return _
            EqualityComparer(Of List(Of CarbRatioRecord)).Default.Equals(x, y) AndAlso
               Me.CarbRatios.Equals(obj:=other.CarbRatios) AndAlso
               Me.CurrentTarget.Equals(obj:=other.CurrentTarget) AndAlso
               Me.InsulinRealAit.Equals(obj:=other.InsulinRealAit) AndAlso
               Me.InsulinTypeName = other.InsulinTypeName AndAlso
               (Me.PumpAit - other.PumpAit).AlmostZero AndAlso
               Me.UseAdvancedAitDecay = other.UseAdvancedAitDecay AndAlso
               Me.UserName = other.UserName
    End Function

    Public Function GetActiveInsulinIncrements() As Integer
        Return If(Me.UseAdvancedAitDecay = CheckState.Checked,
                  CInt(Me.InsulinRealAit * 12),
                  CInt(Me.PumpAit * 12))

    End Function

    Public Function GetCarbRatio(forTime As TimeOnly) As Single

        For Each carbRatio As CarbRatioRecord In Me.CarbRatios
            If forTime.IsBetween(start:=carbRatio.StartTime, [end]:=carbRatio.EndTime) Then
                Return carbRatio.CarbRatio
            End If
        Next
        Stop
        Throw UnreachableException(paramName:=NameOf(forTime))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(
            Me.PumpAit,
            Me.CarbRatios,
            Me.InsulinRealAit,
            Me.InsulinTypeName,
            Me.UseAdvancedAitDecay,
            Me.UserName)
    End Function

    Public Function GetPumpAitString() As String
        Return $"Pump AIT {Me.PumpAit.ToHoursMinutes}".CleanSpaces
    End Function

End Class
