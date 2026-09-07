Imports System.IO
Imports System.Text.Json
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class FileUtilitiesTests

    <Fact>
    Public Sub DeserializeJsonElementFromString_ValidJson_ReturnsElement()
        Dim jsonPath As String = Path.Combine(AppContext.BaseDirectory, "TestData", "simple_object.json")
        Dim json As String = File.ReadAllText(path:=jsonPath)
        Dim elem As JsonElement
        If Not json.TryFromJson(result:=elem) Then
            elem = Nothing
        End If
        elem.ValueKind.Should().Be(expected:=JsonValueKind.Object)
        elem.GetProperty(propertyName:="a").GetInt32().Should().Be(expected:=1)
        elem.GetProperty(propertyName:="b").GetString().Should().Be(expected:="text")
    End Sub

    <Fact>
    Public Sub ReadJsonElementFromFile_ValidFile_ReturnsElement()
        Dim temp As String = Path.GetTempFileName()
        Try
            Dim xTruePath As String = Path.Combine(AppContext.BaseDirectory, "TestData", "x_true.json")
            File.WriteAllText(path:=temp, contents:=File.ReadAllText(path:=xTruePath))
            Dim elem As JsonElement = FileUtilities.ReadJsonElementFromFile(temp)
            elem.ValueKind.Should().Be(expected:=JsonValueKind.Object)
            elem.GetProperty(propertyName:="x").GetBoolean().Should().BeTrue()
        Finally
            If File.Exists(path:=temp) Then File.Delete(path:=temp)
        End Try
    End Sub

    <Fact>
    Public Sub ReadTokenFileAndDataFile_ValidToken_ReturnsValues()
        Dim temp As String = Path.GetTempFileName()
        Dim tokenPath As String = Path.Combine(AppContext.BaseDirectory, "TestData", "token_basic.json")
        Dim json As String = File.ReadAllText(path:=tokenPath)
        Try
            File.WriteAllText(path:=temp, contents:=json)
            Dim elem As JsonElement = FileUtilities.ReadTokenFile(tokenBaseFileName:=temp)
            elem.ValueKind.Should().Be(expected:=JsonValueKind.Object)
            elem.GetProperty(propertyName:="access_token").GetString().Should().Be("a")

            Dim tokenData As TokenData = FileUtilities.ReadTokenDataFile(tokenBaseFileName:=temp)
            tokenData.Should().NotBeNull()
            tokenData.AccessToken.Should().Be(expected:="a")
            tokenData.RefreshToken.Should().Be(expected:="r")
            tokenData.Scope.Should().Be(expected:="s")
            tokenData.ClientId.Should().Be(expected:="c")
        Finally
            If File.Exists(path:=temp) Then File.Delete(path:=temp)
        End Try
    End Sub

End Class
