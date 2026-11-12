' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.ExceptionServices
Imports System.Runtime.InteropServices
Imports System.Text.Json
Imports System.Threading
Imports System.Windows.Forms
Imports CareLink
Imports Xunit

Public Class RawDataViewerDialogTests
    Private Const EM_GETSCROLLPOS As Integer = &H4DD
    Private Const Format As String = "yyyy-MM-dd HH:mm:ss"
    Private Const SB_BOTTOM As Integer = 7
    Private Const SB_PAGEDOWN As Integer = 3
    Private Const SearchAllChildren As Boolean = True
    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_MOUSEWHEEL As Integer = &H20A
    Private Const WM_VSCROLL As Integer = &H115

    Private Shared ReadOnly s_value1 As String() =
        {"$[""changed""] = 2", "$[""removed""] = null", "$[""items""][1] = 2", "", ""}

    Private Shared ReadOnly s_value2 As String() =
        {"$[""changed""] = 3", "", "$[""items""][1] = 4", "$[""items""][2] = 5", "$[""added""] = {}"}

    Private Shared Function Parse(json As String) As JsonElement
        Using document As JsonDocument = JsonDocument.Parse(json)
            Return document.RootElement.Clone()
        End Using
    End Function

    Private Shared Sub CheckForDataChanges()
        NotifyPatientDataElementUpdated()
    End Sub

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

    Private Shared Function ScrollY(control As RichTextBox) As Integer
        Dim position As Drawing.Point
        SendPointMessage(control.Handle, message:=EM_GETSCROLLPOS, wParam:=IntPtr.Zero, position)
        Return position.Y
    End Function

    <DllImport("user32.dll", EntryPoint:="SendMessageW")>
    Private Shared Function SendPointMessage(handle As IntPtr,
                                             message As Integer,
                                             wParam As IntPtr,
                                             ByRef position As Drawing.Point) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="SendMessageW")>
    Private Shared Function SendWindowMessage(handle As IntPtr,
                                              message As Integer,
                                              wParam As IntPtr,
                                              lParam As IntPtr) As IntPtr
    End Function

    <Theory>
    <InlineData("{""a"":1}", "{""a"":1,""b"":null}", "", "$[""b""] = null")>
    <InlineData("{""a"":1,""b"":null}", "{""a"":1}", "$[""b""] = null", "")>
    <InlineData("{""a"":1,""b"":2}", "{""b"":3,""a"":4}", "$[""a""] = 1" & vbLf & "$[""b""] = 2", "$[""a""] = 4" & vbLf & "$[""b""] = 3")>
    <InlineData("{""a"":{}}", "{""a"":{""b"":1}}", "$[""a""] = {}" & vbLf, vbLf & "$[""a""][""b""] = 1")>
    <InlineData("{""items"":[{""id"":1},{""id"":2},{""id"":3}]}", "{""items"":[{""id"":4},{""id"":1},{""id"":2}]}", vbLf & "$[""items""][2][""id""] = 3", "$[""items""][0][""id""] = 4" & vbLf)>
    <InlineData("[1,2]", "[0,1,2]", "", "$[0] = 0")>
    <InlineData("[null,null]", "[null]", "$[1] = null", "")>
    <InlineData("{""items"":[]}", "{""items"":[1]}", "$[""items""] = []" & vbLf, vbLf & "$[""items""][0] = 1")>
    <InlineData("[1,1,2]", "[1,2,2]", "$[1] = 1" & vbLf, vbLf & "$[2] = 2")>
    <InlineData("{""a"":[1,2],""b"":[3]}", "{""a"":[3,1],""b"":[2]}", vbLf & "$[""a""][1] = 2" & vbLf & "$[""b""][0] = 3", "$[""a""][0] = 3" & vbLf & vbLf & "$[""b""][0] = 2")>
    <InlineData("{""items"":[{""id"":1,""value"":2}]}", "{""items"":[{""id"":1,""value"":3}]}", "$[""items""][0][""value""] = 2", "$[""items""][0][""value""] = 3")>
    <InlineData("{""items"":[{""values"":[1,2]}]}", "{""items"":[{""values"":[3,1]}]}", vbLf & "$[""items""][0][""values""][1] = 2", "$[""items""][0][""values""][0] = 3" & vbLf)>
    Public Sub DifferenceToggle_AlignsMissingAndReorderedValues(previousJson As String, currentJson As String,
                                                                  expectedPrevious As String, expectedCurrent As String)
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(Parse(previousJson), Parse(currentJson))
                    dialog.Show()
                    Dim previous As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox =
                        DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    toggle.Checked = True
                    Assert.Equal(expected:=expectedPrevious, actual:=previous.Text)
                    Assert.Equal(expected:=expectedCurrent, actual:=current.Text)
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub DifferenceToggle_ShowsChangedPathsAndRestoresFullDocuments()
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(
                    prevJson:=Parse(json:="{""same"":1,""changed"":2,""removed"":null,""items"":[1,2]}"),
                    json:=Parse(json:="{""same"":1,""changed"":3,""added"":{},""items"":[1,4,5]}"))
                    dialog.Show()
                    Dim previous As RichTextBox =
                    DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                    DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox =
                    DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    toggle.Checked = False
                    Dim previousText As String = previous.Text
                    Dim currentText As String = current.Text
                    Dim originalBounds As Drawing.Rectangle = current.Bounds
                    Dim previousWordWrap As Boolean = previous.WordWrap
                    Dim currentWordWrap As Boolean = current.WordWrap

                    toggle.Checked = SearchAllChildren
                    Assert.True(condition:=previous.Visible)
                    Assert.Equal(expected:=DockStyle.None, actual:=current.Dock)
                    Assert.False(condition:=previous.WordWrap)
                    Assert.False(condition:=current.WordWrap)
                    Assert.Equal(expected:=String.Join(separator:=vbLf, s_value1), actual:=previous.Text)
                    Assert.Equal(expected:=String.Join(separator:=vbLf, s_value2), actual:=current.Text)

                    toggle.Checked = False
                    Assert.Equal(expected:=previousText, actual:=previous.Text)
                    Assert.Equal(expected:=currentText, actual:=current.Text)
                    Assert.Equal(expected:=originalBounds, actual:=current.Bounds)
                    Assert.Equal(expected:=previousWordWrap, actual:=previous.WordWrap)
                    Assert.Equal(expected:=currentWordWrap, actual:=current.WordWrap)
                End Using
            End Sub

        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub DifferenceView_ScrollsToRemovedValuesAtEnd()
        Dim action As Action =
            Sub()
                Dim previousFields As New List(Of String) From {
                    """common"":1"}
                For index As Integer = 0 To 159
                    previousFields.Add(item:=$"{JsonSerializer.Serialize(value:=$"removed{index}")}:1")
                Next
                Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:=$"{{{String.Join(separator:=",", values:=previousFields)}}}"),
                                                        json:=Parse(json:="{""common"":1}"))
                    dialog.Show()
                    Dim previous As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox =
                        DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    toggle.Checked = True
                    SendWindowMessage(current.Handle,
                                      message:=WM_VSCROLL,
                                      wParam:=New IntPtr(value:=SB_BOTTOM),
                                      lParam:=IntPtr.Zero)
                    Assert.True(condition:=ScrollY(control:=current) > 0)
                    Assert.Equal(expected:=ScrollY(control:=current), actual:=ScrollY(control:=previous))
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub IdenticalDocuments_HidePreviousAndDockCurrent_WhenToggleChanges()
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:="{""a"":1,""b"":[true,null]}"),
                                                        json:=Parse(json:="{""b"":[true,null],""a"":1}"))
                    dialog.Show()
                    Dim previous As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox =
                        DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    Dim currentText As String = current.Text
                    Assert.False(condition:=previous.Visible)
                    Assert.Equal(expected:=DockStyle.Top, actual:=current.Dock)
                    toggle.Checked = SearchAllChildren
                    Assert.False(condition:=previous.Visible)
                    Assert.Equal(expected:=DockStyle.Top, actual:=current.Dock)
                    Assert.Equal(expected:=currentText, actual:=current.Text)
                End Using
            End Sub

        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub MissingPreviousData_ShowsFullCurrentDocument()
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(prevJson:=New JsonElement(), json:=Parse(json:="{""a"":1}"))
                    dialog.Show()
                    Dim previous As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox =
                        DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    Dim currentText As String = current.Text
                    toggle.Checked = SearchAllChildren
                    Assert.False(condition:=previous.Visible)
                    Assert.Equal(expected:=DockStyle.Top, actual:=current.Dock)
                    Assert.Equal(expected:=currentText, actual:=current.Text)
                    Assert.Contains(expectedSubstring:="""a""", actualString:=current.Text)
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub Refresh_UpdatesDocumentsAndComparisonAcrossLayoutChanges()
        Dim action As Action =
            Sub()
                Dim originalPrevious As JsonElement = PreviousPatientDataElement
                Dim originalCurrent As JsonElement = PatientDataElement
                Try
                    Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:="{""value"":1}"),
                                                            json:=Parse(json:="{""value"":1}"))
                        dialog.Show()
                        Dim previous As RichTextBox =
                            DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                        Dim current As RichTextBox =
                            DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                        Dim toggle As CheckBox =
                            DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                        Dim refresh As Button =
                            DirectCast(dialog.Controls.Find(key:="btnRefresh", SearchAllChildren)(0), Button)
                        Dim search As TextBox =
                            DirectCast(dialog.Controls.Find(key:="txtFind", SearchAllChildren)(0), TextBox)

                        PreviousPatientDataElement = Parse(json:="{""value"":2}")
                        PatientDataElement = Parse(json:="{""value"":3}")
                        CheckForDataChanges()
                        refresh.PerformClick()
                        Assert.True(condition:=previous.Visible)
                        Assert.Equal(expected:=DockStyle.None, actual:=current.Dock)
                        Assert.Equal(expected:="$[""value""] = 2", actual:=previous.Text)
                        Assert.Equal(expected:="$[""value""] = 3", actual:=current.Text)
                        Dim sideBySideBounds As Drawing.Rectangle = current.Bounds
                        search.Text = "value"

                        PatientDataElement = Parse(json:="{""value"":2}")
                        CheckForDataChanges()
                        refresh.PerformClick()
                        Assert.False(condition:=previous.Visible)
                        Assert.Equal(expected:=DockStyle.Top, actual:=current.Dock)
                        Assert.Contains(expectedSubstring:="""value""", actualString:=current.Text)
                        Assert.Equal(expected:="", actual:=search.Text)

                        PreviousPatientDataElement = New JsonElement()
                        PatientDataElement = Parse(json:="{""new"":4}")
                        CheckForDataChanges()
                        refresh.PerformClick()
                        Assert.False(condition:=previous.Visible)
                        Assert.Contains(expectedSubstring:="""new""", actualString:=current.Text)

                        PreviousPatientDataElement = Parse(json:="{""new"":4}")
                        PatientDataElement = Parse(json:="{""new"":5}")
                        CheckForDataChanges()
                        refresh.PerformClick()
                        Assert.True(condition:=previous.Visible)
                        Assert.Equal(expected:=DockStyle.None, actual:=current.Dock)
                        Assert.Equal(expected:=sideBySideBounds, actual:=current.Bounds)
                        Assert.Equal(expected:="$[""new""] = 4", actual:=previous.Text)
                        Assert.Equal(expected:="$[""new""] = 5", actual:=current.Text)
                        toggle.Checked = False
                        Assert.Contains(expectedSubstring:="""new""", actualString:=previous.Text)
                        Assert.Contains(expectedSubstring:="4", actualString:=previous.Text)
                        Assert.Contains(expectedSubstring:="5", actualString:=current.Text)
                    End Using
                Finally
                    PreviousPatientDataElement = originalPrevious
                    PatientDataElement = originalCurrent
                End Try
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub RefreshMonitoring_IgnoresServerTimeAndRestartsAfterRefresh()
        Dim action As Action =
            Sub()
                Dim originalPrevious As JsonElement = PreviousPatientDataElement
                Dim originalCurrent As JsonElement = PatientDataElement
                Try
                    PreviousPatientDataElement = Parse(json:="{""currentServerTime"":1,""value"":1}")
                    PatientDataElement = Parse(json:="{""currentServerTime"":1,""value"":2}")
                    Using dialog As New RawDataViewerDialog(prevJson:=PreviousPatientDataElement,
                                                            json:=PatientDataElement)
                        dialog.Show()
                        Dim refresh As Button =
                            DirectCast(dialog.Controls.Find(key:="btnRefresh", SearchAllChildren)(0), Button)
                        Dim current As RichTextBox =
                            DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                        Assert.False(condition:=refresh.Enabled)

                        PreviousPatientDataElement = Parse(json:="{""value"":1,""currentServerTime"":2}")
                        PatientDataElement = Parse(json:="{""value"":2,""currentServerTime"":2}")
                        CheckForDataChanges()
                        Assert.False(condition:=refresh.Enabled)

                        PreviousPatientDataElement = Parse(json:="{""currentServerTime"":3,""value"":3}")
                        CheckForDataChanges()
                        Assert.True(condition:=refresh.Enabled)
                        refresh.PerformClick()
                        Assert.False(condition:=refresh.Enabled)
                        Assert.Contains(expectedSubstring:="2", actualString:=current.Text)

                        PatientDataElement = Parse(json:="{""currentServerTime"":4,""value"":4}")
                        CheckForDataChanges()
                        Assert.True(condition:=refresh.Enabled)
                        refresh.PerformClick()
                        Assert.False(condition:=refresh.Enabled)
                        Assert.Contains(expectedSubstring:="4", actualString:=current.Text)
                        CheckForDataChanges()
                        Assert.False(condition:=refresh.Enabled)
                    End Using
                    ' A disposed dialog must be unsubscribed.
                    NotifyPatientDataElementUpdated()
                Finally
                    PreviousPatientDataElement = originalPrevious
                    PatientDataElement = originalCurrent
                End Try
            End Sub
        RunOnSta(action)
    End Sub

    <Theory>
    <InlineData("{""nested"":{""currentServerTime"":1}}", "{""nested"":{""currentServerTime"":2}}")>
    <InlineData("{""value"":1}", "{""value"":1,""added"":null}")>
    <InlineData("{""items"":[1,2]}", "{""items"":[2,1]}")>
    <InlineData("{""value"":1}", "{""value"":2}")>
    Public Sub RefreshMonitoring_DetectsOtherCurrentDataChanges(initial As String, updated As String)
        Dim action As Action =
            Sub()
                Dim originalPrevious As JsonElement = PreviousPatientDataElement
                Dim originalCurrent As JsonElement = PatientDataElement
                Try
                    PreviousPatientDataElement = New JsonElement()
                    PatientDataElement = Parse(json:=initial)
                    Using dialog As New RawDataViewerDialog(prevJson:=PreviousPatientDataElement,
                                                            json:=PatientDataElement)
                        dialog.Show()
                        Dim refresh As Button =
                            DirectCast(dialog.Controls.Find(key:="btnRefresh", SearchAllChildren)(0), Button)
                        PatientDataElement = Parse(json:=updated)
                        CheckForDataChanges()
                        Assert.True(condition:=refresh.Enabled)
                    End Using
                Finally
                    PreviousPatientDataElement = originalPrevious
                    PatientDataElement = originalCurrent
                End Try
            End Sub
        RunOnSta(action)
    End Sub

    <Theory>
    <InlineData("[1,2,null]", "[null,1,2]")>
    <InlineData("[{""a"":1,""b"":2},{""a"":3}]", "[{""a"":3},{""b"":2,""a"":1}]")>
    <InlineData("{""items"":[{""values"":[1,2]}]}", "{""items"":[{""values"":[2,1]}]}")>
    Public Sub ReorderedArrayItems_HidePreviousDocument(previousJson As String, currentJson As String)
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:=previousJson),
                                                        json:=Parse(json:=currentJson))
                    dialog.Show()
                    Dim previous As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox =
                        DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    toggle.Checked = True
                    Assert.False(condition:=previous.Visible)
                    Assert.Equal(expected:=DockStyle.Top, actual:=current.Dock)
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub ScrollAndSearch_KeepBothDocumentsAtTheSameVerticalPosition()
        Dim action As Action =
            Sub()
                Dim previousFields As New List(Of String)
                Dim currentFields As New List(Of String)
                For index As Integer = 0 To 159
                    Dim name As String = If(index = 10 OrElse index = 130, $"needle{index}", $"field{index}")
                    previousFields.Add(item:=$"{JsonSerializer.Serialize(value:=name)}:1")
                    currentFields.Add(item:=$"{JsonSerializer.Serialize(value:=name)}:2")
                Next
                Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:=$"{{{String.Join(separator:=",", previousFields)}}}"),
                                                        json:=Parse(json:=$"{{{String.Join(separator:=",", currentFields)}}}"))
                    dialog.Show()
                    Dim previous As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox =
                        DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    Dim search As TextBox =
                        DirectCast(dialog.Controls.Find(key:="txtFind", SearchAllChildren)(0), TextBox)
                    Dim findNext As Button =
                        DirectCast(dialog.Controls.Find(key:="btnFindNext", SearchAllChildren)(0), Button)
                    Dim findAll As Button =
                        DirectCast(dialog.Controls.Find(key:="btnFindAll", SearchAllChildren)(0), Button)
                    toggle.Checked = True
                    Assert.Equal(expected:=RichTextBoxScrollBars.Horizontal, actual:=previous.ScrollBars)

                    SendWindowMessage(current.Handle,
                                      message:=WM_VSCROLL,
                                      wParam:=New IntPtr(value:=SB_PAGEDOWN),
                                      lParam:=IntPtr.Zero)
                    Assert.True(condition:=ScrollY(control:=current) > 0)
                    Assert.Equal(expected:=ScrollY(control:=current), actual:=ScrollY(control:=previous))

                    Dim beforePreviousScroll As Integer = ScrollY(control:=previous)
                    SendWindowMessage(previous.Handle,
                                      message:=WM_VSCROLL,
                                      wParam:=New IntPtr(value:=SB_PAGEDOWN),
                                      lParam:=IntPtr.Zero)
                    Assert.True(condition:=ScrollY(control:=previous) > beforePreviousScroll)
                    Assert.Equal(expected:=ScrollY(control:=previous), actual:=ScrollY(control:=current))
                    beforePreviousScroll = ScrollY(control:=previous)
                    previous.Focus()
                    Dim mousePosition As Drawing.Point = previous.PointToScreen(p:=New Drawing.Point(x:=10, y:=10))
                    Dim mouseCoordinates As Integer =
                        (mousePosition.X And &HFFFF) Or ((mousePosition.Y And &HFFFF) << 16)
                    SendWindowMessage(previous.Handle,
                                      message:=WM_MOUSEWHEEL,
                                      wParam:=New IntPtr(value:=120 << 16),
                                      lParam:=New IntPtr(value:=mouseCoordinates))
                    Application.DoEvents()
                    Assert.True(condition:=ScrollY(control:=previous) < beforePreviousScroll)
                    Assert.Equal(expected:=ScrollY(control:=previous), actual:=ScrollY(control:=current))

                    current.Focus()
                    current.Select(start:=0, length:=0)
                    SendWindowMessage(current.Handle,
                                      message:=WM_KEYDOWN,
                                      wParam:=New IntPtr(value:=Keys.End),
                                      lParam:=IntPtr.Zero)
                    SendWindowMessage(current.Handle,
                                      message:=WM_KEYDOWN,
                                      wParam:=New IntPtr(value:=CInt(Keys.PageDown)),
                                      lParam:=IntPtr.Zero)
                    Assert.Equal(expected:=ScrollY(control:=current), actual:=ScrollY(control:=previous))

                    search.Text = "needle"
                    Assert.Equal(expected:=ScrollY(control:=current), actual:=ScrollY(control:=previous))
                    Dim beforeNext As Integer = ScrollY(control:=current)
                    findNext.PerformClick()
                    Assert.True(condition:=ScrollY(control:=current) > beforeNext)
                    Assert.Equal(expected:=ScrollY(control:=current), actual:=ScrollY(control:=previous))
                    findAll.PerformClick()
                    Assert.Equal(expected:=ScrollY(control:=current), actual:=ScrollY(control:=previous))

                    toggle.Checked = False
                    Assert.Equal(expected:=RichTextBoxScrollBars.Both, actual:=previous.ScrollBars)
                    SendWindowMessage(current.Handle,
                                      message:=WM_VSCROLL,
                                      wParam:=New IntPtr(value:=SB_PAGEDOWN),
                                      lParam:=IntPtr.Zero)
                    Assert.True(condition:=ScrollY(control:=current) > 0)
                    Assert.Equal(expected:=ScrollY(control:=current), actual:=ScrollY(control:=previous))
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Theory>
    <InlineData(1, "1 min")>
    <InlineData(1440, "1 day")>
    <InlineData(1501, "1 day, 1 hr and 1 min")>
    <InlineData(2880, "2 days")>
    Public Sub DurationElements_ShowFormattedUnits(minutes As Integer, expected As String)
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:="{}"),
                                                        json:=Parse(json:=$"{{""durationMinutes"":{minutes}}}"))
                    dialog.Show()
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Assert.Contains(expectedSubstring:=$"// {expected}", actualString:=current.Text)
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub SnoozeDuration_IsDisplayedAsMinutesInRawAndDifferenceViews()
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:="{""snoozed"":{""duration"":600}}"),
                                                        json:=Parse(json:="{""snoozed"":{""duration"":1200}}"))
                    dialog.Show()
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim differenceOnly As CheckBox =
                        DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)

                    Assert.Contains(expectedSubstring:="// 20 min", actualString:=current.Text)
                    differenceOnly.Checked = True
                    Assert.Contains(expectedSubstring:="// 20 min", actualString:=current.Text)
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub TimeElements_ShowLocalTimeAndDoNotWrap()
        Dim action As Action =
            Sub()
                Dim expectedSeconds As String =
                    DateTimeOffset.FromUnixTimeSeconds(seconds:=1700000000).ToLocalTime().ToString(Format)
                Dim expectedMillis As String =
                    DateTimeOffset.FromUnixTimeMilliseconds(milliseconds:=1700000001000).ToLocalTime().ToString(Format)
                Using dialog As New RawDataViewerDialog(prevJson:=Parse(json:="{""lastTime"":1600000000,""count"":1700000000}"),
                                                        json:=Parse(json:="{""lastTime"":1700000000,""otherTIME"":1700000001000,""count"":1700000000}"))
                    dialog.Show()
                    Dim previous As RichTextBox = DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox = DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim toggle As CheckBox = DirectCast(dialog.Controls.Find(key:="CheckBoxDifferenceOnly", SearchAllChildren)(0), CheckBox)
                    Assert.False(condition:=current.WordWrap)
                    Assert.Contains(expectedSubstring:="// " & expectedSeconds, actualString:=current.Text)
                    Assert.Contains(expectedSubstring:="// " & expectedMillis, actualString:=current.Text)
                    Assert.DoesNotContain(expectedSubstring:="1700000000," & vbLf & "// ", actualString:=current.Text)
                    Assert.DoesNotContain(expectedSubstring:="""count"": 1700000000,  //", actualString:=current.Text)
                    toggle.Checked = True
                    Assert.False(condition:=current.WordWrap)
                    Assert.False(condition:=previous.WordWrap)
                    Assert.Contains(expectedSubstring:="= 1700000000  // " & expectedSeconds, actualString:=current.Text)
                    Assert.Contains(expectedSubstring:="= 1600000000  // ", actualString:=previous.Text)
                End Using
            End Sub
        RunOnSta(action)
    End Sub

    <Fact>
    Public Sub UnmatchedLastPreviousSgs_IsHiddenAndNewSgsAreOrderedByIndex()
        Dim action As Action =
            Sub()
                Using dialog As New RawDataViewerDialog(
                    prevJson:=Parse(json:="{""sgs"":[{""sg"":1},{""sg"":2},{""sg"":9}]}"),
                    json:=Parse(json:="{""sgs"":[{""sg"":0},{""sg"":1},{""sg"":2}]}"))
                    dialog.Show()
                    Dim previous As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="PreviousRawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Dim current As RichTextBox =
                        DirectCast(dialog.Controls.Find(key:="RawDataRTB", SearchAllChildren)(0), RichTextBox)
                    Assert.DoesNotContain(expectedSubstring:="= 9", actualString:=previous.Text)
                    Assert.Contains(expectedSubstring:="$[""sgs""][0][""sg""] = 0", actualString:=current.Text)
                End Using
            End Sub
        RunOnSta(action)
    End Sub

End Class
