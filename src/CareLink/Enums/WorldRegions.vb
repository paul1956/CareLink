' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Module WorldRegions

    Public Enum WorldRegion

        <Description("United States")>
        UnitedStates

        <Description("Trial")>
        Trial

        <Description("Africa")>
        Africa

        <Description("Antarctica")>
        Antarctica

        <Description("Asia")>
        Asia

        <Description("Europe")>
        Europe

        <Description("North America")>
        NorthAmerica

        <Description("Oceania")>
        Oceania

        <Description("South America")>
        SouthAmerica

        <Description("Transcontinental")>
        Transcontinental

    End Enum

    <Extension>
    Public Function GetServer(region As WorldRegion) As String
        Select Case region
            Case WorldRegion.UnitedStates
                Return WorldRegion.UnitedStates.ToString
            Case WorldRegion.Trial
                Return WorldRegion.Trial.ToString
        End Select
        Return WorldRegion.Europe.ToString

    End Function

End Module
