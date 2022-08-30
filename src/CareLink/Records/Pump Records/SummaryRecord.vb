' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SummaryRecord

    Protected Friend Sub New(index As ItemIndexs, entry As KeyValuePair(Of String, String))
        Me.RecordNumber = index
        Me.Key = entry.Key
        Me.Value = entry.Value?.ToString(CurrentUICulture)
    End Sub

#If True Then ' Prevent reordering

    Public Property RecordNumber As Integer
    Public Property Key As String
    Public Property Value As String
#End If  ' Prevent reordering
End Class
