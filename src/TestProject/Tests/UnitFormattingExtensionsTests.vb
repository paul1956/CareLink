' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports Xunit

Public Class UnitFormattingExtensionsTests

    <Theory>
    <InlineData(-1, "-1 days")>
    <InlineData(0, "0 days")>
    <InlineData(1, "1 day")>
    <InlineData(2, "2 days")>
    Public Sub ToUnits_FormatsSignedCounts(count As Integer, expected As String)
        Assert.Equal(expected,
                     actual:=count.ToUnits(unit:="day"))
    End Sub

    <Fact>
    Public Sub ToUnits_FormatsUnsignedAndLongCounts()
        Assert.Equal(expected:="1 hour",
                     actual:=1UI.ToUnits(unit:="hour"))
        Assert.Equal(expected:="2 hours",
                     actual:=2UI.ToUnits(unit:="hour"))
        Assert.Equal(expected:=$"{UInteger.MaxValue:N0} seconds",
                     actual:=UInteger.MaxValue.ToUnits(unit:="second", numberFormat:="N0"))
        Assert.Equal(expected:=$"{Long.MaxValue:N0} minutes",
                     actual:=Long.MaxValue.ToUnits(unit:="minute", numberFormat:="N0"))
    End Sub

    <Fact>
    Public Sub ToUnits_AppliesFormattingOptions()
        Assert.Equal(expected:="[1" & vbCrLf & "Minute]",
                     actual:=1.ToUnits(unit:="Minute", prefix:="[", suffix:="]", separator:=vbCrLf))
        Assert.Equal(expected:="[arrows]",
                     actual:=2.ToUnits(unit:="arrow", prefix:="[", suffix:="]", includeValue:=False))
        Assert.Equal(expected:="1000 hours",
                     actual:=1000.ToUnits(unit:="hour"))
        Assert.Equal(expected:=$"{1000:N0} hours",
                     actual:=1000.ToUnits(unit:="hour", numberFormat:="N0"))
        Assert.Equal(expected:="2hours",
                     actual:=2.ToUnits(unit:="hour", separator:=""))
    End Sub

End Class
