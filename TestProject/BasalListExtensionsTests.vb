Imports System.Collections.Generic
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
<UISettings(MaxAttempts:=10)>
Public Class BasalListExtensionsTests

    Public Sub New()
        ' Ensure any global state is default if needed
    End Sub

    <Fact>
    Public Sub EmptyList_Returns_EmptyResults()
        ' Arrange
        Dim list As New List(Of Basal)()

        ' Act / Assert
        list.ActiveBasalPattern().Should().Be(String.Empty)
        Double.IsNaN(list.GetBasalPerHour()).Should().BeTrue()
        list.Subtitle().Should().Be(String.Empty)

        Dim cc As List(Of Basal) = list.ClassCollection()
        cc.Should().NotBeSameAs(list)
        cc.Should().BeEmpty()
    End Sub

    <Fact>
    Public Sub DefaultBasalAsFirstElement_IsTreatedAsEmpty()
        ' Arrange
        Dim list As New List(Of Basal) From {New Basal()}

        ' Act / Assert
        list.ActiveBasalPattern().Should().Be(String.Empty)
        Double.IsNaN(list.GetBasalPerHour()).Should().BeTrue()
        list.Subtitle().Should().Be(String.Empty)

        Dim cc As List(Of Basal) = list.ClassCollection()
        cc.Should().NotBeSameAs(list)
        cc.Should().BeEmpty()
    End Sub

    <Fact>
    Public Sub NonEmptyList_Returns_ValuesFromFirstBasal()
        ' Arrange
        Dim b As New Basal With {
            .ActiveBasalPattern = "BASAL1",
            .BasalRate = 1.25
        }
        Dim list As New List(Of Basal) From {b}

        ' Act / Assert
        list.ActiveBasalPattern().Should().Be("BASAL1")
        list.GetBasalPerHour().Should().Be(1.25)
        list.Subtitle().Should().Be("- BASAL1")

        Dim cc As List(Of Basal) = list.ClassCollection()
        cc.Should().BeSameAs(list)
    End Sub

    <Fact>
    Public Sub GetBasalPerHour_Uses_Max_For_Temp_When_Percentage_Positive()
        ' Arrange
        Dim b As New Basal With {
            .ActiveBasalPattern = "TEMP",
            .BasalRate = 1.0,
            .TempBasalRate = 2.0,
            .tempBasalPercentage = 10.0F
        }
        Dim list As New List(Of Basal) From {b}

        ' Act / Assert
        list.GetBasalPerHour().Should().Be(2.0)
    End Sub

    <Fact>
    Public Sub GetBasalPerHour_Uses_Min_For_Temp_When_Percentage_Zero()
        ' Arrange
        Dim b As New Basal With {
            .ActiveBasalPattern = "TEMP",
            .BasalRate = 1.0,
            .TempBasalRate = 2.0,
            .tempBasalPercentage = 0.0F
        }
        Dim list As New List(Of Basal) From {b}

        ' Act / Assert
        list.GetBasalPerHour().Should().Be(1.0)
    End Sub

End Class
