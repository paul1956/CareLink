' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
Public Class ColorDictionaryHelpersTests
    Implements IDisposable

    Private ReadOnly _graphColorsPath As String = GetGraphColorsFileNameWithPath()
    Private ReadOnly _backupPath As String = Path.Combine(Path.GetTempPath(), "GraphColors.Csv.bak")
    Private ReadOnly _originalDictionary As New Dictionary(Of String, KnownColor)(GraphColorDictionary)
    Private ReadOnly _backupMade As Boolean

    Public Sub New()
        ' Backup existing file if present
        If File.Exists(path:=_graphColorsPath) Then
            File.Copy(sourceFileName:=_graphColorsPath, destFileName:=_backupPath, overwrite:=True)
            _backupMade = True
        End If
    End Sub

    <Fact>
    Public Sub GetColorDictionaryBindingSource_ReturnsBindingSourceWithDictionary()
        ' Act
        Dim bs As BindingSource = GetColorDictionaryBindingSource()

        ' Assert
        bs.Should().NotBeNull()
        CType(bs.DataSource, Dictionary(Of String, KnownColor)).Should().BeSameAs(GraphColorDictionary)
    End Sub

    <Fact>
    Public Sub GetGraphLineColor_ForSuspend_IsSemiTransparent()
        ' Arrange
        Dim baseKnown As KnownColor = GraphColorDictionary(key:="Suspend")
        Dim baseColor As Color = baseKnown.ToColor()

        ' Act
        Dim c As Color = GetGraphLineColor(key:="Suspend")

        ' Assert
        c.A.Should().Be(expected:=128)
        c.R.Should().Be(expected:=baseColor.R)
        c.G.Should().Be(expected:=baseColor.G)
        c.B.Should().Be(expected:=baseColor.B)
    End Sub

    <Fact>
    Public Sub GetGraphLineColor_ForOtherKey_ReturnsOpaqueBaseColor()
        ' Act
        Dim c As Color = GetGraphLineColor(key:="Active Insulin")

        ' Assert
        c.A.Should().Be(expected:=255)
        c.Should().Be(expected:=GraphColorDictionary(key:="Active Insulin").ToColor())
    End Sub

    <Fact>
    Public Sub UpdateColorDictionary_And_GetColorDictionaryFromFile_WriteToFile_Workflow()
        ' Arrange - create a minimal CSV that changes one color
        Directory.CreateDirectory(path:=Path.GetDirectoryName(_graphColorsPath))
        Using sw As New StreamWriter(path:=_graphColorsPath, append:=False)
            sw.WriteLine(value:="Key,ForegroundColor,BackgroundColor")
            sw.WriteLine(value:="Active Insulin,Black,White")
        End Using

        ' Act - load file which should update only the existing key
        GetColorDictionaryFromFile()

        ' Assert updated value
        GraphColorDictionary(key:="Active Insulin").Should().Be(expected:=KnownColor.Black)

        ' Act - update value programmatically and write back to file
        UpdateColorDictionary(key:="Active Insulin", item:=KnownColor.Lime)
        WriteColorDictionaryToFile()

        ' Assert file contains the updated KnownColor name for the key
        Dim text As String = File.ReadAllText(path:=_graphColorsPath)
        text.Should().Contain(expected:="Active Insulin,Lime,")
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' CA1816 requires calling GC.SuppressFinalize in Dispose
        GC.SuppressFinalize(obj:=Me)

        ' Restore in-memory dictionary
        GraphColorDictionary.Clear()
        For Each kvp As KeyValuePair(Of String, KnownColor) In _originalDictionary
            GraphColorDictionary.Add(kvp.Key, kvp.Value)
        Next

        ' Restore or remove on-disk file
        Try
            If _backupMade AndAlso File.Exists(path:=_backupPath) Then
                File.Copy(sourceFileName:=_backupPath, destFileName:=_graphColorsPath, overwrite:=True)
                File.Delete(path:=_backupPath)
            ElseIf File.Exists(path:=_graphColorsPath) Then
                File.Delete(path:=_graphColorsPath)
            End If
        Catch
            ' Best effort - ignore errors
        End Try
    End Sub

End Class
