' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PdfDeviceInfo

    Public Sub New(deviceFamily As String, deviceModel As String)
        Me.Family = deviceFamily
        Me.Model = deviceModel
    End Sub

    ''' <summary>
    '''  The major device family like MiniMed Flex or MiniMed 780G
    ''' </summary>
    Public Property Family As String

    ''' <summary>
    '''  The device model like MMT-XXXX
    ''' </summary>
    Public Property Model As String

End Class
