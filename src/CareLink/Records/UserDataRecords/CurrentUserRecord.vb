' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CurrentUserRecord
    Implements IEquatable(Of CurrentUserRecord)

    Public Sub New(userName As String)
        Me.UserName = userName
        Me.UseAdvancedAitDecay = CheckState.Indeterminate
    End Sub

    Public Property CarbRatios As New List(Of CarbRatioRecord)
    Public Property InsulinRealAit As Single
    Public Property InsulinTypeName As String
    Public Property PumpAit As Single
    Public Property UseAdvancedAitDecay As CheckState
    Public Property UserName As String

    Friend Function Clone() As CurrentUserRecord

        Return New CurrentUserRecord(Me.UserName) With {
                .PumpAit = Me.PumpAit,
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
               EqualityComparer(Of List(Of CarbRatioRecord)).Default.Equals(Me.CarbRatios, other.CarbRatios) AndAlso
               Me.PumpAit.Equals(other.PumpAit) AndAlso
               Me.InsulinRealAit.Equals(other.InsulinRealAit) AndAlso
               Me.InsulinTypeName = other.InsulinTypeName AndAlso
               Me.UseAdvancedAitDecay = other.UseAdvancedAitDecay AndAlso
               Me.UserName = other.UserName
    End Function

    Public Function GetActiveInsulinIncrements() As Integer
        If Me.UseAdvancedAitDecay = CheckState.Checked Then
            Return CInt(Me.InsulinRealAit * 60 / 5)
        Else
            Return CInt(Me.PumpAit * 60 / 5)
        End If

    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Me.PumpAit, Me.CarbRatios, Me.InsulinRealAit, Me.InsulinTypeName, Me.UseAdvancedAitDecay, Me.UserName)
    End Function

    Public Function GetPumpAitString() As String
        Dim hours As Integer = CInt(Me.PumpAit)
        Return $"Pump AIT {New TimeSpan(hours, CInt((Me.PumpAit - hours) * 60), 0):h\:mm}"
    End Function

    Public Function GetIobChartTitle() As String
        Dim msg As String
        If CurrentUser.UseAdvancedAitDecay = CheckState.Checked Then
            msg = $"Advanced Decay, AIT will Decay over {Me.InsulinRealAit} hours"
        Else
            msg = $"AIT will Decay over {Me.PumpAit} hours"
        End If
        Return $"Running Insulin On Board (IOB) {s_listOfManualBasal.GetSubTitle(msg)}"
    End Function

End Class
