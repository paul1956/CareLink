' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms.VisualStyles

Public Class DataGridViewDisableButtonCell
    Inherits DataGridViewButtonCell

    Private _enabledValue As Boolean

    Public Property Enabled() As Boolean
        Get
            Return _enabledValue
        End Get
        Set(value As Boolean)
            _enabledValue = value
        End Set
    End Property

    ' Override the Clone method so that the Enabled property is copied.
    Public Overrides Function Clone() As Object
        Dim cell As DataGridViewDisableButtonCell =
            CType(MyBase.Clone(), DataGridViewDisableButtonCell)
        cell.Enabled = Me.Enabled
        Return cell
    End Function

    ' By default, enable the button cell.
    Public Sub New()
        _enabledValue = True
    End Sub

    Protected Overrides Sub Paint(graphics As Graphics,
                                    clipBounds As Rectangle,
                                    cellBounds As Rectangle,
                                    rowIndex As Integer,
                                    elementState As DataGridViewElementStates,
                                    value As Object,
                                    formattedValue As Object,
                                    errorText As String,
                                    cellStyle As DataGridViewCellStyle,
                                    advancedBorderStyle As DataGridViewAdvancedBorderStyle,
                                    paintParts As DataGridViewPaintParts)

        ' The button cell is disabled, so paint the border,
        ' background, and disabled button for the cell.
        If Not _enabledValue Then

            ' Draw the background of the cell, if specified.
            If (paintParts And DataGridViewPaintParts.Background) =
                DataGridViewPaintParts.Background Then

                Dim cellBackground As New SolidBrush(cellStyle.BackColor)
                graphics.FillRectangle(cellBackground, cellBounds)
                cellBackground.Dispose()
            End If

            ' Draw the cell borders, if specified.
            If (paintParts And DataGridViewPaintParts.Border) =
                DataGridViewPaintParts.Border Then

                Me.PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                    advancedBorderStyle)
            End If

            ' Calculate the area in which to draw the button.
            Dim buttonArea As Rectangle = cellBounds
            Dim buttonAdjustment As Rectangle = Me.BorderWidths(advancedBorderStyle)
            buttonArea.X += buttonAdjustment.X
            buttonArea.Y += buttonAdjustment.Y
            buttonArea.Height -= buttonAdjustment.Height
            buttonArea.Width -= buttonAdjustment.Width

            ' Draw the disabled button.
            ButtonRenderer.DrawButton(graphics, buttonArea,
                PushButtonState.Disabled)

            ' Draw the disabled button text.
            If TypeOf Me.FormattedValue Is String Then
                TextRenderer.DrawText(graphics,
                                        CStr(Me.FormattedValue),
                                        Me.DataGridView.Font,
                                        buttonArea,
                                        SystemColors.GrayText)
            End If
        Else
            ' The button cell is enabled, so let the base class
            ' handle the painting.
            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex,
                elementState, value, formattedValue, errorText,
                cellStyle, advancedBorderStyle, paintParts)
        End If
    End Sub

End Class
