' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TableLayoutPanelExtensions

    Private ReadOnly tablesSupportingExportToExcel As New List(Of String) From {
                NameOf(Form1.TableLayoutPanelAutoBasalDelivery),
                NameOf(Form1.TableLayoutPanelAutoModeStatus),
                NameOf(Form1.TableLayoutPanelBgReadings),
                NameOf(Form1.TableLayoutPanelBasal),
                NameOf(Form1.TableLayoutPanelCalibration),
                NameOf(Form1.TableLayoutPanelInsulin),
                NameOf(Form1.TableLayoutPanelLimits),
                NameOf(Form1.TableLayoutPanelMeal),
                NameOf(Form1.TableLayoutPanelSgs),
                NameOf(Form1.TableLayoutPanelTimeChange)
            }

    Private ReadOnly tablesSupportingCopyToClipboard As New List(Of String) From {
               NameOf(Form1.TableLayoutPanelActiveInsulin),
               NameOf(Form1.TableLayoutPanelAutoBasalDelivery),
               NameOf(Form1.TableLayoutPanelBgReadings),
               NameOf(Form1.TableLayoutPanelBannerState),
               NameOf(Form1.TableLayoutPanelBasal),
               NameOf(Form1.TableLayoutPanelCalibration),
               NameOf(Form1.TableLayoutPanelLastAlarm),
               NameOf(Form1.TableLayoutPanelLastSG),
               NameOf(Form1.TableLayoutPanelLowGlucoseSuspended),
               NameOf(Form1.TableLayoutPanelNotificationHistory),
               NameOf(Form1.TableLayoutPanelTherapyAlgorithm)
            }

    <Extension>
    Friend Sub SetTabName(table As TableLayoutPanel, rowIndex As ItemIndexes)
        Select Case True
            Case TypeOf table.Controls(0) Is TableLayoutPanel
                Select Case True
                    Case TypeOf CType(table.Controls(0), TableLayoutPanel).Controls(0) Is Button

                        Dim helpString As String

                        If tablesSupportingExportToExcel.Contains(table.Name) Then
                            helpString = ": Right Click on Table for table Export Options"
                        ElseIf tablesSupportingCopyToClipboard.Contains(table.Name) Then
                            helpString = ": Right Click on Table for cell(s) Export Options"
                        Else
                            helpString = ""
                        End If

                        CType(table.Controls(0), TableLayoutPanel).Controls(1).Text = $"{CInt(rowIndex)} {rowIndex}{helpString}"
                    Case TypeOf CType(table.Controls(0), TableLayoutPanel).Controls(0) Is Label
                        CType(table.Controls(0), TableLayoutPanel).Controls(0).Text = $"{CInt(rowIndex)} {rowIndex}"
                End Select

            Case TypeOf table.Controls(0) Is Button
                Stop
            Case TypeOf table.Controls(0) Is Label
                CType(table.Controls(0), Label).Text = $"{CInt(rowIndex)} {rowIndex}"
        End Select

    End Sub

End Module
