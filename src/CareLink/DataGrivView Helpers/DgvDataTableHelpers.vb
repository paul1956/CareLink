' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods and delegates for displaying
'''  <see cref="DataTable"/> objects in <see cref="DataGridView"/> controls
'''  within a <see cref="TableLayoutPanel"/>. Handles initialization,
'''  data binding, and optional column visibility.
''' </summary>
Friend Module DgvDataTableHelpers

    Private Const BindingAttr As BindingFlags =
        BindingFlags.Public Or BindingFlags.Instance

#Region "DataGridView column sizing constants"

    Friend Const AllCells As DataGridViewAutoSizeColumnMode =
        DataGridViewAutoSizeColumnMode.AllCells

    Friend Const AllCellsExceptHeader As DataGridViewAutoSizeColumnMode =
        DataGridViewAutoSizeColumnMode.AllCellsExceptHeader

    Friend Const ColumnHeader As DataGridViewAutoSizeColumnMode =
        DataGridViewAutoSizeColumnMode.ColumnHeader

    Friend Const Fill As DataGridViewAutoSizeColumnMode =
        DataGridViewAutoSizeColumnMode.Fill

    Friend Const None As DataGridViewAutoSizeColumnMode =
        DataGridViewAutoSizeColumnMode.None

    Friend Const NotSet As DataGridViewAutoSizeColumnMode =
        DataGridViewAutoSizeColumnMode.NotSet

#End Region

#Region "DataGridView column sort mode constants"

    Friend Const Automatic As DataGridViewColumnSortMode =
        DataGridViewColumnSortMode.Automatic

    Friend Const NotSortable As DataGridViewColumnSortMode =
        DataGridViewColumnSortMode.NotSortable

    Friend Const Programmatic As DataGridViewColumnSortMode =
        DataGridViewColumnSortMode.Programmatic

#End Region

    ''' <summary>
    '''  Delegate for attaching event handlers to a <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="dgv">
    '''  The <see cref="DataGridView"/> to attach handlers to.
    ''' </param>
    Friend Delegate Sub attachHandlers(dgv As DataGridView)

    Private ReadOnly Property Separator As Char() = New Char() {" "c}

    ''' <summary>
    '''  Measures the SG columns once and lets the last visible column fill the remaining space.
    ''' </summary>
    ''' <param name="dgv">The sensor glucose grid to size.</param>
    <Extension>
    Private Sub ConfigureSgColumnSizing(dgv As DataGridView)
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        Dim lastColumn As DataGridViewColumn = Nothing
        For Each column As DataGridViewColumn In dgv.Columns
            column.AutoSizeMode = None
            If column.Visible AndAlso (lastColumn Is Nothing OrElse column.DisplayIndex > lastColumn.DisplayIndex) Then
                lastColumn = column
            End If
            If column.DataPropertyName = NameOf(SG.SensorState) Then
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.False
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
        Next

        For Each column As DataGridViewColumn In dgv.Columns
            If column.Visible AndAlso column IsNot lastColumn Then
                Select Case column.DataPropertyName
                    Case NameOf(SG.Sg), NameOf(SG.SgMgdL), NameOf(SG.SgMmolL)
                        column.HeaderCell.Style.WrapMode = DataGridViewTriState.True
                        Dim headerStyle As DataGridViewCellStyle = column.HeaderCell.InheritedStyle
                        Dim headerWidth As Integer = 0
                        Dim separator As String() = New String() {vbCrLf}
                        For Each line As String In column.HeaderText.Split(separator,
                                                                           options:=StringSplitOptions.None)
                            headerWidth =
                                Math.Max(headerWidth, TextRenderer.MeasureText(text:=line, headerStyle.Font).Width)
                        Next
                        headerWidth +=
                            headerStyle.Padding.Horizontal + CInt(Math.Ceiling(a:=8.0 * dgv.DeviceDpi / 96.0))
                        Dim contentWidth As Integer =
                            column.GetPreferredWidth(autoSizeColumnMode:=AllCellsExceptHeader,
                                                     fixedHeight:=True)
                        column.Width = Math.Max(column.MinimumWidth, Math.Max(headerWidth, contentWidth))
                    Case Else
                        column.Width = Math.Max(column.MinimumWidth,
                            column.GetPreferredWidth(autoSizeColumnMode:=AllCells,
                                                     fixedHeight:=True))
                End Select
                If column.SortMode <> NotSortable AndAlso dgv.ColumnHeadersVisible Then
                    Dim style As DataGridViewCellStyle = column.HeaderCell.InheritedStyle
                    Dim headerWidth As Integer = 0
                    Dim separator As String() = New String() {vbCrLf}
                    For Each line As String In column.HeaderText.Split(separator,
                                                                       options:=StringSplitOptions.None)
                        headerWidth = Math.Max(headerWidth, TextRenderer.MeasureText(line, style.Font).Width)
                    Next
                    headerWidth += style.Padding.Horizontal + CInt(Math.Ceiling(32.0 * dgv.DeviceDpi / 96.0))
                    column.Width = Math.Max(column.Width, headerWidth)
                End If
            End If
        Next

        If lastColumn IsNot Nothing Then
            If lastColumn.DataPropertyName = NameOf(SG.SensorState) Then
                lastColumn.MinimumWidth = Math.Max(lastColumn.MinimumWidth,
                    lastColumn.GetPreferredWidth(autoSizeColumnMode:=AllCells, fixedHeight:=True))
            Else
                lastColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                If dgv.ColumnHeadersVisible Then
                    lastColumn.MinimumWidth = Math.Max(lastColumn.MinimumWidth,
                        lastColumn.GetPreferredWidth(autoSizeColumnMode:=ColumnHeader, fixedHeight:=True))
                End If
            End If
            lastColumn.AutoSizeMode = Fill
        End If
    End Sub

    ''' <summary>
    '''  Splits a space-separated string into two approximately equal halves,
    '''  inserting a CRLF between them without breaking words.
    ''' </summary>
    ''' <param name="input">The original title string.</param>
    ''' <returns>The string split into two lines.</returns>
    Private Function SplitHeader(input As String) As String
        If String.IsNullOrWhiteSpace(value:=input) Then
            Return input ' Return as-is if empty or null
        End If

        ' Normalize non-breaking spaces to normal spaces so splitting and
        ' prefix/matching logic treat them identically and remain idempotent.
        If input.Contains(value:=NonBreakingSpace) Then
            input = input.Replace(oldValue:=NonBreakingSpace, newValue:=" "c)
        End If

        ' If the header already contains line breaks (from a previous pass),
        ' normalize them to spaces so this function is idempotent and will not
        ' insert additional line breaks on subsequent calls.
        If input.Contains(value:=vbCrLf) OrElse
           input.Contains(value:=vbLf) OrElse
           input.Contains(value:=vbCr) Then
            input = input.Replace(oldValue:=vbCrLf, newValue:=" ") _
                         .Replace(oldValue:=vbLf, newValue:=" ") _
                         .Replace(oldValue:=vbCr, newValue:=" ")
        End If

        ' Normalize spaces
        Dim words() As String = input.Split(Separator, Options)
        If words.Length = 1 Then
            Return input ' Single word, nothing to split
        End If

        If input.Length < 12 Then
            Return input
        End If

        If words(0) = "Sensor" AndAlso words.Length = 3 Then
            Return String.Join(separator:=vbCrLf,
                               value:=words)
        End If
        ' Find the midpoint in characters
        Dim totalLength As Integer = input.Length
        Dim halfLength As Integer = totalLength \ 2

        ' Build first half until we pass the midpoint
        Dim firstHalf As String = ""
        Dim secondHalf As String = ""
        Dim currentLength As Integer = 0

        For i As Integer = 0 To words.Length - 1
            If currentLength + words(i).Length <= halfLength OrElse firstHalf = "" Then
                If firstHalf <> "" Then firstHalf &= " "
                firstHalf &= words(i)
                currentLength += words(i).Length + 1
            Else
                If secondHalf <> "" Then secondHalf &= " "
                secondHalf &= words(i)
            End If
        Next

        Return $"{firstHalf}{vbCrLf}{secondHalf}"
    End Function

    ''' <summary>
    '''  Sets DataGridView column headers to the DisplayName attribute values of the class properties.
    ''' </summary>
    ''' <param name="dgv">
    '''  The DataGridView to apply display names to.
    ''' </param>
    ''' <typeparam name="T">
    '''  The class type whose properties' DisplayName attributes will be used.
    ''' </typeparam>
    <Extension>
    Friend Sub ApplyDisplayNames(Of T)(dgv As DataGridView)
        Dim props As PropertyInfo() =
            GetType(T).GetProperties(BindingAttr)

        For Each col As DataGridViewColumn In dgv.Columns
            Dim predicate As Func(Of PropertyInfo, Boolean) =
                Function(p As PropertyInfo) As Boolean
                    Return p.Name = col.DataPropertyName
                End Function

            Dim prop As PropertyInfo = props.FirstOrDefault(predicate)
            If prop IsNot Nothing Then
                Dim displayNameAttr As DisplayNameAttribute =
                    prop.GetCustomAttribute(Of DisplayNameAttribute)()

                If displayNameAttr IsNot Nothing Then
                    Dim result As String = String.Empty
                    Dim input As String = displayNameAttr.DisplayName
                    ' Preserve non-breaking-space rendering only for DgvSGs where required.
                    If dgv IsNot Nothing AndAlso dgv.Name = NameOf(Form1.DgvSGs) AndAlso
                       (input.Contains(value:="From Pump") OrElse input.Contains(value:="As Date")) Then
                        input = input.Replace(oldValue:=" "c, newValue:=NonBreakingSpace)
                    End If
                    col.HeaderText = SplitHeader(input)
                End If
            End If
            If dgv.Name = NameOf(Form1.DgvSGs) Then Continue For
            If col.HeaderText.Contains(value:="Record") Then
                col.AutoSizeMode = None
                col.Width = If(dgv.Name = NameOf(Form1.DgvSGs),
                               100,
                               70)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            ElseIf col.HeaderText.Equals(value:="Time Change") Then
                col.AutoSizeMode = AllCells
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            ElseIf col.HeaderText.Contains(value:="Time") Then
                col.AutoSizeMode = AllCellsExceptHeader
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
        Next
    End Sub

    ''' <summary>
    '''  Sizes columns to their contents and lets the last visible column fill the remaining space.
    ''' </summary>
    ''' <param name="dgv">The grid to size.</param>
    <Extension>
    Friend Sub ConfigureContentColumnSizing(dgv As DataGridView)
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        Dim lastColumn As DataGridViewColumn = Nothing
        For Each column As DataGridViewColumn In dgv.Columns
            If column.Visible AndAlso (lastColumn Is Nothing OrElse column.DisplayIndex > lastColumn.DisplayIndex) Then
                lastColumn = column
            End If
            If column.DataPropertyName = NameOf(SG.SensorState) Then
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.False
            End If
            If dgv.Name = NameOf(Form1.DgvSummary) AndAlso
               column.DataPropertyName = NameOf(SummaryRecord.RecordNumber) Then
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False
                Dim headerStyle As DataGridViewCellStyle = column.HeaderCell.InheritedStyle
                Dim headerWidth As Integer = TextRenderer.MeasureText(column.HeaderText, headerStyle.Font).Width +
                    headerStyle.Padding.Horizontal + CInt(Math.Ceiling(16.0 * dgv.DeviceDpi / 96.0))
                column.MinimumWidth = Math.Max(column.MinimumWidth, headerWidth)
            End If
            column.AutoSizeMode = AllCells
        Next

        If lastColumn IsNot Nothing Then
            If lastColumn.DataPropertyName = NameOf(SG.SensorState) Then
                lastColumn.MinimumWidth = Math.Max(lastColumn.MinimumWidth, lastColumn.Width)
            Else
                lastColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                If dgv.ColumnHeadersVisible Then
                    lastColumn.MinimumWidth = Math.Max(lastColumn.MinimumWidth,
                        lastColumn.GetPreferredWidth(autoSizeColumnMode:=ColumnHeader, fixedHeight:=True))
                End If
            End If
            lastColumn.AutoSizeMode = Fill
        End If
    End Sub

    ''' <summary>
    '''  Displays a <see cref="DataTable"/> in a <see cref="DataGridView"/> within
    '''  a <see cref="TableLayoutPanel"/>. Initializes the <see cref="DataGridView"/>,
    '''  sets its data source, and refreshes the panel.
    ''' </summary>
    ''' <param name="realPanel">
    '''  The <see cref="TableLayoutPanel"/> containing the <see cref="DataGridView"/>.
    ''' </param>
    ''' <param name="table">The <see cref="DataTable"/> to display.</param>
    ''' <param name="dgv">The <see cref="DataGridView"/> to display the data in.</param>
    ''' <param name="rowIndex">
    '''  The row index in the panel, typically of type <see cref="ServerDataEnum"/>.
    ''' </param>
    <Extension>
    Friend Sub DisplayDataTable(Of T)(realPanel As TableLayoutPanel,
                                      table As DataTable,
                                      dgv As DataGridView,
                                      rowIndex As ServerDataEnum)

        ' Entry: DisplayDataTable

        realPanel?.SetTableName(rowIndex, isClearedNotifications:=False)
        Dim isSgGrid As Boolean = dgv.Name = NameOf(Form1.DgvSGs)
        If isSgGrid Then dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgv.InitializeDgv()
        Try
            ' Notify form that a binding is starting so cursor stays busy until all bindings finish
            BindingStarted(owner:=My.Forms.Form1, dgv)
        Catch
            ' Swallow to avoid crashing display path
        End Try
        dgv.DataSource = table
        ' Binding assigned; any outstanding diagnostics removed.
        If isSgGrid Then HideDataGridViewColumnsByName(Of SG)(dgv)
        dgv.ApplyDisplayNames(Of T)()

        Dim cols As Integer =
            If(dgv.Columns IsNot Nothing,
               dgv.Columns.Count,
               0)
        ' DataSource assigned

        ' Allow cell content to wrap and calculate row heights
        dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        If isSgGrid Then
            dgv.ConfigureSgColumnSizing()
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
        Else
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            dgv.ConfigureContentColumnSizing()
        End If
        dgv.RowHeadersVisible = False
        realPanel?.Refresh()
    End Sub

    ''' <summary>
    '''  Displays a <see cref="DataTable"/> in a <see cref="DataGridView"/> within
    '''  a <see cref="TableLayoutPanel"/>, using the class name and row index
    '''  to identify the context. Optionally hides the "RecordNumber" column.
    '''  If the table is empty, displays an empty <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="realPanel">
    '''  The <see cref="TableLayoutPanel"/> containing the <see cref="DataGridView"/>.
    ''' </param>
    ''' <param name="table">The <see cref="DataTable"/> to display.</param>
    ''' <param name="className">The class name context for the data.</param>
    ''' <param name="rowIndex">
    '''  The row index in the panel, typically of type <see cref="ServerDataEnum"/>.
    ''' </param>
    ''' <param name="hideRecordNumberColumn">
    '''  If <see langword="True"/>, hides the "RecordNumber" column if present.
    ''' </param>
    <Extension>
    Friend Sub DisplayDataTable(Of T)(realPanel As TableLayoutPanel,
                                      table As DataTable,
                                      className As String,
                                      rowIndex As ServerDataEnum,
                                      Optional hideRecordNumberColumn As Boolean = False)

        realPanel.SetTableName(rowIndex, isClearedNotifications:=False)
        If table?.Rows.Count > 0 Then
            Dim index As Integer = realPanel.Controls.Count - 1
            Dim dgv As DataGridView =
                TryCast(realPanel.Controls(index), DataGridView)

            dgv.InitializeDgv()
            Dim savedMode As DataGridViewAutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            ' For large tables avoid expensive per-row measurement by disabling
            ' AutoSizeRows and using a fixed RowTemplate height before binding.
            Dim largeGrid As Boolean =
                table IsNot Nothing AndAlso
                table.Rows.Count > 100

            If largeGrid Then
                dgv.SuspendLayout()
                dgv.Visible = False
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            End If

            Try
                Try
                    ' Notify form that a binding is starting so cursor stays busy until all bindings finish
                    BindingStarted(owner:=My.Forms.Form1, dgv)
                Catch
                    Stop
                End Try
                dgv.DataSource = Nothing
                dgv.DataSource = table
            Finally
                If largeGrid Then
                    dgv.Visible = True
                    dgv.ResumeLayout(False)
                End If
            End Try
            dgv.ApplyDisplayNames(Of T)()
            dgv.RowHeadersVisible = False
            'WrapColumnHeaderTextOneWordPerLine(dgv)

            Const columnName As String = "RecordNumber"
            If hideRecordNumberColumn AndAlso
                dgv.Columns(index:=0).Name = columnName Then
                dgv.Columns(columnName).Visible = False
            End If
            dgv.ConfigureContentColumnSizing()
            ' DisplayDataTable complete (className overload)
        Else
            DgvNoRecordsFound(realPanel, className)
        End If
        realPanel.Refresh()
    End Sub

End Module
