' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

Public Module FileHelpers

    Friend Function GetTestDataPath(<CallerFilePath> Optional path As String = "") As String
        ' Robust lookup for TestData:
        ' 1) Walk up from the caller file and return the first <dir>\TestData found.
        ' 2) If "Tests" or "*.Tests" is encountered, prefer its sibling TestData.
        ' 3) If a solution file (*.slnx) is found, treat that directory as repo root and probe common locations.
        ' 4) Fall back to the previous behavior.
        Try
            Dim fileDirectory As DirectoryInfo = New FileInfo(path).Directory
            Dim current As DirectoryInfo = fileDirectory
            Dim repoRoot As DirectoryInfo = Nothing

            While current IsNot Nothing
                Dim candidate As String = IO.Path.Combine(current.FullName, "TestData")
                If IO.Directory.Exists(candidate) Then
                    Return candidate
                End If

                If String.Equals(current.Name, "Tests", StringComparison.OrdinalIgnoreCase) OrElse _
                   current.Name.EndsWith(".Tests", StringComparison.OrdinalIgnoreCase) Then
                    Dim siblingParent As DirectoryInfo = current.Parent
                    If siblingParent IsNot Nothing Then
                        Dim siblingCandidate As String = IO.Path.Combine(siblingParent.FullName, "TestData")
                        If IO.Directory.Exists(siblingCandidate) Then
                            Return siblingCandidate
                        End If
                    End If
                End If

                Try
                    Dim slnxFiles As FileInfo() = current.GetFiles("*.slnx")
                    If slnxFiles IsNot Nothing AndAlso slnxFiles.Length > 0 Then
                        repoRoot = current
                        Exit While
                    End If
                Catch
                    ' Ignore and continue walking up
                End Try

                current = current.Parent
            End While

            If repoRoot IsNot Nothing Then
                Dim probes As String() = {
                    IO.Path.Combine(repoRoot.FullName, "TestData"),
                    IO.Path.Combine(repoRoot.FullName, "tests", "TestData"),
                    IO.Path.Combine(repoRoot.FullName, "src", "TestProject", "TestData"),
                    IO.Path.Combine(repoRoot.FullName, "src", "TestProject", "Tests", "TestData")
                }

                For Each probe As String In probes
                    If IO.Directory.Exists(probe) Then
                        Return probe
                    End If
                Next
            End If
        Catch ex As Exception
            ' Ignore and fall through to fallback
        End Try

        ' Fallback: use the direct parent of the caller file (previous behavior)
        Dim parent As DirectoryInfo = IO.Directory.GetParent(path)
        Return If(parent Is Nothing,
                  String.Empty,
                  IO.Path.Combine(parent.FullName, "TestData"))
    End Function

End Module
