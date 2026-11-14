' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class StringBuilderExtensionsTests

    <Fact>
    Public Shared Sub TrimEnd_RemovesStringAtEnd_WhenPresent()
        Dim sb As New StringBuilder(value:="HelloWorld")
        Dim result As StringBuilder = sb.TrimEnd(value:="World")
        result.Should().BeSameAs(expected:=sb)
        sb.ToString().Should().Be(expected:="Hello")
    End Sub

    <Fact>
    Public Sub TrimEnd_DoesNothing_WhenStringDoesNotMatchAtEnd()
        Dim sb As New StringBuilder(value:="HelloWorld")
        Dim result As StringBuilder = sb.TrimEnd(value:="world") ' case sensitive
        sb.ToString().Should().Be(expected:="HelloWorld")
    End Sub

    <Fact>
    Public Sub TrimEnd_RemovesOnlyOneOccurrence_OfValueAtEnd()
        Dim sb As New StringBuilder(value:="abcabc")
        Dim result As StringBuilder = sb.TrimEnd(value:="abc")
        result.Should().BeSameAs(expected:=sb)
        sb.ToString().Should().Be(expected:="abc")
    End Sub

    <Fact>
    Public Sub TrimEnd_ReturnsNothing_WhenStringBuilderIsNothing()
        Dim sb As StringBuilder = Nothing
        Dim result As StringBuilder = TrimEnd(sb, value:="x")
        result.Should().BeNull()
    End Sub

    <Fact>
    Public Sub TrimEnd_TrimChar_NoTrailingCharacters_NoChange()
        Dim sb As New StringBuilder(value:="test")
        Dim result As StringBuilder = sb.TrimEnd(trimChar:="!"c)
        result.Should().BeSameAs(expected:=sb)
    End Sub

    <Fact>
    Public Sub TrimEnd_TrimChar_RemovesTrailingCharacters()
        Dim sb As New StringBuilder(value:="test!!!")
        Dim result As StringBuilder = sb.TrimEnd(trimChar:="!"c)
        result.Should().BeSameAs(expected:=sb)
    End Sub

End Class
