' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Data
Imports System.Runtime.ExceptionServices
Imports System.Threading
Imports System.Windows.Forms
Imports CareLink
Imports Xunit

<Collection("Sequential")>
Public Class SgGridSizingTests

    <Fact>
    Public Sub ConfigureContentColumnSizing_SummaryRecordHeaderFitsAfterRefreshAndResize()
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using owner As New Form1(), host As New Form()
                        Dim dgv As DataGridView = owner.DgvSummary
                        host.ClientSize = New Drawing.Size(width:=600, height:=300)
                        dgv.Dock = DockStyle.Fill
                        host.Controls.Add(value:=dgv)
                        host.Show()
                        Dim records As New List(Of SummaryRecord) From {
                            New SummaryRecord(recordNumber:=0, key:=NameOf(ServerDataEnum.clientTimeZoneName),
                                              value:="America/Los_Angeles", message:="")}

                        For refresh As Integer = 1 To 2
                            dgv.UpdateSummaryTab(classCollection:=records, sort:=False, hideHeaderColumn:=False)
                            owner.UpdateDgvHeaders(dgv)
                            Application.DoEvents()
                            For Each dataColumn As DataGridViewColumn In dgv.Columns
                                If dataColumn.Visible Then
                                    Dim padding As Padding =
                                        dgv.Rows(index:=0).Cells(dataColumn.Index).GetFormattedStyle().Padding
                                    Assert.True(condition:=padding.Left >= 2 AndAlso padding.Right >= 2,
                                                userMessage:=$"{dataColumn.Name} padding was {padding}.")
                                End If
                            Next
                            Dim column As DataGridViewColumn =
                                dgv.Columns(columnName:=NameOf(SummaryRecord.RecordNumber))
                            Assert.Equal(expected:="Record Number", actual:=column.HeaderText)
                            For Each width As Integer In {600, 400}
                                host.ClientSize = New Drawing.Size(width, height:=300)
                                Application.DoEvents()
                                Assert.Equal(expected:=DataGridViewTriState.False,
                                             actual:=column.HeaderCell.InheritedStyle.WrapMode)
                                Dim requiredWidth As Integer =
                                    TextRenderer.MeasureText(text:=column.HeaderText,
                                                             font:=column.HeaderCell.InheritedStyle.Font).Width +
                                                                   column.HeaderCell.InheritedStyle.Padding.Horizontal +
                                                                   CInt(Math.Ceiling(a:=16.0 * dgv.DeviceDpi / 96.0))
                                Assert.True(condition:=column.Width >= requiredWidth,
                                            userMessage:=$"Actual={column.Width}, required={requiredWidth}")
                                Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.Fill,
                                             actual:=dgv.Columns(columnName:=NameOf(SummaryRecord.Message)).AutoSizeMode)
                            Next
                            dgv.SetHeaderColors(isDarkMode:=True, highContrast:=False)
                            Using bitmap As New Drawing.Bitmap(dgv.Width, dgv.Height)
                                dgv.DrawToBitmap(bitmap, targetBounds:=dgv.ClientRectangle)
                                Dim header As Drawing.Rectangle =
                                    dgv.GetCellDisplayRectangle(columnIndex:=0,
                                                                rowIndex:=-1,
                                                                cutOverflow:=False)
                                Dim pixel As Drawing.Color = bitmap.GetPixel(x:=header.Left + 4, y:=header.Top + 4)
                                Assert.True(condition:=pixel.R < 100 AndAlso pixel.G < 100 AndAlso pixel.B < 100,
                                            userMessage:=$"Header pixel was {pixel}")
                            End Using
                        Next
                    End Using
                Catch ex As Exception
                    failure = ex
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

    <Fact>
    Public Sub ConfigureContentColumnSizing_UsesLastVisibleColumnAndPreservesBasalGrid()
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using host As New Form(), dgv As New DataGridView(), basal As New DataGridView()
                        host.ClientSize = New Drawing.Size(width:=600, height:=300)
                        dgv.Dock = DockStyle.Fill
                        host.Controls.Add(value:=dgv)
                        dgv.Columns.Add(columnName:="First", headerText:="First")
                        dgv.Columns.Add(columnName:="LastVisible", headerText:="A long final column header")
                        dgv.Columns.Add(columnName:="Hidden", headerText:="Hidden")
                        dgv.Columns(columnName:="Hidden").Visible = False
                        dgv.Rows.Add("Value", "Message", "Hidden value")
                        host.Show()

                        dgv.ConfigureContentColumnSizing()
                        Dim last As DataGridViewColumn = dgv.Columns(columnName:="LastVisible")
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.AllCells,
                                     actual:=dgv.Columns(columnName:="First").AutoSizeMode)
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.Fill, actual:=last.AutoSizeMode)
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.AllCells,
                                     actual:=dgv.Columns(columnName:="Hidden").AutoSizeMode)
                        Assert.True(condition:=last.Width >= last.GetPreferredWidth(
                                        autoSizeColumnMode:=DataGridViewAutoSizeColumnMode.ColumnHeader,
                                        fixedHeight:=True))

                        last.DataPropertyName = NameOf(SG.SensorState)
                        dgv.Rows(index:=0).Cells(last.Index).Value = "No Error Message"
                        dgv.ConfigureContentColumnSizing()
                        Assert.Equal(expected:=DataGridViewTriState.False,
                                     actual:=dgv.Rows(index:=0).Cells(last.Index).InheritedStyle.WrapMode)
                        Assert.True(condition:=last.Width >= last.GetPreferredWidth(
                            autoSizeColumnMode:=DataGridViewAutoSizeColumnMode.AllCells, fixedHeight:=True))

                        basal.Name = NameOf(Form1.DgvBasalPerHour)
                        basal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                        basal.Columns.Add(columnName:="Basal", headerText:="Basal")
                        basal.ConfigureContentColumnSizing()
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnsMode.Fill,
                                     actual:=basal.AutoSizeColumnsMode)
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.NotSet,
                                     actual:=basal.Columns(columnName:="Basal").AutoSizeMode)
                    End Using
                Catch ex As Exception
                    failure = ex
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

    <Theory>
    <InlineData(5)>
    <InlineData(300)>
    Public Sub DisplayDataTable_SizesSensorColumnsAfterRefresh(rowCount As Integer)
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using owner As New Form1(), host As New Form(), table As New DataTable()
                        Dim dgv As DataGridView = owner.DgvSGs
                        host.ClientSize = New Drawing.Size(width:=1200, height:=400)
                        dgv.Dock = DockStyle.Fill
                        host.Controls.Add(value:=dgv)
                        host.Show()

                        table.Columns.Add(columnName:=NameOf(SG.RecordNumber), type:=GetType(Integer))
                        table.Columns.Add(columnName:=NameOf(SG.Sg), type:=GetType(Single))
                        table.Columns.Add(columnName:=NameOf(SG.SgMgdL), type:=GetType(Single))
                        table.Columns.Add(columnName:=NameOf(SG.SgMmolL), type:=GetType(Single))
                        table.Columns.Add(columnName:=NameOf(SG.SensorState), type:=GetType(String))
                        table.Columns.Add(columnName:=NameOf(SG.Message), type:=GetType(String))
                        For index As Integer = 1 To rowCount
                            If index = rowCount Then
                                table.Rows.Add(index, 107.0F, 107.0F, 5.9F, "SENSOR_NOT_READY",
                                    String.Concat(Enumerable.Repeat(element:="A long sensor message ", count:=30)))
                            Else
                                table.Rows.Add(index, 107.0F, 107.0F, 5.9F, "NO_ERROR_MESSAGE", "---")
                            End If
                        Next

                        For refresh As Integer = 1 To 2
                            DisplayDataTable(Of SG)(realPanel:=Nothing, table, dgv, rowIndex:=ServerDataEnum.sgs)
                            Application.DoEvents()

                            For Each column As DataGridViewColumn In dgv.Columns
                                If column.Visible Then
                                    Dim padding As Padding =
                                        dgv.Rows(index:=0).Cells(column.Index).GetFormattedStyle().Padding
                                    Assert.True(condition:=padding.Left >= 2 AndAlso padding.Right >= 2,
                                                userMessage:=$"{column.Name} padding was {padding}.")
                                End If
                            Next
                            Dim state As DataGridViewColumn = dgv.Columns(columnName:=NameOf(SG.SensorState))
                            Dim message As DataGridViewColumn = dgv.Columns(columnName:=NameOf(SG.Message))
                            Dim glucoseColumns As String() = {NameOf(SG.Sg), NameOf(SG.SgMgdL), NameOf(SG.SgMmolL)}
                            For Each columnName As String In glucoseColumns
                                Dim glucose As DataGridViewColumn = dgv.Columns(columnName)
                                Dim headerLines As String() = glucose.HeaderText.Split(separator:=New String() {vbCrLf},
                                                                                       options:=StringSplitOptions.None)
                                Assert.Equal(expected:=3, actual:=headerLines.Length)
                                Assert.Equal(expected:=DataGridViewTriState.True,
                                             actual:=glucose.HeaderCell.InheritedStyle.WrapMode)
                                Dim headerFont As Drawing.Font = glucose.HeaderCell.InheritedStyle.Font
                                For Each line As String In headerLines
                                    Assert.True(condition:=glucose.Width >= TextRenderer.MeasureText(text:=line, font:=headerFont).Width)
                                Next
                                Dim singleLine As String = String.Join(separator:=" ", value:=headerLines)
                                Assert.True(condition:=glucose.Width < TextRenderer.MeasureText(text:=singleLine, font:=headerFont).Width)
                                Assert.True(condition:=dgv.ColumnHeadersHeight >= 3 * TextRenderer.MeasureText(text:="Glucose", font:=headerFont).Height)
                            Next
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnsMode.None, actual:=dgv.AutoSizeColumnsMode)
                            Assert.Equal(expected:=DataGridViewAutoSizeRowsMode.DisplayedCells, actual:=dgv.AutoSizeRowsMode)
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.None, actual:=state.AutoSizeMode)
                            Assert.Equal(expected:=DataGridViewTriState.False, actual:=state.DefaultCellStyle.WrapMode)
                            Assert.Equal(expected:=DataGridViewTriState.False,
                                         actual:=dgv.Rows(index:=0).Cells(state.Index).InheritedStyle.WrapMode)
                            dgv.Rows(index:=0).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                            Dim stateStyle As DataGridViewCellStyle = dgv.Rows(index:=0).Cells(state.Index).GetFormattedStyle()
                            Assert.Equal(expected:=DataGridViewTriState.False, actual:=stateStyle.WrapMode)
                            Assert.Equal(expected:=DataGridViewContentAlignment.MiddleCenter, actual:=stateStyle.Alignment)
                            Assert.Equal(expected:="NO_ERROR_MESSAGE", actual:=dgv.Rows(index:=0).Cells(state.Index).FormattedValue)
                            Assert.True(condition:=state.Width >= TextRenderer.MeasureText(
                                text:="NO_ERROR_MESSAGE", font:=stateStyle.Font).Width)
                            dgv.Rows(index:=0).DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.Fill, actual:=message.AutoSizeMode)
                            Assert.Equal(expected:=DataGridViewTriState.True, actual:=message.DefaultCellStyle.WrapMode)
                            Assert.True(condition:=state.Width >= state.GetPreferredWidth(autoSizeColumnMode:=DataGridViewAutoSizeColumnMode.AllCells, fixedHeight:=True))
                            Assert.True(condition:=message.Width > state.Width)
                            Dim stateWidth As Integer = state.Width
                            Dim normalHeight As Integer = dgv.Rows(index:=0).Height
                            dgv.FirstDisplayedScrollingRowIndex = rowCount - 1
                            Application.DoEvents()
                            Assert.True(condition:=dgv.Rows(index:=rowCount - 1).Height > normalHeight)

                            Dim oldMessageWidth As Integer = message.Width
                            host.ClientSize = New Drawing.Size(width:=host.ClientSize.Width + 200, height:=400)
                            Application.DoEvents()
                            Assert.True(condition:=message.Width > oldMessageWidth)
                            Assert.Equal(expected:=stateWidth, actual:=state.Width)

                            table.DefaultView.Sort = $"{NameOf(SG.RecordNumber)} ASC"
                            Application.DoEvents()
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.None, actual:=state.AutoSizeMode)
                            Assert.Equal(expected:=stateWidth, actual:=state.Width)
                            Assert.Equal(expected:=DataGridViewTriState.False,
                                         actual:=dgv.Rows(index:=0).Cells(state.Index).InheritedStyle.WrapMode)
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.Fill, actual:=message.AutoSizeMode)
                        Next
                    End Using
                Catch ex As Exception
                    failure = ex
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

    <Fact>
    Public Sub InitializeDgv_AddsHorizontalCellPaddingWithoutChangingVerticalPadding()
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using host As New Form(), dgv As New DataGridView()
                        host.Controls.Add(value:=dgv)
                        host.Show()
                        dgv.InitializeDgv()
                        dgv.Columns.Add(columnName:="value", headerText:="")
                        dgv.Rows.Add("A long enough value to dominate sizing")
                        Dim column As DataGridViewColumn = dgv.Columns(0)
                        Dim paddedWidth As Integer = column.GetPreferredWidth(
                            autoSizeColumnMode:=DataGridViewAutoSizeColumnMode.AllCellsExceptHeader, fixedHeight:=True)
                        Assert.Equal(expected:=New Padding(left:=2, top:=0, right:=2, bottom:=0),
                                     actual:=dgv.Rows(index:=0).Cells(index:=0).InheritedStyle.Padding)
                        dgv.DefaultCellStyle.Padding = Padding.Empty
                        Dim unpaddedWidth As Integer = column.GetPreferredWidth(
                            autoSizeColumnMode:=DataGridViewAutoSizeColumnMode.AllCellsExceptHeader, fixedHeight:=True)
                        Assert.Equal(expected:=4, actual:=paddedWidth - unpaddedWidth)
                        dgv.InitializeDgv()
                        Assert.Equal(expected:=New Padding(left:=2, top:=0, right:=2, bottom:=0),
                                     actual:=dgv.DefaultCellStyle.Padding)
                        Dim style As New DataGridViewCellStyle()
                        style.SetCellStyle(align:=DataGridViewContentAlignment.MiddleRight,
                                           pad:=New Padding(left:=0, top:=1, right:=1, bottom:=1))
                        Assert.Equal(expected:=New Padding(left:=2, top:=1, right:=3, bottom:=1), actual:=style.Padding)
                        Assert.Equal(expected:=DataGridViewContentAlignment.MiddleRight, actual:=style.Alignment)
                    End Using
                Catch ex As Exception
                    failure = ex
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

    <Theory>
    <InlineData(True)>
    <InlineData(False)>
    Public Sub SensorGrid_SortGlyphIsRenderedAndTracksSort(isDarkMode As Boolean)
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using owner As New Form1(), host As New Form(), table As New DataTable()
                        Dim dgv As DataGridView = owner.DgvSGs
                        host.ClientSize = New Drawing.Size(width:=1200, height:=400)
                        host.Controls.Add(value:=dgv)
                        host.Show()
                        table.Columns.Add(columnName:=NameOf(SG.RecordNumber), type:=GetType(Integer))
                        table.Columns.Add(columnName:=NameOf(SG.Sg), type:=GetType(Single))
                        table.Columns.Add(columnName:=NameOf(SG.SgMgdL), type:=GetType(Single))
                        table.Columns.Add(columnName:=NameOf(SG.SgMmolL), type:=GetType(Single))
                        table.Columns.Add(columnName:=NameOf(SG.SensorState), type:=GetType(String))
                        table.Columns.Add(columnName:=NameOf(SG.Message), type:=GetType(String))
                        table.Rows.Add(3, 107.0F, 107.0F, 5.9F, "NO_ERROR_MESSAGE", "Three")
                        table.Rows.Add(2, 107.0F, 107.0F, 5.9F, "NO_ERROR_MESSAGE", "Two")
                        table.Rows.Add(1, 107.0F, 107.0F, 5.9F, "NO_ERROR_MESSAGE", "One")
                        DisplayDataTable(Of SG)(realPanel:=Nothing, table, dgv, rowIndex:=ServerDataEnum.sgs)
                        dgv.SetHeaderColors(isDarkMode, highContrast:=False)
                        Application.DoEvents()

                        Dim column As DataGridViewColumn = dgv.Columns(columnName:=NameOf(SG.RecordNumber))
                        Assert.Equal(expected:=SortOrder.Descending, actual:=column.HeaderCell.SortGlyphDirection)
                        For Each direction As System.ComponentModel.ListSortDirection In {
                            System.ComponentModel.ListSortDirection.Descending,
                            System.ComponentModel.ListSortDirection.Ascending}
                            dgv.Sort(dataGridViewColumn:=column, direction)
                            Application.DoEvents()
                            Dim expectedGlyph As SortOrder = If(direction = System.ComponentModel.ListSortDirection.Ascending,
                                                                SortOrder.Ascending,
                                                                SortOrder.Descending)
                            Assert.Equal(expected:=If(expectedGlyph = SortOrder.Ascending, 1, 3),
                                         actual:=CInt(dgv.Rows(index:=0).Cells(index:=0).Value))
                            Assert.Equal(expected:=expectedGlyph, actual:=column.HeaderCell.SortGlyphDirection)
                            Dim style As DataGridViewCellStyle = column.HeaderCell.InheritedStyle
                            Dim longestLine As Integer =
                                column.HeaderText.Split(separator:=New String() {vbCrLf}, options:=StringSplitOptions.None).Max(
                                    selector:=Function(line As String)
                                                  Return TextRenderer.MeasureText(text:=line, style.Font).Width
                                              End Function)
                            Assert.True(condition:=column.Width >= longestLine + style.Padding.Horizontal +
                                        CInt(Math.Ceiling(a:=32.0 * dgv.DeviceDpi / 96.0)))
                            Using sorted As New Drawing.Bitmap(dgv.Width, dgv.Height),
                                  unsorted As New Drawing.Bitmap(dgv.Width, dgv.Height)
                                dgv.DrawToBitmap(bitmap:=sorted, targetBounds:=dgv.ClientRectangle)
                                column.HeaderCell.SortGlyphDirection = SortOrder.None
                                dgv.DrawToBitmap(bitmap:=unsorted, targetBounds:=dgv.ClientRectangle)
                                Dim bounds As Drawing.Rectangle =
                                    dgv.GetCellDisplayRectangle(columnIndex:=column.Index,
                                                                rowIndex:=-1,
                                                                cutOverflow:=False)
                                Dim contrastingPixels As Integer = 0
                                Dim scale As Double = dgv.DeviceDpi / 96.0
                                Dim glyphWidth As Integer = CInt(Math.Ceiling(a:=9.0 * scale))
                                Dim glyphHeight As Integer = CInt(Math.Ceiling(a:=7.0 * scale))
                                Dim glyphLeft As Integer = bounds.Right - style.Padding.Right -
                                    CInt(Math.Ceiling(a:=5.0 * scale)) - glyphWidth
                                Dim glyphTop As Integer = bounds.Top + ((bounds.Height - glyphHeight) \ 2)
                                For y As Integer = glyphTop To glyphTop + glyphHeight - 1
                                    For x As Integer = glyphLeft To glyphLeft + glyphWidth - 1
                                        Dim pixel As Drawing.Color = sorted.GetPixel(x, y)
                                        If pixel.ToArgb() <> unsorted.GetPixel(x, y).ToArgb() AndAlso
                                           If(isDarkMode, pixel.R >= 180 AndAlso pixel.G >= 180 AndAlso pixel.B >= 180,
                                              pixel.R <= 100 AndAlso pixel.G <= 100 AndAlso pixel.B <= 100) Then
                                            contrastingPixels += 1
                                        End If
                                    Next
                                Next
                                Assert.True(condition:=contrastingPixels >= 5,
                                            userMessage:=$"{expectedGlyph} glyph has {contrastingPixels} contrasting pixels.")
                            End Using
                            column.HeaderCell.SortGlyphDirection = expectedGlyph
                        Next
                        Using ascending As New Drawing.Bitmap(dgv.Width, dgv.Height),
                              descending As New Drawing.Bitmap(dgv.Width, dgv.Height)
                            column.HeaderCell.SortGlyphDirection = SortOrder.Ascending
                            dgv.DrawToBitmap(bitmap:=ascending, targetBounds:=dgv.ClientRectangle)
                            column.HeaderCell.SortGlyphDirection = SortOrder.Descending
                            dgv.DrawToBitmap(bitmap:=descending, targetBounds:=dgv.ClientRectangle)
                            Dim bounds As Drawing.Rectangle =
                                dgv.GetCellDisplayRectangle(columnIndex:=column.Index, rowIndex:=-1, cutOverflow:=False)
                            Dim changedPixels As Integer = 0
                            Dim glyphRegionWidth As Integer = CInt(Math.Ceiling(a:=24.0 * dgv.DeviceDpi / 96.0))
                            For y As Integer = bounds.Top To bounds.Bottom - 1
                                For x As Integer = Math.Max(bounds.Left, bounds.Right - glyphRegionWidth) To bounds.Right - 1
                                    If ascending.GetPixel(x, y).ToArgb() <> descending.GetPixel(x, y).ToArgb() Then
                                        changedPixels += 1
                                    End If
                                Next
                            Next
                            Assert.True(condition:=changedPixels >= 5,
                                        userMessage:=$"Ascending and descending glyphs differ by {changedPixels} pixels.")
                        End Using
                    End Using
                Catch ex As Exception
                    failure = ex
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

End Class
