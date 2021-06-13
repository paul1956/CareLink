' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module SupportFunctions

    Public Enum RoundTo
        Second
        Minute
        Hour
        Day
    End Enum

    <Extension>
    Friend Sub Adjustlist(myList As List(Of Insulin), startIndex As Integer, count As Integer)
        For i As Integer = startIndex To startIndex + count
            If i >= myList.Count Then Exit Sub
            myList(i) = myList(i).Adjust()
        Next
    End Sub

    <Extension>
    Friend Function ConditionalSum(myList As List(Of Insulin), start As Integer, length As Integer) As Double
        If start + length > myList.Count Then
            length = myList.Count - start
        End If
        Return myList.GetRange(start, length).Sum(Function(i As Insulin) i.CurrentInsulinLevel)
    End Function

    <Extension>
    Friend Function GetDecimalValue(item As Dictionary(Of String, String), ParamArray values() As String) As Double
        Dim returnValueString As String = ""
        For Each value As String In values
            If item.TryGetValue(value, returnValueString) Then
                Return Double.Parse(returnValueString)
            End If
        Next
        Return Double.NaN
    End Function

    <Extension>
    Friend Function GetMilitaryHour(selectedStartTime As String) As Integer
        Return CInt(Format(Date.Parse(selectedStartTime), "HH"))
    End Function

    <Extension>
    Friend Function RoundDouble(value As Double, decimalDigits As Integer) As Double

        Return Math.Round(value, decimalDigits)
    End Function

    <Extension>
    Friend Function RoundDouble(value As String, decimalDigits As Integer) As Double

        Return Math.Round(Double.Parse(value), decimalDigits)
    End Function

    <Extension>
    Friend Function RoundSingle(value As Double, decimalDigits As Integer) As Single

        Return CSng(Math.Round(value, decimalDigits))
    End Function

    Friend Sub SetValue(myList As List(Of KeyValuePair(Of Double, Double)), ByRef value As KeyValuePair(Of Double, Double))
        For Each v As IndexClass(Of KeyValuePair(Of Double, Double)) In myList.WithIndex()
            If Not v.IsLast Then
                If myList(v.Index).Key >= value.Key Then
                    Continue For
                End If
            End If
            myList(v.Index) = New KeyValuePair(Of Double, Double)(v.Value.Key, value.Value)
            value = myList(v.Index)
            Exit For
        Next
    End Sub

    <Extension()>
    Friend Function ToDisplay(d As SortedDictionary(Of Double, Double)) As Dictionary(Of String, Double)
        Dim result As New Dictionary(Of String, Double)
        For Each entry As KeyValuePair(Of Double, Double) In d
            result.Add(Date.FromOADate(entry.Key).ToLongTimeString(), Math.Round(entry.Value, 3))
        Next
        Return result
    End Function

    <Extension>
    Public Function RoundDown(d As Date, rt As RoundTo) As Date
        Dim dtRounded As New DateTime()

        Select Case rt
            Case RoundTo.Second
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second)
            Case RoundTo.Minute
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0)
            Case RoundTo.Hour
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0)
            Case RoundTo.Day
                dtRounded = New DateTime(d.Year, d.Month, d.Day, 0, 0, 0)
        End Select

        Return dtRounded
    End Function

    <Extension>
    Public Function SafeGetSgDateTime(sgList As List(Of Dictionary(Of String, String)), index As Integer) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            index -= 1
        End If
        If sgList(index).TryGetValue("previousDateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString)
        ElseIf sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString)
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString.Split("-")(0))
        Else
            sgDateTime = Now
        End If
        If sgDateTime.Year = 2000 Then
            sgDateTime = Date.Now - ((sgList.Count - index) * Form1._FiveMinutes)
        End If
        If sgList(index).Count < 7 Then
            sgDateTime = sgDateTime.AddMinutes(5)
        End If
        Return sgDateTime
    End Function

    <Extension()>
    Public Function ToSgList(innerJson As List(Of Dictionary(Of String, String))) As List(Of SgRecord)
        Dim sGs As New List(Of SgRecord)
        For i As Integer = 0 To innerJson.Count - 1
            sGs.Add(New SgRecord(innerJson, i))
        Next
        Return sGs
    End Function
End Module
