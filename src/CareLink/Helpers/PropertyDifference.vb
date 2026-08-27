' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PropertyDifference
    Public Property Name As String
    Public Property Value1 As Object
    Public Property Value2 As Object

    Private Shared Function FormatValue(value As Object) As String
        If value Is Nothing Then Return "<Nothing>"
        Return value.ToString()
    End Function

    Public Overrides Function ToString() As String
        Return $"{Me.Name}: [{FormatValue(value:=Me.Value1)}] <> [{FormatValue(value:=Me.Value2)}]"
    End Function

End Class
