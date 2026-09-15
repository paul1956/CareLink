' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class ParserSmokeTests

    <Fact>
    Public Sub ParseFlex_SmokeTest()
        ' Minimal synthetic page text that includes the main sections the parser expects.
        Dim lines As New List(Of String) From {
            "John Doe  Header",
            "Max Basal Rate  2.5 U/Hr",
            "Basal pattern settings",
            "normal (active)  (U/Hr)  12:00 AM - 12:00 AM  0.8",
            "Active insulin time  2 h",
            "Max bolus  10 u",
            "Bolus increments  A  B  C  0.5 U",
            "12.5",
            "Carb ratio (g/U)",
            "2.0",
            "Insulin sensitivity",
            "Blood Glucose Target Range",
            "4 - 6",
            "SmartGuard™ settings",
            "Label  On",
            "X  Target 5.0",
            "X  Auto On",
            "Low reservoir  20",
            "Low pump battery  On  2 hours",
            "Low sensor life  Less than 24 hours  soon",
            "Glucose settings - lows",
            "Header  Day  Night",
            "Header2  07:00  23:00",
            "Header3  3  4",
            "Header4  1  2",
            "Header5  On  Off",
            "Header6  Before  After",
            "Header7  10  20",
            "Header8  5  6",
            "Header9  2  3",
            "Header10  1  2",
            "Header11  On  Off",
            "Glucose settings - highs",
            "H  Day  Night",
            "H2  07:00  23:00",
            "H3  8  9",
            "H4  Rise1  Rise2",
            "H5  On  Off",
            "H6  Before  After",
            "H7  30",
            "H8  5  6",
            "Alert volume and mute",
            "Pump sound  On",
            "Pump vibration  Off",
            "Lost communication  On"
        }

        Dim pageText As String = String.Join(Environment.NewLine, lines)

        Dim record As New PdfSettingsRecord()
        ParseFlex(record, pageText)

        ' Smoke assertions: parsing should complete and mark the record valid.
        record.IsValid.Should().BeTrue("ParseFlex should mark a well-formed page text as valid")
        record.UserName.Should().NotBeNullOrWhiteSpace()
        record.Bolus.Should().NotBeNull()
        record.Basal.Should().NotBeNull()
    End Sub

End Class
