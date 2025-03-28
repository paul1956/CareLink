' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TableLayoutPanelExtensions

    Private ReadOnly s_tablesSupportingCopyToClipboard As New List(Of String) From {
        NameOf(Form1.TableLayoutPanelActiveInsulinTop),
        NameOf(Form1.TableLayoutPanelAutoBasalDeliveryTop),
        NameOf(Form1.TableLayoutPanelBgReadingsTop),
        NameOf(Form1.TableLayoutPanelBannerStateTop),
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

    Private Sub SetTableName(ByRef innerTable As TableLayoutPanel, tableName As String)
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
            Case TypeOf CType(innerTable.Controls(0), TableLayoutPanel).Controls(0) Is Label
                CType(innerTable.Controls(0), TableLayoutPanel).Controls(0).Text = tableName
        End Select
    End Sub

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
            Case TypeOf table.Controls(0) Is TableLayoutPanel
                SetTableName(CType(table.Controls(0), TableLayoutPanel), tableName)
            Case TypeOf table.Controls(0) Is Label
                CType(table.Controls(0), Label).Text = $"{CInt(rowIndex)} {rowIndex}"
            Case Else
                Stop
        End Select

    End Sub

End Module
