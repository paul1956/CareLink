Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.Json

Public Module JsonElementExtension
    Private ReadOnly s_indentedOptions As New JsonWriterOptions() With {.Indented = True}

    <Extension> _
    Public Function ToStringIndented(value As JsonElement) As String
        Using stream As New MemoryStream()
            Using writer As New Utf8JsonWriter(stream, s_indentedOptions)
                value.WriteTo(writer)
                writer.Flush()
                Dim indented As String = Encoding.UTF8.GetString(stream.GetBuffer(), 0, CInt(stream.Length))

                Return indented
            End Using
        End Using
    End Function

    Public Function dumps(value As Dictionary(Of String,String)) As String
      Return JsonSerializer.Serialize(value)
     End Function

End Module
