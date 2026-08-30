' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel

Public Module ImageEnums

    Public Enum ImageEnum

        <Description("AboutBox")>
        AboutBox

        <Description("AboutBoxDark")>
        AboutBoxDark

        <Description("AdvancedView")>
        AdvancedView

        <Description("AdvancedViewDark")>
        AdvancedViewDark

        <Description("CalibrationDot")>
        CalibrationDot

        <Description("CalibrationDotRed")>
        CalibrationDotRed

        <Description("CalibrationNotReady")>
        CalibrationNotReady

        <Description("CalibrationUnavailable")>
        CalibrationUnavailable

        <Description("Copy")>
        Copy

        <Description("CopyDark")>
        CopyDark

        <Description("Exit")>
        [Exit]

        <Description("ExitDark")>
        ExitDark

        <Description("ExitFullScreen")>
        ExitFullScreen

        <Description("ExitFullScreenDark")>
        ExitFullScreenDark

        <Description("ExportData")>
        ExportData

        <Description("FeedbackSmile_16x")>
        FeedbackSmile_16x

        <Description("FeedbackSmile_16xDark")>
        FeedbackSmile_16xDark

        <Description("FlexActiveInsulinReset")>
        FlexActiveInsulinReset

        <Description("FlexPump")>
        FlexPump

        <Description("GridLight")>
        GridLight

        <Description("InfusionLife12To24Hours")>
        InfusionLife12To24Hours

        <Description("InfusionLifeExpired")>
        InfusionLifeExpired

        <Description("InfusionLifeOver24Hours")>
        InfusionLifeOver24Hours

        <Description("InfusionLifeUnder12Hours")>
        InfusionLifeUnder12Hours

        <Description("InfusionLifeUnknown")>
        InfusionLifeUnknown

        <Description("InfusionNotsetup")>
        InfusionNotsetup

        <Description("InsulinVial")>
        InsulinVial

        <Description("InsulinVialTiny")>
        InsulinVialTiny

        <Description("LoginDark")>
        LoginDark

        <Description("LoginLight")>
        LoginLight

        <Description("MealImage")>
        MealImage

        <Description("MealImageLarge")>
        MealImageLarge

        <Description("NotificationAlert_16x")>
        NotificationAlert_16x

        <Description("NotificationAlertGray_16x")>
        NotificationAlertGray_16x

        <Description("NotificationAlertRed_16x")>
        NotificationAlertRed_16x

        <Description("OpenFile")>
        OpenFile

        <Description("OpenFileDark")>
        OpenFileDark

        <Description("OpenProjectFolder")>
        OpenProjectFolder

        <Description("OpenProjectFolderDark")>
        OpenProjectFolderDark

        <Description("PumpBattery780GCritical")>
        PumpBattery780GCritical

        <Description("PumpBattery780GFull")>
        PumpBattery780GFull

        <Description("PumpBattery780GHigh")>
        PumpBattery780GHigh

        <Description("PumpBattery780GLow")>
        PumpBattery780GLow

        <Description("PumpBattery780GMedium")>
        PumpBattery780GMedium

        <Description("PumpBattery780GUnknown")>
        PumpBattery780GUnknown

        <Description("PumpBatteryFlex1To10Hours")>
        PumpBatteryFlex1To10Hours

        <Description("PumpBatteryFlexDepleted")>
        PumpBatteryFlexDepleted

        <Description("PumpBatteryFlexFull")>
        PumpBatteryFlexFull

        <Description("PumpBatteryFlexLessThen1Hour")>
        PumpBatteryFlexLessThen1Hour

        <Description("PumpBatteryFlexUnknown")>
        PumpBatteryFlexUnknown

        <Description("PumpConnectivityToInstinctOK")>
        PumpConnectivityToInstinctOK

        <Description("PumpConnectivityToPhoneNotOK")>
        PumpConnectivityToPhoneNotOK

        <Description("PumpConnectivityToPhoneOK")>
        PumpConnectivityToPhoneOK

        <Description("PumpConnectivityToSimpleraOK")>
        PumpConnectivityToSimpleraOK

        <Description("PumpConnectivityToTransmitterNotOK")>
        PumpConnectivityToTransmitterNotOK

        <Description("PumpConnectivityToTransmitterOK")>
        PumpConnectivityToTransmitterOK

        <Description("PumpConnectivityToTransmitterUnknown")>
        PumpConnectivityToTransmitterUnknown

        <Description("QuestionMark")>
        QuestionMark

        <Description("ReservoirEmpty")>
        ReservoirEmpty

        <Description("ReservoirRemainsOver01Percent")>
        ReservoirRemainsOver01Percent

        <Description("ReservoirRemainsOver15Percent")>
        ReservoirRemainsOver15Percent

        <Description("ReservoirRemainsOver29Percent")>
        ReservoirRemainsOver29Percent

        <Description("ReservoirRemainsOver43Percent")>
        ReservoirRemainsOver43Percent

        <Description("ReservoirRemainsOver57Percent")>
        ReservoirRemainsOver57Percent

        <Description("ReservoirRemainsOver71Percent")>
        ReservoirRemainsOver71Percent

        <Description("ReservoirRemainsOver85Percent")>
        ReservoirRemainsOver85Percent

        <Description("ReservoirRemainsUnknown")>
        ReservoirRemainsUnknown

        <Description("SelectAll")>
        SelectAll

        <Description("SelectAllDark")>
        SelectAllDark

        <Description("SensorCommunicationLost")>
        SensorCommunicationLost

        <Description("SensorExpirationUnknown")>
        SensorExpirationUnknown

        <Description("SensorExpired")>
        SensorExpired

        <Description("SensorExpiringSoon")>
        SensorExpiringSoon

        <Description("SensorLifeNotOK")>
        SensorLifeNotOK

        <Description("SensorLifeOK")>
        SensorLifeOK

        <Description("Shield")>
        Shield

        <Description("ShieldDisabled")>
        ShieldDisabled

        <Description("SmartGuardFlexSuspended")>
        SmartGuardFlexSuspended

        <Description("SmartGuardShield")>
        SmartGuardShield

        <Description("TransmitterBatteryCritical")>
        TransmitterBatteryCritical

        <Description("TransmitterBatteryFull")>
        TransmitterBatteryFull

        <Description("TransmitterBatteryLow")>
        TransmitterBatteryLow

        <Description("TransmitterBatteryMedium")>
        TransmitterBatteryMedium

        <Description("TransmitterBatteryOK")>
        TransmitterBatteryOK

        <Description("TransmitterBatteryUnknown")>
        TransmitterBatteryUnknown

#Region "Not used programically only testing"

        <Description("iconimage")>
        iconimage

#End Region

    End Enum

End Module
