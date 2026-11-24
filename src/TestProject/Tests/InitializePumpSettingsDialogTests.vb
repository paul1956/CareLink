' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.ExceptionServices
Imports System.Threading
Imports System.Windows.Forms
Imports CareLink
Imports Xunit

<Collection("Sequential")>
Public Class InitializePumpSettingsDialogTests

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
                            DirectCast(DirectCast(endCell, DataGridViewComboBoxCell).DataSource, KeyValuePair(Of String, TimeOnly)())
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
                             Dim onClick As Reflection.MethodInfo = GetType(DataGridView).GetMethod(
                                 name:="OnCellContentClick",
                                 bindingAttr:=Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
                             onClick.Invoke(obj:=grid,
                                            parameters:={New DataGridViewCellEventArgs(columnIndex:=dialog.ColumnSave.Index, rowIndex:=0)})

                             Assert.Equal(expected:=2,
                                          actual:=grid.RowCount)
                             Dim lastRow As DataGridViewRow = grid.Rows(index:=1)
                             Assert.Equal(expected:=New TimeOnly(hour:=7, minute:=30).ToHoursMinutes(),
                                          actual:=CStr(lastRow.Cells(columnName:=NameOf(dialog.ColumnStart)).Value))
                             Dim endCell As DataGridViewCell = lastRow.Cells(columnName:=NameOf(dialog.ColumnEnd))
                             Assert.Equal(expected:=Eleven59,
                                          actual:=DirectCast(endCell.Value, TimeOnly))
                             Assert.False(condition:=String.IsNullOrEmpty(value:=CStr(endCell.FormattedValue)))
                             Assert.IsType(Of Decimal)([object]:=lastRow.Cells(columnName:=NameOf(dialog.ColumnNumericUpDown)).Value)

                             onClick.Invoke(obj:=grid,
                                            parameters:={New DataGridViewCellEventArgs(columnIndex:=dialog.ColumnSave.Index, rowIndex:=1)})
                             Assert.True(condition:=dialog.OK_Button.Enabled)
                             Assert.False(condition:=grid.Enabled)
                         End Using
                     Finally
                         SystemVariables.CurrentUser = originalUser
                     End Try
                 End Sub)
    End Sub

End Class
