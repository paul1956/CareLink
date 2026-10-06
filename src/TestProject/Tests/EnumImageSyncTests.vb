' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class EnumImageSyncTests

    <Fact>
    Public Sub Every_Enum_Description_Should_Have_A_Matching_Image_File_And_Vice_Versa()
        ' 1. Arrange paths
        Dim imageDirectory As String =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images")

        ' Ensure the folder actually exists before scanning
        Directory.Exists(path:=imageDirectory) _
                 .Should() _
                 .BeTrue(because:=$"the image directory should exist at {imageDirectory}")

        ' 2. Get file names from directory (lowercase, no extension)
        Dim allowedExtensions As String() = {".png"}
        Dim predicate As Func(Of String, Boolean) =
            Function(f)
                Dim value As String = Path.GetExtension(path:=f).ToLower()
                Return allowedExtensions.Contains(value)
            End Function

        Dim selector As Func(Of String, String) =
            Function(f As String)
                Return Path.GetFileNameWithoutExtension(path:=f).ToLower()
            End Function

        Dim filesInDir As List(Of String) =
            Directory.GetFiles(path:=imageDirectory,
                               searchPattern:="*.png",
                               searchOption:=SearchOption.TopDirectoryOnly) _
                     .Where(predicate) _
                     .Select(selector) _
                     .ToList()

        ' 3. Get Description attributes from Enum (lowercase)
        Dim enumType As Type = GetType(ImageEnum) ' Replace with your actual Enum

        Dim enumDescriptions As List(Of String) =
            [Enum].GetValues(enumType) _
                .Cast(Of [Enum])() _
                .Select(selector:=
                    Function(enumVal)
                        Return GetEnumDescription(enumVal).ToLower()
                    End Function) _
                .ToList()

        ' 4. Also gather files from any downloaded bundles (manifests) and treat them as valid
        '    sources for enum images. Manifest files live in the user's Documents\CareLink folder.
        Dim bundleFiles As New HashSet(Of String)(Comparer)
        Try
            Dim manifestFolder As String =
                Path.Combine(Environment.GetFolderPath(folder:=Environment.SpecialFolder.MyDocuments), "CareLink")
            If Directory.Exists(path:=manifestFolder) Then
                Dim strings As String() =
                    Directory.GetFiles(path:=manifestFolder,
                                       searchPattern:="*.manifest.json",
                                       searchOption:=SearchOption.TopDirectoryOnly)

                For Each mf As String In strings
                    Try
                        Dim manifest As IconBundleManifest = Nothing
                        Dim loaded As Boolean = IconBundleManifest.TryLoadFromFile(path:=mf, manifest:=manifest)
                        If Not loaded OrElse manifest Is Nothing Then
                            Continue For
                        End If

                        If manifest.files IsNot Nothing Then
                            For Each rel As String In manifest.files
                                If String.IsNullOrEmpty(rel) Then Continue For
                                Dim nameOnly As String =
                                    Path.GetFileNameWithoutExtension(path:=rel).ToLower()
                                bundleFiles.Add(item:=nameOnly)
                            Next
                        End If
                    Catch
                        ' ignore malformed manifest
                    End Try
                Next
            End If
        Catch
            ' ignore any IO errors reading manifests
        End Try

        ' 4. Find exactly what is missing using case-insensitive Except
        '    An Enum description is considered present if it exists in the embedded Images
        '    folder OR in any downloaded bundle manifest.
        Dim combinedFiles As New HashSet(Of String)(filesInDir, TextComparisonConstants.Comparer)
        For Each bf As String In bundleFiles
            combinedFiles.Add(item:=bf)
        Next

        Dim missingFiles As List(Of String) =
            enumDescriptions.Except(second:=combinedFiles,
                                    Comparer).ToList()

        ' Files present in the embedded Images folder that do not
        ' correspond to any Enum description or enum name are still
        ' considered unexpected and reported as missingEnums.
        Dim enumNames As List(Of String) =
            [Enum].GetValues(enumType) _
                .Cast(Of [Enum])() _
                .Select(selector:=Function(enumVal As [Enum]) As String
                                      Return enumVal.ToString().ToLower()
                                  End Function) _
                .ToList()

        Dim validNames As New HashSet(Of String)(collection:=enumDescriptions, Comparer)
        For Each n As String In enumNames
            validNames.Add(item:=n)
        Next
        Dim missingEnums As List(Of String) =
            filesInDir.Except(second:=validNames,
                              Comparer).ToList()

        ' Files present in the embedded Images folder that also exist in any downloaded
        ' bundle manifest are allowed (they are considered backups). Filter those out
        ' from the unexpected list so the test only reports truly orphaned image files.
        If bundleFiles.Count > 0 Then
            missingEnums = missingEnums.Where(
                predicate:=Function(f As String) As Boolean
                               Return Not bundleFiles.Contains(item:=f)
                           End Function).ToList()
        End If

        ' 5. Assert with clean, explicit failure messages
        Const separator As String = ", "
        Dim because As String =
            "because the following Enum descriptions are missing their physical " &
            $"image files in the directory: {String.Join(separator, values:=missingFiles)}"
        missingFiles.Should().BeEmpty(because)

        because =
            "because the following image files exist in the directory but are " &
            $"missing a matching Enum description: {String.Join(separator, values:=missingEnums)}"

        missingEnums.Should().BeEmpty(because)
    End Sub

End Class
