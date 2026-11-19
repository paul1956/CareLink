' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for <see cref="DataGridView"/> to simplify
'''   initialization and configuration.
''' </summary>
Public Module DgvInitializationExtensions

    Private Const DefaultCellFontIncreasePoints As Single = 1.0F
    Private ReadOnly s_gridFonts As New ConditionalWeakTable(Of DataGridView, GridFontState)()

    Private Sub Dgv_SystemColorsChanged(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        dgv.SetAlternatingRowColor(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        dgv.SetHeaderColors(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
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

        dgv.RowTemplate.Height = Math.Max(dgv.RowTemplate.Height,
            CInt(Math.Ceiling(a:=state.Font.GetHeight(dpi:=dgv.DeviceDpi) + (8.0 * dgv.DeviceDpi / 96.0))))
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
            If(TypeOf items Is BindingList(Of T), DirectCast(items, BindingList(Of T)), New BindingList(Of T)(items.ToList()))

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
            .DefaultCellStyle.Padding = New Padding(left:=GridCellHorizontalPadding, top:=0,
                                                    right:=GridCellHorizontalPadding, bottom:=0)
            .Dock = dock
            .Location = New Point(x:=0, y:=0)
            .Padding = New Padding(all:=0)
            .ReadOnly = True
            .RowTemplate.Height = 24
            .TabIndex = 0
        End With

        dgv.ApplyCellFontIncrease()
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
