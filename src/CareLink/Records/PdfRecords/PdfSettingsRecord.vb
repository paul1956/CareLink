' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PdfSettingsRecord

    ''' <summary>
    ''' Initializes an empty PdfSettingsRecord without parsing a file.
    ''' Use this constructor in tests or when you want to populate fields manually.
    ''' Visible to the test assembly via InternalsVisibleTo; not public to external consumers.
    ''' </summary>
    Friend Sub New()
        ' Intentionally do not perform parsing. Fields are left at their defaults.
    End Sub

    ''' <summary>
    ''' Whether parsing succeeded. Mutable within this class only.
    ''' </summary>
    Private ReadOnly _isValid As Boolean = False

    ''' <summary>
    '''  Initializes a new instance of the <see cref="PdfSettingsRecord"/> class
    '''  by extracting data from the specified PDF file.
    ''' </summary>
    ''' <param name="pdfFilePath">
    '''  The full path to the PDF file containing device settings.
    ''' </param>
    ''' <param name="notTesting">
    '''  If set to <see langword="True"/>, WinForms support is required so the cursor
    '''  will change to a wait cursor during processing.
    ''' </param>
    Public Sub New(pdfFilePath As String)
        ' Delegate parsing to the PdfSettingsParserRoot so this class remains a data holder.
        Try
            Parse(record:=Me, pdfFilePath)
        Catch
            ' If parsing fails, mark record invalid but keep object usable.
            _isValid = False
        End Try
    End Sub

    Public Property Basal As New PumpBasalRecord

    Public Property Bolus As New DeviceBolusRecord

    ''' <summary>
    '''  The detected device family (e.g., "MiniMed Flex").
    ''' </summary>
    Public Property DeviceFamily As String

    ''' <summary>
    '''  The detected device model (e.g., "MMT-????").
    ''' </summary>
    Public Property DeviceModel As String

    Public Property HighAlerts As New HighAlertsRecord

    Public ReadOnly Property IsValid As Boolean
        Get
            Return _isValid
        End Get
    End Property

    Public Property LowAlerts As New LowAlertsRecord

    Public Property Notes As New NotesRecord

    Public Property PresetBolus As New Dictionary(Of String, PresetBolusRecord) From {
                {"Bolus 1", New PresetBolusRecord},
                {"Breakfast", New PresetBolusRecord},
                {"Dinner", New PresetBolusRecord},
                {"Lunch", New PresetBolusRecord},
                {"Snack", New PresetBolusRecord},
                {"Bolus 2", New PresetBolusRecord},
                {"Bolus 3", New PresetBolusRecord},
                {"Bolus 4", New PresetBolusRecord}}

    Public Property PresetTemp As New Dictionary(Of String, PresetTempRecord) From {
                {"High Activity", New PresetTempRecord},
                {"Moderate Activity", New PresetTempRecord},
                {"Low Activity", New PresetTempRecord},
                {"Sick", New PresetTempRecord},
                {"Temp 1", New PresetTempRecord},
                {"Temp 2", New PresetTempRecord},
                {"Temp 3", New PresetTempRecord},
                {"Temp 4", New PresetTempRecord}
            }

    Public Property Reminders As New RemindersRecord()

    Public Property Sensor As New SensorRecord

    Public Property SmartGuard As New SmartGuardRecord

    Public Property UserName As String

    Public Property Utilities As New UtilitiesRecord

    Public Shared Sub GetSnoozeInfo(listOfAllTextLines As List(Of String),
                                    target As String,
                                    ByRef snoozeOn As String,
                                    ByRef snoozeTime As String)

        Dim snoozeLine As String
        snoozeOn = "Off"
        snoozeLine = listOfAllTextLines.FindLine(value:=target)
        Dim index As Integer = snoozeLine.IndexOf(value:=")"c)
        If index >= 0 Then
            snoozeLine = snoozeLine.Substring(startIndex:=0, length:=index + 1)
            index = snoozeLine.IndexOf(value:="Snooze ")
            snoozeLine = snoozeLine.Substring(startIndex:=index).Trim(trimChar:=")"c)
            Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
            Dim splitSnoozeLine As String() = snoozeLine.Split(separator:=" ", options)
            If splitSnoozeLine.Length = 2 Then
                snoozeOn = "On"
                snoozeTime = splitSnoozeLine(1)
            Else
                Stop
            End If
        Else
            Stop
        End If
    End Sub

End Class
