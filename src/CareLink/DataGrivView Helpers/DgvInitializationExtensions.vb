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

    Private Sub Dgv_SystemColorsChanged(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        dgv.SetAlternatingRowColor(Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        dgv.SetHeaderColors(Application.IsDarkModeEnabled, SystemInformation.HighContrast)
    End Sub

    Private Sub DgvContentSizing_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        dgv.ConfigureContentColumnSizing()
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

        dgv.SetAlternatingRowColor(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        dgv.SetHeaderColors(isDarkMode:=Application.IsDarkModeEnabled, SystemInformation.HighContrast)
        RemoveHandler dgv.SystemColorsChanged, AddressOf Dgv_SystemColorsChanged
        AddHandler dgv.SystemColorsChanged, AddressOf Dgv_SystemColorsChanged

        RemoveHandler dgv.DataBindingComplete, AddressOf DgvContentSizing_DataBindingComplete
        If dgv.Name <> NameOf(Form1.DgvBasalPerHour) AndAlso dgv.Name <> NameOf(Form1.DgvSGs) Then
            AddHandler dgv.DataBindingComplete, AddressOf DgvContentSizing_DataBindingComplete
        End If
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

End Module
