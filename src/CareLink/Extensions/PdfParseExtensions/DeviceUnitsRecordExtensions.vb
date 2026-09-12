' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DeviceUnitsRecordExtensions

    <Extension>
    Public Sub InitializeFromString(this As DeviceUnitsRecord, line As String)
        If line Is Nothing Then Return
        Try
            Dim txt() As String = line.Split(",")
            If txt.Length = 2 Then
                this.CarbUnits = txt(0).Trim
                this.BgUnits = txt(1).Trim
            End If
        Catch
        End Try
    End Sub

End Module
