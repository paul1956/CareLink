' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TableLayoutPanelExtensions

    Private ReadOnly s_tablesSupportingCopyToClipboard As New List(Of String) From {
        NameOf(Form1.TableLayoutPanelActiveInsulinTop),
        NameOf(Form1.TableLayoutPanelAutoBasalDeliveryTop),
        NameOf(Form1.TableLayoutPanelBgReadingsTop),
        NameOf(Form1.TableLayoutPanelPumpBannerStateTop),
        NameOf(Form1.TableLayoutPanelBasalTop),
        NameOf(Form1.TableLayoutPanelCalibrationTop),
        NameOf(Form1.TableLayoutPanelLastAlarmTop),
        NameOf(Form1.TableLayoutPanelLastSgTop),
        NameOf(Form1.TableLayoutPanelLowGlucoseSuspendedTop),
        NameOf(Form1.TableLayoutPanelNotificationActiveTop),
        NameOf(Form1.TableLayoutPanelNotificationsClearedTop),
        NameOf(Form1.TableLayoutPanelTherapyAlgorithmStateTop),
        NameOf(Form1.TableLayoutPanelTimeChangeTop)}

    Private ReadOnly s_tablesSupportingExportToExcel As New List(Of String) From {
        NameOf(Form1.TableLayoutPanelAutoBasalDeliveryTop),
        NameOf(Form1.TableLayoutPanelAutoModeStatusTop),
        NameOf(Form1.TableLayoutPanelBgReadingsTop),
        NameOf(Form1.TableLayoutPanelBasalTop),
        NameOf(Form1.TableLayoutPanelCalibrationTop),
        NameOf(Form1.TableLayoutPanelInsulinTop),
        NameOf(Form1.TableLayoutPanelLimitsTop),
        NameOf(Form1.TableLayoutPanelMealTop),
        NameOf(Form1.TableLayoutPanelNotificationActiveTop),
        NameOf(Form1.TableLayoutPanelNotificationsClearedTop),
        NameOf(Form1.TableLayoutPanelSgsTop),
        NameOf(Form1.TableLayoutPanelTimeChangeTop)}

    ''' <summary>
    '''  Sets the display name of a table, including help text for export/copy options if supported.
    ''' </summary>
    ''' <param name="innerTable">The <see cref="TableLayoutPanel"/> whose name is to be set.</param>
    ''' <param name="tableName">The base name to display for the table.</param>
    Private Sub SetTableName(ByRef innerTable As TableLayoutPanel, tableName As String)
        Try
            Select Case True
                Case TypeOf innerTable.Controls(0) Is Button
                    Dim helpString As String
                    If s_tablesSupportingExportToExcel.Contains(innerTable.Name) Then
                        helpString = ": Right Click on Table for Export Options including Excel"
                    ElseIf s_tablesSupportingCopyToClipboard.Contains(innerTable.Name) Then
                        helpString = ": Right Click on Table for cell(s) Export Options"
                    Else
                        helpString = ""
                    End If
                    If tableName = "Markers" Then
                        tableName = $"{CType(innerTable.Parent.Parent, TabPage).Text} Markers"
                    End If
                    innerTable.Controls(1).Text = $"{tableName}{helpString}"
                Case TypeOf innerTable.Controls(0) Is DataGridView
                    CType(innerTable.Controls(0), DataGridView).Parent.Controls(0).Text = tableName
                Case TypeOf CType(innerTable.Controls(0), TableLayoutPanel).Controls(0) Is Label
                    CType(innerTable.Controls(0), TableLayoutPanel).Controls(0).Text = tableName
            End Select
        Catch ex As Exception
            Stop
            ' Handle any exceptions that may occur during the setting of the table name.
        End Try
    End Sub

    ''' <summary>
    '''  Extension method to set the table name for a <see cref="TableLayoutPanel"/> based on the specified row index
    '''  and notification state.
    ''' </summary>
    ''' <param name="table">The <see cref="TableLayoutPanel"/> to update.</param>
    ''' <param name="rowIndex">The <see cref="ServerDataIndexes"/> value representing the table's data type.</param>
    ''' <param name="isClearedNotifications">Indicates if the table represents cleared notifications.</param>
    <Extension>
    Friend Sub SetTableName(table As TableLayoutPanel, rowIndex As ServerDataIndexes, isClearedNotifications As Boolean)
        Dim tableName As String = rowIndex.ToString.ToTitleCase
        If tableName = "Notification History" Then
            tableName = If(isClearedNotifications, "Cleared Notifications", "Active Notification")
        ElseIf tableName = "Sgs" Then
            tableName = "Sensor Glucose Values"
        End If

        Select Case True
            Case TypeOf table.Parent Is SplitterPanel
                SetTableName(CType(CType(table.Parent.Parent, SplitContainer).Panel1.Controls(0), TableLayoutPanel), tableName)
            Case TypeOf table.Parent Is TabPage
                SetTableName(CType(table.Controls(0).Parent, TableLayoutPanel), tableName)
            Case TypeOf table.Controls(0) Is TableLayoutPanel
                SetTableName(CType(table.Controls(0), TableLayoutPanel), tableName)
            Case TypeOf table.Controls(0) Is Label
                CType(table.Controls(0), Label).Text = $"{CInt(rowIndex)} {rowIndex}"
            Case Else
                Stop
        End Select
    End Sub

End Module
