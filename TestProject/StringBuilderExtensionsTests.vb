' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports System.Text
Imports Xunit

Public Class StringBuilderExtensionsTests

    <Fact>
    Public Shared Sub TrimEnd_RemovesStringAtEnd_WhenPresent()
        Dim sb As New StringBuilder("HelloWorld")
        Dim result As StringBuilder = sb.TrimEnd("World")
        result.Should().BeSameAs(sb)
        sb.ToString().Should().Be("Hello")
    End Sub

    <Fact>
    Public Sub TrimEnd_DoesNothing_WhenStringDoesNotMatchAtEnd()
        Dim sb As New StringBuilder("HelloWorld")
        Dim result As StringBuilder = sb.TrimEnd("world") ' case sensitive
        sb.ToString().Should().Be("HelloWorld")
    End Sub

    <Fact>
    Public Sub TrimEnd_ReturnsNothing_WhenStringBuilderIsNothing()
        Dim sb As StringBuilder = Nothing
        Dim result As StringBuilder = TrimEnd(sb, "x")
        result.Should().BeNull()
    End Sub

    <Fact>
    Public Sub TrimEnd_RemovesOnlyOneOccurrence_OfValueAtEnd()
        Dim sb As New StringBuilder("abcabc")
        Dim result As StringBuilder = sb.TrimEnd("abc")
        result.Should().BeSameAs(sb)
        sb.ToString().Should().Be("abc")
    End Sub

    <Fact>
    Public Sub TrimEnd_TrimChar_RemovesTrailingCharacters()
        Dim sb As New StringBuilder("test!!!")
        Dim result As StringBuilder = sb.TrimEnd("!"c)
        result.Should().BeSameAs(sb)
    End Sub

    <Fact>
    Public Sub TrimEnd_TrimChar_NoTrailingCharacters_NoChange()
        Dim sb As New StringBuilder("test")
        Dim result As StringBuilder = sb.TrimEnd("!"c)
        result.Should().BeSameAs(sb)
    End Sub

End Class
