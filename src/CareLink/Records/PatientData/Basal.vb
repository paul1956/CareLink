' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Basal
    Implements IEquatable(Of Basal)

    <DisplayName("Active Basal Pattern")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    <JsonPropertyName("activeBasalPattern")>
    Public Property ActiveBasalPattern As String

    <DisplayName("Basal Rate")>
    <Column(Order:=1, TypeName:=NameOf([Double]))>
    <JsonPropertyName("basalRate")>
    Public Property BasalRate As Double

    <DisplayName("Temp Basal Rate")>
    <Column(Order:=2, TypeName:=NameOf([Double]))>
    <JsonPropertyName("tempBasalRate")>
    Public Property TempBasalRate As Double

    <DisplayName("Temp Basal Percentage")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    <JsonPropertyName("tempBasalPercentage")>
    Public Property tempBasalPercentage As Single

    <DisplayName("Temp Basal Type")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    <JsonPropertyName("tempBasalType")>
    Public Property tempBasalType As String

    <DisplayName("Preset Temp Name")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    <JsonPropertyName("presetTempName")>
    Public Property presetTempName As String

    <DisplayName("Temp Basal Duration Remaining")>
    <Column(Order:=6, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("tempBasalDurationRemaining")>
    Public Property tempBasalDurationRemaining As Integer

    Public Shared Operator <>(left As Basal, right As Basal) As Boolean
        Return Not left = right
    End Operator

    Public Shared Operator =(left As Basal, right As Basal) As Boolean
        Return EqualityComparer(Of Basal).Default.Equals(left, right)
    End Operator

    ''' <summary>
    '''  Determines whether the current instance is equal to another object.
    ''' </summary>
    ''' <param name="obj">The object to compare with the current instance.</param>
    ''' <returns>
    '''  <see langword="True"/> if the current instance is equal to the specified object;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, Basal))
    End Function

    ''' <summary>
    '''  Gets the basal rate per hour based on the active basal pattern and
    '''  temporary basal settings.
    ''' </summary>
    ''' <returns>
    '''  The basal rate per hour, or NaN if no active basal pattern is set.
    ''' </returns>
    Public Function GetBasalPerHour() As Double
        If Me.ActiveBasalPattern Is Nothing Then
            Return Double.NaN
        End If
        Select Case Me.ActiveBasalPattern
            Case "BASAL1",
                 "BASAL2",
                 "BASAL3",
                 "BASAL4",
                 "BASAL5",
                 "WORKDAY",
                 "DAYOFF",
                 "SICKDAY"
                Return Me.BasalRate
            Case Else
                Return If(Me.tempBasalPercentage > 0,
                          Math.Max(Me.BasalRate, Me.TempBasalRate),
                          Math.Min(Me.BasalRate, Me.TempBasalRate))
        End Select
    End Function

    ''' <summary>
    '''  Gets the type of basal delivery based on the active basal pattern and
    '''  temporary basal settings.
    ''' </summary>
    ''' <returns>
    '''  A string representing the type of basal delivery.
    ''' </returns>
    Public Function GetBasalType() As String
        Select Case True
            Case Not String.IsNullOrWhiteSpace(Me.ActiveBasalPattern)
                Return Me.ActiveBasalPattern
            Case Not String.IsNullOrWhiteSpace(Me.presetTempName)
                Return Me.presetTempName
            Case Not String.IsNullOrWhiteSpace(Me.presetTempName)
                Return Me.tempBasalType
            Case Else
                Return "MANUAL_BASAL_DELIVERY"
        End Select
    End Function

    ''' <summary>
    '''  Gets the hash code for the current instance.
    ''' </summary>
    ''' <returns>
    '''  An integer representing the hash code of the current instance.
    ''' </returns>
    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(
            Me.ActiveBasalPattern,
            Me.BasalRate,
            Me.presetTempName,
            Me.tempBasalDurationRemaining,
            Me.tempBasalPercentage,
            Me.TempBasalRate,
            Me.tempBasalType)
    End Function

    ''' <summary>
    '''  Determines whether the current instance is equal to another
    '''  instance of the same type.
    ''' </summary>
    ''' <param name="other">The other instance to compare with.</param>
    ''' <returns>
    '''  <see langword="True"/> if the current instance is equal to the other instance;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Overloads Function Equals(other As Basal) As Boolean _
        Implements IEquatable(Of Basal).Equals

        Return other IsNot Nothing AndAlso
               Me.ActiveBasalPattern = other.ActiveBasalPattern AndAlso
               Me.BasalRate = other.BasalRate AndAlso
               Me.presetTempName = other.presetTempName AndAlso
               Me.tempBasalDurationRemaining = other.tempBasalDurationRemaining AndAlso
               Me.tempBasalPercentage = other.tempBasalPercentage AndAlso
               Me.TempBasalRate = other.TempBasalRate AndAlso
               Me.tempBasalType = other.tempBasalType
    End Function

End Class
