Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
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
        If File.Exists(_graphColorsPath) Then
            File.Copy(_graphColorsPath, _backupPath, overwrite:=True)
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
        Dim baseKnown As KnownColor = GraphColorDictionary("Suspend")
        Dim baseColor As Color = baseKnown.ToColor()

        ' Act
        Dim c As Color = GetGraphLineColor("Suspend")

        ' Assert
        c.A.Should().Be(128)
        c.R.Should().Be(baseColor.R)
        c.G.Should().Be(baseColor.G)
        c.B.Should().Be(baseColor.B)
    End Sub

    <Fact>
    Public Sub GetGraphLineColor_ForOtherKey_ReturnsOpaqueBaseColor()
        ' Act
        Dim c As Color = GetGraphLineColor("Active Insulin")

        ' Assert
        c.A.Should().Be(255)
        c.Should().Be(GraphColorDictionary("Active Insulin").ToColor())
    End Sub

    <Fact>
    Public Sub UpdateColorDictionary_And_GetColorDictionaryFromFile_WriteToFile_Workflow()
        ' Arrange - create a minimal CSV that changes one color
        Directory.CreateDirectory(Path.GetDirectoryName(_graphColorsPath))
        Using sw As New StreamWriter(_graphColorsPath, append:=False)
            sw.WriteLine("Key,ForegroundColor,BackgroundColor")
            sw.WriteLine("Active Insulin,Black,White")
        End Using

        ' Act - load file which should update only the existing key
        GetColorDictionaryFromFile()

        ' Assert updated value
        GraphColorDictionary("Active Insulin").Should().Be(KnownColor.Black)

        ' Act - update value programmatically and write back to file
        UpdateColorDictionary("Active Insulin", KnownColor.Lime)
        WriteColorDictionaryToFile()

        ' Assert file contains the updated KnownColor name for the key
        Dim text As String = File.ReadAllText(_graphColorsPath)
        text.Should().Contain("Active Insulin,Lime,")
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' CA1816 requires calling GC.SuppressFinalize in Dispose
        GC.SuppressFinalize(Me)

        ' Restore in-memory dictionary
        GraphColorDictionary.Clear()
        For Each kvp As KeyValuePair(Of String, KnownColor) In _originalDictionary
            GraphColorDictionary.Add(kvp.Key, kvp.Value)
        Next

        ' Restore or remove on-disk file
        Try
            If _backupMade AndAlso File.Exists(_backupPath) Then
                File.Copy(_backupPath, _graphColorsPath, overwrite:=True)
                File.Delete(_backupPath)
            ElseIf File.Exists(_graphColorsPath) Then
                File.Delete(_graphColorsPath)
            End If
        Catch
            ' Best effort - ignore errors
        End Try
    End Sub
End Class
