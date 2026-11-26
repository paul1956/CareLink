' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.ExceptionServices
Imports System.Text.Json
Imports System.Text.Json.Nodes
Imports System.Threading
Imports System.Windows.Forms
Imports CareLink
Imports Xunit

<Collection("Sequential")>
Public Class ExceptionReportTests

    <Fact>
    Public Sub CleanPatientData_RedactsRepeatedPropertiesWithoutChangingOtherValues()
        Dim previousPatientData As JsonElement = PatientDataElement
        Try
            Const json As String = "{""firstName"":""firstName"",""other"":""firstName""," &
                                   """items"":[{""FIRSTNAME"":""a,b""},{""firstName"":""last""}]}"
            Const expected As String = "{""firstName"":""First"",""other"":""firstName""," &
                                       """items"":[{""FIRSTNAME"":""First""},{""firstName"":""First""}]}"
            PatientDataElement = json.FromJson(Of JsonElement)()

            Dim actual As String = CleanPatientData()

            Assert.True(condition:=JsonNode.DeepEquals(JsonNode.Parse(json:=expected), JsonNode.Parse(json:=actual)))
        Finally
            PatientDataElement = previousPatientData
        End Try
    End Sub

    <Fact>
    Public Sub ReportFile_RoundTripsExceptionStackTraceAndCleanPatientData()
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Dim previousPatientData As JsonElement = PatientDataElement
                Dim path As String =
                    IO.Path.Combine(IO.Path.GetTempPath(), $"CareLink-report-{Guid.NewGuid():N}.txt")
                Try
                    Dim sample As String = File.ReadAllText(path:=FileIoHelpers.GetTestDataPath())
                    Using document As JsonDocument = JsonDocument.Parse(json:=sample)
                        PatientDataElement = document.RootElement.Clone()
                    End Using

                    Const exceptionText As String = "Sample exception"
                    Dim stackTraceText As String = $"First stack frame{Environment.NewLine}Second stack frame"
                    Dim expected As JsonNode = JsonNode.Parse(json:=sample)
                    expected(propertyName:="firstName") = JsonValue.Create(value:="First")
                    expected(propertyName:="lastName") = JsonValue.Create(value:="Last")
                    expected(propertyName:="conduitSerialNumber") = JsonValue.Create(value:=Guid.Empty.ToString())
                    expected(propertyName:="medicalDeviceInformation")(propertyName:="systemId") =
                        JsonValue.Create(value:="40000000000 0000")
                    expected(propertyName:="medicalDeviceInformation")(propertyName:="deviceSerialNumber") =
                        JsonValue.Create(value:="NG4000000H")
                    Dim expectedJson As String = CleanPatientData()
                    ExceptionHandlerDialog.CreateReportFile(exceptionText, stackTraceText, withPath:=path)

                    Using dialog As New ExceptionHandlerDialog(),
                          exceptionBox As New TextBox(),
                          traceBox As New TextBox()

                        Dim actualJson As String = dialog.ReportFile(exTextBox:=exceptionBox,
                                                                    traceTextBox:=traceBox,
                                                                    reportNameWithPath:=path)
                        Assert.Equal(expected:=exceptionText,
                                     actual:=exceptionBox.Text)
                        Assert.Equal(expected:=$"{stackTraceText}{Environment.NewLine}",
                                     actual:=traceBox.Text)
                        Assert.Equal(expected:=expectedJson,
                                     actual:=actualJson)

                        Dim loaded As JsonElement = actualJson.FromJson(Of JsonElement)()
                        Assert.Equal(expected:="First",
                                     actual:=loaded.GetProperty(propertyName:="firstName").GetString())
                        Assert.Equal(expected:="Last",
                                     actual:=loaded.GetProperty(propertyName:="lastName").GetString())

                        Dim expected1 As String =
                            PatientDataElement.GetProperty(propertyName:="medicalDeviceFamily").GetString()
                        Assert.Equal(expected:=expected1,
                                     actual:=loaded.GetProperty(propertyName:="medicalDeviceFamily").GetString())
                        Assert.True(condition:=JsonNode.DeepEquals(expected, JsonNode.Parse(json:=loaded.GetRawText())))

                        ' A truncated payload must not be silently repaired by appending a brace.
                        Dim report As String = File.ReadAllText(path)
                        Assert.EndsWith(expectedEndString:="}", actualString:=report.TrimEnd())
                        Dim closingBrace As Integer = report.LastIndexOf(value:="}"c)
                        File.WriteAllText(path, contents:=report.Remove(startIndex:=closingBrace, count:=1))
                        Dim truncatedJson As String = dialog.ReportFile(exTextBox:=exceptionBox,
                                                                        traceTextBox:=traceBox,
                                                                        reportNameWithPath:=path)
                        Assert.ThrowsAny(Of JsonException)(testCode:=Sub() JsonDocument.Parse(json:=truncatedJson))
                    End Using
                Catch ex As Exception
                    failure = ex
                Finally
                    PatientDataElement = previousPatientData
                    If File.Exists(path) Then
                        File.Delete(path)
                    End If
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

End Class
