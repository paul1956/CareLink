' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Text
Imports System.Text.Json

''' <summary>
''' Recursively scans an object's [JsonExtensionData] dictionary after
''' deserialization, extracts every non-null key/value pair, flattens
''' nested JSON structures (ValueKind.Object) into dotted key paths,
''' and builds a consolidated audit log of all unexpected fields found
''' in the JSON input.
''' </summary>
Public NotInheritable Class ExtensionDataAuditScanner

    Private Sub New()
        ' Static utility; not meant to be instantiated.
    End Sub

    ''' <summary>
    '''  Scans an object that exposes a [JsonExtensionData] dictionary
    '''  (typically a Dictionary(Of String, JsonElement)) and returns a
    '''  flattened list of every non-null field discovered, along with a
    '''  consolidated audit log.
    ''' </summary>
    ''' <param name="target">
    '''  The deserialized POCO whose [JsonExtensionData] property should
    '''  be inspected. Pass Nothing to obtain an empty report.
    ''' </param>
    ''' <returns>
    '''  An <see cref="AuditReport"/> containing the flattened field list
    ''' and the human-readable audit log.
    ''' </returns>
    Public Shared Function Scan(target As Object) As AuditReport
        Dim report As New AuditReport()

        If target Is Nothing Then
            report.AppendLine(line:="[audit] No target object supplied; nothing to scan.")
            Return report
        End If
        Dim line As String
        Dim extensionData As Object = ResolveExtensionData(target)
        If extensionData Is Nothing Then
            line = $"[audit] No [JsonExtensionData] dictionary found on type {target.GetType().FullName}."
            report.AppendLine(line:=line)
            Return report
        End If

        Dim entries As Dictionary(Of String, JsonElement) = TryCast(
            extensionData, Dictionary(Of String, JsonElement))

        If entries Is Nothing Then
            line = "[audit] Extension data is present but is not a " &
                   "Dictionary(Of String, JsonElement); cannot scan."
            report.AppendLine(line)
            Return report
        End If

        line = $"[audit] Begin scan of {entries.Count} unexpected top-level field(s) on {target.GetType().Name}."
        report.AppendLine(line)

        For Each pair As KeyValuePair(Of String, JsonElement) In entries
            ' Root path is the property name as it appeared in the JSON.
            FlattenElement(path:=pair.Key, element:=pair.Value, report:=report)
        Next

        line = $"[audit] Scan complete. {report.Fields.Count} non-null field(s) recorded."
        report.AppendLine(line)
        Return report
    End Function

    ''' <summary>
    '''  Recursively flattens a single <see cref="JsonElement"/> into the
    '''  report. Objects are descended into; scalars become leaf entries;
    '''  nulls are skipped so they never appear in the audit output.
    ''' </summary>
    ''' <param name="path">
    '''  Dotted key path of the element being processed.
    ''' </param>
    ''' <param name="element">The <see cref="JsonElement"/> to inspect.</param>
    ''' <param name="report">The accumulating report.</param>
    Private Shared Sub FlattenElement(path As String,
                                      element As JsonElement,
                                      report As AuditReport)

        Select Case element.ValueKind
            Case JsonValueKind.Null
                ' Per requirement: skip nulls entirely.
                Exit Sub

            Case JsonValueKind.Object
                ' Nested object: recurse into every child property, building
                ' a dotted path so the audit log stays flat and traceable.
                For Each prop As JsonProperty In element.EnumerateObject()
                    Dim childPath As String = $"{path}.{prop.Name}"
                    FlattenElement(path:=childPath, element:=prop.Value, report:=report)
                Next

            Case JsonValueKind.Array
                ' Preserve array position in the key path for traceability.
                Dim index As Integer = 0
                For Each item As JsonElement In element.EnumerateArray()
                    Dim childPath As String = $"{path}[{index}]"
                    FlattenElement(path:=childPath, element:=item, report:=report)
                    index += 1
                Next

            Case Else
                ' Scalar leaf value: record it.
                Dim value As String = ElementToString(element)
                report.AddField(key:=path, value)
                Dim line As String = $"[field] {path} = {value}"
                report.AppendLine(line)
        End Select

    End Sub

    ''' <summary>
    '''  Locates the [JsonExtensionData]-decorated property on the target
    '''  via reflection so callers do not have to pass the dictionary by hand.
    '''  Returns the first public instance property carrying the attribute.
    ''' </summary>
    Private Shared Function ResolveExtensionData(target As Object) As Object
        Dim predicate As Func(Of PropertyInfo, Boolean) =
            Function(p)
                Return Attribute.IsDefined(element:=p, attributeType:=GetType(Serialization.JsonExtensionDataAttribute))
            End Function
        Dim propInfo As PropertyInfo =
            target.GetType().GetProperties().FirstOrDefault(predicate)

        If propInfo Is Nothing Then Return Nothing

        Return propInfo.GetValue(obj:=target, index:=Nothing)
    End Function

    ''' <summary>
    ''' Holds the flattened field list and the consolidated, human-readable
    ''' audit log produced during a scan.
    ''' </summary>
    Public NotInheritable Class AuditReport
        Private ReadOnly _log As New StringBuilder()

        Public ReadOnly Property Fields As New List(Of KeyValuePair(Of String, String))

        Public ReadOnly Property Log As String
            Get
                Return _log.ToString()
            End Get
        End Property

        Friend Sub AddField(key As String, value As String)
            Fields.Add(New KeyValuePair(Of String, String)(key, value))
        End Sub

        Friend Sub AppendLine(line As String)
            _log.AppendLine(line)
        End Sub

    End Class

End Class
