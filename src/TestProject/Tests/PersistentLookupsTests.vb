' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports CareLink
Imports Xunit

<Collection("Sequential")>
Public Class PersistentLookupsTests
    Implements IDisposable

    Private Shared ReadOnly s_expectedPlaceholders As String() = {"sg", "units"}
    Private Shared ReadOnly s_expectedPrompts As String() = {"", "(futureValue) (> 30 minutes)"}

    Private ReadOnly _folder As String =
        Path.Combine(Path.GetTempPath(), $"CareLinkLookups-{Guid.NewGuid()}")

    Private Shared Function ThrowUnexpectedConfirmation(warning As String) As DialogResult
        Throw New InvalidOperationException(message:=$"Unexpected warning: {warning}")
    End Function

    Private Shared Function ThrowUnexpectedPrompt(id As String) As String
        Throw New InvalidOperationException(message:=$"Unexpected prompt for {id}.")
    End Function

    Private Function CreateLookups(Optional prompt As Func(Of String, String, String) = Nothing,
                                   Optional confirm As Func(Of String, DialogResult) = Nothing) As PersistentLookups
        If prompt Is Nothing Then
            prompt = Function(id As String, current As String)
                         Return ThrowUnexpectedPrompt(id)
                     End Function
        End If
        If confirm Is Nothing Then
            confirm = Function(warning As String)
                          Return ThrowUnexpectedConfirmation(warning)
                      End Function
        End If
        Return New PersistentLookups(folder:=_folder, prompt, confirm)
    End Function

    <Fact>
    Public Sub BuiltInLookups_DistinguishFamiliesAndNormalizeFlexKeys()
        Dim prompt As Func(Of String, String, String) =
            Function(id As String, current As String)
                Return "legacy one"
            End Function
        Dim lookups As PersistentLookups = Me.CreateLookups(prompt)
        Dim savedCulture As CultureInfo = CultureInfo.CurrentCulture
        Try
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name:="fr-FR")
            Assert.Equal(expected:="legacy one",
                         actual:=lookups.ErrorLookup(faultId:="1").Message)
            Assert.Contains(expectedSubstring:="Urgent low sensor glucose",
                            actualString:=lookups.ErrorLookup(faultId:="1.0").Message)
            Assert.Contains(expectedSubstring:="Urgent low sensor glucose",
                            actualString:=lookups.ErrorLookup(faultId:="1.000").Message)
            Assert.Equal(expected:="legacy one",
                         actual:=Me.CreateLookups().ErrorLookup(faultId:="1").Message)
            Assert.Contains(expectedSubstring:="Unknown",
                            actualString:=lookups.ErrorLookup(faultId:="1.0500").Message)
            Assert.Contains(expectedSubstring:="Pump Error",
                            actualString:=lookups.ErrorLookup(faultId:="2").Message)
            Assert.Contains(expectedSubstring:="Reservoir Low",
                            actualString:=lookups.ErrorLookup(faultId:="2.5").Message)
        Finally
            CultureInfo.CurrentCulture = savedCulture
        End Try
    End Sub

    <Fact>
    Public Sub CancelledPrompt_DoesNotSaveAndMayBeRetried()
        Dim count As Integer = 0
        Dim lookups As PersistentLookups = Me.CreateLookups(
            prompt:=Function(id As String, current As String)
                        count += 1
                        Return If(count = 1, "", "Saved (sg)")
                    End Function)
        Assert.Null([object]:=lookups.ErrorLookup(faultId:="9000"))
        Assert.False(condition:=File.Exists(path:=Path.Combine(_folder, "LegacyErrors.json")))
        Assert.Equal(expected:="Saved (sg)",
                     actual:=lookups.ErrorLookup(faultId:="9000").Message)
        Assert.Equal(expected:="Saved (sg)",
                     actual:=Me.CreateLookups().ErrorLookup(faultId:="9000").Message)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If Directory.Exists(path:=_folder) Then
            For Each name As String In New String() {"FlexErrors.json",
                                                     "LegacyErrors.json",
                                                     "FlexErrors.json.tmp",
                                                     "LegacyErrors.json.tmp"}
                Dim filePath As String = Path.Combine(_folder, name)
                If File.Exists(path:=filePath) Then
                    File.Delete(path:=filePath)
                End If
            Next
            Directory.Delete(path:=_folder)
        End If
        GC.SuppressFinalize(obj:=Me)
    End Sub

    <Fact>
    Public Sub DuplicateNormalizedKeys_AreRejected()
        Directory.CreateDirectory(path:=_folder)
        File.WriteAllText(path:=Path.Combine(_folder, "FlexErrors.json"),
                          contents:="{""1.05"":""a"",""1.050"":""b""}")
        Assert.Throws(Of InvalidDataException)(testCode:=Function() Me.CreateLookups())
    End Sub

    <Fact>
    Public Sub Merge_UsesCurrentValuesAndKeepsUnknownOrMissingPlaceholders()
        Dim lookups As PersistentLookups = Me.CreateLookups(
            prompt:=Function(id As String, current As String) "(deliveredAmount) (futureValue) (unitsRemaining)",
            confirm:=Function(warning As String) DialogResult.Yes)
        Dim message As String = Nothing
        Dim data As New Dictionary(Of String, String)(Comparer) From {
            {"AdditionalInfo", "{""deliveredAmount"":""1.2""}"}}

        Assert.True(condition:=TranslateNotificationMessageId(jsonDictionary:=data,
                                                              faultId:="9000",
                                                              message,
                                                              lookups))
        Assert.Equal(expected:="1.2 (futureValue) (unitsRemaining)",
                     actual:=message)
        data(key:="AdditionalInfo") = "{""deliveredAmount"":""2.4"",""unitsRemaining"":""10""}"
        Assert.True(condition:=TranslateNotificationMessageId(jsonDictionary:=data,
                                                              faultId:="9000",
                                                              message,
                                                              lookups))
        Assert.Equal(expected:="2.4 (futureValue) 10",
                     actual:=message)
        data.Remove(key:="AdditionalInfo")
        data.Add(key:="AdditionalInfo:deliveredAmount",
                 value:="(unitsRemaining)")
        data.Add(key:="AdditionalInfo:unitsRemaining",
                 value:="12")
        Assert.True(condition:=TranslateNotificationMessageId(jsonDictionary:=data,
                                                              faultId:="9000",
                                                              message,
                                                              lookups))
        Assert.Equal(expected:="(unitsRemaining) (futureValue) 12",
                     actual:=message)
    End Sub

    <Theory>
    <InlineData(DialogResult.Yes)>
    <InlineData(DialogResult.Cancel)>
    Public Sub NewMessage_UnknownPlaceholderCanBeSavedOrCancelled(choice As DialogResult)
        Dim lookups As PersistentLookups = Me.CreateLookups(
            prompt:=Function(id As String, current As String) "Value (futureValue)",
            confirm:=Function(warning As String) choice)
        Dim result As PersistentLookups.NotificationTemplate =
            lookups.ErrorLookup(faultId:="9000")

        If choice = DialogResult.Yes Then
            Assert.Equal(expected:="Value (futureValue)",
                         actual:=result.Message)
            Assert.Equal(expected:=result.Message,
                         actual:=Me.CreateLookups().ErrorLookup(faultId:="9000").Message)
        Else
            Assert.Null([object]:=result)
            Assert.False(condition:=File.Exists(path:=Path.Combine(_folder, "LegacyErrors.json")))
        End If
    End Sub

    <Fact>
    Public Sub NewMessage_WarnsAndAllowsEditThenSave()
        Dim prompts As New List(Of String)()
        Dim warnings As New List(Of String)()
        Dim lookups As PersistentLookups = Me.CreateLookups(
            prompt:=Function(id As String, current As String)
                        prompts.Add(item:=current)
                        Return If(prompts.Count = 1,
                                  "(futureValue) (> 30 minutes)",
                                  "(sg) (units)")
                    End Function,
            confirm:=Function(warning As String)
                         warnings.Add(item:=warning)
                         Return DialogResult.No
                     End Function)
        Dim template As PersistentLookups.NotificationTemplate =
            lookups.ErrorLookup(faultId:="9.123")

        Assert.Equal(expected:="(sg) (units)",
                     actual:=template.Message)
        Assert.Equal(expected:=s_expectedPlaceholders,
                     actual:=template.Placeholders)
        Assert.Equal(expected:=s_expectedPrompts,
                     actual:=prompts)
        Assert.Single(collection:=warnings)
        Assert.Contains(expectedSubstring:="futureValue",
                        actualString:=warnings(index:=0))
        Assert.DoesNotContain(expectedSubstring:="30 minutes",
                              actualString:=warnings(index:=0))
        Assert.Equal(expected:=template.Message,
                     actual:=Me.CreateLookups().ErrorLookup(faultId:="9.1230").Message)
    End Sub

    <Fact>
    Public Sub Translate_CancelledAndNonnumericIds_ReturnFalse()
        Dim lookups As PersistentLookups = Me.CreateLookups(
            prompt:=Function(id As String, current As String) String.Empty)
        Dim data As New Dictionary(Of String, String)()
        Dim message As String = String.Empty

        Assert.False(condition:=TranslateNotificationMessageId(jsonDictionary:=data,
                                                               faultId:="9000",
                                                               message,
                                                               lookups))
        Assert.False(condition:=TranslateNotificationMessageId(jsonDictionary:=data,
                                                               faultId:="BC_SID_MAX_FILL_DROPS_QUESITION",
                                                               message,
                                                               lookups))
        Assert.Equal(expected:=String.Empty,
                     actual:=message)
    End Sub

    <Fact>
    Public Sub UnrepresentableFlexId_IsRejectedRatherThanRounded()
        Dim lookups As PersistentLookups = Me.CreateLookups()
        Dim testCode As Func(Of Object) =
                Function()
                    Return lookups.ErrorLookup(faultId:="1.00000000000000000000000000001")
                End Function
        Assert.Throws(Of FormatException)(testCode)
    End Sub

    <Fact>
    Public Sub UserMessages_ArePersistedAndBuiltInsTakePrecedence()
        Directory.CreateDirectory(path:=_folder)
        File.WriteAllText(path:=Path.Combine(_folder, "FlexErrors.json"),
                          contents:="{""1.0"":""old built-in"",""9.500"":""saved (sg)""}")
        Dim lookups As PersistentLookups = Me.CreateLookups(
            prompt:=Function(id As String, current As String) "new (deliveredAmount)")

        Assert.Contains(expectedSubstring:="Urgent low sensor glucose",
                        actualString:=lookups.ErrorLookup(faultId:="1.00").Message)
        Assert.Equal(expected:="saved (sg)",
                     actual:=lookups.ErrorLookup(faultId:="9.5").Message)
        Assert.Equal(expected:="new (deliveredAmount)",
                     actual:=lookups.ErrorLookup(faultId:="9000").Message)
        Assert.Equal(expected:="new (deliveredAmount)",
                     actual:=lookups.ErrorLookup(faultId:="9001").Message)
        Dim reloaded As PersistentLookups = Me.CreateLookups()
        Assert.Equal(expected:="new (deliveredAmount)",
                     actual:=reloaded.ErrorLookup(faultId:="9000").Message)
        Assert.Equal(expected:="new (deliveredAmount)",
                     actual:=reloaded.ErrorLookup(faultId:="9001").Message)
        Assert.Equal(expected:="saved (sg)",
                     actual:=reloaded.ErrorLookup(faultId:="9.5000").Message)
    End Sub

End Class
