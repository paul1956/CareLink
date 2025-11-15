' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class LowAlertsRecord
    Private _snoozeTime As TimeSpan

    Public Sub New(sTable As StringTable, listOfAllTextLines As List(Of String))
        _snoozeTime = New TimeSpan(hours:=0, minutes:=20, seconds:=0)
        PdfSettingsRecord.GetSnoozeInfo(
            listOfAllTextLines,
            target:="Low Alerts",
            Me.SnoozeOn,
            snoozeTime:=_snoozeTime)

        Dim valueUnits As String = EmptyString
        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
            Dim s As StringTable.Row = e.Value
            If e.IsFirst Then
                valueUnits = s.Columns(index:=0) _
                              .Replace(oldValue:="Start Low Time (", newValue:=EmptyString) _
                              .Trim(trimChar:=")"c)
                Continue For
            End If

            Dim value As String = sTable.Rows(index:=e.Index + 1).Columns(index:=0)
            Dim endTimeOnly As TimeOnly
            If e.IsLast OrElse IsNullOrWhiteSpace(value) Then
                endTimeOnly = Midnight
            Else
                Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
                endTimeOnly = TimeOnly.Parse(
                    s:=sTable.Rows(index:=e.Index + 1) _
                             .Columns(index:=0) _
                             .Split(separator:=" ", options)(0))
            End If

            Dim item As New LowAlertRecord(s, valueUnits) With {.End = endTimeOnly}
            If item.IsValid Then
                Me.LowAlert.Add(item)
            Else
                Exit For
            End If
        Next

    End Sub

    Public Sub New()
    End Sub

    Public Property LowAlert As New List(Of LowAlertRecord)

    Public Property SnoozeOn As String = "Off"

    Public WriteOnly Property SnoozeTime As TimeSpan
        Set
            _snoozeTime = Value
        End Set
    End Property

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
            If triggerTime.IsBetween(alert.Start, alert.[End]) Then
                Return alert
            End If
        Next
        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return If(Me.SnoozeOn = "On", _snoozeTime.ToFormattedTimeSpan(unit:="hr"), "Off")
    End Function

End Class
