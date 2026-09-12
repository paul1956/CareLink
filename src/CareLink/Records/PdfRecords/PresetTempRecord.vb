' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetTempRecord

    Public Sub New()
    End Sub

    Public Sub New(r As StringTable.Row, key As String)
        Me.InitializeFromRow(row:=r, key:=key)
    End Sub

    Public Property Duration As TimeSpan
    Public Property DurationUnits As String
    Public Property IsValid As Boolean = False
    Public Property PresetAmount As PresetAmountRecord

End Class
