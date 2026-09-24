' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports DocumentFormat.OpenXml.Spreadsheet

''' <summary>
'''  Provides extension methods and delegates for displaying
'''  <see cref="DataTable"/> objects in <see cref="DataGridView"/> controls
'''  within a <see cref="TableLayoutPanel"/>. Handles initialization,
'''  data binding, and optional column visibility.
''' </summary>
Friend Module DgvDataTableHelpers

    Private Const BindingAttr As BindingFlags =
        BindingFlags.Public Or BindingFlags.Instance

    ''' <summary>
    '''  Delegate for attaching event handlers to a <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="dgv">
    '''  The <see cref="DataGridView"/> to attach handlers to.
    ''' </param>
    Friend Delegate Sub attachHandlers(dgv As DataGridView)

    Private ReadOnly Property Options As StringSplitOptions =
            StringSplitOptions.RemoveEmptyEntries

    Private ReadOnly Property Separator As Char() = New Char() {" "c}

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
                    col.HeaderText = SplitHeader(input)
                End If
            End If
            If col.HeaderText.Contains(value:="Record") Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                col.Width = If(dgv.Name = NameOf(Form1.DgvSGs),
                               100,
                               70)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            ElseIf col.HeaderText.Equals(value:="Time Change") Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            ElseIf col.HeaderText.Contains(value:="Time") Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
        Next
    End Sub

    'Private Sub WrapColumnHeaderTextOneWordPerLine(dgv As DataGridView)
    '    If dgv Is Nothing OrElse dgv.Columns Is Nothing OrElse dgv.Columns.Count = 0 Then
    '        Return
    '    End If
    '    dgv.ColumnHeadersDefaultCellStyle.WrapMode =
    '        DataGridViewTriState.True
    '    Dim headerFont As Font =
    '        If(dgv.ColumnHeadersDefaultCellStyle.Font, dgv.Font)

    '    Dim maxWords As Integer = 1
    '    For Each col As DataGridViewColumn In dgv.Columns
    '        Dim text As String = If(col.HeaderText, String.Empty)
    '        text = text.Replace(oldValue:=vbCrLf, newValue:=" ").
    '                    Replace(oldValue:=vbLf, newValue:=" ").
    '                    Replace(oldValue:=vbCr, newValue:=" ")
    '        Dim words As String() =
    '            text.Split(Separator, Options)
    '        If words.Length = 0 Then
    '            Continue For
    '        End If
    '        If words.Length > 1 Then
    '            col.HeaderText =
    '                String.Join(separator:=Environment.NewLine,
    '                            value:=words)
    '        End If
    '        If words.Length > maxWords Then
    '            maxWords = words.Length
    '        End If
    '    Next

    '    dgv.ColumnHeadersHeightSizeMode =
    '        DataGridViewColumnHeadersHeightSizeMode.EnableResizing
    '    Dim padding As Integer = 6
    '    dgv.ColumnHeadersHeight =
    '        ((headerFont.Height + 2) * maxWords) + padding
    'End Sub

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

        realPanel?.SetTableName(rowIndex, isClearedNotifications:=False)
        dgv.InitializeDgv()
        dgv.DataSource = table
        dgv.ApplyDisplayNames(Of T)()

        ' Allow cell content to wrap and calculate row heights
        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        ' Ensure columns expand to fill available width
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Convert any per-column AutoSizeMode (for example AllCells)
        ' to Fill and preserve relative widths by using current column
        ' widths to compute FillWeight.
        If dgv.Columns IsNot Nothing AndAlso dgv.Columns.Count > 0 Then
            Dim selector As Func(Of DataGridViewColumn, Integer) =
                Function(c As DataGridViewColumn) As Integer
                    Return c.Width
                End Function
            ' Resize columns to fit content first so Width reflects content size
            Const autoSizeColumnsMode As DataGridViewAutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            dgv.AutoResizeColumns(autoSizeColumnsMode)

            ' Cap overly wide columns so they don't dominate FillWeight
            Const maxColumnWidth As Integer = 400
            Dim cappedSelector As Func(Of DataGridViewColumn, Integer) =
                Function(c As DataGridViewColumn) As Integer
                    Return Math.Min(c.Width, maxColumnWidth)
                End Function

            Dim totalWidth As Integer =
                dgv.Columns.Cast(Of DataGridViewColumn)().Sum(selector:=cappedSelector)

            If totalWidth > 0 Then
                For Each c As DataGridViewColumn In dgv.Columns
                    Dim capped As Integer = Math.Min(c.Width, maxColumnWidth)
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    c.FillWeight = CSng(capped) / totalWidth * 100.0F
                Next
            Else
                For Each c As DataGridViewColumn In dgv.Columns
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    c.FillWeight = 100.0F
                Next
            End If
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
            dgv.DataSource = Nothing
            dgv.DataSource = table
            dgv.ApplyDisplayNames(Of T)()

            ' Ensure columns expand to fill available width
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ' Convert any per-column AutoSizeMode (for example AllCells) to Fill
            If dgv.Columns IsNot Nothing AndAlso dgv.Columns.Count > 0 Then
                Dim selector As Func(Of DataGridViewColumn, Integer) =
                    Function(c As DataGridViewColumn) As Integer
                        Return c.Width
                    End Function

                Dim totalWidth As Integer =
                    dgv.Columns.Cast(Of DataGridViewColumn)().Sum(selector)
                If dgv.Columns(index:=0).Name = "RecordNumber" Then
                    dgv.Columns(index:=0).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    dgv.Columns(index:=0).Width = 50
                    totalWidth -= 50
                End If
                If totalWidth > 0 Then
                    For Each c As DataGridViewColumn In dgv.Columns
                        If c.Name = "RecordNumber" Then
                            Continue For
                        End If
                        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        c.FillWeight = CSng(c.Width) / totalWidth * 100.0F
                    Next
                Else
                    For Each c As DataGridViewColumn In dgv.Columns
                        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        c.FillWeight = 100.0F
                    Next
                End If
            End If
            dgv.RowHeadersVisible = False
            'WrapColumnHeaderTextOneWordPerLine(dgv)

            Const columnName As String = "RecordNumber"
            If hideRecordNumberColumn AndAlso
                dgv.Columns(index:=0).Name = columnName Then
                dgv.Columns(columnName).Visible = False
            End If
        Else
            DgvNoRecordsFound(realPanel, className)
        End If
        realPanel.Refresh()
    End Sub

End Module
