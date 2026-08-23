Imports System.IO
Imports System.Text.Json
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class FileUtilitiesTests

    <Fact>
    Public Sub DeserializeJsonElementFromString_ValidJson_ReturnsElement()
        Dim json As String = "{""a"":1,""b"":""text""}"
        Dim elem As JsonElement = json.FromJson(Of JsonElement)(DeserializationOptions)
        elem.ValueKind.Should().Be(expected:=JsonValueKind.Object)
        elem.GetProperty("a").GetInt32().Should().Be(1)
        elem.GetProperty("b").GetString().Should().Be("text")
    End Sub

    <Fact>
    Public Sub ReadJsonElementFromFile_ValidFile_ReturnsElement()
        Dim temp As String = Path.GetTempFileName()
        Try
            File.WriteAllText(temp, "{""x"":true}")
            Dim elem As JsonElement = FileUtilities.ReadJsonElementFromFile(temp)
            elem.ValueKind.Should().Be(expected:=JsonValueKind.Object)
            elem.GetProperty("x").GetBoolean().Should().BeTrue()
        Finally
            If File.Exists(temp) Then File.Delete(temp)
        End Try
    End Sub

    <Fact>
    Public Sub ReadTokenFileAndDataFile_ValidToken_ReturnsValues()
        Dim temp As String = Path.GetTempFileName()
        Dim json As String = "{""access_token"":""a"",""refresh_token"":""r"",""scope"":""s"",""client_id"":""c""}"
        Try
            File.WriteAllText(temp, json)
            Dim elem As JsonElement = FileUtilities.ReadTokenFile(tokenBaseFileName:=temp)
            elem.ValueKind.Should().Be(expected:=JsonValueKind.Object)
            elem.GetProperty("access_token").GetString().Should().Be("a")

            Dim tokenData As TokenData = FileUtilities.ReadTokenDataFile(tokenBaseFileName:=temp)
            tokenData.Should().NotBeNull()
            tokenData.AccessToken.Should().Be("a")
            tokenData.RefreshToken.Should().Be("r")
            tokenData.Scope.Should().Be("s")
            tokenData.ClientId.Should().Be("c")
        Finally
            If File.Exists(temp) Then File.Delete(temp)
        End Try
    End Sub

End Class
