' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class supportedReportRecord
    Public notFor As String
    Public onlyFor As String
    Public report As String

    Sub New(Values As Dictionary(Of String, String))
        If Values.Count <> 3 Then
            Throw New Exception($"{NameOf(supportedReportRecord)}({Values}) contains {Values.Count} entries, 3 expected.")
        End If
        report = Values(NameOf(report))
        onlyFor = Values(NameOf(onlyFor))
        notFor = Values(NameOf(notFor))

    End Sub

    Private Function GetDebuggerDisplay() As String
        Return report.ToString()
    End Function

End Class
