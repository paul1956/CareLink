' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module PumpInfo

    ''' <summary>
    '''  Gets the display name of a pump model based on its model number.
    ''' </summary>
    ''' <param name="modelNumber">The model number of the pump.</param>
    ''' <returns>
    '''  The display name of the pump if recognized;
    '''  otherwise, "Unknown".
    ''' </returns>
    Public Function GetPumpName() As String
        Select Case PatientData.MedicalDeviceInformation.ModelNumber
            Case "MMT-1812"
                Return "MiniMed™ 740G--mg/dL"
            Case "MMT-1880"
                Return "MiniMed™ 770G"
            Case "MMT-1884"
                Return "MiniMed™ 780G-US Update"
            Case "MMT-1885"
                Return "MiniMed™ 780G-mmol/L"
            Case "MMT-1886"
                Return "MiniMed™ 780G-mg/dL"
            Case "MMT-8162"
                Return "MiniMed™ Flex-mg/dL"
            Case Else
                Return "Unknown"
        End Select
    End Function

    ''' <summary>
    '''  Get the pump Family
    ''' </summary>
    ''' <returns>
    '''  Returns <see langword="True"/> if pump is based on Flex;
    '''  Otherwise <see langword="False"/> assumes older version
    ''' </returns>
    Public Function IsFlex() As Boolean
        Return GetPumpName().ContainsNoCase(value:="Flex")
    End Function

End Module
