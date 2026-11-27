' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports DataGridViewColumnControls

''' <summary>
'''  Provides extension methods for <see cref="DataGridView"/> to simplify
'''   initialization and configuration.
''' </summary>
Public Module DgvInitializationExtensions

    Private Const DefaultCellFontIncreasePoints As Single = 1.0F
    Private Const FillerCellFontIncreasePoints As Single = 2.0F
    Private ReadOnly s_gridFonts As New ConditionalWeakTable(Of DataGridView, GridFontState)()

    Private Sub Dgv_SystemColorsChanged(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        dgv.SetAlternatingRowColor(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        dgv.SetHeaderColors(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        dgv.Invalidate()
    End Sub

    Private Sub DgvContentSizing_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        dgv.ConfigureContentColumnSizing()
    End Sub

    Private Sub DgvFont_Disposed(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        Dim state As GridFontState = Nothing
        If s_gridFonts.TryGetValue(key:=dgv, value:=state) Then
            state.Font?.Dispose()
            s_gridFonts.Remove(key:=dgv)
        End If
    End Sub

    ''' <summary>
    '''  Applies a point-size increase to a grid's cell and header fonts.
    ''' </summary>
    ''' <param name="dgv">The grid whose fonts will be adjusted.</param>
    ''' <param name="increasePoints">The nonnegative increase in points relative to the grid font.</param>
    <Extension>
    Friend Sub ApplyCellFontIncrease(dgv As DataGridView,
                                     Optional increasePoints As Single = DefaultCellFontIncreasePoints)
        If dgv.Name = NameOf(Form1.DgvBasalPerHour) Then Return
        If Not Single.IsFinite(f:=increasePoints) OrElse increasePoints < 0 Then
            Throw New ArgumentOutOfRangeException(paramName:=NameOf(increasePoints))
        End If

        Dim state As GridFontState = Nothing
        If Not s_gridFonts.TryGetValue(key:=dgv, value:=state) Then
            state = New GridFontState()
            s_gridFonts.Add(key:=dgv, value:=state)
            AddHandler dgv.Disposed, AddressOf DgvFont_Disposed
        End If

        Dim baseFont As Font = dgv.Font
        Dim font As New Font(family:=baseFont.FontFamily,
                             emSize:=baseFont.SizeInPoints + increasePoints,
                             baseFont.Style,
                             unit:=GraphicsUnit.Point,
                             baseFont.GdiCharSet,
                             baseFont.GdiVerticalFont)
        Dim previousFont As Font = Nothing
        If state.Font IsNot Nothing AndAlso state.Font.Equals(obj:=font) Then
            font.Dispose()
        Else
            previousFont = state.Font
            state.Font = font
        End If
        dgv.DefaultCellStyle.Font = state.Font
        dgv.ColumnHeadersDefaultCellStyle.Font = state.Font
        dgv.RowHeadersDefaultCellStyle.Font = state.Font
        previousFont?.Dispose()

        dgv.RowTemplate.Height =
            Math.Max(dgv.RowTemplate.Height,
                     CInt(Math.Ceiling(a:=state.Font.GetHeight(dpi:=dgv.DeviceDpi) + (8.0 * dgv.DeviceDpi / 96.0))))
    End Sub

    <Extension>
    Friend Sub ApplyForm1GridFonts(dgv As DataGridView)
        If TypeOf dgv.FindForm() IsNot Form1 Then Return

        Dim hasFiller As Boolean = dgv.Columns.Cast(Of DataGridViewColumn)().
            Any(Function(column As DataGridViewColumn) column.DataPropertyName = NameOf(BasalPerHour.Filler))
        dgv.ApplyCellFontIncrease(increasePoints:=If(hasFiller,
                                                     FillerCellFontIncreasePoints,
                                                     DefaultCellFontIncreasePoints))
        dgv.ColumnHeadersDefaultCellStyle.Font = s_font12Bold
    End Sub

    ''' <summary>
    ''' Binds the specified list to the DataGridView using a BindingList and BindingSource
    ''' so that later additions/removals update the grid automatically.
    ''' </summary>
    <Extension>
    Friend Sub BindToList(Of T)(dgv As DataGridView, items As IList(Of T))
        If items Is Nothing Then
            dgv.DataSource = Nothing
            Return
        End If

        Dim list As BindingList(Of T) =
            If(TypeOf items Is BindingList(Of T),
               DirectCast(items, BindingList(Of T)),
               New BindingList(Of T)(list:=items.ToList()))

        Dim bs As New BindingSource(dataSource:=list,
                                    dataMember:=Nothing)
        dgv.DataSource = bs
    End Sub

    ''' <summary>
    '''  Initializes the specified <see cref="DataGridView"/> with default settings
    '''  for appearance and behavior.
    ''' </summary>
    ''' <param name="dgv">
    '''  The <see cref="DataGridView"/> to initialize.
    ''' </param>
    ''' <param name="dock">
    '''  The docking style for the <see cref="DataGridView"/>.
    '''  If not specified , defaults to <see cref="DockStyle.Fill"/>.
    ''' </param>
    <Extension>
    Friend Sub InitializeDgv(dgv As DataGridView,
                             Optional dock As DockStyle = DockStyle.Fill)

        With dgv
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .BorderStyle = BorderStyle.None
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .DataSource = Nothing
            .DefaultCellStyle.Padding =
                New Padding(left:=GridCellHorizontalPadding,
                            top:=0,
                            right:=GridCellHorizontalPadding,
                            bottom:=0)
            .Dock = dock
            .Location = New Point(x:=0, y:=0)
            .Padding = New Padding(all:=0)
            .ReadOnly = True
            .RowTemplate.Height = 24
            .TabIndex = 0
        End With

        dgv.ApplyCellFontIncrease()
        dgv.ApplyForm1GridFonts()
        dgv.InitializeGridTheme()

        RemoveHandler dgv.DataBindingComplete, AddressOf DgvContentSizing_DataBindingComplete
        If dgv.Name <> NameOf(Form1.DgvBasalPerHour) AndAlso dgv.Name <> NameOf(Form1.DgvSGs) Then
            AddHandler dgv.DataBindingComplete, AddressOf DgvContentSizing_DataBindingComplete
        End If
    End Sub

    ''' <summary>
    '''  Applies grid theme colors and refreshes them when system colors change.
    ''' </summary>
    ''' <param name="dgv">The grid to theme.</param>
    <Extension>
    Friend Sub InitializeGridTheme(dgv As DataGridView)
        dgv.SetAlternatingRowColor(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        dgv.SetHeaderColors(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        RemoveHandler dgv.SystemColorsChanged, AddressOf Dgv_SystemColorsChanged
        AddHandler dgv.SystemColorsChanged, AddressOf Dgv_SystemColorsChanged
    End Sub

    Friend Sub PaintDarkUserButton(graphics As Graphics, cellBounds As Rectangle, font As Font,
                                   text As String, enabled As Boolean, dpi As Integer)
        Dim inset As Integer = Math.Max(1, CInt(Math.Round(a:=2.0 * dpi / 96.0)))
        Dim bounds As Rectangle = Rectangle.Inflate(rect:=cellBounds, x:=-inset, y:=-inset)
        If bounds.Width <= 1 OrElse bounds.Height <= 1 Then Return

        Dim fill As Color = Color.FromArgb(red:=56, green:=56, blue:=56)
        Dim border As Color = If(enabled, Color.FromArgb(red:=170, green:=170, blue:=170),
                                 Color.FromArgb(red:=105, green:=105, blue:=105))
        Dim foreground As Color = If(enabled, Color.White, Color.FromArgb(red:=155, green:=155, blue:=155))
        Using brush As New SolidBrush(color:=fill), pen As New Pen(color:=border)
            graphics.FillRectangle(brush, rect:=bounds)
            graphics.DrawRectangle(pen, bounds.X, bounds.Y, width:=bounds.Width - 1, height:=bounds.Height - 1)
        End Using
        TextRenderer.DrawText(dc:=graphics, text, font, bounds, foreColor:=foreground,
                              flags:=TextFormatFlags.HorizontalCenter Or
                                     TextFormatFlags.VerticalCenter Or
                                     TextFormatFlags.SingleLine Or
                                     TextFormatFlags.EndEllipsis Or
                                     TextFormatFlags.PreserveGraphicsClipping)
    End Sub

    Friend Sub PaintDarkUserCheckBox(graphics As Graphics, cellBounds As Rectangle,
                                     checkState As CheckState, dpi As Integer)
        Dim size As Integer =
            Math.Min(Math.Min(cellBounds.Width, cellBounds.Height),
                     Math.Max(1, CInt(Math.Round(a:=14.0 * dpi / 96.0))))
        If size < 4 Then Return
        Dim bounds As New Rectangle(x:=cellBounds.X + ((cellBounds.Width - size) \ 2),
                                    y:=cellBounds.Y + ((cellBounds.Height - size) \ 2), width:=size, height:=size)
        Dim fill As Color = If(checkState = CheckState.Unchecked,
                               Color.FromArgb(red:=56, green:=56, blue:=56),
                               Color.FromArgb(red:=0, green:=100, blue:=180))
        Using brush As New SolidBrush(color:=fill),
              border As New Pen(color:=Color.FromArgb(red:=180, green:=180, blue:=180)),
              mark As New Pen(color:=Color.White, width:=Math.Max(1.0F, CSng(size) / 8.0F))

            graphics.FillRectangle(brush, rect:=bounds)
            graphics.DrawRectangle(pen:=border,
                                   bounds.X,
                                   bounds.Y,
                                   width:=bounds.Width - 1,
                                   height:=bounds.Height - 1)
            If checkState = CheckState.Checked Then
                Dim points As Point() =
                    {New Point(x:=bounds.X + (size \ 4), y:=bounds.Y + (size \ 2)),
                     New Point(x:=bounds.X + ((size * 2) \ 5), y:=bounds.Y + ((size * 3) \ 4)),
                     New Point(x:=bounds.X + ((size * 3) \ 4), y:=bounds.Y + (size \ 4))}
                graphics.DrawLines(pen:=mark,
                                   points)
            ElseIf checkState = CheckState.Indeterminate Then
                graphics.DrawLine(pen:=mark,
                                  x1:=bounds.X + (size \ 4),
                                  y1:=bounds.Y + (size \ 2),
                                  x2:=bounds.X + ((size * 3) \ 4),
                                  y2:=bounds.Y + (size \ 2))
            End If
        End Using
    End Sub

    <Extension>
    Friend Sub PaintDarkUserGridCell(dgv As DataGridView, e As DataGridViewCellPaintingEventArgs,
                                     isDarkMode As Boolean, highContrast As Boolean)
        If Not isDarkMode OrElse highContrast OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return

        Dim cell As DataGridViewCell = dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex)
        Dim button As DataGridViewDisableButtonCell = TryCast(cell, DataGridViewDisableButtonCell)
        If button Is Nothing AndAlso TypeOf cell IsNot DataGridViewCheckBoxCell Then Return

        If button Is Nothing Then
            Dim nativeParts As DataGridViewPaintParts = DataGridViewPaintParts.Background Or
                DataGridViewPaintParts.SelectionBackground Or DataGridViewPaintParts.Border Or
                DataGridViewPaintParts.Focus Or DataGridViewPaintParts.ErrorIcon
            e.Paint(e.ClipBounds, paintParts:=e.PaintParts And nativeParts)
        Else
            ' PaintBackground calls the button cell's Paint, which also draws a disabled button.
            If (e.PaintParts And DataGridViewPaintParts.Background) <> 0 Then
                Using brush As New SolidBrush(color:=e.CellStyle.BackColor)
                    e.Graphics.FillRectangle(brush, rect:=Rectangle.Intersect(e.CellBounds, e.ClipBounds))
                End Using
            End If
            If (e.PaintParts And DataGridViewPaintParts.Border) <> 0 Then
                Dim graphicsState As Drawing2D.GraphicsState = e.Graphics.Save()
                Try
                    e.Graphics.SetClip(rect:=Rectangle.Intersect(e.CellBounds, e.ClipBounds),
                                       combineMode:=Drawing2D.CombineMode.Intersect)
                    Using pen As New Pen(color:=dgv.GridColor)
                        e.Graphics.DrawLine(pen,
                                            x1:=e.CellBounds.Right - 1,
                                            y1:=e.CellBounds.Top,
                                            x2:=e.CellBounds.Right - 1,
                                            y2:=e.CellBounds.Bottom - 1)
                        e.Graphics.DrawLine(pen,
                                            x1:=e.CellBounds.Left,
                                            y1:=e.CellBounds.Bottom - 1,
                                            x2:=e.CellBounds.Right - 1,
                                            y2:=e.CellBounds.Bottom - 1)
                    End Using
                Finally
                    e.Graphics.Restore(gstate:=graphicsState)
                End Try
            End If
        End If

        If (e.PaintParts And DataGridViewPaintParts.ContentForeground) <> 0 Then
            If button IsNot Nothing Then
                Dim buttonBounds As Rectangle = e.CellBounds
                If dgv.ShowCellErrors AndAlso Not String.IsNullOrEmpty(value:=e.ErrorText) AndAlso
                   (e.PaintParts And DataGridViewPaintParts.ErrorIcon) <> 0 Then
                    buttonBounds.Width =
                        Math.Max(0, buttonBounds.Width - CInt(Math.Ceiling(a:=20.0 * dgv.DeviceDpi / 96.0)))
                End If
                Dim graphicsState As Drawing2D.GraphicsState = e.Graphics.Save()
                Try
                    e.Graphics.SetClip(rect:=Rectangle.Intersect(a:=e.CellBounds,
                                                                 b:=e.ClipBounds),
                                       combineMode:=Drawing2D.CombineMode.Intersect)
                    PaintDarkUserButton(e.Graphics,
                                        cellBounds:=buttonBounds,
                                        e.CellStyle.Font,
                                        text:=Convert.ToString(value:=e.FormattedValue),
                                        button.Enabled,
                                        dpi:=dgv.DeviceDpi)
                    If (e.PaintParts And DataGridViewPaintParts.Focus) <> 0 AndAlso
                        dgv.Focused AndAlso
                        dgv.CurrentCell Is cell Then

                        Dim inset As Integer = Math.Max(2, CInt(Math.Round(a:=4.0 * dgv.DeviceDpi / 96.0)))
                        Dim focusBounds As Rectangle = Rectangle.Inflate(rect:=buttonBounds, x:=-inset, y:=-inset)
                        If focusBounds.Width > 0 AndAlso focusBounds.Height > 0 Then
                            ControlPaint.DrawFocusRectangle(e.Graphics,
                                                            rectangle:=focusBounds,
                                                            foreColor:=Color.White,
                                                            backColor:=Color.FromArgb(red:=56, green:=56, blue:=56))
                        End If
                    End If
                Finally
                    e.Graphics.Restore(gstate:=graphicsState)
                End Try
            Else
                Dim checkState As CheckState = CheckState.Unchecked
                If TypeOf e.FormattedValue Is CheckState Then
                    checkState = DirectCast(e.FormattedValue, CheckState)
                ElseIf TypeOf e.FormattedValue Is Boolean AndAlso CBool(e.FormattedValue) Then
                    checkState = CheckState.Checked
                End If
                PaintDarkUserCheckBox(e.Graphics,
                                      e.CellBounds,
                                      checkState,
                                      dpi:=dgv.DeviceDpi)
            End If
        End If

        e.Handled = True
    End Sub

    <Extension>
    Friend Sub SetAlternatingRowColor(dgv As DataGridView, isDarkMode As Boolean, highContrast As Boolean)
        dgv.AlternatingRowsDefaultCellStyle.BackColor =
            If(highContrast,
               Color.Empty,
               If(isDarkMode,
                  Color.FromArgb(red:=72, green:=72, blue:=72),
                  Color.FromArgb(red:=230, green:=230, blue:=230)))
    End Sub

    <Extension>
    Friend Sub SetHeaderColors(dgv As DataGridView, isDarkMode As Boolean, highContrast As Boolean)
        Dim useDarkHeaders As Boolean = isDarkMode AndAlso Not highContrast
        dgv.EnableHeadersVisualStyles = Not useDarkHeaders
        For Each style As DataGridViewCellStyle In {dgv.ColumnHeadersDefaultCellStyle, dgv.RowHeadersDefaultCellStyle}
            style.BackColor = If(useDarkHeaders, Color.FromArgb(red:=38, green:=47, blue:=58), Color.Empty)
            style.ForeColor = If(useDarkHeaders, Color.White, Color.Empty)
            style.SelectionBackColor = If(useDarkHeaders, Color.FromArgb(red:=38, green:=47, blue:=58), Color.Empty)
            style.SelectionForeColor = If(useDarkHeaders, Color.White, Color.Empty)
        Next
    End Sub

    Private NotInheritable Class GridFontState
        Friend Property Font As Font
    End Class
End Module
