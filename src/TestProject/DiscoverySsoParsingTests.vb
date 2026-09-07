Imports CareLink
Imports FluentAssertions
Imports System.IO
Imports Xunit

Public Class DiscoverySsoParsingTests

    <Fact>
    Public Sub ParseDiscoveryJson_Populates_CPEntry()
        Dim discoveryJsonPath As String = Path.Combine(AppContext.BaseDirectory, "TestData", "discovery.json")
        Dim json As String = File.ReadAllText(path:=discoveryJsonPath)

        Dim discovery As DiscoveryRoot = Nothing
        Dim parsed As Boolean
        Try
            discovery = System.Text.Json.JsonSerializer.Deserialize(Of DiscoveryRoot)(json)
            parsed = discovery IsNot Nothing AndAlso discovery.CP IsNot Nothing AndAlso discovery.CP.Count = 1
        Catch ex As Exception
            parsed = False
        End Try

        parsed.Should().BeTrue()
        discovery.CP(0).Region.Should().Be("US")
        discovery.CP(0).SSOConfiguration.Should().Be("https://example.com/sso.json")
        discovery.CP(0).UseSSOConfiguration.Should().Be("SSOConfiguration")
    End Sub

    <Fact>
    Public Sub ParseSsoJson_Populates_Server()
        Dim ssoJsonPath As String = Path.Combine(AppContext.BaseDirectory, "TestData", "sso.json")
        Dim ssoJson As String = File.ReadAllText(path:=ssoJsonPath)

        Dim ssoConfig As SsoConfig = Nothing
        Dim parsed As Boolean
        Try
            ssoConfig = System.Text.Json.JsonSerializer.Deserialize(Of SsoConfig)(ssoJson)
            parsed = ssoConfig IsNot Nothing
        Catch ex As Exception
            parsed = False
        End Try

        parsed.Should().BeTrue()
        ssoConfig.Server.Hostname.Should().Be("sso.example.com")
        ssoConfig.Server.Port.Should().Be(443)
        ssoConfig.Server.Prefix.Should().Be("auth")
        ssoConfig.Client.ClientId.Should().Be("test-client")
        ssoConfig.Client.ClientSecret.Should().Be("test-secret")
    End Sub

End Class
