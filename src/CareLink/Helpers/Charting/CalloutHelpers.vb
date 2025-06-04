' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module CalloutHelpers
    ''' <summary>
    '''  Add or update an annotation for the last data point in the treatment chart.
    '''  If an annotation already exists for the last data point, it updates the text and position.
    '''  If no annotation exists, it creates a new CalloutAnnotation with the specified tag text.
    ''' </summary>
    ''' <param name="treatmentChart"></param>
    ''' <param name="lastDataPoint"></param>
    ''' <param name="tagText"></param>
    <Extension>
    Private Sub AddOrUpdateAnnotation(treatmentChart As Chart, lastDataPoint As DataPoint, tagText As String)
        Dim annotation As CalloutAnnotation = treatmentChart.FindAnnotation(lastDataPoint)
        With annotation
            If annotation IsNot Nothing Then
                Dim yValue As Double = lastDataPoint.YValues(0) - ((lastDataPoint.YValues(0) - annotation.AnchorDataPoint.YValues(0)) / 2)
                .Text = $"{tagText}{vbCrLf}{annotation.Text}"
                .AnchorDataPoint.SetValueXY(lastDataPoint.XValue, yValue)
                Exit Sub
            End If
        End With
        Dim item As New CalloutAnnotation With {
                    .Alignment = ContentAlignment.BottomCenter,
                    .AnchorDataPoint = lastDataPoint,
                    .Text = tagText,
                    .Visible = True
                }
        item.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
        item.SmartLabelStyle.Enabled = True
        item.SmartLabelStyle.IsMarkerOverlappingAllowed = False
        item.SmartLabelStyle.IsOverlappedHidden = False
        Dim movingDirection As LabelAlignmentStyles = LabelAlignmentStyles.Bottom
        item.SmartLabelStyle.MovingDirection = movingDirection
        treatmentChart.Annotations.Add(item)
    End Sub

    ''' <summary>
    '''  Get the annotation text based on the marker tag array.
    ''' </summary>
    ''' <param name="markerTag">Array of strings representing the marker tags.</param>
    ''' <returns>Formatted annotation text.</returns>
    Private Function GetAnnotationText(markerTag() As String) As String
        Dim annotationText As String = ""
        Select Case markerTag.Length
            Case 1
                annotationText = $"{markerTag(0)}"
            Case 2
                annotationText = $"{markerTag(0)} {markerTag(1)}"
            Case 3
                annotationText = $"{markerTag(0)} {markerTag(1)} {markerTag(2)}"
            Case Else
                Stop
        End Select

        Return annotationText
    End Function

    ''' <summary>`
    '''  Create a callout annotation for the last data point in the treatment chart.
    '''  This method sets the color, marker size, and style for the last data point,
    '''  and adds or updates an annotation with the specified tag text.
    ''' </summary>
    ''' <param name="treatmentChart"></param>
    ''' <param name="lastDataPoint"></param>
    ''' <param name="markerBorderColor"></param>
    ''' <param name="tagText"></param>
    <Extension>
    Friend Sub CreateCallout(treatmentChart As Chart, lastDataPoint As DataPoint, markerBorderColor As Color, tagText As String)
        lastDataPoint.Color = markerBorderColor
        lastDataPoint.MarkerBorderWidth = 2
        lastDataPoint.MarkerBorderColor = markerBorderColor
        lastDataPoint.MarkerSize = 20
        lastDataPoint.MarkerStyle = MarkerStyle.Square
        lastDataPoint.Tag = tagText
        treatmentChart.AddOrUpdateAnnotation(lastDataPoint, tagText)
    End Sub

    ''' <summary>
    '''  Finds the CalloutAnnotation associated with the last data point in the treatment chart.
    '''  If no annotation exists for the last data point, it returns Nothing.
    ''' </summary>
    ''' <param name="treatmentChart"></param>
    ''' <param name="lastDataPoint"></param>
    ''' <returns>A CalloutAnnotation if found; otherwise, Nothing.</returns>
    <Extension>
    Friend Function FindAnnotation(treatmentChart As Chart, lastDataPoint As DataPoint) As CalloutAnnotation
        For Each e As IndexClass(Of Annotation) In treatmentChart.Annotations.ToList.WithIndex
            Dim annotation As CalloutAnnotation = CType(e.Value, CalloutAnnotation)
            If annotation.AnchorDataPoint Is Nothing Then Continue For
            If annotation.AnchorDataPoint.XValue = lastDataPoint.XValue Then
                Return CType(treatmentChart.Annotations.Item(e.Index), CalloutAnnotation)
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    '''  Dictionary to hold callout annotations for different charts.
    ''' </summary
    ''' <param name="chart1"></param>
    ''' <param name="currentDataPoint"></param>
    ''' <param name="annotationText"></param>
    <Extension>
    Friend Sub SetupCallout(chart1 As Chart, currentDataPoint As DataPoint, annotationText As String)

        If chart1.Name = "TreatmentMarkersChart" AndAlso
            (annotationText.StartsWith("Bolus ") OrElse
             annotationText.StartsWith("Meal ")) Then
            Return
        End If
        With s_calloutAnnotations(chart1.Name)
            .AnchorDataPoint = currentDataPoint
            .Text = annotationText
            If .Visible = False Then
                .Visible = True
            End If
        End With
    End Sub

    ''' <summary>
    '''  Sets up a callout annotation for the current data point in the chart.
    '''  This method retrieves the annotation text based on the provided marker tag and applies it to the chart.
    ''' </summary>
    ''' <param name="chart1"></param>
    ''' <param name="currentDataPoint"></param>
    ''' <param name="markerTag"></param>
    <Extension>
    Friend Sub SetUpCallout(chart1 As Chart, currentDataPoint As DataPoint, markerTag() As String)
        Dim annotationText As String = GetAnnotationText(markerTag)
        chart1.SetupCallout(currentDataPoint, annotationText)
    End Sub

End Module
