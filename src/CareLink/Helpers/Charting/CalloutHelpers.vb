' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms.DataVisualization.Charting

Friend Module CalloutHelpers

    Friend ReadOnly s_calloutAnnotations As New Dictionary(Of String, CalloutAnnotation) From {
            {"SummaryChart", New CalloutAnnotation},
            {"ActiveInsulinChart", New CalloutAnnotation},
            {"TreatmentMarkersChart", New CalloutAnnotation}
       }

    Friend calloutBounds As New Dictionary(Of Double, Rectangle)

    Friend s_treatmentCalloutAnnotations As New Dictionary(Of Double, CalloutAnnotation)

End Module
