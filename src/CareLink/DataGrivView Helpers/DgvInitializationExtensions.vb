' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Collections.Generic

''' <summary>
'''  Provides extension methods for <see cref="DataGridView"/> to simplify
'''   initialization and configuration.
''' </summary>
Public Module DgvInitializationExtensions

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

        Dim emSize As Single =
            If(dgv.Name = NameOf(Form1.DgvBasalPerHour),
               12.0!,
               10.0!)

        With dgv
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .AlternatingRowsDefaultCellStyle =
                New DataGridViewCellStyle With {
                    .BackColor = Color.FromArgb(red:=45, green:=45, blue:=45),
                    .ForeColor = Color.White,
                    .SelectionBackColor = Color.FromArgb(red:=51, green:=153, blue:=255),
                    .SelectionForeColor = Color.White}
            .BorderStyle = BorderStyle.None
            .ColumnHeadersDefaultCellStyle =
                New DataGridViewCellStyle With {
                    .Alignment = DataGridViewContentAlignment.MiddleCenter,
                    .BackColor = Color.FromArgb(red:=38, green:=47, blue:=58),
                    .Font = New Font(FamilyName, emSize, style:=FontStyle.Bold),
                    .WrapMode = DataGridViewTriState.True}
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .DataSource = Nothing
            .Dock = dock
            .Location = New Point(x:=0, y:=0)
            .Padding = New Padding(all:=0)
            .ReadOnly = True
            .RowsDefaultCellStyle =
                New DataGridViewCellStyle With {
                    .BackColor = Color.FromArgb(red:=180, green:=180, blue:=180),
                    .Font = New Font(FamilyName, emSize, style:=FontStyle.Bold),
                    .ForeColor = Color.Black,
                    .SelectionBackColor = Color.FromArgb(red:=51, green:=153, blue:=255),
                    .SelectionForeColor = Color.White}
            .RowTemplate.Height = 24
            .TabIndex = 0
        End With
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
    ''' Binds the specified enumerable to the DataGridView. If the enumerable
    ''' is already a BindingList(Of T) it will be used directly; otherwise a
    ''' BindingList(Of T) is created from the sequence.
    ''' </summary>
    <Extension>
    Friend Sub BindToList(Of T)(dgv As DataGridView, items As IEnumerable(Of T))
        If items Is Nothing Then
            dgv.DataSource = Nothing
            Return
        End If

        ' If items is already a BindingList use it directly
        If TypeOf items Is BindingList(Of T) Then
            dgv.DataSource = New BindingSource(DirectCast(items, BindingList(Of T)), Nothing)
            Return
        End If

        ' If items is an IList(Of T) we can pass it to BindingList constructor
        Dim list As BindingList(Of T) =
            If(TypeOf items Is IList(Of T), New BindingList(Of T)(DirectCast(items, IList(Of T))), New BindingList(Of T)(items.ToList()))

        dgv.DataSource = New BindingSource(list, Nothing)
    End Sub

    ''' <summary>
    ''' Binds an existing IBindingList to the DataGridView via a BindingSource.
    ''' This preserves the custom IBindingList implementation semantics.
    ''' </summary>
    <Extension>
    Friend Sub BindToList(Of T)(dgv As DataGridView, items As IBindingList)
        If items Is Nothing Then
            dgv.DataSource = Nothing
            Return
        End If

        dgv.DataSource = New BindingSource(items, Nothing)
    End Sub

End Module
