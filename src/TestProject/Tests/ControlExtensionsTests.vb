' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
Public Class ControlExtensionsTests

    Private Shared Function ContrastRatio(first As Drawing.Color, second As Drawing.Color) As Double
        Dim firstLuminance As Double = RelativeLuminance(first)
        Dim secondLuminance As Double = RelativeLuminance(second)
        Return (Math.Max(firstLuminance, secondLuminance) + 0.05) /
               (Math.Min(firstLuminance, secondLuminance) + 0.05)
    End Function

    Private Shared Function LinearChannel(channel As Byte) As Double
        Dim value As Double = channel / 255.0
        Return If(value <= 0.04045, value / 12.92, Math.Pow((value + 0.055) / 1.055, 2.4))
    End Function

    Private Shared Function RelativeLuminance(color As Drawing.Color) As Double
        Return (0.2126 * LinearChannel(color.R)) + (0.7152 * LinearChannel(color.G)) +
               (0.0722 * LinearChannel(color.B))
    End Function

    <Theory>
    <InlineData(0, True)>
    <InlineData(1, True)>
    <InlineData(0, False)>
    <InlineData(1, False)>
    Public Sub CellFormattingApplyColor_PreservesLegendColorWithReadableContrast(rowIndex As Integer, isDark As Boolean)
        Using dgv As New DataGridView()
            dgv.Columns.Add(columnName:="Value", headerText:="Value")
            dgv.AllowUserToAddRows = False
            dgv.Rows.Add(count:=2)
            dgv.RowsDefaultCellStyle.BackColor = If(isDark, Drawing.Color.FromArgb(48, 48, 48), Drawing.Color.White)
            dgv.SetAlternatingRowColor(isDark, highContrast:=False)

            For Each textColor As Drawing.Color In {Drawing.Color.Red, Drawing.Color.Yellow}
                Dim style As DataGridViewCellStyle = dgv.Rows(index:=rowIndex).Cells(index:=0).InheritedStyle.Clone()
                Dim args As New DataGridViewCellFormattingEventArgs(0, rowIndex, "100", GetType(String), style)
                dgv.CellFormattingApplyColor(args, textColor)

                args.FormattingApplied.Should().BeTrue()
                style.ForeColor.Should().Be(expected:=textColor)
                ContrastRatio(style.ForeColor, style.BackColor).Should().BeGreaterThanOrEqualTo(expected:=4.5)
                ContrastRatio(style.SelectionForeColor, style.SelectionBackColor).Should().
                    BeGreaterThanOrEqualTo(expected:=4.5)
                style.SelectionBackColor.Should().NotBe(unexpected:=style.BackColor)
                If textColor = Drawing.Color.Yellow Then
                    style.SelectionForeColor.Should().Be(expected:=textColor)
                Else
                    style.SelectionForeColor.R.Should().Be(expected:=CByte(255))
                    style.SelectionForeColor.G.Should().BeLessThan(expected:=CByte(100))
                    style.SelectionForeColor.B.Should().BeLessThan(expected:=CByte(100))
                End If
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
            Dim expectedStripe As Drawing.Color =
                If(SystemInformation.HighContrast,
                   Drawing.Color.Empty,
                   If(Application.IsDarkModeEnabled,
                      Drawing.Color.FromArgb(red:=72, green:=72, blue:=72),
                      Drawing.Color.FromArgb(red:=230, green:=230, blue:=230)))
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
                    Be(expected:=Drawing.Color.FromArgb(red:=38, green:=47, blue:=58))
                dgv.ColumnHeadersDefaultCellStyle.ForeColor.Should().Be(expected:=Drawing.Color.White)
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
            dgv.RowsDefaultCellStyle.BackColor = Drawing.Color.Black
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Drawing.Color.White
            dgv.Rows.Add()
            dgv.Rows.Add()

            dgv.Rows(index:=0).IsDarkRow().Should().BeTrue()
            dgv.Rows(index:=1).IsDarkRow().Should().BeFalse()

            dgv.RowsDefaultCellStyle.BackColor = Drawing.Color.White
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Drawing.Color.Black
            dgv.Rows(index:=0).IsDarkRow().Should().BeFalse()
            dgv.Rows(index:=1).IsDarkRow().Should().BeTrue()
        End Using
    End Sub

    <Fact>
    Public Sub SetAlternatingRowColor_HighContrastRestoresInheritedColor()
        Using dgv As New DataGridView()
            dgv.SetAlternatingRowColor(isDarkMode:=True, highContrast:=False)
            dgv.SetAlternatingRowColor(isDarkMode:=False, highContrast:=True)
            dgv.AlternatingRowsDefaultCellStyle.BackColor.Should().Be(expected:=Drawing.Color.Empty)
        End Using
    End Sub

    <Theory>
    <InlineData(True, False, 72)>
    <InlineData(False, False, 230)>
    Public Sub SetAlternatingRowColor_UsesThemeStripe(isDarkMode As Boolean, highContrast As Boolean, shade As Integer)
        Using dgv As New DataGridView()
            dgv.SetAlternatingRowColor(isDarkMode, highContrast)
            dgv.AlternatingRowsDefaultCellStyle.BackColor.Should().
                Be(expected:=Drawing.Color.FromArgb(red:=shade, green:=shade, blue:=shade))
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
            dgv.ColumnHeadersDefaultCellStyle.BackColor.Should().Be(expected:=Drawing.Color.Empty)
            dgv.ColumnHeadersDefaultCellStyle.ForeColor.Should().Be(expected:=Drawing.Color.Empty)
            dgv.RowHeadersDefaultCellStyle.BackColor.Should().Be(expected:=Drawing.Color.Empty)
            dgv.RowHeadersDefaultCellStyle.ForeColor.Should().Be(expected:=Drawing.Color.Empty)
        End Using
    End Sub

End Class
