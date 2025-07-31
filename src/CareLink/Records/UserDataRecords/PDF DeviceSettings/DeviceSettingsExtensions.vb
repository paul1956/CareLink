' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DeviceSettingsExtensions

    <Extension>
    Friend Function GetSingleLineValue(Of T)(sTable As StringTable, key As String) As T
        Dim typeOfT As Type = GetType(T)
        For Each r As StringTable.Row In sTable.Rows
            Dim v As String = r.Columns(index:=0)
            If v.StartsWith(value:=key) Then
                Dim value As String = v.Remove(key).Trim
                If value = v Then
                    value = r.Columns(1)
                End If
                If typeOfT Is GetType(String) Then
                    Return CType(CObj(value), T)
                End If
                value = value.Split(" ", StringSplitOptions.RemoveEmptyEntries)(0)
                If typeOfT Is GetType(Single) Then
                    value = value.Replace(","c, CareLinkDecimalSeparator)
                    Return If(IsNumeric(value),
                              CType(CObj(value), T),
                              CType(CObj(Single.NaN), T)
                             )
                End If
                If typeOfT Is GetType(Integer) Then
                    Return If(IsNumeric(value),
                              CType(CObj(value), T),
                              CType(CObj(0), T)
                             )
                End If
                If typeOfT Is GetType(TimeOnly) Then
                    Dim timeOnly As TimeOnly = Nothing
                    Return If(TimeOnly.TryParse(value, result:=timeOnly),
                              CType(CObj(timeOnly), T),
                              CType(CObj(Eleven59), T)
                             )
                End If
                If typeOfT Is GetType(TimeSpan) Then
                    Dim timeSpan As TimeSpan = Nothing
                    Return If(TimeSpan.TryParse(value, result:=timeSpan),
                              CType(CObj(timeSpan), T),
                              CType(CObj(ZeroTickSpan), T)
                             )
                End If
                If typeOfT Is GetType(Boolean) Then
                    Dim bol As Boolean = Nothing
                    Return If(Boolean.TryParse(value, result:=bol),
                              CType(CObj(bol), T),
                              CType(CObj(False), T)
                             )
                End If
                Stop
            End If
        Next
        Stop
        If typeOfT Is GetType(String) Then
            Return CType(CObj(""), T)
        End If
        If typeOfT Is GetType(Single) Then
            Return CType(CObj(Single.NaN), T)
        End If
        If typeOfT Is GetType(Integer) Then
            Return CType(CObj(0), T)
        End If
        If typeOfT Is GetType(TimeOnly) Then
            Return Nothing
        End If
        If typeOfT Is GetType(TimeSpan) Then
            Return Nothing
        End If
        If typeOfT Is GetType(Boolean) Then
            Return CType(CObj(False), T)
        End If
        Stop
        Return Nothing
    End Function

    <Extension>
    Public Function EqualCarbRatios(deviceCarbRatios As List(Of DeviceCarbRatioRecord), carbRatios As List(Of CarbRatioRecord)) As Boolean

        Dim toCarbRatioList As List(Of CarbRatioRecord) = deviceCarbRatios.ToCarbRatioList
        If toCarbRatioList.Count <> carbRatios.Count Then
            Return False
        End If
        For Each e As IndexClass(Of CarbRatioRecord) In toCarbRatioList.WithIndex
            Dim deviceCarbRatio As CarbRatioRecord = e.Value
            If Not deviceCarbRatio.Equals(carbRatios(e.Index)) Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension>
    Public Function ToCarbRatioList(deviceCarbRatios As List(Of DeviceCarbRatioRecord)) As List(Of CarbRatioRecord)
        Dim carbRatios As New List(Of CarbRatioRecord)
        For Each e As IndexClass(Of DeviceCarbRatioRecord) In deviceCarbRatios.WithIndex
            Dim deviceCarbRatio As DeviceCarbRatioRecord = e.Value
            carbRatios.Add(New CarbRatioRecord With {
                            .StartTime = deviceCarbRatio.Time,
                            .CarbRatio = deviceCarbRatio.Ratio,
                            .EndTime = If(e.IsLast,
                                          Eleven59,
                                          deviceCarbRatios(e.Index + 1).Time
                                         )
            })
        Next
        Return carbRatios
    End Function

End Module
