' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TableLayoutPanelExtensions

    Private ReadOnly s_tablesSupportingCopyToClipboard As New List(Of String) From {
        NameOf(Form1.TlpActiveInsulinTop),
        NameOf(Form1.TlpAutoBasalDeliveryTop),
        NameOf(Form1.TlpBgReadingsTop),
        NameOf(Form1.TlpPumpBannerStateTop),
        NameOf(Form1.TlpBasalTop),
        NameOf(Form1.TlpCalibrationTop),
        NameOf(Form1.TlpLastAlarmTop),
        NameOf(Form1.TlpLastSgTop),
        NameOf(Form1.TlpLowGlucoseSuspendedTop),
        NameOf(Form1.TlpNotificationActiveTop),
        NameOf(Form1.TlpNotificationsClearedTop),
        NameOf(Form1.TlpTherapyAlgorithmStateTop),
        NameOf(Form1.TlpTimeChangeTop)}

    Private ReadOnly s_tablesSupportingExportToExcel As New List(Of String) From {
        NameOf(Form1.TlpAutoBasalDeliveryTop),
        NameOf(Form1.TlpAutoModeStatusTop),
        NameOf(Form1.TlpBgReadingsTop),
        NameOf(Form1.TlpBasalTop),
        NameOf(Form1.TlpCalibrationTop),
        NameOf(Form1.TlpInsulinTop),
        NameOf(Form1.TlpLimitsTop),
        NameOf(Form1.TlpMealTop),
        NameOf(Form1.TlpNotificationActiveTop),
        NameOf(Form1.TlpNotificationsClearedTop),
        NameOf(Form1.TlpSgsTop),
        NameOf(Form1.TlpTimeChangeTop)}

    ''' <summary>
    '''  Sets the display name of a table, including help text for export/copy
    '''  options if supported.
    ''' </summary>
    ''' <param name="panel">
    '''  The <see cref="TableLayoutPanel"/> whose name is to be set.
    ''' </param>
    ''' <param name="tableName">The base name to display for the table.</param>
    Private Sub SetTableName(ByRef panel As TableLayoutPanel, tableName As String)
        Try
            Dim c As Control = panel.Controls(index:=0)
            Select Case True
                Case TypeOf c Is Button
                    Dim helpString As String
                    If s_tablesSupportingExportToExcel.Contains(item:=panel.Name) Then
                        helpString =
                            ": Right Click on Table for Export Options including Excel"
                    ElseIf s_tablesSupportingCopyToClipboard.Contains(item:=panel.Name) Then
                        helpString =
                            ": Right Click on Table for cell(s) Export Options"
                    Else
                        helpString = ""
                    End If
                    If tableName = "Markers" Then
                        tableName = $"{CType(panel.Parent.Parent, TabPage).Text} Markers"
                    End If
                    panel.Controls(index:=1).Text = $"{tableName}{helpString}"
                Case TypeOf c Is DataGridView
                    CType(c, DataGridView).Parent.Controls(index:=0).Text = tableName
                Case TypeOf CType(c, TableLayoutPanel).Controls(index:=0) Is Label
                    CType(c, TableLayoutPanel).Controls(index:=0).Text = tableName
            End Select
        Catch ex As Exception
            Stop
            ' Handle any exceptions that may occur during the setting of the table name.
        End Try
    End Sub

    ''' <summary>
    '''  Extension method to set the table name for a <see cref="TableLayoutPanel"/>
    '''  based on the specified row index and notification state.
    ''' </summary>
    ''' <param name="panel">The <see cref="TableLayoutPanel"/> to update.</param>
    ''' <param name="rowIndex">
    '''  The <see cref="ServerDataIndexes"/> value representing the table's data type.
    ''' </param>
    ''' <param name="isClearedNotifications">
    '''  Indicates if the table represents cleared notifications.
    ''' </param>
    <Extension>
    Friend Sub SetTableName(
        panel As TableLayoutPanel,
        rowIndex As ServerDataIndexes,
        isClearedNotifications As Boolean)

        Dim tableName As String = rowIndex.ToString.ToTitleCase
        If tableName = "Notification History" Then
            tableName = If(isClearedNotifications,
                           "Cleared Notifications",
                           "Active Notification")
        ElseIf tableName = "Sgs" Then
            tableName = "Sensor Glucose Values"
        End If

        Try
            ' Prevent crashes when the table is not initialized or disposed.
            Dim c As Control
            Select Case True
                Case TypeOf panel.Parent Is SplitterPanel
                    Dim splitContainer As SplitContainer =
                        CType(panel.Parent.Parent, SplitContainer)
                    c = splitContainer.Panel1.Controls(index:=0)
                    SetTableName(panel:=CType(c, TableLayoutPanel), tableName)
                Case TypeOf panel.Parent Is TabPage
                    c = panel.Controls(index:=0).Parent
                    SetTableName(panel:=CType(c, TableLayoutPanel), tableName)
                Case TypeOf panel.Controls(index:=0) Is TableLayoutPanel
                    c = panel.Controls(index:=0)
                    SetTableName(panel:=CType(c, TableLayoutPanel), tableName)
                Case TypeOf panel.Controls(index:=0) Is Label
                    CType(panel.Controls(index:=0), Label).Text =
                        $"{CInt(rowIndex)} {rowIndex}"
                Case Else
                    Stop
            End Select
        Catch
        End Try
    End Sub

End Module
