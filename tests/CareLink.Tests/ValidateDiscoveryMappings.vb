Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports Xunit
Imports FluentAssertions

' Import the namespaces where types live
Imports CareLink
Imports CareLink.Extensions
Imports CareLink.Records.DiscoverRecords
Imports CareLink.Utilities

Public Class ValidateDiscoveryMappings

    <Fact>
    Public Async Function Discovery_Mappings_Are_Consistent() As Task
        ' Get US and EU discovery roots using the existing Discover helper
        Dim usDiscovery As DiscoveryRoot = Await Discover.GetDiscoveryDataAsync(countryCode:="US")
        Dim euDiscovery As DiscoveryRoot = Await Discover.GetDiscoveryDataAsync(countryCode:="EU")

        Dim discoveries As List(Of DiscoveryRoot) = New List(Of DiscoveryRoot) From {usDiscovery, euDiscovery}

        For Each discovery As DiscoveryRoot In discoveries
            discovery.SupportedCountries.Should().NotBeNull("supportedCountries should be present in discovery JSON")

            For Each countryDict As Dictionary(Of String, CountryInfo) In discovery.SupportedCountries
                For Each kvp As KeyValuePair(Of String, CountryInfo) In countryDict
                    Dim countryName As String = kvp.Key

                    ' Check key exists in s_countryNameToRegionList
                    RegionCountryLists.s_countryNameToRegionList.ContainsKey(countryName).Should().BeTrue($"Country '{countryName}' must be defined in s_countryNameToRegionList")

                    ' Check the region found for that country maps to a server region code
                    Dim region As String = RegionCountryLists.s_countryNameToRegionList(countryName)
                    RegionCountryLists.s_regionToServerMapping.ContainsKey(region).Should().BeTrue($"Region '{region}' for country '{countryName}' must be present in s_regionToServerMapping")
                Next
            Next

            ' Validate CP entries: their Region value should match one of the server mapping values (e.g. "US", "EU", "CLINICAL")
            For Each cp As CPEntry In discovery.CP
                cp.Region.Should().NotBeNullOrWhiteSpace()
                Dim matched As Boolean = RegionCountryLists.s_regionToServerMapping.ContainsValue(cp.Region)
                matched.Should().BeTrue($"CP.Region '{cp.Region}' should be one of the server mapping values (e.g., 'US','EU','CLINICAL')")
            Next
        Next
    End Function
End Class
