' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
Public Class ControlExtensionsTests

    Private Const BindingAttr As BindingFlags =
        BindingFlags.Instance Or BindingFlags.NonPublic

    Private Shared Function ContrastRatio(first As Color, second As Color) As Double
        Dim firstLuminance As Double = RelativeLuminance(color:=first)
        Dim secondLuminance As Double = RelativeLuminance(color:=second)
        Return (Math.Max(firstLuminance, secondLuminance) + 0.05) /
               (Math.Min(firstLuminance, secondLuminance) + 0.05)
    End Function

    Private Shared Function LinearChannel(channel As Byte) As Double
        Dim value As Double = channel / 255.0
        Return If(value <= 0.04045,
                  value / 12.92,
                  Math.Pow(x:=(value + 0.055) / 1.055, y:=2.4))
    End Function

    Private Shared Function RelativeLuminance(color As Color) As Double
        Return (0.2126 * LinearChannel(channel:=color.R)) + (0.7152 * LinearChannel(channel:=color.G)) +
               (0.0722 * LinearChannel(channel:=color.B))
    End Function

    <Fact>
    Public Sub ApplyCellFontIncrease_UsesGridFontAndDoesNotAccumulate()
        Using dgv As New DataGridView(), basal As New DataGridView()
            basal.Name = NameOf(Form1.DgvBasalPerHour)
            Dim baseline As Single = dgv.Font.SizeInPoints
            Dim basalFont As Font = basal.DefaultCellStyle.Font
            Dim basalColumnFont As Font = basal.ColumnHeadersDefaultCellStyle.Font
            Dim basalRowFont As Font = basal.RowHeadersDefaultCellStyle.Font
            dgv.DefaultCellStyle.Font = dgv.Font
            dgv.ColumnHeadersDefaultCellStyle.Font = dgv.Font

            dgv.InitializeDgv()
            dgv.DefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            dgv.ColumnHeadersDefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            dgv.RowHeadersDefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            dgv.Columns.Add(columnName:="Value", headerText:="Value")
            dgv.Rows.Add("value")
            dgv.Columns(index:=0).HeaderCell.InheritedStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            dgv.Rows(index:=0).HeaderCell.InheritedStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            Dim style As DataGridViewCellStyle = dgv.Rows(index:=0).Cells(index:=0).InheritedStyle.Clone()
            style.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            Dim args As New DataGridViewCellFormattingEventArgs(0, 0, "value", GetType(String), style)
            args.CellFormattingApplyBold()
            Using boldFont As Font = args.CellStyle.Font
                boldFont.SizeInPoints.Should().Be(expected:=baseline + 3.0F)
                boldFont.Bold.Should().BeTrue()
            End Using

            dgv.InitializeDgv()
            dgv.DefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            dgv.ColumnHeadersDefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            dgv.RowHeadersDefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 1.0F)
            dgv.ApplyCellFontIncrease(increasePoints:=3.0F)
            dgv.DefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 3.0F)
            dgv.Columns(index:=0).HeaderCell.InheritedStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 3.0F)
            dgv.Rows(index:=0).HeaderCell.InheritedStyle.Font.SizeInPoints.Should().Be(expected:=baseline + 3.0F)
            dgv.RowTemplate.Height.Should().BeGreaterThanOrEqualTo(expected:=
                CInt(Math.Ceiling(a:=dgv.DefaultCellStyle.Font.GetHeight(dpi:=dgv.DeviceDpi))))
            dgv.ApplyCellFontIncrease(increasePoints:=0.0F)
            dgv.DefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline)
            dgv.ColumnHeadersDefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline)
            dgv.RowHeadersDefaultCellStyle.Font.SizeInPoints.Should().Be(expected:=baseline)

            basal.InitializeDgv()
            basal.DefaultCellStyle.Font.Should().BeSameAs(expected:=basalFont)
            basal.ColumnHeadersDefaultCellStyle.Font.Should().BeSameAs(expected:=basalColumnFont)
            basal.RowHeadersDefaultCellStyle.Font.Should().BeSameAs(expected:=basalRowFont)
        End Using
    End Sub

    <Theory>
    <InlineData(0, True)>
    <InlineData(1, True)>
    <InlineData(0, False)>
    <InlineData(1, False)>
    Public Sub CellFormattingApplyColor_PreservesLegendColorWithReadableContrast(rowIndex As Integer,
                                                                                 isDark As Boolean)
        Using dgv As New DataGridView()
            dgv.Columns.Add(columnName:="Value", headerText:="Value")
            dgv.AllowUserToAddRows = False
            dgv.Rows.Add(count:=2)
            dgv.RowsDefaultCellStyle.BackColor =
                    If(isDark,
                       Color.FromArgb(red:=48, green:=48, blue:=48),
                       Color.White)
            dgv.SetAlternatingRowColor(isDarkMode:=isDark, highContrast:=False)

            For Each textColor As Color In {Color.Red, Color.Yellow}
                Dim style As DataGridViewCellStyle = dgv.Rows(index:=rowIndex).Cells(index:=0).InheritedStyle.Clone()
                Dim args As New DataGridViewCellFormattingEventArgs(columnIndex:=0,
                                                                    rowIndex,
                                                                    value:="100",
                                                                    desiredType:=GetType(String),
                                                                    cellStyle:=style)
                dgv.CellFormattingApplyColor(e:=args, textColor)

                args.FormattingApplied.Should().BeTrue()
                style.BackColor.Should().
                    Be(expected:=If(rowIndex Mod 2 = 0,
                                    dgv.RowsDefaultCellStyle.BackColor,
                                    dgv.AlternatingRowsDefaultCellStyle.BackColor))
                ContrastRatio(first:=style.ForeColor, second:=style.BackColor).Should().
                    BeGreaterThanOrEqualTo(expected:=4.5)
                ContrastRatio(first:=style.SelectionForeColor,
                              second:=style.SelectionBackColor).Should().BeGreaterThanOrEqualTo(expected:=4.5)
                dgv.RowsDefaultCellStyle.BackColor.Should().
                    Be(expected:=If(isDark,
                                    Color.FromArgb(red:=48, green:=48, blue:=48),
                                    Color.White))
            Next
        End Using
    End Sub

    <Fact>
    Public Sub CenterLabelXOnParent_CentersLabel_WithAutoSizeFalse()
        Using parent As New Panel()
            parent.Width = 200
            Dim lbl As New Label() With {.Width = 50, .AutoSize = False}
            parent.Controls.Add(value:=lbl)

            ' Act
            lbl.CenterLabelOnParent()

            ' Assert
            lbl.Left.Should().Be(expected:=(parent.Width - lbl.Width) \ 2)
        End Using
    End Sub

    <Fact>
    Public Sub CenterLabelXOnParent_NullOrDisposed_DoesNotThrow()
        ' Null
        Dim subCallNull As Action =
            Sub()
                CenterLabelOnParent(lbl:=Nothing)
            End Sub
        subCallNull.Should().NotThrow()

        ' Disposed
        Dim lbl As New Label()
        lbl.Dispose()
        Dim subCallDisposed As Action =
            Sub()
                CenterLabelOnParent(lbl)
            End Sub
        subCallDisposed.Should().NotThrow()
    End Sub

    <Fact>
    Public Sub CenterXOnParent_CentersControl_CenterAndHalves()
        Using parent As New Panel()
            parent.Width = 200
            Dim c As New Control() With {.Width = 40}
            parent.Controls.Add(value:=c)

            ' Center in middle
            c.CenterXOnParent()
            c.Left.Should().Be(expected:=(parent.Width - c.Width) \ 2)

            ' Center on left half
            c.CenterXOnParent(onLeftHalf:=True)
            Dim expectedLeftLeftHalf As Integer = Math.Max(0, ((parent.Width \ 2) - c.Width) \ 2)
            c.Left.Should().Be(expected:=expectedLeftLeftHalf)

            ' Center on right half
            c.CenterXOnParent(onLeftHalf:=False)
            Dim halfWidth As Integer = parent.Width \ 2
            Dim expectedRight As Integer = Math.Max(0, halfWidth + ((halfWidth - c.Width) \ 2))
            c.Left.Should().Be(expected:=expectedRight)
        End Using
    End Sub

    <Fact>
    Public Sub CenterXOnParent_LabelAutoSize_UsesPreferredWidthWhenAvailable()
        Using parent As New Panel()
            parent.Width = 300
            Dim lbl As New Label() With {
                .AutoSize = True,
                .Text = New String(c:="X"c, count:=40)}
            parent.Controls.Add(value:=lbl)

            ' Act
            lbl.CenterXOnParent()

            ' Assert uses PreferredWidth
            Dim ctrlWidth As Integer = lbl.PreferredWidth
            lbl.Left.Should().Be(expected:=(parent.Width - ctrlWidth) \ 2)
        End Using
    End Sub

    <Fact>
    Public Sub CenterXOnParent_NoParentOrDisposed_DoesNotThrowAndLeavesLeftUnchanged()
        ' No parent
        Dim ctrl As New Control With {
            .Width = 30,
            .Left = 5}

        Dim actNoParent As Action =
            Sub()
                CenterXOnParent(ctrl)
            End Sub
        actNoParent.Should().NotThrow()
        ctrl.Left.Should().Be(expected:=5)

        ' Disposed
        Dim d As New Control() With {.Width = 20}
        d.Dispose()
        d.Left = 7
        Dim actDisposed As Action =
            Sub()
                CenterXOnParent(ctrl:=d)
            End Sub
        actDisposed.Should().NotThrow()
        d.Left.Should().Be(expected:=7)
    End Sub

    <Fact>
    Public Sub CenterXYOnParent_CentersLabel_WithVerticalOffset()
        Using parent As New Panel()
            parent.Width = 300
            parent.Height = 120
            Dim lbl As New Label() With {
                .AutoSize = True,
                .Text = "Hello"}
            parent.Controls.Add(value:=lbl)

            ' Act
            lbl.CenterXYOnParent(verticalOffset:=5)

            ' Assert horizontal centered
            Dim ctrlWidth As Integer =
                If(lbl.AutoSize,
                   lbl.PreferredWidth,
                   lbl.Width)
            lbl.Left.Should().Be(expected:=(parent.Width - ctrlWidth) \ 2)

            ' Assert vertical centered with offset
            Dim ctrlHeight As Integer =
                If(lbl.AutoSize,
                   lbl.PreferredHeight,
                   lbl.Height)
            Dim totalHeight As Integer =
                ctrlHeight + lbl.Margin.Top + lbl.Margin.Bottom
            lbl.Top.Should().Be(expected:=((parent.Height - totalHeight) \ 2) + 5)
        End Using
    End Sub

    <Fact>
    Public Sub ControlPositionsForm_UsesGridThemeAndHidesRowHeaders()
        Using form As New ControlPositionsForm(), expected As New DataGridView()
            Dim dgv As DataGridView = form.DgvControlPositions
            expected.InitializeGridTheme()
            dgv.RowHeadersVisible.Should().BeFalse()
            dgv.SelectionMode.Should().Be(expected:=DataGridViewSelectionMode.FullRowSelect)
            dgv.AlternatingRowsDefaultCellStyle.BackColor.Should().
                Be(expected:=expected.AlternatingRowsDefaultCellStyle.BackColor)
            dgv.ColumnHeadersDefaultCellStyle.BackColor.Should().
                Be(expected:=expected.ColumnHeadersDefaultCellStyle.BackColor)
            dgv.ColumnHeadersDefaultCellStyle.ForeColor.Should().
                Be(expected:=expected.ColumnHeadersDefaultCellStyle.ForeColor)
            dgv.EnableHeadersVisualStyles.Should().Be(expected:=expected.EnableHeadersVisualStyles)

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Magenta
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Magenta

            Dim onSystemColorsChanged As MethodInfo =
                GetType(Control).GetMethod(name:="OnSystemColorsChanged", BindingAttr)
            onSystemColorsChanged.Invoke(obj:=dgv, parameters:={EventArgs.Empty})
            dgv.AlternatingRowsDefaultCellStyle.BackColor.Should().
                Be(expected:=expected.AlternatingRowsDefaultCellStyle.BackColor)
            dgv.ColumnHeadersDefaultCellStyle.BackColor.Should().
                Be(expected:=expected.ColumnHeadersDefaultCellStyle.BackColor)
        End Using
    End Sub

    <Fact>
    Public Sub FindControlByName_DisposedChild_ReturnsNothing()
        Using parent As New Panel()
            Dim btn As New Button() With {.Name = "btn"}
            parent.Controls.Add(value:=btn)
            ' Dispose child to force early return
            btn.Dispose()

            Dim result As Control =
                parent.Controls.FindControlByName(controlName:="btn")
            result.Should().BeNull()
        End Using
    End Sub

    <Fact>
    Public Sub FindControlByName_FindsExistingControl_ReturnsControl()
        Using parent As New Panel()
            Dim btn As New Button() With {.Name = "myButton"}
            parent.Controls.Add(value:=btn)

            Dim result As Control =
                parent.Controls.FindControlByName(controlName:="myButton")
            result.Should().NotBeNull()
            result.Should().BeSameAs(expected:=btn)
        End Using
    End Sub

    <Fact>
    Public Sub FindControlByName_NullControls_ReturnsNothing()
        Dim result As Control =
            FindControlByName(controls:=Nothing, controlName:="anything")
        result.Should().BeNull()
    End Sub

    <Fact>
    Public Sub FindHorizontalAndVerticalMidpoint_ReturnsCorrectValues_AndHandlesNull()
        Dim c As New Control() With {
            .Left = 10,
            .Top = 20,
            .Width = 30,
            .Height = 40}
        c.FindHorizontalMidpoint().Should().Be(expected:=10 + (30 \ 2))
        c.FindVerticalMidpoint().Should().Be(expected:=20 + (40 \ 2))

        Dim nullCtrl As Control = Nothing
        nullCtrl.FindHorizontalMidpoint().Should().Be(expected:=0)
        nullCtrl.FindVerticalMidpoint().Should().Be(expected:=0)

        Dim disposed As New Control()
        disposed.Dispose()
        disposed.FindHorizontalMidpoint().Should().Be(expected:=0)
        disposed.FindVerticalMidpoint().Should().Be(expected:=0)
    End Sub

    <Fact>
    Public Sub InitializeDgv_PreservesNativeColorsAndConfiguresLayout()
        Using dgv As New DataGridView(), defaults As New DataGridView()
            dgv.InitializeDgv(dock:=DockStyle.Top)

            dgv.RowsDefaultCellStyle.BackColor.Should().Be(expected:=defaults.RowsDefaultCellStyle.BackColor)
            dgv.RowsDefaultCellStyle.ForeColor.Should().Be(expected:=defaults.RowsDefaultCellStyle.ForeColor)
            dgv.RowsDefaultCellStyle.SelectionBackColor.Should().
                Be(expected:=defaults.RowsDefaultCellStyle.SelectionBackColor)
            dgv.RowsDefaultCellStyle.SelectionForeColor.Should().
                Be(expected:=defaults.RowsDefaultCellStyle.SelectionForeColor)
            Dim expectedStripe As Color =
                If(SystemInformation.HighContrast,
                   Color.Empty,
                   If(Application.IsDarkModeEnabled,
                      Color.FromArgb(red:=72, green:=72, blue:=72),
                      Color.FromArgb(red:=230, green:=230, blue:=230)))
            dgv.AlternatingRowsDefaultCellStyle.BackColor.Should().Be(expected:=expectedStripe)
            dgv.AlternatingRowsDefaultCellStyle.ForeColor.Should().
                Be(expected:=defaults.AlternatingRowsDefaultCellStyle.ForeColor)
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor.Should().
                Be(expected:=defaults.AlternatingRowsDefaultCellStyle.SelectionBackColor)
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor.Should().
                Be(expected:=defaults.AlternatingRowsDefaultCellStyle.SelectionForeColor)
            Dim useDarkHeaders As Boolean = Application.IsDarkModeEnabled AndAlso Not SystemInformation.HighContrast
            dgv.EnableHeadersVisualStyles.Should().Be(expected:=Not useDarkHeaders)
            If useDarkHeaders Then
                dgv.ColumnHeadersDefaultCellStyle.BackColor.Should().
                    Be(expected:=Color.FromArgb(red:=38, green:=47, blue:=58))
                dgv.ColumnHeadersDefaultCellStyle.ForeColor.Should().Be(expected:=Color.White)
            End If
            dgv.ColumnHeadersDefaultCellStyle.Alignment.Should().Be(expected:=DataGridViewContentAlignment.MiddleCenter)
            dgv.ColumnHeadersDefaultCellStyle.WrapMode.Should().Be(expected:=DataGridViewTriState.True)
            dgv.Dock.Should().Be(expected:=DockStyle.Top)
            dgv.ReadOnly.Should().BeTrue()
            dgv.AllowUserToAddRows.Should().BeFalse()
            dgv.AllowUserToDeleteRows.Should().BeFalse()
        End Using
    End Sub

    <Fact>
    Public Sub IsDarkRow_UsesInheritedBackgroundInsteadOfRowParity()
        Using dgv As New DataGridView()
            dgv.Columns.Add(columnName:="Value", headerText:="Value")
            dgv.RowsDefaultCellStyle.BackColor = Color.Black
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.White
            dgv.Rows.Add()
            dgv.Rows.Add()

            dgv.Rows(index:=0).IsDarkRow().Should().BeTrue()
            dgv.Rows(index:=1).IsDarkRow().Should().BeFalse()

            dgv.RowsDefaultCellStyle.BackColor = Color.White
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Black
            dgv.Rows(index:=0).IsDarkRow().Should().BeFalse()
            dgv.Rows(index:=1).IsDarkRow().Should().BeTrue()
        End Using
    End Sub

    <Theory>
    <InlineData(True)>
    <InlineData(False)>
    Public Sub PaintDarkUserButton_UsesDarkFillForEnabledAndDisabled(enabled As Boolean)
        Using bitmap As New Bitmap(width:=100, height:=30),
              graphics As Graphics = Graphics.FromImage(bitmap),
              font As New Font(familyName:="Segoe UI", emSize:=9.0F)
            graphics.Clear(color:=Color.Black)
            PaintDarkUserButton(graphics,
                                cellBounds:=New Rectangle(x:=0, y:=0, width:=100, height:=30),
                                font,
                                text:="Delete Row", enabled, dpi:=96)
            bitmap.GetPixel(x:=4, y:=4).ToArgb().Should().
                Be(expected:=Color.FromArgb(red:=56, green:=56, blue:=56).ToArgb())
            Dim expectedBorder As Integer = If(enabled,
                                               170,
                                               105)
            bitmap.GetPixel(x:=2, y:=2).ToArgb().Should().
                Be(expected:=Color.FromArgb(red:=expectedBorder,
                                            green:=expectedBorder,
                                            blue:=expectedBorder).ToArgb())
        End Using
    End Sub

    <Theory>
    <InlineData(CheckState.Unchecked, 56, 56, 56)>
    <InlineData(CheckState.Checked, 0, 100, 180)>
    <InlineData(CheckState.Indeterminate, 0, 100, 180)>
    Public Sub PaintDarkUserCheckBox_UsesThemeColorsAndState(state As CheckState, red As Integer,
                                                            green As Integer, blue As Integer)
        Using bitmap As New Bitmap(width:=40, height:=40), graphics As Graphics = Graphics.FromImage(bitmap)
            graphics.Clear(color:=Color.Black)
            PaintDarkUserCheckBox(graphics,
                                  cellBounds:=New Rectangle(x:=0, y:=0, width:=40, height:=40),
                                  checkState:=state,
                                  dpi:=96)
            bitmap.GetPixel(x:=14, y:=14).ToArgb().Should().
                Be(expected:=Color.FromArgb(red, green, blue).ToArgb())
            bitmap.GetPixel(x:=13, y:=13).ToArgb().Should().
                Be(expected:=Color.FromArgb(red:=180, green:=180, blue:=180).ToArgb())
            Dim hasWhiteMark As Boolean = False
            For x As Integer = 14 To 25
                For y As Integer = 14 To 25
                    hasWhiteMark = hasWhiteMark OrElse bitmap.GetPixel(x, y).ToArgb() = Color.White.ToArgb()
                Next
            Next
            hasWhiteMark.Should().Be(expected:=state <> CheckState.Unchecked)
        End Using
    End Sub

    <Theory>
    <InlineData(False)>
    <InlineData(True)>
    Public Sub PaintDarkUserGridCell_ButtonPaintDoesNotTouchAdjacentCell(enabled As Boolean)
        Using dgv As New DataGridView(), bitmap As New Bitmap(width:=240, height:=40),
              graphics As Graphics = Graphics.FromImage(bitmap)
            graphics.Clear(color:=Color.Magenta)
            dgv.AllowUserToAddRows = False
            Dim column As New DataGridViewColumnControls.DataGridViewDisableButtonColumn With {
                .CellTemplate = New CountingButtonCell()}
            dgv.Columns.Add(dataGridViewColumn:=column)
            dgv.Columns.Add(columnName:="Next", headerText:="Next")
            dgv.Rows.Add()
            Dim cell As CountingButtonCell = DirectCast(dgv.Rows(index:=0).Cells(index:=0), CountingButtonCell)
            cell.Enabled = enabled
            Dim bounds As New Rectangle(x:=20, y:=5, width:=100, height:=30)
            Dim clip As New Rectangle(x:=40, y:=5, width:=40, height:=30)
            Dim args As New DataGridViewCellPaintingEventArgs(dataGridView:=dgv,
                                                              graphics:=graphics,
                                                              clipBounds:=clip,
                                                              cellBounds:=bounds,
                                                              rowIndex:=0,
                                                              columnIndex:=0,
                                                              cellState:=DataGridViewElementStates.None,
                                                              value:="Delete Row",
                                                              formattedValue:="Delete Row",
                                                              errorText:=Nothing,
                                                              cellStyle:=cell.InheritedStyle,
                                                              advancedBorderStyle:=New DataGridViewAdvancedBorderStyle(),
                                                              paintParts:=DataGridViewPaintParts.All)
            dgv.PaintDarkUserGridCell(args, isDarkMode:=True, highContrast:=False)
            Assert.Equal(expected:=0,
                         actual:=cell.PaintCalls)
            Assert.NotEqual(expected:=Color.Magenta.ToArgb(),
                            actual:=bitmap.GetPixel(x:=45, y:=10).ToArgb())
            For x As Integer = 0 To bitmap.Width - 1
                For y As Integer = 0 To bitmap.Height - 1
                    If Not clip.Contains(x, y) Then
                        Assert.Equal(expected:=Color.Magenta.ToArgb(),
                                     actual:=bitmap.GetPixel(x, y).ToArgb())
                    End If
                Next
            Next
        End Using
    End Sub

    <Theory>
    <InlineData(False)>
    <InlineData(True)>
    Public Sub PaintDarkUserGridCell_DrawsButtonForEnabledAndDisabledCells(enabled As Boolean)
        Using dgv As New DataGridView(), bitmap As New Bitmap(width:=100, height:=30),
              graphics As Graphics = Graphics.FromImage(bitmap)
            dgv.AllowUserToAddRows = False
            dgv.Columns.Add(dataGridViewColumn:=New DataGridViewColumnControls.DataGridViewDisableButtonColumn())
            dgv.Rows.Add()
            Dim cell As DataGridViewColumnControls.DataGridViewDisableButtonCell =
                DirectCast(dgv.Rows(index:=0).Cells(index:=0), DataGridViewColumnControls.DataGridViewDisableButtonCell)
            cell.Enabled = enabled
            Dim bounds As New Rectangle(x:=0, y:=0, width:=100, height:=30)
            Dim args As New DataGridViewCellPaintingEventArgs(dataGridView:=dgv,
                                                              graphics:=graphics,
                                                              clipBounds:=bounds,
                                                              cellBounds:=bounds,
                                                              rowIndex:=0,
                                                              columnIndex:=0,
                                                              cellState:=DataGridViewElementStates.None,
                                                              value:="Delete Row",
                                                              formattedValue:="Delete Row",
                                                              errorText:=Nothing,
                cellStyle:=cell.InheritedStyle, advancedBorderStyle:=New DataGridViewAdvancedBorderStyle(),
                paintParts:=DataGridViewPaintParts.All)
            dgv.PaintDarkUserGridCell(e:=args,
                                      isDarkMode:=True,
                                      highContrast:=False)
            args.Handled.Should().BeTrue()
            bitmap.GetPixel(x:=4, y:=4).ToArgb().Should().
                Be(expected:=Color.FromArgb(red:=56, green:=56, blue:=56).ToArgb())
            Dim expectedBorder As Integer = If(enabled, 170, 105)
            bitmap.GetPixel(x:=2, y:=2).ToArgb().Should().
                Be(expected:=Color.FromArgb(red:=expectedBorder,
                                            green:=expectedBorder,
                                            blue:=expectedBorder).ToArgb())
        End Using
    End Sub

    <Theory>
    <InlineData(False)>
    <InlineData(True)>
    Public Sub PaintDarkUserGridCell_DrawsFormattedCheckBoxValue(value As Boolean)
        Using dgv As New DataGridView(),
              bitmap As New Bitmap(width:=40, height:=40),
              graphics As Graphics = Graphics.FromImage(bitmap)

            dgv.AllowUserToAddRows = False
            dgv.Columns.Add(dataGridViewColumn:=New DataGridViewCheckBoxColumn())
            dgv.Rows.Add()
            Dim bounds As New Rectangle(x:=0, y:=0, width:=40, height:=40)
            Dim args As New DataGridViewCellPaintingEventArgs(dataGridView:=dgv,
                                                              graphics:=graphics,
                                                              clipBounds:=bounds,
                                                              cellBounds:=bounds,
                                                              rowIndex:=0,
                                                              columnIndex:=0,
                                                              cellState:=DataGridViewElementStates.None,
                                                              value:=value,
                                                              formattedValue:=value,
                                                              errorText:=Nothing,
                                                              cellStyle:=dgv.Rows(index:=0).Cells(index:=0).InheritedStyle,
                                                              advancedBorderStyle:=New DataGridViewAdvancedBorderStyle(),
                                                              paintParts:=DataGridViewPaintParts.ContentForeground)
            dgv.PaintDarkUserGridCell(e:=args, isDarkMode:=True, highContrast:=False)
            args.Handled.Should().BeTrue()
            Dim expected As Color = If(value,
                                       Color.FromArgb(red:=0, green:=100, blue:=180),
                                       Color.FromArgb(red:=56, green:=56, blue:=56))
            bitmap.GetPixel(x:=14, y:=14).ToArgb().Should().Be(expected:=expected.ToArgb())
        End Using
    End Sub

    <Theory>
    <InlineData(False, False)>
    <InlineData(True, True)>
    Public Sub PaintDarkUserGridCell_LeavesNativeRenderingForLightAndHighContrast(isDarkMode As Boolean,
                                                                                 highContrast As Boolean)
        Using dgv As New DataGridView(), bitmap As New Bitmap(width:=40, height:=40),
              graphics As Graphics = Graphics.FromImage(bitmap)
            dgv.AllowUserToAddRows = False
            dgv.Columns.Add(dataGridViewColumn:=New DataGridViewCheckBoxColumn())
            dgv.Rows.Add()
            Dim bounds As New Rectangle(x:=0, y:=0, width:=40, height:=40)
            Dim args As New DataGridViewCellPaintingEventArgs(dataGridView:=dgv,
                                                              graphics:=graphics,
                                                              clipBounds:=bounds,
                                                              cellBounds:=bounds,
                                                              rowIndex:=0,
                                                              columnIndex:=0,
                                                              cellState:=DataGridViewElementStates.None,
                                                              value:=False,
                                                              formattedValue:=False,
                                                              errorText:=Nothing,
                                                              cellStyle:=dgv.DefaultCellStyle,
                                                              advancedBorderStyle:=New DataGridViewAdvancedBorderStyle(),
                                                              paintParts:=DataGridViewPaintParts.All)
            dgv.PaintDarkUserGridCell(e:=args,
                                      isDarkMode,
                                      highContrast)
            args.Handled.Should().BeFalse()
        End Using
    End Sub

    <Fact>
    Public Sub SetAlternatingRowColor_HighContrastRestoresInheritedColor()
        Using dgv As New DataGridView()
            dgv.SetAlternatingRowColor(isDarkMode:=True, highContrast:=False)
            dgv.SetAlternatingRowColor(isDarkMode:=False, highContrast:=True)
            dgv.AlternatingRowsDefaultCellStyle.BackColor.Should().Be(expected:=Color.Empty)
        End Using
    End Sub

    <Theory>
    <InlineData(True, False, 72)>
    <InlineData(False, False, 230)>
    Public Sub SetAlternatingRowColor_UsesThemeStripe(isDarkMode As Boolean,
                                                      highContrast As Boolean,
                                                      shade As Integer)
        Using dgv As New DataGridView()
            dgv.SetAlternatingRowColor(isDarkMode, highContrast)
            dgv.AlternatingRowsDefaultCellStyle.BackColor.Should().
                Be(expected:=Color.FromArgb(red:=shade, green:=shade, blue:=shade))
        End Using
    End Sub

    <Fact>
    Public Sub SetHeaderColors_DarkHeadersHaveReadableContrast()
        Using dgv As New DataGridView()
            dgv.SetHeaderColors(isDarkMode:=True, highContrast:=False)
            dgv.EnableHeadersVisualStyles.Should().BeFalse()
            For Each style As DataGridViewCellStyle In {dgv.ColumnHeadersDefaultCellStyle, dgv.RowHeadersDefaultCellStyle}
                ContrastRatio(first:=style.ForeColor, second:=style.BackColor).Should().BeGreaterThanOrEqualTo(expected:=4.5)
                ContrastRatio(first:=style.SelectionForeColor, second:=style.SelectionBackColor).Should().
                    BeGreaterThanOrEqualTo(expected:=4.5)
            Next
        End Using
    End Sub

    <Theory>
    <InlineData(False, False)>
    <InlineData(True, True)>
    Public Sub SetHeaderColors_RestoresNativeStyles(isDarkMode As Boolean, highContrast As Boolean)
        Using dgv As New DataGridView()
            dgv.SetHeaderColors(isDarkMode:=True, highContrast:=False)
            dgv.SetHeaderColors(isDarkMode, highContrast)
            dgv.EnableHeadersVisualStyles.Should().BeTrue()
            dgv.ColumnHeadersDefaultCellStyle.BackColor.Should().Be(expected:=Color.Empty)
            dgv.ColumnHeadersDefaultCellStyle.ForeColor.Should().Be(expected:=Color.Empty)
            dgv.RowHeadersDefaultCellStyle.BackColor.Should().Be(expected:=Color.Empty)
            dgv.RowHeadersDefaultCellStyle.ForeColor.Should().Be(expected:=Color.Empty)
        End Using
    End Sub

    Private Class CountingButtonCell
        Inherits DataGridViewColumnControls.DataGridViewDisableButtonCell

        Public Property PaintCalls As Integer

        Protected Overrides Sub Paint(graphics As Graphics,
                                      clipBounds As Rectangle,
                                      cellBounds As Rectangle,
                                      rowIndex As Integer,
                                      elementState As DataGridViewElementStates,
                                      value As Object,
                                      formattedValue As Object,
                                      errorText As String,
                                      cellStyle As DataGridViewCellStyle,
                                      advancedBorderStyle As DataGridViewAdvancedBorderStyle,
                                      paintParts As DataGridViewPaintParts)
            Me.PaintCalls += 1
            MyBase.Paint(graphics,
                         clipBounds,
                         cellBounds,
                         rowIndex,
                         elementState,
                         value,
                         formattedValue,
                         errorText,
                         cellStyle,
                         advancedBorderStyle,
                         paintParts)
        End Sub

    End Class
End Class
