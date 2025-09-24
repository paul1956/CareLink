' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

''' <summary>
'''  Provides extension methods and helpers for managing callout annotations
'''  on chart controls.
''' </summary>
Friend Module CalloutHelpers

    ''' <summary>
    '''  Adds or updates a callout annotation for the last data point in the treatment chart.
    '''  If an annotation already exists for the last data point, it updates the text
    '''  and position.
    '''  If no annotation exists, it creates a new <see cref="CalloutAnnotation"/>
    '''  with the specified text text.
    ''' </summary>
    ''' <param name="chart">
    '''  The chart to which the annotation will be added or updated.
    ''' </param>
    ''' <param name="lastDataPoint">The data point to anchor the annotation to.</param>
    ''' <param name="text">The text to display in the annotation.</param>
    <Extension>
    Private Sub AddOrUpdateAnnotation(chart As Chart, lastDataPoint As DataPoint, text As String)
        Dim annotation As CalloutAnnotation = chart.FindAnnotation(lastDataPoint)
        With annotation
            If annotation IsNot Nothing Then
                .Text = $"{text}{vbCrLf}{annotation.Text}"
                Dim yValues0 As Double = lastDataPoint.YValues(0)
                Dim yValue As Double = yValues0 - ((yValues0 - annotation.AnchorDataPoint.YValues(0)) / 2)
                .AnchorDataPoint.SetValueXY(lastDataPoint.XValue, yValue)
                Exit Sub
            End If
        End With
        Dim item As New CalloutAnnotation With {
            .Alignment = ContentAlignment.BottomCenter,
            .AnchorDataPoint = lastDataPoint,
            .Text = text,
            .Visible = True}
        item.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
        item.SmartLabelStyle.Enabled = True
        item.SmartLabelStyle.IsMarkerOverlappingAllowed = False
        item.SmartLabelStyle.IsOverlappedHidden = False
        item.SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Bottom
        chart.Annotations.Add(item)
    End Sub

    ''' <summary>
    '''  Gets the annotation text based on the marker text array.
    ''' </summary>
    ''' <param name="markerTags">An array of strings representing the marker tags.</param>
    ''' <returns>A formatted annotation text string.</returns>
    Private Function GetAnnotationText(markerTags() As String) As String
        Dim annotationText As String = ""
        Select Case markerTags.Length
            Case 1
                annotationText = $"{markerTags(0)}"
            Case 2
                annotationText = $"{markerTags(0)} {markerTags(1)}"
            Case 3
                annotationText = $"{markerTags(0)} {markerTags(1)} {markerTags(2)}"
            Case Else
                Stop
        End Select

        Return annotationText
    End Function

    ''' <summary>
    '''  Creates a callout annotation for the last data point in the treatment chart.
    '''  This method sets the color, marker size, and style for the last data point,
    '''  and adds or updates an annotation with the specified text text.
    ''' </summary>
    ''' <param name="chart">The chart to which the callout will be added.</param>
    ''' <param name="lastDataPoint">The data point to annotate.</param>
    ''' <param name="markerBorderColor">
    '''  The color to use for the marker border and fill.
    ''' </param>
    ''' <param name="text">The text to display in the callout annotation.</param>
    <Extension>
    Friend Sub CreateCallout(chart As Chart, lastDataPoint As DataPoint, markerBorderColor As Color, text As String)
        lastDataPoint.Color = markerBorderColor
        lastDataPoint.MarkerBorderWidth = 2
        lastDataPoint.MarkerBorderColor = markerBorderColor
        lastDataPoint.MarkerSize = 20
        lastDataPoint.MarkerStyle = MarkerStyle.Square
        lastDataPoint.Tag = text
        chart.AddOrUpdateAnnotation(lastDataPoint, text)
    End Sub

    ''' <summary>
    '''  Finds the <see cref="CalloutAnnotation"/> associated with the last data point
    '''  in the treatment chart.
    '''  If no annotation exists for the last data point,
    '''  it returns <see langword="Nothing"/>.
    ''' </summary>
    ''' <param name="treatmentChart">The chart to search for the annotation.</param>
    ''' <param name="lastDataPoint">The data point to find the annotation for.</param>
    ''' <returns>
    '''  A <see cref="CalloutAnnotation"/> if found;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
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
    '''  Sets up a callout annotation for the specified chart and
    '''  data point with the given annotation text.
    '''  If the chart is "TreatmentMarkersChart" and the annotation text starts
    '''  with "Bolus " or "Meal ", the method returns without making changes.
    '''  Otherwise, it updates the anchor data point and text of the callout annotation,
    '''  and ensures it is visible.
    ''' </summary>
    ''' <param name="chart">The chart to update the callout annotation for.</param>
    ''' <param name="currentDataPoint">The data point to anchor the annotation to.</param>
    ''' <param name="text">The text to display in the annotation.</param>
    <Extension>
    Friend Sub SetupCallout(chart As Chart, currentDataPoint As DataPoint, text As String)
        If chart.Name = "TreatmentMarkersChart" AndAlso
            (text.StartsWith(value:="Bolus ") OrElse
             text.StartsWith(value:="Meal ")) Then
            Return
        End If
        With s_calloutAnnotations(key:=chart.Name)
            .AnchorDataPoint = currentDataPoint
            .Text = text
            If .Visible = False Then
                .Visible = True
            End If
        End With
    End Sub

    ''' <summary>
    '''  Sets up a callout annotation for the current data point in the chart.
    '''  This method retrieves the annotation text based on the provided marker text
    '''  and applies it to the chart.
    ''' </summary>
    ''' <param name="chart">The chart to update the callout annotation for.</param>
    ''' <param name="currentDataPoint">The data point to anchor the annotation to.</param>
    ''' <param name="markerTags">An array of strings representing the marker tags.</param>
    <Extension>
    Friend Sub SetUpCallout(chart As Chart, currentDataPoint As DataPoint, markerTags() As String)
        Dim text As String = GetAnnotationText(markerTags)
        chart.SetupCallout(currentDataPoint, text)
    End Sub

End Module
