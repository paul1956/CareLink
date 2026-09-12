' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class LowAlertsRecord

    Public Sub New(sTable As StringTable, listOfAllTextLines As List(Of String))
        Me.InitializeFromStringTable(sTable, listOfAllTextLines)
    End Sub

    Public Sub New()
    End Sub

    Public Property LowAlert As New List(Of LowAlertRecord)

    Public Property SnoozeOn As String = "Off"

    Public Property SnoozeTime As TimeSpan

    Private Function GetDebuggerDisplay() As String
        Return Me.ToString()
    End Function

    '/ <summary>
    '''  Gets the <see cref="LowAlertRecord"/> that applies to the specified time.
    ''' </summary>
    ''' <param name="triggerTime">The time to check.</param>
    ''' <returns>
    '''  The matching <see cref="LowAlertRecord"/>;
    '''  otherwise <see langword="Nothing"/> if none found.
    ''' </returns>
    Public Shared Function GetLowAlertRecord(triggerTime As TimeOnly) As LowAlertRecord
        If CurrentPdf.LowAlerts.LowAlert.Count = 1 Then
            Return CurrentPdf.LowAlerts.LowAlert(index:=0)
        End If
        For Each alert As LowAlertRecord In CurrentPdf.LowAlerts.LowAlert
            If triggerTime.IsBetween(start:=TimeOnly.Parse(s:=alert.Start),
                                     [end]:=TimeOnly.Parse(s:=alert.[End])) Then
                Return alert
            End If
        Next
        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return If(Me.SnoozeOn = "On", _SnoozeTime.ToFormattedTimeSpan(unit:="hr"), "Off")
    End Function

End Class
