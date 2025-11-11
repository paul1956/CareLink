Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
<UISettings(MaxAttempts:=10)>
Public Class ControlExtensionsTests

    <Fact>
    Public Sub CenterLabelXOnParent_CentersLabel_WithAutoSizeFalse()
        Using parent As New Panel()
            parent.Width = 200
            Dim lbl As New Label() With {.Width = 50, .AutoSize = False}
            parent.Controls.Add(lbl)

            ' Act
            lbl.CenterLabelXOnParent()

            ' Assert
            lbl.Left.Should().Be((parent.Width - lbl.Width) \ 2)
        End Using
    End Sub

    <Fact>
    Public Sub CenterLabelXOnParent_NullOrDisposed_DoesNotThrow()
        ' Null
        Dim subCallNull As Action = Sub() ControlExtensions.CenterLabelXOnParent(Nothing)
        subCallNull.Should().NotThrow()

        ' Disposed
        Dim lbl As New Label()
        lbl.Dispose()
        Dim subCallDisposed As Action = Sub() ControlExtensions.CenterLabelXOnParent(lbl)
        subCallDisposed.Should().NotThrow()
    End Sub

    <Fact>
    Public Sub CenterXOnParent_CentersControl_CenterAndHalves()
        Using parent As New Panel()
            parent.Width = 200
            Dim c As New Control() With {.Width = 40}
            parent.Controls.Add(c)

            ' Center in middle
            c.CenterXOnParent()
            c.Left.Should().Be((parent.Width - c.Width) \ 2)

            ' Center on left half
            c.CenterXOnParent(onLeftHalf:=True)
            Dim expectedLeftLeftHalf As Integer = Math.Max(0, ((parent.Width \ 2) - c.Width) \ 2)
            c.Left.Should().Be(expectedLeftLeftHalf)

            ' Center on right half
            c.CenterXOnParent(onLeftHalf:=False)
            Dim halfWidth As Integer = parent.Width \ 2
            Dim expectedRight As Integer = Math.Max(0, halfWidth + ((halfWidth - c.Width) \ 2))
            c.Left.Should().Be(expectedRight)
        End Using
    End Sub

    <Fact>
    Public Sub CenterXOnParent_NoParentOrDisposed_DoesNotThrowAndLeavesLeftUnchanged()
        ' No parent
        Dim c As New Control With {
            .Width = 30,
            .Left = 5}

        Dim actNoParent As Action = Sub() ControlExtensions.CenterXOnParent(c)
        actNoParent.Should().NotThrow()
        c.Left.Should().Be(5)

        ' Disposed
        Dim d As New Control() With {.Width = 20}
        d.Dispose()
        d.Left = 7
        Dim actDisposed As Action = Sub() ControlExtensions.CenterXOnParent(d)
        actDisposed.Should().NotThrow()
        d.Left.Should().Be(7)
    End Sub

    <Fact>
    Public Sub CenterXOnParent_LabelAutoSize_UsesPreferredWidthWhenAvailable()
        Using parent As New Panel()
            parent.Width = 300
            Dim lbl As New Label() With {.AutoSize = True, .Text = New String("X"c, 40)}
            parent.Controls.Add(lbl)

            ' Act
            lbl.CenterXOnParent()

            ' Assert uses PreferredWidth
            Dim ctrlWidth As Integer = lbl.PreferredWidth
            lbl.Left.Should().Be((parent.Width - ctrlWidth) \ 2)
        End Using
    End Sub

    <Fact>
    Public Sub CenterXYOnParent_CentersLabel_WithVerticalOffset()
        Using parent As New Panel()
            parent.Width = 300
            parent.Height = 120
            Dim lbl As New Label() With {.AutoSize = True, .Text = "Hello"}
            parent.Controls.Add(lbl)

            ' Act
            lbl.CenterXYOnParent(verticalOffset:=5)

            ' Assert horizontal centered
            Dim ctrlWidth As Integer = If(lbl.AutoSize, lbl.PreferredWidth, lbl.Width)
            lbl.Left.Should().Be((parent.Width - ctrlWidth) \ 2)

            ' Assert vertical centered with offset
            Dim ctrlHeight As Integer = If(lbl.AutoSize, lbl.PreferredHeight, lbl.Height)
            Dim totalHeight As Integer = ctrlHeight + lbl.Margin.Top + lbl.Margin.Bottom
            lbl.Top.Should().Be(((parent.Height - totalHeight) \ 2) + 5)
        End Using
    End Sub

    <Fact>
    Public Sub FindControlByName_FindsExistingControl_ReturnsControl()
        Using parent As New Panel()
            Dim btn As New Button() With {.Name = "myButton"}
            parent.Controls.Add(btn)

            Dim result As Control = parent.Controls.FindControlByName("myButton")
            result.Should().NotBeNull()
            result.Should().BeSameAs(btn)
        End Using
    End Sub

    <Fact>
    Public Sub FindControlByName_DisposedChild_ReturnsNothing()
        Using parent As New Panel()
            Dim btn As New Button() With {.Name = "btn"}
            parent.Controls.Add(btn)
            ' Dispose child to force early return
            btn.Dispose()

            Dim result As Control = parent.Controls.FindControlByName("btn")
            result.Should().BeNull()
        End Using
    End Sub

    <Fact>
    Public Sub FindControlByName_NullControls_ReturnsNothing()
        Dim result As Control = ControlExtensions.FindControlByName(Nothing, "anything")
        result.Should().BeNull()
    End Sub

    <Fact>
    Public Sub FindHorizontalAndVerticalMidpoint_ReturnsCorrectValues_AndHandlesNull()
        Dim c As New Control() With {.Left = 10, .Top = 20, .Width = 30, .Height = 40}
        c.FindHorizontalMidpoint().Should().Be(10 + (30 \ 2))
        c.FindVerticalMidpoint().Should().Be(20 + (40 \ 2))

        Dim nullCtrl As Control = Nothing
        nullCtrl.FindHorizontalMidpoint().Should().Be(0)
        nullCtrl.FindVerticalMidpoint().Should().Be(0)

        Dim disposed As New Control()
        disposed.Dispose()
        disposed.FindHorizontalMidpoint().Should().Be(0)
        disposed.FindVerticalMidpoint().Should().Be(0)
    End Sub

    <Fact>
    Public Sub SetDgvCustomHeadersVisualStyles_AppliesToNestedDataGridViews()
        Using parent As New Panel()
            Dim dgv1 As New DataGridView()
            Dim container As New Panel()
            Dim dgv2 As New DataGridView()

            parent.Controls.Add(dgv1)
            container.Controls.Add(dgv2)
            parent.Controls.Add(container)

            ' Precondition: default true for EnableHeadersVisualStyles
            dgv1.EnableHeadersVisualStyles.Should().BeTrue()
            dgv2.EnableHeadersVisualStyles.Should().BeTrue()

            ' Act
            parent.SetDgvCustomHeadersVisualStyles()

            ' Assert
            dgv1.EnableHeadersVisualStyles.Should().BeFalse()
            dgv2.EnableHeadersVisualStyles.Should().BeFalse()
            dgv1.ColumnHeadersDefaultCellStyle.BackColor.Should().Be(System.Drawing.Color.Black)
            dgv1.ColumnHeadersDefaultCellStyle.ForeColor.Should().Be(System.Drawing.Color.White)
            dgv2.ColumnHeadersDefaultCellStyle.BackColor.Should().Be(System.Drawing.Color.Black)
            dgv2.ColumnHeadersDefaultCellStyle.ForeColor.Should().Be(System.Drawing.Color.White)
        End Using
    End Sub

    <Fact>
    Public Sub SetDgvCustomHeadersVisualStyles_NullOrDisposed_DoesNotThrow()
        Dim actNull As Action = Sub() ControlExtensions.SetDgvCustomHeadersVisualStyles(Nothing)
        actNull.Should().NotThrow()

        Dim p As New Panel()
        p.Dispose()
        Dim actDisposed As Action = Sub() ControlExtensions.SetDgvCustomHeadersVisualStyles(p)
        actDisposed.Should().NotThrow()
    End Sub

End Class
