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

    <Theory>
    <InlineData(5)>
    <InlineData(150)>
    Public Sub DisplayDataTable_SizesSensorColumnsAfterRefresh(rowCount As Integer)
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using owner As New Form1(), host As New Form(), table As New DataTable()
                        Dim dgv As DataGridView = owner.DgvSGs
                        host.ClientSize = New Drawing.Size(width:=1200, height:=400)
                        dgv.Dock = DockStyle.Fill
                        host.Controls.Add(dgv)
                        host.Show()

                        table.Columns.Add(columnName:=NameOf(SG.RecordNumber), type:=GetType(Integer))
                        table.Columns.Add(columnName:=NameOf(SG.SensorState), type:=GetType(String))
                        table.Columns.Add(columnName:=NameOf(SG.Message), type:=GetType(String))
                        For index As Integer = 1 To rowCount
                            table.Rows.Add(index, "NO_ERROR_MESSAGE", "---")
                        Next

                        For refresh As Integer = 1 To 2
                            DisplayDataTable(Of SG)(realPanel:=Nothing, table, dgv, rowIndex:=ServerDataEnum.sgs)
                            Application.DoEvents()

                            Dim state As DataGridViewColumn = dgv.Columns(NameOf(SG.SensorState))
                            Dim message As DataGridViewColumn = dgv.Columns(NameOf(SG.Message))
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnsMode.None, actual:=dgv.AutoSizeColumnsMode)
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.AllCells, actual:=state.AutoSizeMode)
                            Assert.Equal(expected:=DataGridViewTriState.False, actual:=state.DefaultCellStyle.WrapMode)
                            Assert.Equal(expected:=DataGridViewTriState.False,
                                         actual:=dgv.Rows(0).Cells(state.Index).InheritedStyle.WrapMode)
                            Assert.Equal(expected:="No Error Message", actual:=dgv.Rows(0).Cells(state.Index).FormattedValue)
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.Fill, actual:=message.AutoSizeMode)
                            Assert.Equal(expected:=DataGridViewTriState.True, actual:=message.DefaultCellStyle.WrapMode)
                            Assert.True(condition:=state.Width >= state.GetPreferredWidth(
                                DataGridViewAutoSizeColumnMode.AllCells, fixedHeight:=True))
                            Assert.True(condition:=message.Width > state.Width)

                            Dim oldMessageWidth As Integer = message.Width
                            host.ClientSize = New Drawing.Size(width:=host.ClientSize.Width + 200, height:=400)
                            Application.DoEvents()
                            Assert.True(condition:=message.Width > oldMessageWidth)

                            table.DefaultView.Sort = $"{NameOf(SG.RecordNumber)} ASC"
                            Application.DoEvents()
                            Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.AllCells, actual:=state.AutoSizeMode)
                            Assert.Equal(expected:=DataGridViewTriState.False,
                                         actual:=dgv.Rows(0).Cells(state.Index).InheritedStyle.WrapMode)
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
    Public Sub ConfigureContentColumnSizing_UsesLastVisibleColumnAndPreservesBasalGrid()
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using host As New Form(), dgv As New DataGridView(), basal As New DataGridView()
                        host.ClientSize = New Drawing.Size(width:=600, height:=300)
                        dgv.Dock = DockStyle.Fill
                        host.Controls.Add(dgv)
                        dgv.Columns.Add(columnName:="First", headerText:="First")
                        dgv.Columns.Add(columnName:="LastVisible", headerText:="A long final column header")
                        dgv.Columns.Add(columnName:="Hidden", headerText:="Hidden")
                        dgv.Columns("Hidden").Visible = False
                        dgv.Rows.Add("Value", "Message", "Hidden value")
                        host.Show()

                        dgv.ConfigureContentColumnSizing()
                        Dim last As DataGridViewColumn = dgv.Columns("LastVisible")
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.AllCells,
                                     actual:=dgv.Columns("First").AutoSizeMode)
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.Fill, actual:=last.AutoSizeMode)
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.AllCells,
                                     actual:=dgv.Columns("Hidden").AutoSizeMode)
                        Assert.True(condition:=last.Width >= last.GetPreferredWidth(
                            DataGridViewAutoSizeColumnMode.ColumnHeader, fixedHeight:=True))

                        last.DataPropertyName = NameOf(SG.SensorState)
                        dgv.Rows(0).Cells(last.Index).Value = "No Error Message"
                        dgv.ConfigureContentColumnSizing()
                        Assert.Equal(expected:=DataGridViewTriState.False,
                                     actual:=dgv.Rows(0).Cells(last.Index).InheritedStyle.WrapMode)
                        Assert.True(condition:=last.Width >= last.GetPreferredWidth(
                            DataGridViewAutoSizeColumnMode.AllCells, fixedHeight:=True))

                        basal.Name = NameOf(Form1.DgvBasalPerHour)
                        basal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                        basal.Columns.Add(columnName:="Basal", headerText:="Basal")
                        basal.ConfigureContentColumnSizing()
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnsMode.Fill, actual:=basal.AutoSizeColumnsMode)
                        Assert.Equal(expected:=DataGridViewAutoSizeColumnMode.NotSet,
                                     actual:=basal.Columns("Basal").AutoSizeMode)
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
