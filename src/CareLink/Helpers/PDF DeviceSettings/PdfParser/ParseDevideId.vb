' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ParseDeviceModel

    Private Const ComparisonType As StringComparison =
        StringComparison.OrdinalIgnoreCase

    Friend Function GetDeviceFamilyAndModel(pageText As String) As PdfDeviceInfo
        ' Attempt to extract a more complete device model / data source string from page2
        Dim deviceFamily As String
        Dim deviceModel As String = String.Empty
        Try
            ' Prefer an explicit "Data source(s)" header when present
            Dim dsIndex As Integer = pageText.IndexOf(value:="Data source", ComparisonType)
            If dsIndex >= 0 Then
                ' Move to the end of the header and take the remainder of that line
                Dim afterHeader As String = pageText.Substring(startIndex:=dsIndex)
                ' Now just take the first line after the header
                Dim line As String = afterHeader.SplitLines(Trim:=True)(index:=0)
                ' Find the first colon following the header ("Data sources - ...")
                Dim miniMedIndex As Integer = line.IndexOf(value:="MiniMed")
                If miniMedIndex >= 0 Then
                    line = line.Substring(startIndex:=miniMedIndex).TrimStart
                End If
                ' Take up to the end of the first line
                Dim endOfLine As Integer = line.IndexOfAny(anyOf:={ControlChars.Cr, ControlChars.Lf, "("c})
                If endOfLine >= 0 Then
                    line = line.Substring(startIndex:=0, length:=endOfLine)
                End If
                Dim splitdeviceInfo As String() = line.Trim().Split(separator:=",")
                deviceFamily = splitdeviceInfo(0)
                If splitdeviceInfo.Length > 0 Then
                    deviceModel = splitdeviceInfo(1).Trim
                End If
            Else
                ' Fallback: search for "Flex"
                deviceModel = String.Empty
                deviceFamily =
                    If(pageText.Contains(value:="Flex", ComparisonType),
                       "MiniMed Flex",
                       "MiniMed 780G")
            End If
        Catch
            deviceFamily = "MiniMed Flex"
        End Try

        Return New PdfDeviceInfo(deviceFamily, deviceModel)
    End Function

End Module
