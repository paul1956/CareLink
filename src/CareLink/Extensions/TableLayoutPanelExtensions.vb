' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TableLayoutPanelExtensions

    Private ReadOnly tablesSupportingExport As New List(Of String) From {
                NameOf(Form1.TableLayoutPanelAutoBasalDelivery),
                NameOf(Form1.TableLayoutPanelInsulin),
                NameOf(Form1.TableLayoutPanelMeal),
                NameOf(Form1.TableLayoutPanelSgs)
    }

    <Extension>
    Friend Sub SetTabName(table As TableLayoutPanel, rowIndex As ItemIndexes)
        Select Case True
            Case TypeOf table.Controls(0) Is TableLayoutPanel
                Select Case True
                    Case TypeOf CType(table.Controls(0), TableLayoutPanel).Controls(0) Is Button
                        Dim helpString As String = If(tablesSupportingExport.Contains(table.Name), ": Right Click on Table for Export Options", "")
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
