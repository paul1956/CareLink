' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Windows.Forms.ComboBox

<DefaultEvent(NameOf(SelectedIndexChanged))>
<DefaultProperty(NameOf(Items))>
<DefaultBindingProperty(NameOf(Text))>
<Designer("System.Windows.Forms.Design.ComboBoxDesigner, " & AssemblyRef.SystemDesign)>
Public Class ToolStripComboBoxEx
    Inherits ToolStripControlHost

    Public Sub New()
        MyBase.New(New ComboBox)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Event BackgroundImageChanged As EventHandler

    Public Event BackgroundImageLayoutChanged As EventHandler

    Public Event DrawItem As DrawItemEventHandler

    Public Event DropDown As EventHandler

    Public Event DropDownStyleChanged As EventHandler

    Public Event MeasureItem As MeasureItemEventHandler

    Public Event PaddingChanged As EventHandler

    Public Event SelectedIndexChanged As EventHandler

    Public Event SelectionChangeCommitted As EventHandler

    Public Event TextUpdate As EventHandler

    Public Overrides Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(value As Color)
            MyBase.BackColor = value
        End Set
    End Property

    Public Property DataSource As BindingSource
        Get
            Return CType(CType(Me.Control, ComboBox).DataSource, BindingSource)
        End Get
        Set
            CType(Me.Control, ComboBox).DataSource = Value
        End Set
    End Property

    Public Property DisplayMember As String
        Get
            Return CType(Me.Control, ComboBox).DisplayMember
        End Get
        Set
            CType(Me.Control, ComboBox).DisplayMember = Value
        End Set
    End Property

    Public Property DropDownStyle As ComboBoxStyle
        Get
            Return CType(Me.Control, ComboBox).DropDownStyle
        End Get
        Set
            CType(Me.Control, ComboBox).DropDownStyle = Value
        End Set
    End Property

    Public Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(value As Font)
            MyBase.Font = value
        End Set
    End Property

    Public Overrides Property ForeColor As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(value As Color)
            MyBase.ForeColor = value
        End Set
    End Property

    Public Property FormattingEnabled As Boolean
        Get
            Return CType(Me.Control, ComboBox).FormattingEnabled
        End Get
        Set
            CType(Me.Control, ComboBox).FormattingEnabled = Value
        End Set
    End Property

    Public Overloads ReadOnly Property items As ObjectCollection
        Get
            Return CType(Me.Control, ComboBox).Items
        End Get
    End Property

    Public Property Location As Point
        Get
            Return CType(Me.Control, ComboBox).Location
        End Get
        Set
            CType(Me.Control, ComboBox).Location = Value
        End Set
    End Property

    Public Property SelectedIndex As Integer
        Get
            Return CType(Me.Control, ComboBox).SelectedIndex
        End Get
        Set
            CType(Me.Control, ComboBox).SelectedIndex = Value
        End Set
    End Property

    Public Overloads Property SelectedItem As Object
        Get
            Return CType(Me.Control, ComboBox).SelectedItem
        End Get
        Set
            CType(Me.Control, ComboBox).SelectedItem = Value
        End Set
    End Property

    Public Property SelectedValue As Object
        Get
            Return CType(Me.Control, ComboBox).SelectedValue.ToString
        End Get
        Set
            CType(Me.Control, ComboBox).SelectedValue = Value
        End Set
    End Property

    Public Overloads Property TabIndex As Integer
        Get
            Return CType(Me.Control, ComboBox).TabIndex
        End Get
        Set
            CType(Me.Control, ComboBox).TabIndex = Value
        End Set
    End Property

    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
        End Set
    End Property

    Public Property ValueMember As String
        Get
            Return CType(Me.Control, ComboBox).ValueMember
        End Get
        Set
            CType(Me.Control, ComboBox).ValueMember = Value
        End Set
    End Property

    Private Sub HandleBackgroundImageChanged(sender As Object, e As EventArgs)
        RaiseEvent BackgroundImageChanged(Me, e)
    End Sub

    Private Sub HandleBackgroundImageLayoutChanged(sender As Object, e As EventArgs)
        RaiseEvent BackgroundImageLayoutChanged(Me, e)
    End Sub

    Private Sub HandleDrawItem(sender As Object, e As DrawItemEventArgs)
        RaiseEvent DrawItem(Me, e)
    End Sub

    Private Sub HandleDropDown(sender As Object, e As EventArgs)
        RaiseEvent DropDown(Me, e)
    End Sub

    Private Sub HandleDropDownStyleChanged(sender As Object, e As EventArgs)
        RaiseEvent DropDownStyleChanged(Me, e)
    End Sub

    Private Sub HandleMeasureItem(sender As Object, e As MeasureItemEventArgs)
        RaiseEvent MeasureItem(Me, e)
    End Sub

    Private Sub HandlePaddingChanged(sender As Object, e As EventArgs)
        RaiseEvent PaddingChanged(Me, e)
    End Sub

    Private Sub HandleSelectedIndexChanged(sender As Object, e As EventArgs)
        RaiseEvent SelectedIndexChanged(Me, e)
    End Sub

    Private Sub HandleSelectionChangeCommitted(sender As Object, e As EventArgs)
        RaiseEvent SelectionChangeCommitted(Me, e)
    End Sub

    Private Sub HandleTextUpdate(sender As Object, e As EventArgs)
        RaiseEvent TextUpdate(Me, e)
    End Sub

    Protected Overrides Sub OnParentChanged(oldParent As ToolStrip, newParent As ToolStrip)
        MyBase.OnParentChanged(oldParent, newParent)
    End Sub

    Protected Overrides Sub OnSubscribeControlEvents(c As Control)

        ' Call the base so the base events are connected.
        MyBase.OnSubscribeControlEvents(c)

        ' Cast the control to a ComboBox control.
        ' Add the event.
        AddHandler CType(c, ComboBox).BackgroundImageChanged, AddressOf Me.HandleBackgroundImageChanged
        AddHandler CType(c, ComboBox).BackgroundImageLayoutChanged, AddressOf Me.HandleBackgroundImageLayoutChanged
        AddHandler CType(c, ComboBox).DrawItem, AddressOf Me.HandleDrawItem
        AddHandler CType(c, ComboBox).DropDown, AddressOf Me.HandleDropDown
        AddHandler CType(c, ComboBox).DropDownStyleChanged, AddressOf Me.HandleDropDownStyleChanged
        AddHandler CType(c, ComboBox).MeasureItem, AddressOf Me.HandleMeasureItem
        AddHandler CType(c, ComboBox).PaddingChanged, AddressOf Me.HandlePaddingChanged
        AddHandler CType(c, ComboBox).SelectedIndexChanged, AddressOf Me.HandleSelectedIndexChanged
        AddHandler CType(c, ComboBox).SelectionChangeCommitted, AddressOf Me.HandleSelectionChangeCommitted
        AddHandler CType(c, ComboBox).TextUpdate, AddressOf Me.HandleTextUpdate

    End Sub

    Protected Overrides Sub OnUnsubscribeControlEvents(c As Control)
        ' Call the base method so the basic events are unsubscribe from.
        MyBase.OnUnsubscribeControlEvents(c)

        ' Cast the control to a ComboBox control.
        ' Add the event.
        RemoveHandler CType(c, ComboBox).BackgroundImageChanged, AddressOf Me.HandleBackgroundImageChanged
        RemoveHandler CType(c, ComboBox).BackgroundImageLayoutChanged, AddressOf Me.HandleBackgroundImageLayoutChanged
        RemoveHandler CType(c, ComboBox).DrawItem, AddressOf Me.HandleDrawItem
        RemoveHandler CType(c, ComboBox).DropDown, AddressOf Me.HandleDropDown
        RemoveHandler CType(c, ComboBox).DropDownStyleChanged, AddressOf Me.HandleDropDownStyleChanged
        RemoveHandler CType(c, ComboBox).MeasureItem, AddressOf Me.HandleMeasureItem
        RemoveHandler CType(c, ComboBox).PaddingChanged, AddressOf Me.HandlePaddingChanged
        RemoveHandler CType(c, ComboBox).SelectedIndexChanged, AddressOf Me.HandleSelectedIndexChanged
        RemoveHandler CType(c, ComboBox).SelectionChangeCommitted, AddressOf Me.HandleSelectionChangeCommitted
        RemoveHandler CType(c, ComboBox).TextUpdate, AddressOf Me.HandleTextUpdate

    End Sub

    Public Function FindString(s As String) As Integer
        Return CType(Me.Control, ComboBox).FindString(s)
    End Function

    Public Function FindStringExact(s As String) As Integer
        Return CType(Me.Control, ComboBox).FindStringExact(s)
    End Function

End Class
