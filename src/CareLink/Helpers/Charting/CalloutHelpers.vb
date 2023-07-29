' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module CalloutHelpers

    Friend ReadOnly s_calloutAnnotations As New Dictionary(Of String, CalloutAnnotation) From {
            {"SummaryChart", New CalloutAnnotation},
            {"ActiveInsulinChart", New CalloutAnnotation},
            {"TreatmentMarkersChart", New CalloutAnnotation}
       }

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

    <Extension>
    Friend Sub SetUpCallout(chart1 As Chart, currentDataPoint As DataPoint, markerTag() As String)
        Dim annotationText As String = GetAnnotationText(markerTag)
        chart1.SetupCallout(currentDataPoint, annotationText)
    End Sub

End Module
