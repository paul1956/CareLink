' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module PersonalRemindersRecordExtensions

    <Extension>
    Public Sub InitializeFromRow(this As PersonalRemindersRecord, row As StringTable.Row, key As String)
        If row Is Nothing Then Return
        Try
            ' PersonalRemindersRecord only stores a Time string
            If row.Columns.Count > 0 Then
                this.Time = row.Columns(index:=0).Remove(s:=key).CleanSpaces
            End If
        Catch
        End Try
    End Sub

End Module
