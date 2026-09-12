' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetBolusRecord

    Public Sub New()
    End Sub

    ' Parsing moved to PresetBolusRecordExtensions.InitializeFromRow

    Public Property Bolus As String
    Public Property BolusTypeNormal As Boolean
    Public Property Duration As String
    Public Property IsValid As Boolean = False
    Public Property Square As String

End Class
