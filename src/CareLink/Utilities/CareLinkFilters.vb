' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module CareLinkFilters

    Friend ReadOnly s_alwaysFilter As New List(Of String) From {
            "kind",
            "relativeOffset",
            "version"
            }

    Private ReadOnly s_lastAlarmFilter As New List(Of String) From {
            "code",
            "GUID",
            "instanceId",
            "kind",
            "referenceGUID",
            "relativeOffset",
            "version"
            }

    Private ReadOnly s_markersFilter As New List(Of String) From {
            "id",
            "index",
            "kind",
            "relativeOffset",
            "version"
            }

    Private ReadOnly s_notificationHistoryFilter As New List(Of String) From {
            "faultId",
            "GUID",
            "id",
            "index",
            "instanceId",
            "kind",
            "referenceGUID",
            "relativeOffset",
            "version"
            }

    ' do not rename or move up
    Friend ReadOnly s_zFilterList As New Dictionary(Of Integer, List(Of String)) From {
        {ItemIndexs.lastAlarm, s_lastAlarmFilter},
        {ItemIndexs.lastSG, s_alwaysFilter},
        {ItemIndexs.markers, s_markersFilter},
        {ItemIndexs.notificationHistory, s_notificationHistoryFilter}
        }

End Module
