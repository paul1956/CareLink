' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.ExceptionServices
Imports System.Threading
Imports System.Windows.Forms
Imports CareLink
Imports Xunit

<Collection("Sequential")>
Public Class InitializePumpSettingsDialogTests

    Private Const BindingAttr As BindingFlags =
        BindingFlags.Instance Or BindingFlags.NonPublic

    Private Shared Sub RunOnSta(action As Action)
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    action()
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
    Public Sub DisabledButtons_DoNotChangeSchedule()
        Dim action As Action =
            Sub()
                Dim originalUser As CurrentUserRecord = SystemVariables.CurrentUser
                Try
                    Dim user As New CurrentUserRecord(userName:="Test", useAdvancedAitDecay:=CheckState.Unchecked)
                    user.CarbRatios.Add(item:=New CarbRatioRecord With {
                                           .StartTime = New TimeOnly(hour:=0, minute:=0),
                                           .EndTime = New TimeOnly(hour:=7, minute:=30), .CarbRatio = 5.0F})
                    user.CarbRatios.Add(item:=New CarbRatioRecord With {
                                           .StartTime = New TimeOnly(hour:=7, minute:=30),
                                           .EndTime = Eleven59, .CarbRatio = 8.0F})
                    SystemVariables.CurrentUser = user
                    Using dialog As New InitializePumpSettingsDialog(CurrentUserRecord:=user, newUser:=False)
                        dialog.Show()
                        Dim grid As DataGridView = dialog.InitializeDataGridView
                        Dim onClick As MethodInfo =
                            GetType(DataGridView).GetMethod(name:="OnCellContentClick",
                                                            BindingAttr)
                        Dim deleteCell As DataGridViewColumnControls.DataGridViewDisableButtonCell =
                            DirectCast(grid.Rows(index:=0).Cells(columnName:=NameOf(dialog.ColumnDeleteRow)),
                                        DataGridViewColumnControls.DataGridViewDisableButtonCell)

                        Assert.IsType(Of ScheduleButtonCell)([object]:=deleteCell)
                        Assert.False(condition:=deleteCell.Enabled)
                        Dim contentClicks As Integer = 0
                        AddHandler grid.CellContentClick, Sub(sender As Object, e As DataGridViewCellEventArgs)
                                                              contentClicks += 1
                                                          End Sub
                        Dim onKeyUp As MethodInfo =
                            GetType(ScheduleButtonCell).GetMethod(name:="OnKeyUp",
                                                                  BindingAttr)
                        onKeyUp.Invoke(obj:=deleteCell,
                                       parameters:={New KeyEventArgs(keyData:=Keys.Space), 0})
                        Assert.Equal(expected:=0,
                                     actual:=contentClicks)

                        Dim parameters As Object() =
                            {New DataGridViewCellEventArgs(columnIndex:=dialog.ColumnDeleteRow.Index,
                                                           rowIndex:=0)}
                        onClick.Invoke(obj:=grid,
                                       parameters)
                        onClick.Invoke(obj:=grid,
                                       parameters:={New DataGridViewCellEventArgs(columnIndex:=dialog.ColumnSave.Index,
                                                                                  rowIndex:=0)})
                        Assert.Equal(expected:=2,
                                     actual:=grid.RowCount)
                        For Each row As DataGridViewRow In grid.Rows
                            For Each cell As DataGridViewCell In row.Cells
                                Assert.Equal(expected:=String.Empty,
                                             actual:=cell.ToolTipText)
                            Next
                        Next
                        Assert.True(condition:=grid.ShowCellErrors)
                        Dim dataGridViewDisableButtonCell As DataGridViewColumnControls.DataGridViewDisableButtonCell =
                            DirectCast(grid.Rows(index:=1).Cells(columnName:=NameOf(dialog.ColumnDeleteRow)),
                                       DataGridViewColumnControls.DataGridViewDisableButtonCell)
                        Assert.True(condition:=dataGridViewDisableButtonCell.Enabled)
                        Dim parameters1 As Object() =
                            {New DataGridViewCellEventArgs(columnIndex:=dialog.ColumnDeleteRow.Index,
                                                           rowIndex:=1)}
                        onClick.Invoke(obj:=grid,
                                       parameters:=parameters1)
                        Assert.Equal(expected:=1,
                                     actual:=grid.RowCount)
                    End Using
                Finally
                    SystemVariables.CurrentUser = originalUser
                End Try
            End Sub

        RunOnSta(action)
    End Sub

    <Theory>
    <InlineData(0, 0)>
    <InlineData(23, 59)>
    <InlineData(7, 15)>
    Public Sub Load_FormatsSavedScheduleWithoutDataErrors(endHour As Integer, endMinute As Integer)
        Dim action As Action =
            Sub()
                Dim originalUser As CurrentUserRecord = SystemVariables.CurrentUser
                Try
                    Dim user As New CurrentUserRecord(userName:="Test", useAdvancedAitDecay:=CheckState.Unchecked)
                    If endHour <> 0 OrElse endMinute <> 0 Then
                        user.CarbRatios.Add(
                        item:=New CarbRatioRecord With {
                            .StartTime = New TimeOnly(hour:=0, minute:=0),
                            .EndTime = New TimeOnly(hour:=endHour, minute:=endMinute),
                            .CarbRatio = 12.5F})
                    End If
                    SystemVariables.CurrentUser = user
                    Using dialog As New InitializePumpSettingsDialog(CurrentUserRecord:=user, newUser:=False)
                        Dim grid As DataGridView = dialog.InitializeDataGridView
                        AddHandler grid.DataError, Sub(sender As Object, e As DataGridViewDataErrorEventArgs)
                                                       e.ThrowException = True
                                                   End Sub
                        dialog.Show()
                        Dim row As DataGridViewRow = grid.Rows(index:=0)
                        Dim endCell As DataGridViewCell = row.Cells(columnName:=NameOf(dialog.ColumnEnd))
                        Dim ratioCell As DataGridViewCell = row.Cells(columnName:=NameOf(dialog.ColumnNumericUpDown))
                        Dim expectedEnd As TimeOnly = If(endHour = 0 OrElse endHour = 23,
                                                         Eleven59,
                                                         New TimeOnly(hour:=endHour, minute:=endMinute))
                        Assert.Equal(expected:=expectedEnd,
                                     actual:=DirectCast(endCell.Value, TimeOnly))
                        Assert.False(condition:=String.IsNullOrEmpty(value:=CStr(endCell.FormattedValue)))
                        Dim choices As KeyValuePair(Of String, TimeOnly)() =
                            DirectCast(DirectCast(endCell, DataGridViewComboBoxCell).DataSource,
                                                  KeyValuePair(Of String, TimeOnly)())

                        Dim keySelector As Func(Of KeyValuePair(Of String, TimeOnly), TimeOnly) =
                            Function(item As KeyValuePair(Of String, TimeOnly))
                                Return item.Value
                            End Function
                        Assert.Equal(expected:=choices.OrderBy(keySelector),
                                     actual:=choices)
                        Dim filter As Predicate(Of KeyValuePair(Of String, TimeOnly)) =
                        Function(item As KeyValuePair(Of String, TimeOnly))
                            Return item.Value = expectedEnd
                        End Function

                        Assert.Contains(collection:=choices,
                                        filter)
                        Assert.IsType(Of Decimal)([object]:=ratioCell.Value)
                        Assert.False(condition:=String.IsNullOrEmpty(value:=CStr(ratioCell.FormattedValue)))
                    End Using
                Finally
                    SystemVariables.CurrentUser = originalUser
                End Try
            End Sub

        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub NumericCell_PartialRepaintStaysWithinClip()
        Dim action As Action =
            Sub()
                Using form As New Form(),
                        grid As New DataGridView(),
                        bitmap As New Bitmap(width:=220, height:=40),
                        graphics As Graphics = Graphics.FromImage(bitmap)
                    grid.AllowUserToAddRows = False
                    grid.Columns.Add(dataGridViewColumn:=New DataGridViewNumericUpDownColumn With {
                                        .CellTemplate = New TestNumericCell()})
                    grid.Columns.Add(columnName:="Next", headerText:="Next")
                    grid.Rows.Add()
                    form.Controls.Add(value:=grid)
                    form.Show()
                    grid.CurrentCell = grid.Rows(index:=0).Cells(index:=1)
                    Dim cell As TestNumericCell = DirectCast(grid.Rows(index:=0).Cells(index:=0), TestNumericCell)
                    Dim bounds As New Rectangle(x:=20, y:=5, width:=100, height:=30)
                    Dim clip As New Rectangle(x:=40, y:=5, width:=40, height:=30)
                    graphics.Clear(color:=Color.Magenta)
                    cell.PaintForTest(graphics, clip, bounds, style:=cell.InheritedStyle)
                    Dim painted As Boolean = False
                    For x As Integer = 0 To bitmap.Width - 1
                        For y As Integer = 0 To bitmap.Height - 1
                            If Not clip.Contains(x, y) Then
                                Assert.Equal(expected:=Color.Magenta.ToArgb(),
                                             actual:=bitmap.GetPixel(x, y).ToArgb())
                            ElseIf bitmap.GetPixel(x, y).ToArgb() <> Color.Magenta.ToArgb() Then
                                painted = True
                            End If
                        Next
                    Next
                    Assert.True(condition:=painted)
                End Using
            End Sub

        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub SaveRow_AddsNextPeriodAndCompletesAtEndOfDay()
        RunOnSta(Sub()
                     Dim originalUser As CurrentUserRecord = SystemVariables.CurrentUser
                     Try
                         Dim user As New CurrentUserRecord(userName:="Test", useAdvancedAitDecay:=CheckState.Unchecked)
                         SystemVariables.CurrentUser = user
                         Using dialog As New InitializePumpSettingsDialog(CurrentUserRecord:=user, newUser:=False)
                             Dim grid As DataGridView = dialog.InitializeDataGridView
                             AddHandler grid.DataError, Sub(sender As Object, e As DataGridViewDataErrorEventArgs)
                                                            e.ThrowException = True
                                                        End Sub
                             dialog.Show()
                             grid.Enabled = True
                             grid.Rows(index:=0).Cells(columnName:=NameOf(dialog.ColumnEnd)).Value =
                                 New TimeOnly(hour:=7, minute:=30)
                             Dim onClick As MethodInfo = GetType(DataGridView).GetMethod(
                                 name:="OnCellContentClick",
                                 BindingAttr)
                             Dim parameters As Object() =
                                {New DataGridViewCellEventArgs(columnIndex:=dialog.ColumnSave.Index, rowIndex:=0)}
                             onClick.Invoke(obj:=grid, parameters)

                             Assert.Equal(expected:=2,
                                          actual:=grid.RowCount)
                             Dim lastRow As DataGridViewRow = grid.Rows(index:=1)
                             For Each cell As DataGridViewCell In lastRow.Cells
                                 Assert.Equal(expected:=String.Empty,
                                              actual:=cell.ToolTipText)
                             Next
                             Assert.Equal(expected:=New TimeOnly(hour:=7, minute:=30).ToHoursMinutes(),
                                          actual:=CStr(lastRow.Cells(columnName:=NameOf(dialog.ColumnStart)).Value))
                             Dim endCell As DataGridViewCell = lastRow.Cells(columnName:=NameOf(dialog.ColumnEnd))
                             Assert.Equal(expected:=Eleven59,
                                          actual:=DirectCast(endCell.Value, TimeOnly))
                             Assert.False(condition:=String.IsNullOrEmpty(value:=CStr(endCell.FormattedValue)))

                             Dim value As Object =
                                lastRow.Cells(columnName:=NameOf(dialog.ColumnNumericUpDown)).Value
                             Assert.IsType(Of Decimal)([object]:=value)

                             Dim parameters1 As Object() =
                                {New DataGridViewCellEventArgs(columnIndex:=dialog.ColumnSave.Index, rowIndex:=1)}
                             onClick.Invoke(obj:=grid, parameters:=parameters1)
                             Assert.True(condition:=dialog.OK_Button.Enabled)
                             Assert.False(condition:=grid.Enabled)
                         End Using
                     Finally
                         SystemVariables.CurrentUser = originalUser
                     End Try
                 End Sub)
    End Sub

    Private Class TestNumericCell
        Inherits DataGridViewNumericUpDownCell

        Public Sub PaintForTest(graphics As Graphics, clip As Rectangle, bounds As Rectangle,
                                style As DataGridViewCellStyle)
            MyBase.Paint(graphics, clip, bounds, rowIndex:=0, cellState:=DataGridViewElementStates.None,
                         value:=5D, formattedValue:="5.0", errorText:=String.Empty, cellStyle:=style,
                         advancedBorderStyle:=New DataGridViewAdvancedBorderStyle(),
                         paintParts:=DataGridViewPaintParts.ContentForeground)
        End Sub

    End Class
End Class
