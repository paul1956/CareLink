Imports System.Globalization
Imports System.Text.Json
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
<UISettings(MaxAttempts:=10)>
Public Class SgExtensionsTests

    Public Sub New()
        ' Ensure default state
        NativeMmolL = False
    End Sub

    <Fact>
    Public Sub ScaleSg_Single_Mgdl_ReturnsFormattedString()
        ' Arrange
        NativeMmolL = False
        Dim value As Single = 100.0F
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Dim expected As String = value.ToString(provider)

        ' Act
        Dim actual As String = value.ScaleSg()

        ' Assert
        actual.Should().Be(expected)
        RestoreDefaults()
    End Sub

    <Fact>
    Public Sub ScaleSg_Single_MmolL_ReturnsScaledAndRoundedString()
        ' Arrange
        NativeMmolL = True
        Dim value As Single = 100.0F
        Dim scaled As Single = (value / MmolLUnitsDivisor).RoundToSingle(digits:=1, considerValue:=True)
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Dim expected As String = scaled.ToString(provider)

        ' Act
        Dim actual As String = value.ScaleSg()

        ' Assert
        actual.Should().Be(expected)

        ' Cleanup
        RestoreDefaults()
    End Sub

    <Fact>
    Public Sub ScaleSg_String_DelegatesToParseSingle_And_Scale()
        ' Arrange
        NativeMmolL = True
        Dim input As String = "100"
        Dim parsed As Single = input.ParseSingle()
        Dim scaled As Single = (parsed / MmolLUnitsDivisor).RoundToSingle(digits:=1, considerValue:=True)
        Dim expected As String = scaled.ToString(CultureInfo.CurrentUICulture)

        ' Act
        Dim actual As String = input.ScaleSg()

        ' Assert
        actual.Should().Be(expected)

        ' Cleanup
        RestoreDefaults()
    End Sub

    <Fact>
    Public Sub ScaleSg_KeyValuePair_With_Number_JsonElement_ReturnsFormatted()
        ' Arrange
        NativeMmolL = False
        Dim element As JsonElement = JsonDocument.Parse("100").RootElement
        Dim kvp As New KeyValuePair(Of String, Object)("sg", element)
        Dim expected As String = 100.0F.ToString(CultureInfo.CurrentUICulture)

        ' Act
        Dim actual As String = kvp.ScaleSg()

        ' Assert
        actual.Should().Be(expected)
        RestoreDefaults()
    End Sub

    <Fact>
    Public Sub ScaleSg_KeyValuePair_With_Null_JsonElement_ReturnsEmptyString()
        ' Arrange
        Dim element As JsonElement = JsonDocument.Parse("null").RootElement
        Dim kvp As New KeyValuePair(Of String, Object)("sg", element)

        ' Act
        Dim actual As String = kvp.ScaleSg()

        ' Assert
        actual.Should().Be(String.Empty)
        RestoreDefaults()
    End Sub

End Class
