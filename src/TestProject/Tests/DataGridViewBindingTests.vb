Imports System.Data
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
Public Class DataGridViewBindingTests

    <StaFact>
    Public Sub UpdateDgvHeaders_DoesNotThrow_OnBoundGrid()
        ' Create a DataGridView and bind a small DataTable to it.
        Using dgv As New DataGridView()
            dgv.Name = "DgvLastSG"
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Dim dataGridViewColumn As New DataGridViewTextBoxColumn() With {
                .Name = "TimestampAsString",
                .HeaderText = "Timestamp From Pump"}

            dgv.Columns.Add(dataGridViewColumn:=dataGridViewColumn)

            Dim table As New DataTable()
            table.Columns.Add(columnName:="TimestampAsString",
                              type:=GetType(String))
            table.Rows.Add("now")
            dgv.DataSource = table

            ' Get the non-public instance method UpdateDgvHeaders
            ' via reflection.
            Const bindingAttr As BindingFlags =
                BindingFlags.NonPublic Or
                BindingFlags.Instance

            Dim mi As MethodInfo =
                GetType(Form1).GetMethod(name:="UpdateDgvHeaders",
                                         bindingAttr)

            ' Create an instance of Form1. Use Activator to allow non-public ctor if needed.
            Dim formInstance As Object =
                Activator.CreateInstance(type:=GetType(Form1),
                                         nonPublic:=True)

            Dim act As Action =
                Sub()
                    mi.Invoke(obj:=formInstance,
                              parameters:=New Object() {dgv})
                End Sub

            act.Should().NotThrow()
        End Using
    End Sub

End Class
