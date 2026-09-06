' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class Client2IntegrationTests

    <Fact>
    Public Async Function ResolveEndpointConfigAsync_Integration_Works() As Task
        ' This is an integration test that requires network access to the real discovery endpoint.
        ' It is skipped by default — set environment variable RUN_INTEGRATION_TESTS=1 to enable.
#If Not DEBUG Then
        If Environment.GetEnvironmentVariable(variable:="RUN_INTEGRATION_TESTS") <> "1" Then
            ' Skip the test when the environment variable is not set. Return early so the test is effectively skipped.
            Return
        End If
#End If

        Dim discoveryUrl As String = CareLinkService.DiscoveryUrlNa
        Dim endpointConfig As EndpointConfig = Nothing
        Try
            endpointConfig =
                Await CareLinkService.ResolveEndpointConfigAsync(discoveryUrl,
                                                                 serverRegion:=Region.NorthAmerica)
        Catch ex As Exception
            Assert.True(condition:=False,
                        userMessage:=$"ResolveEndpointConfigAsync threw: {ex.Message}")
        End Try

        endpointConfig.Should().NotBeNull()
        endpointConfig.SsoJson.Should().NotBeNullOrWhiteSpace()

        Dim sso As SsoConfig = Nothing
        Dim parsed As Boolean = endpointConfig.SsoJson.TryFromJson(result:=sso)
        parsed.Should().BeTrue(because:="SSO JSON should parse into SsoConfig")
        sso.Server.Should().NotBeNull()
    End Function

End Class
