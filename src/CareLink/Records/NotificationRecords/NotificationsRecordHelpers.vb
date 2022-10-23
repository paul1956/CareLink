﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Class NotificationsRecordHelpers

    Friend Shared ReadOnly rowsToHide As New List(Of String) From {
            NameOf(ActiveNotificationsRecord.GUID),
            NameOf(ActiveNotificationsRecord.index),
            NameOf(ActiveNotificationsRecord.instanceId),
            NameOf(ActiveNotificationsRecord.kind),
            NameOf(ActiveNotificationsRecord.referenceGUID),
            NameOf(ActiveNotificationsRecord.relativeOffset),
            NameOf(ActiveNotificationsRecord.version),
            NameOf(ClearedNotificationsRecord.faultId),
            NameOf(ClearedNotificationsRecord.pnpId),
            NameOf(ClearedNotificationsRecord.RecordNumber),
            NameOf(ClearedNotificationsRecord.referenceGUID)
        }

End Class