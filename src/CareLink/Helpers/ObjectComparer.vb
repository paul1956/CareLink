' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection

Public Module ObjectComparer

    Public Function FindDifferences(Of T)(first As T, second As T) _
        As List(Of PropertyDifference)

        Dim differences As New List(Of PropertyDifference)

        If first Is Nothing AndAlso second Is Nothing Then
            Return differences
        End If

        If first Is Nothing OrElse second Is Nothing Then
            Dim item As New PropertyDifference With {
                .Name = "<Object>",
                .Value1 = first,
                .Value2 = second}
            differences.Add(item)
            Return differences
        End If

        For Each prop As PropertyInfo In GetType(T).GetProperties()
            ' Ignore properties that cannot be read or need index arguments.
            If Not prop.CanRead OrElse prop.GetIndexParameters().Length > 0 Then Continue For

            Dim value1 As Object = prop.GetValue(obj:=first)
            Dim value2 As Object = prop.GetValue(obj:=second)

            If Not Equals(value1, value2) Then
                differences.Add(New PropertyDifference With {
                    .Name = prop.Name,
                    .Value1 = value1,
                    .Value2 = value2})
            End If
        Next

        Return differences
    End Function

End Module
