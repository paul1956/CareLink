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

    Friend s_treatmentCalloutAnnotations As New Dictionary(Of Double, CalloutAnnotation)

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

    Friend Sub SetCalloutVisibility(name As String)
        With s_calloutAnnotations(name)
            If .Visible Then
                .Visible = False
            End If
        End With
    End Sub

    <Extension>
    Friend Sub SetupCallout(chart1 As Chart, currentDataPoint As DataPoint, annotationText As String)

        If chart1.Name = "TreatmentMarkersChart" AndAlso annotationText.StartsWith("Bolus ") Then
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
