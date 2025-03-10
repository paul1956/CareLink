' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TableLayoutPanelExtensions

    Private ReadOnly s_tablesSupportingExportToExcel As New List(Of String) From {
                NameOf(Form1.TableLayoutPanelAutoBasalDelivery),
                NameOf(Form1.TableLayoutPanelAutoModeStatus),
                NameOf(Form1.TableLayoutPanelBgReadings),
                NameOf(Form1.TableLayoutPanelBasal),
                NameOf(Form1.TableLayoutPanelCalibration),
                NameOf(Form1.TableLayoutPanelInsulin),
                NameOf(Form1.TableLayoutPanelLimits),
                NameOf(Form1.TableLayoutPanelMeal),
                NameOf(Form1.TableLayoutPanelNotificationActive),
                NameOf(Form1.TableLayoutPanelNotificationsCleared),
                NameOf(Form1.TableLayoutPanelSgs),
                NameOf(Form1.TableLayoutPanelTimeChange)
            }

    Private ReadOnly s_tablesSupportingCopyToClipboard As New List(Of String) From {
               NameOf(Form1.TableLayoutPanelActiveInsulin),
               NameOf(Form1.TableLayoutPanelAutoBasalDelivery),
               NameOf(Form1.TableLayoutPanelBgReadings),
               NameOf(Form1.TableLayoutPanelBannerState),
               NameOf(Form1.TableLayoutPanelBasal),
               NameOf(Form1.TableLayoutPanelCalibration),
               NameOf(Form1.TableLayoutPanelLastAlarm),
               NameOf(Form1.TableLayoutPanelLastSG),
               NameOf(Form1.TableLayoutPanelLowGlucoseSuspended),
               NameOf(Form1.TableLayoutPanelNotificationActive),
               NameOf(Form1.TableLayoutPanelNotificationsCleared),
               NameOf(Form1.TableLayoutPanelTherapyAlgorithm)
            }

    <Extension>
    Friend Sub SetTabName(table As TableLayoutPanel, rowIndex As ServerDataIndexes, isClearedNotifications As Boolean)
        Dim tableName As String = rowIndex.ToString.ToTitleCase
        If tableName = "Notification History" Then
            tableName = If(isClearedNotifications, "Cleared Notifications", "Active Notification")
        ElseIf tableName = "Sgs" Then
            tableName = "Sensor Glucose Values"
        End If

        Select Case True
            Case TypeOf table.Controls(0) Is TableLayoutPanel
                Select Case True
                    Case TypeOf CType(table.Controls(0), TableLayoutPanel).Controls(0) Is Button
                        Dim helpString As String
                        If s_tablesSupportingExportToExcel.Contains(table.Name) Then
                            helpString = ": Right Click on Table for Export Options including Excel"
                        ElseIf s_tablesSupportingCopyToClipboard.Contains(table.Name) Then
                            helpString = ": Right Click on Table for cell(s) Export Options"
                        Else
                            helpString = ""
                        End If
                        If tableName = "Markers" Then
                            tableName = $"{CType(table.Parent, TabPage).Text} Markers"
                        End If
                        CType(table.Controls(0), TableLayoutPanel).Controls(1).Text = $"{tableName}{helpString}"
                    Case TypeOf CType(table.Controls(0), TableLayoutPanel).Controls(0) Is Label
                        CType(table.Controls(0), TableLayoutPanel).Controls(0).Text = tableName
                End Select

            Case TypeOf table.Controls(0) Is Button
                Stop
            Case TypeOf table.Controls(0) Is Label
                CType(table.Controls(0), Label).Text = $"{CInt(rowIndex)} {rowIndex}"
        End Select

    End Sub

End Module
