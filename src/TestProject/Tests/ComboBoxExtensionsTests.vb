' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
<UISettings(MaxAttempts:=10)>
Public Class ComboBoxExtensionsTests

    <Fact>
    Public Sub IndexOfKey_FindsExistingKey_ReturnsIndex()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:=KeyValuePair.Create(key:="one", value:=1))
            cb.Items.Add(item:=KeyValuePair.Create(key:="two", value:=2))
            cb.Items.Add(item:=KeyValuePair.Create(key:="three", value:=3))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)(key:="two")

            ' Assert
            idx.Should().Be(expected:=1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_NotFound_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:=KeyValuePair.Create(key:="a", value:=10))
            cb.Items.Add(item:=KeyValuePair.Create(key:="b", value:=20))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)("z")

            ' Assert
            idx.Should().Be(expected:=-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_FindsExistingValue_ReturnsIndex()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:=KeyValuePair.Create(key:="k1", value:=100))
            cb.Items.Add(item:=KeyValuePair.Create(key:="k2", value:=200))
            cb.Items.Add(item:=KeyValuePair.Create(key:="k3", value:=300))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(y:=200)

            ' Assert
            idx.Should().Be(expected:=1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_NothingForReferenceType_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:=KeyValuePair.Create(key:="a", value:="alpha"))
            cb.Items.Add(item:=KeyValuePair.Create(key:="b", value:="beta"))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, String)(y:=Nothing)

            ' Assert
            idx.Should().Be(expected:=-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_ValueTypeZero_ReturnsIndex()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:=KeyValuePair.Create(key:="z", value:=0))
            cb.Items.Add(item:=KeyValuePair.Create(key:="y", value:=5))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(y:=0)

            ' Assert
            idx.Should().Be(expected:=0)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_EmptyCollection_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            ' no items added -> Count = 0

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)(key:="any")

            ' Assert
            idx.Should().Be(expected:=-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_EmptyCollection_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            ' no items added -> Count = 0

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(123)

            ' Assert
            idx.Should().Be(expected:=-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_IgnoresNonPairsAndFindsKvpLater()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:="not a pair")
            cb.Items.Add(item:=42)
            cb.Items.Add(item:=KeyValuePair.Create(key:="findme", value:=7))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)(key:="findme")

            ' Assert
            idx.Should().Be(expected:=2)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_IgnoresNonPairsAndFindsValueLater()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:="x")
            cb.Items.Add(item:=Date.Now)
            cb.Items.Add(item:=KeyValuePair.Create(key:="k", value:=999))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(y:=999)

            ' Assert
            idx.Should().Be(expected:=2)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_OnlyNonPairs_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:="a")
            cb.Items.Add(item:=1)

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)(key:="a")

            ' Assert
            idx.Should().Be(expected:=-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_OnlyNonPairs_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:="a")
            cb.Items.Add(item:=1)

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(y:=1)

            ' Assert
            idx.Should().Be(expected:=-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_NullKeyReferenceType_FindsIndex()
        ' Arrange
        Using cb As New ComboBox()
            Dim kvp As New KeyValuePair(Of String, Integer)(key:=Nothing, value:=5)
            cb.Items.Add(item:=kvp)

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)(key:=Nothing)

            ' Assert
            idx.Should().Be(expected:=0)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_DuplicateKeys_ReturnsFirstIndex()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(item:=KeyValuePair.Create(key:="dup", value:=1))
            cb.Items.Add(item:=KeyValuePair.Create(key:="dup", value:=2))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)(key:="dup")

            ' Assert
            idx.Should().Be(expected:=0)
        End Using
    End Sub

End Class
