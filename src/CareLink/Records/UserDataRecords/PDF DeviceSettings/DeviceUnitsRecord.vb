' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class DeviceUnitsRecord

    Public Sub New(line As String)
        Dim txt() As String = line.Split(",")
        If txt.Length = 2 Then
            Me.CarbUnits = txt(0).Trim
            Me.BgUnits = txt(1).Trim
        Else
            Stop
        End If
    End Sub

    Public Property BgUnits As String
    Public Property CarbUnits As String

    Private Function GetDebuggerDisplay() As String
        Return Me.ToString()
    End Function

    Public Overrides Function ToString() As String
        Return $"{Me.CarbUnits}, {Me.BgUnits}"
    End Function

End Class
