' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Spire.Pdf.Utilities

Public Class PumpBasalRecord

    Public Property MaximumBasalRate As Single

    Public Property NamedBasals As New Dictionary(Of String, NamedBasalRecord) From {
                {"Basal 1", New NamedBasalRecord},
                {"Basal 2", New NamedBasalRecord},
                {"Basal 3", New NamedBasalRecord},
                {"Basal 4", New NamedBasalRecord},
                {"Basal 5", New NamedBasalRecord},
                {"Workday", New NamedBasalRecord},
                {"Day Off", New NamedBasalRecord},
                {"Sick Day", New NamedBasalRecord}
           }

End Class
