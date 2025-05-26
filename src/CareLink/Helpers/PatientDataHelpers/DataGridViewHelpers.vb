' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module DataGridViewHelpers
    Private ReadOnly s_alignmentTable As New Dictionary(Of Type, Dictionary(Of String, DataGridViewCellStyle))

    Private ReadOnly s_columnsToHide As New Dictionary(Of Type, List(Of String)) From {
        {GetType(ActiveInsulin), New List(Of String) From {
            NameOf(ActiveInsulin.kind),
            NameOf(ActiveInsulin.Version)}
        },
        {GetType(AutoBasalDelivery), New List(Of String) From {NameOf(AutoBasalDelivery.OAdateTime)}},
        {GetType(AutoModeStatus), New List(Of String) From {
            NameOf(AutoModeStatus.Kind),
            NameOf(AutoModeStatus.Type)}
        },
        {GetType(BannerState), New List(Of String) From {}},
        {GetType(Basal), New List(Of String) From {}},
        {GetType(BgReading), New List(Of String) From {
            NameOf(BgReading.Kind),
            NameOf(BgReading.Type)}
        },
        {GetType(Calibration), New List(Of String) From {
            NameOf(Calibration.Kind),
            NameOf(Calibration.Type)}
        },
        {GetType(CareLinkUserDataRecord), New List(Of String) From {
            NameOf(CareLinkUserDataRecord.ID),
            NameOf(CareLinkUserDataRecord.CareLinkPassword)}
        },
        {GetType(Insulin), New List(Of String) From {
            NameOf(Insulin.Kind),
            NameOf(Insulin.OAdateTime),
            NameOf(Insulin.Type)}
        },
        {GetType(CurrentUserRecord), New List(Of String) From {}},
        {GetType(LastSG), New List(Of String) From {
            "RecordNumber",
            NameOf(LastSG.Kind),
            NameOf(LastSG.Version)}},
        {GetType(Limit), New List(Of String) From {
            NameOf(Limit.Kind),
            NameOf(Limit.Version)}},
        {GetType(LowGlucoseSuspended), New List(Of String) From {
            NameOf(LowGlucoseSuspended.Kind),
            NameOf(LowGlucoseSuspended.Type)}},
        {GetType(Meal), New List(Of String) From {
            NameOf(Meal.Kind),
            NameOf(Meal.Type)}},
        {GetType(SG), New List(Of String) From {
            NameOf(SG.Kind),
            NameOf(SG.OaDateTime),
            NameOf(SG.Version)}},
        {GetType(TherapyAlgorithmState), New List(Of String) From {}},
        {GetType(TimeChange), New List(Of String) From {
            NameOf(TimeChange.Kind),
            NameOf(TimeChange.Type)}}
    }

    ''' <summary>
    '''  Gets the <see cref="DataGridViewCellStyle"/> for a given column name and data type.
    ''' </summary>
    ''' <typeparam name="T">
    '''  The data record type.
    ''' </typeparam>
    ''' <param name="columnName">
    '''  The name of the column for which to retrieve the cell style.
    ''' </param>
    ''' <returns>
    '''  The <see cref="DataGridViewCellStyle"/> for the specified column.
    ''' </returns>
    Friend Function GetCellStyle(Of T As Class)(columnName As String) As DataGridViewCellStyle
        If Not s_alignmentTable.ContainsKey(GetType(T)) Then
            s_alignmentTable(GetType(T)) = New Dictionary(Of String, DataGridViewCellStyle)()
        End If
        Return ClassPropertiesToColumnAlignment(Of T)(s_alignmentTable(GetType(T)), columnName)
    End Function

    ''' <summary>
    '''  Sets the alignment for a specific column in a DataGridView.
    ''' </summary>
    ''' <typeparam name="T">
    '''  The data record type.
    ''' </typeparam>
    ''' <param name="columnName">
    '''  The name of the column to set the alignment for.
    ''' </param>
    ''' <param name="alignment">
    '''  The desired alignment for the column.
    ''' </param>
    Friend Function HideColumn(Of T)(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso
            Not String.IsNullOrWhiteSpace(dataPropertyName) AndAlso
            s_columnsToHide.ContainsKey(GetType(T)) AndAlso
            s_columnsToHide(GetType(T)).Contains(dataPropertyName)
    End Function

End Module
