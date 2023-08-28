' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class DeviceBolusRecord

    Public Property BolusWizard As BolusWizardRecord
    Public Property EasyBolus As New EasyBolusRecord

    Public Property DeviceCarbohydrateRatios As New List(Of DeviceCarbRatioRecord)
    Public Property InsulinSensivity As New List(Of InsulinSensivityRecord)
    Public Property BloodGlucoseTarget As New List(Of BloodGlucoseTargetRecord)

End Class
