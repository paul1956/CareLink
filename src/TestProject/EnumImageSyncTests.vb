' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports Xunit
Imports FluentAssertions
Imports CareLink

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
            Directory.GetFiles(path:=imageDirectory) _
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

        ' 4. Find exactly what is missing using case-insensitive Except
        Dim missingFiles As List(Of String) =
            enumDescriptions.Except(second:=filesInDir,
                                    comparer:=StringComparer.OrdinalIgnoreCase).ToList()
        Dim missingEnums As List(Of String) =
            filesInDir.Except(second:=enumDescriptions,
                              comparer:=StringComparer.OrdinalIgnoreCase).ToList()


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
