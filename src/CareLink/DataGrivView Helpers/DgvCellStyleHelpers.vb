' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module DgvCellStyleHelpers

    Private ReadOnly s_alignmentTable As _
        New Dictionary(Of Type, Dictionary(Of String, DataGridViewCellStyle))

    Private ReadOnly s_columnsToHide As New Dictionary(Of Type, List(Of String)) From {
        {GetType(ActiveInsulin), New List(Of String) From {
            NameOf(ActiveInsulin.kind),
            NameOf(ActiveInsulin.Version)}},
        {GetType(AutoBasalDelivery), New List(Of String) From {
            NameOf(AutoBasalDelivery.OAdateTime)}},
        {GetType(AutoModeStatus), New List(Of String) From {
            NameOf(AutoModeStatus.Kind),
            NameOf(AutoModeStatus.Type)}},
        {GetType(BannerState), New List(Of String) From {}},
        {GetType(Basal), New List(Of String) From {}},
        {GetType(BgReading), New List(Of String) From {
            NameOf(BgReading.Kind),
            NameOf(BgReading.Type)}},
        {GetType(Calibration), New List(Of String) From {
            NameOf(Calibration.Kind),
            NameOf(Calibration.Type)}},
        {GetType(CareLinkUserDataRecord), New List(Of String) From {
            NameOf(CareLinkUserDataRecord.ID),
            NameOf(CareLinkUserDataRecord.CareLinkPassword)}},
        {GetType(Insulin), New List(Of String) From {
            NameOf(Insulin.Kind),
            NameOf(Insulin.OAdateTime),
            NameOf(Insulin.Type)}},
        {GetType(CurrentUserRecord), New List(Of String) From {}},
        {GetType(LastAlarm), New List(Of String) From {}},
        {GetType(LastSG), New List(Of String) From {
            "RecordNumber",
            NameOf(LastSG.Kind),
            NameOf(LastSG.Version)}},
        {GetType(Limit), New List(Of String) From {
            NameOf(Limit.Kind),
            NameOf(Limit.Version)}},
        {GetType(LowGlucoseSuspended), New List(Of String) From {
            NameOf(LowGlucoseSuspended.Kind),
            NameOf(LowGlucoseSuspended.Type)}},
        {GetType(Meal), New List(Of String) From {
            NameOf(Meal.Kind),
            NameOf(Meal.Type)}},
        {GetType(SG), New List(Of String) From {
            NameOf(SG.Kind),
            NameOf(SG.OaDateTime),
            NameOf(SG.Version)}},
        {GetType(TherapyAlgorithmState), New List(Of String) From {}},
        {GetType(TimeChange), New List(Of String) From {
            NameOf(TimeChange.Kind),
            NameOf(TimeChange.Type)}}}

    ''' <summary>
    '''  Gets the <see cref="DataGridViewCellStyle"/> for a given column name and data type.
    ''' </summary>
    ''' <typeparam name="T">
    '''  The data record type.
    ''' </typeparam>
    ''' <param name="name">
    '''  The name of the column for which to retrieve the cell style.
    ''' </param>
    ''' <returns>
    '''  The <see cref="DataGridViewCellStyle"/> for the specified column.
    ''' </returns>
    Public Function GetCellStyle(Of T As Class)(name As String) As DataGridViewCellStyle
        Dim key As Type = GetType(T)
        Dim value As Dictionary(Of String, DataGridViewCellStyle) = Nothing

        If Not s_alignmentTable.TryGetValue(key, value) Then
            value = New Dictionary(Of String, DataGridViewCellStyle)()
            s_alignmentTable(key) = value
        End If
        Return ClassPropertiesToColumnAlignment(Of T)(alignmentTable:=value, name)
    End Function

    ''' <summary>
    '''  Gets the formatted <see cref="DataGridViewCellStyle"/> for the
    '''  specified <see cref="DataGridViewCell"/>,
    '''  simulating the formatting event to retrieve the effective style.
    ''' </summary>
    ''' <param name="cell">
    '''  The <see cref="DataGridViewCell"/> to get the formatted style for.
    ''' </param>
    ''' <returns>
    '''  The <see cref="DataGridViewCellStyle"/> as it would appear after formatting.
    ''' </returns>
    <Extension>
    Friend Function GetFormattedStyle(cell As DataGridViewCell) As DataGridViewCellStyle
        Dim dgv As DataGridView = cell.DataGridView
        If dgv Is Nothing Then
            Return cell.InheritedStyle
        End If

        Const bindingAttr As BindingFlags =
            BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase
        Dim m As MethodInfo = dgv.GetType().GetMethod(
            name:="OnCellFormatting",
            bindingAttr,
            binder:=Nothing,
            types:=New Type() {GetType(DataGridViewCellFormattingEventArgs)},
            modifiers:=Nothing)
        Dim e As New DataGridViewCellFormattingEventArgs(
            cell.ColumnIndex,
            cell.RowIndex,
            cell.Value,
            desiredType:=cell.FormattedValueType,
            cellStyle:=cell.InheritedStyle)
        m.Invoke(obj:=dgv, parameters:=New Object() {e})
        Return e.CellStyle
    End Function

    ''' <summary>
    '''  Sets the alignment for a specific column in a DataGridView.
    ''' </summary>
    ''' <typeparam name="T">The data record type.</typeparam>
    ''' <param name="columnName">The name of the column to set the alignment for.</param>
    ''' <param name="alignment">The desired alignment for the column.</param>
    Friend Function HideColumn(Of T)(item As String) As Boolean
        Dim key As Type = GetType(T)
        Return s_filterJsonData AndAlso
            Not String.IsNullOrWhiteSpace(value:=item) AndAlso
            s_columnsToHide.ContainsKey(key) AndAlso
            s_columnsToHide(key).Contains(item)
    End Function

    ''' <summary>
    '''  Hide column based on column name matching name returned
    '''  by <paramref name="hideColFunc"/>
    ''' </summary>
    ''' <param name="dgv">
    '''  The <see cref="DataGridView"/> whose columns will be hidden.
    ''' </param>
    ''' <typeparam name="T">The type of the data record.</typeparam>
    Friend Sub HideDataGridViewColumnsByName(Of T)(ByRef dgv As DataGridView)
        Dim lastColumnIndex As Integer = dgv.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            If i > 0 AndAlso
               String.IsNullOrWhiteSpace(value:=dgv.Columns(index:=i).DataPropertyName) Then
                Stop
            End If
            Dim item As String = dgv.Columns(index:=i).DataPropertyName
            dgv.Columns(index:=i).Visible = Not HideColumn(Of T)(item)
        Next
    End Sub

    ''' <summary>
    '''  Hides a column if all its cell values match the specified value.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the column.</param>
    ''' <param name="columnName">The name of the column to check.</param>
    ''' <param name="value">The value to compare against each cell in the column.</param>
    Friend Sub HideUnneededColumns(
        ByRef dgv As DataGridView,
        columnName As String,
        value As String)

        Dim isColumnNeeded As Boolean = False
        Dim column As DataGridViewColumn = dgv.Columns(columnName)

        ' Check each cell in the current column
        For Each row As DataGridViewRow In dgv.Rows
            ' Skip checking new rows (if AllowUserToAddRows is True)
            If Not row.IsNewRow Then
                Dim cellValue As Object = row.Cells(column.Index).Value

                ' If the cell has value different then string then the column is needed
                If cellValue IsNot Nothing AndAlso cellValue.ToString <> value Then
                    isColumnNeeded = True
                    Exit For
                End If
            End If
        Next
        column.Visible = isColumnNeeded
    End Sub

    ''' <summary>
    '''  Determines if the row uses a dark background color.
    ''' </summary>
    ''' <param name="row">The <see cref="DataGridViewRow"/> to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the row uses a dark color;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Friend Function IsDarkRow(row As DataGridViewRow) As Boolean
        Return row.Index Mod 2 = 1
    End Function

    ''' <summary>
    '''  Sets the <see cref="DataGridViewCellStyle.Alignment"/> and
    '''  <see cref="DataGridViewCellStyle.Padding"/> properties
    '''  and returns the modified style.
    ''' </summary>
    ''' <param name="cellStyle">The <see cref="DataGridViewCellStyle"/> to modify.</param>
    ''' <param name="alignment">
    '''  The <see cref="DataGridViewContentAlignment"/> to set.
    ''' </param>
    ''' <param name="padding">The <see cref="Padding"/> to set.</param>
    ''' <returns>
    '''  The modified <see cref="DataGridViewCellStyle"/>.
    ''' </returns>
    <Extension>
    Friend Function SetCellStyle(
        cellStyle As DataGridViewCellStyle,
        alignment As DataGridViewContentAlignment,
        padding As Padding) As DataGridViewCellStyle

        cellStyle.Alignment = alignment
        cellStyle.Padding = padding
        Return cellStyle
    End Function

    ''' <summary>
    '''  Sets the cell value to <see cref="String.Empty"/> if the
    '''  value is <see langword="Nothing"/> or "0".
    '''  This is used to avoid displaying "0" in cells where it is not meaningful,
    '''  such as in a duration column.
    ''' </summary>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell
    '''  being formatted.
    ''' </param>
    <Extension>
    Public Sub CellFormatting0Value(ByRef e As DataGridViewCellFormattingEventArgs)
        Dim value As String = Convert.ToString(e.Value)
        If value = String.Empty OrElse value = "0" Then
            e.Value = String.Empty
            e.FormattingApplied = True
        End If
    End Sub

    ''' <summary>
    '''  Applies a <see cref="Color"/> to the cell based on whether it is a URI or not.
    '''  If it is a URI, it applies a purple color;
    '''  otherwise, it applies the specified highlight color.
    '''  Colored cells are also set to bold font style.
    '''  This is useful for highlighting important cells,
    '''  such as those containing URIs or other significant values.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for
    '''  the cell being formatted.
    ''' </param>
    ''' <param name="textColor">The color to use for highlighting.</param>
    ''' <param name="isUri">Indicates if the cell value is a URI.</param>
    ''' <param name="emIncrease"></param>
    <Extension>
    Public Sub CellFormattingApplyBoldColor(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        textColor As Color,
        Optional isUri As Boolean = False,
        Optional emIncrease As Integer = 0)

        Dim value As String = Convert.ToString(e.Value)
        If String.IsNullOrEmpty(value) Then
            e.Value = String.Empty
            e.FormattingApplied = True
            Return
        End If

        If value = ClickToShowDetails Then
            e.Value = value
            Dim row As DataGridViewRow = dgv.Rows(index:=e.RowIndex)
            row.Cells(index:=e.ColumnIndex).ToolTipText = $"{ClickToShowDetails} for {row.Cells(index:=1).Value}."
        End If

        Dim rowRef As DataGridViewRow = dgv.Rows(index:=e.RowIndex)
        With e.CellStyle
            If isUri Then
                Dim uriColor As Color = rowRef.GetTextColor(textColor:=Color.Purple)
                If rowRef.IsDarkRow() Then
                    .SelectionBackColor = uriColor.ContrastingColor()
                    .SelectionForeColor = uriColor
                Else
                    .SelectionBackColor = uriColor
                    .SelectionForeColor = uriColor.ContrastingColor()
                End If
            Else
                .ForeColor = rowRef.GetTextColor(textColor)
            End If
            .Font = New Font(family:= .Font.FontFamily, emSize:= .Font.Size + emIncrease, style:=FontStyle.Italic)
        End With
        e.FormattingApplied = True
    End Sub

    ''' <summary>
    '''  Formats the cell value as a date time string.
    '''  If the value is not a valid date, it will leave the value unchanged.
    '''  It also sets the foreground color based on the row's text color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for
    '''  the cell being formatted.
    ''' </param>
    <Extension>
    Public Sub CellFormattingDateTime(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs)

        Dim value As String = Convert.ToString(e.Value)
        If value <> String.Empty Then
            Try
                e.Value = value.ParseDate(key:="")
            Catch ex As Exception
                e.Value = value
            End Try
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Formats the cell value as an integer with a message appended,
    '''  and sets the foreground color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for the
    '''  cell being formatted.
    ''' </param>
    ''' <param name="message">The message to append to the value.</param>
    <Extension>
    Public Sub CellFormattingInteger(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        message As String)

        e.Value = $"{e.Value} {message}"
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Sets the foreground <see cref="Color"/> of a cell based on the row's text color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for
    '''  the cell being formatted.
    ''' </param>
    <Extension>
    Public Sub CellFormattingSetForegroundColor(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs)
        Dim col As DataGridViewTextBoxColumn =
            TryCast(dgv.Columns(e.ColumnIndex), DataGridViewTextBoxColumn)

        If col IsNot Nothing Then
            e.Value = $"{e.Value}"
            Dim textColor As Color = e.CellStyle.ForeColor
            Dim argb As Integer = textColor.ToArgb()
            If argb <> Color.Black.ToArgb() AndAlso argb <> Color.White.ToArgb() Then
                e.CellStyle.ForeColor = dgv.Rows(index:=e.RowIndex).GetTextColor(textColor)
            End If
            e.CellStyle.Font =
                New Font(prototype:=e.CellStyle.Font, newStyle:=FontStyle.Regular)
            e.FormattingApplied = True
        End If
    End Sub

    ''' <summary>
    '''  Formats the cell value as an SG (Sensor Glucose) value,
    '''  applying color based on TIR (Time In Range) limits.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for
    '''  the cell being formatted.
    ''' </param>
    ''' <param name="partialKey">
    '''  The partial column name key to match for formatting.
    ''' </param>
    <Extension>
    Public Sub CellFormattingSgValue(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        partialKey As String)

        Dim sgColumnName As String = dgv.Columns(index:=e.ColumnIndex).Name
        Dim sensorValue As Single = ParseSingle(e.Value, digits:=1)
        If Single.IsNaN(sensorValue) Then
            e.Value = "NaN"
            dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red)
        Else
            Dim provider As CultureInfo = CultureInfo.CurrentUICulture
            Dim format As String = GetSgFormat()
            Select Case sgColumnName
                Case partialKey
                    e.Value = sensorValue.ToString(format, provider)
                    If sensorValue < GetTirLowLimit() Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red)
                    ElseIf sensorValue > GetTirHighLimit() Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Yellow)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MgdL"
                    e.Value = Convert.ToString(e.Value)
                    If sensorValue < GetTirLowLimit(asMmolL:=False) Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red)
                    ElseIf sensorValue > GetTirHighLimit(asMmolL:=False) Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Yellow)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MmolL"
                    e.Value = sensorValue.ToString(format:="F1", provider)

                    Dim tirLowLimit As Single = GetTirLowLimit(asMmolL:=True)
                    If sensorValue.RoundToSingle(digits:=1) < tirLowLimit Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red)
                    ElseIf sensorValue > GetTirHighLimit(asMmolL:=True) Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Yellow)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case Else
                    Stop
            End Select
        End If
    End Sub

    ''' <summary>
    '''  Formats the cell value as a single-precision floating point
    '''  value with the specified number of digits.
    '''  Also sets the foreground color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for
    '''  the cell being formatted.
    ''' </param>
    ''' <param name="digits">The number of decimal digits to display.</param>
    ''' <returns>The parsed single value.</returns>
    ''' <param name="TrailingText"></param>
    <Extension>
    Public Function CellFormattingSingleValue(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        digits As Integer,
        Optional TrailingText As String = "") As Single

        Dim amount As Single = ParseSingle(e.Value, digits)
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        If TrailingText <> "" Then
            TrailingText = $" {TrailingText}"
        End If
        e.Value = $"{amount.ToString(format:=$"F{digits}", provider)}{TrailingText}"
        dgv.CellFormattingSetForegroundColor(e)
        Return amount
    End Function

    ''' <summary>
    '''  Formats the cell value to center-align it if it contains
    '''  a single word (no spaces) and column is not 0.
    '''  This is useful for displaying single-word values in a visually appealing manner.
    '''  Only alphabetic words (A-Z, a-z) are considered.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.
    ''' </param>
    <Extension>
    Public Sub CellFormattingSingleWord(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs)

        Dim input As String = Convert.ToString(e.Value)
        If e.ColumnIndex > 0 AndAlso
           Text.RegularExpressions.Regex.IsMatch(input, pattern:="^[A-Za-z]+$") Then

            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Formats the cell value as a title-cased string, replacing line breaks with spaces.
    '''  Also sets the foreground color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.
    ''' </param>
    <Extension>
    Public Sub CellFormattingToTitle(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs)

        e.Value = Convert.ToString(e.Value).Replace(oldValue:=vbCrLf, newValue:=" ").ToTitle
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Formats the cell as a URL, applying a color based on selection state.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.
    ''' </param>
    <Extension>
    Public Sub CellFormattingUrl(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs)

        e.Value = Convert.ToString(e.Value)
        Dim cell As DataGridViewCell =
            dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex)
        If cell.Equals(obj:=dgv.CurrentCell) Then
            dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Purple, isUri:=True)
        Else
            Dim textColor As Color = Color.FromArgb(red:=0, green:=160, blue:=204)
            dgv.CellFormattingApplyBoldColor(e, textColor, isUri:=True)
        End If
        e.FormattingApplied = True
    End Sub

End Module
