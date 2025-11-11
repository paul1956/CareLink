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
            cb.Items.Add(KeyValuePair.Create("one", 1))
            cb.Items.Add(KeyValuePair.Create("two", 2))
            cb.Items.Add(KeyValuePair.Create("three", 3))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)("two")

            ' Assert
            idx.Should().Be(1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_NotFound_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(KeyValuePair.Create("a", 10))
            cb.Items.Add(KeyValuePair.Create("b", 20))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)("z")

            ' Assert
            idx.Should().Be(-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_FindsExistingValue_ReturnsIndex()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(KeyValuePair.Create("k1", 100))
            cb.Items.Add(KeyValuePair.Create("k2", 200))
            cb.Items.Add(KeyValuePair.Create("k3", 300))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(200)

            ' Assert
            idx.Should().Be(1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_NothingForReferenceType_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(KeyValuePair.Create("a", "alpha"))
            cb.Items.Add(KeyValuePair.Create("b", "beta"))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, String)(Nothing)

            ' Assert
            idx.Should().Be(-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_ValueTypeZero_ReturnsIndex()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(KeyValuePair.Create("z", 0))
            cb.Items.Add(KeyValuePair.Create("y", 5))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(0)

            ' Assert
            idx.Should().Be(0)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_EmptyCollection_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            ' no items added -> Count = 0

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)("any")

            ' Assert
            idx.Should().Be(-1)
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
            idx.Should().Be(-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_IgnoresNonPairsAndFindsKvpLater()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add("not a pair")
            cb.Items.Add(42)
            cb.Items.Add(KeyValuePair.Create("findme", 7))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)("findme")

            ' Assert
            idx.Should().Be(2)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_IgnoresNonPairsAndFindsValueLater()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add("x")
            cb.Items.Add(Date.Now)
            cb.Items.Add(KeyValuePair.Create("k", 999))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(999)

            ' Assert
            idx.Should().Be(2)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_OnlyNonPairs_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add("a")
            cb.Items.Add(1)

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)("a")

            ' Assert
            idx.Should().Be(-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfY_OnlyNonPairs_ReturnsMinusOne()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add("a")
            cb.Items.Add(1)

            ' Act
            Dim idx As Integer = cb.Items.IndexOfY(Of String, Integer)(1)

            ' Assert
            idx.Should().Be(-1)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_NullKeyReferenceType_FindsIndex()
        ' Arrange
        Using cb As New ComboBox()
            Dim kvp As New KeyValuePair(Of String, Integer)(Nothing, 5)
            cb.Items.Add(kvp)

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)(Nothing)

            ' Assert
            idx.Should().Be(0)
        End Using
    End Sub

    <Fact>
    Public Sub IndexOfKey_DuplicateKeys_ReturnsFirstIndex()
        ' Arrange
        Using cb As New ComboBox()
            cb.Items.Add(KeyValuePair.Create("dup", 1))
            cb.Items.Add(KeyValuePair.Create("dup", 2))

            ' Act
            Dim idx As Integer = cb.Items.IndexOfKey(Of String, Integer)("dup")

            ' Assert
            idx.Should().Be(0)
        End Using
    End Sub

End Class
