Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class ParserSectionTests

    <Fact>
    Public Sub ParseBasal_SetsMaximumBasal()
        Dim allLines As New List(Of String) From {
            "Max Basal Rate  2.5 U/Hr",
            "Basal pattern settings",
            "normal (active)  (U/Hr)  12:00 AM - 12:00 AM  0.8"}
        Dim record As New PdfSettingsRecord()
        Dim parser As New PdfSectionParser(allLines, record)
        parser.ParseBasal()
        record.Basal.MaximumBasalRate.Should().BeApproximately(2.5F, 0.01F)
    End Sub

    <Fact>
    Public Sub ParseBolus_PopulatesEasyBolusAndCarbRatio()
        Dim allLines As New List(Of String) From {
            "Active insulin time  2 h",
            "Max bolus  10 u",
            "Bolus increments  A  B  C  0.5 U",
            "10.0",
            "Carb ratio (g/U)",
            "25",
            "Insulin sensitivity",
            "90 - 110",
            "Blood Glucose Target Range"}
        Dim record As New PdfSettingsRecord()
        Dim parser As New PdfSectionParser(allLines, record)
        parser.ParseBolus()
        record.Bolus.EasyBolus.Should().NotBeNull()
        record.Bolus.DeviceCarbohydrateRatios.Should().NotBeEmpty()
        record.Bolus.BloodGlucoseTarget.Should().NotBeEmpty()
    End Sub

    <Fact>
    Public Sub ParseSmartGuardAndReminders_PopulatesSmartGuardAndReminders()
        Dim allLines As New List(Of String) From {
            "SmartGuard™ settings",
            "Label  On",
            "X  Target 5.0",
            "X  Auto On",
            "Low reservoir  20",
            "Low pump battery  On  2 hours",
            "Low sensor life  Less than 24 hours  soon",
            "Next line  More"
        }
        Dim record As New PdfSettingsRecord()
        Dim parser As New PdfSectionParser(allLines, record)
        parser.ParseSmartGuardAndReminders()
        record.SmartGuard.Should().NotBeNull()
        record.Reminders.Should().NotBeNull()
        record.Reminders.LowPumpBattery.Should().NotBeNull()
    End Sub

    <Fact>
    Public Sub ParseAlertsAndUtilities_PopulatesAlertsAndUtilities()
        Dim allLines As New List(Of String) From {
            "Glucose settings - lows  Day  Night",
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
            "Glucose settings - highs  Day  Night",
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
        Dim record As New PdfSettingsRecord()
        Dim parser As New PdfSectionParser(allLines, record)
        parser.ParseAlertsAndUtilities()
        record.LowAlerts.LowAlert.Should().NotBeEmpty()
        record.HighAlerts.HighAlert.Should().NotBeEmpty()
        record.Utilities.PumpSounds.Should().NotBeNullOrWhiteSpace()
    End Sub

End Class
