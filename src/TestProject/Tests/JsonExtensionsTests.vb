' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
Public Class JsonExtensionsTests

    Private Shared Function Parse(json As String) As JsonElement
        Using doc As JsonDocument = JsonDocument.Parse(json)
            Return doc.RootElement.Clone()
        End Using
    End Function

    <Theory>
    <InlineData("""abc""", "abc")>
    <InlineData("true", "True")>
    <InlineData("false", "False")>
    <InlineData("12.5", "12.5")>
    <InlineData("null", "")>
    <InlineData("""""", "")>
    <InlineData("{}", "")>
    <InlineData("[]", "")>
    <InlineData("{""a"":1}", "{""a"":1}")>
    <InlineData("[1,2]", "[1,2]")>
    Public Sub ElementToString_ReturnsExpectedText(json As String, expected As String)
        Parse(json).ElementToString().Should().Be(expected)
    End Sub

    <Theory>
    <InlineData("null", True)>
    <InlineData("""""", True)>
    <InlineData("{}", True)>
    <InlineData("[]", True)>
    <InlineData("""x""", False)>
    <InlineData("0", False)>
    <InlineData("true", False)>
    <InlineData("false", False)>
    <InlineData("[1]", False)>
    <InlineData("{""a"":1}", False)>
    Public Sub IsEmpty_ReturnsExpectedValue(json As String, expected As Boolean)
        Parse(json).IsEmpty().Should().Be(expected)
    End Sub

    <Fact>
    Public Sub IsEmpty_DefaultElement_IsTrue()
        Dim element As JsonElement = Nothing

        element.IsEmpty().Should().BeTrue()
    End Sub

    <Theory>
    <InlineData("{""a"":1}", True)>
    <InlineData("[1,2,3]", True)>
    <InlineData("""text""", True)>
    <InlineData("true", True)>
    <InlineData("null", True)>
    <InlineData("-12.5e3", True)>
    <InlineData("42", True)>
    <InlineData("", False)>
    <InlineData("   ", False)>
    <InlineData("hello", False)>
    <InlineData("10/4/2026 7:26:20 AM", False)>
    <InlineData("12abc", False)>
    Public Sub IsValidJson_ReturnsExpectedValue(value As String, expected As Boolean)
        value.IsValidJson().Should().Be(expected)
    End Sub

    <Fact>
    Public Sub IsValidJson_NullString_IsFalse()
        Dim value As String = Nothing

        value.IsValidJson().Should().BeFalse()
    End Sub

    <Fact>
    Public Sub FromJson_String_DeserializesCaseInsensitiveAndNumericStrings()
        Dim result As Dictionary(Of String, Integer) = "{""a"":""5"",""B"":6}".FromJson(Of Dictionary(Of String, Integer))()

        result.Should().Contain(New KeyValuePair(Of String, Integer)("a", 5))
        result.Should().Contain(New KeyValuePair(Of String, Integer)("B", 6))
    End Sub

    <Fact>
    Public Sub FromJson_Element_Deserializes()
        Dim result As List(Of Integer) = Parse("[1,2,3]").FromJson(Of List(Of Integer))()

        result.Should().Equal(1, 2, 3)
    End Sub

    <Fact>
    Public Sub ToJson_ThenFromJson_RoundTrips()
        Dim source As New List(Of Integer) From {1, 2, 3}

        Dim json As String = source.ToJson()

        json.FromJson(Of List(Of Integer))().Should().Equal(source)
    End Sub

    <Fact>
    Public Sub ToJson_WritesIndentedOutputWithNativeNumbers()
        Dim json As String = New Dictionary(Of String, Integer) From {{"a", 1}}.ToJson()

        json.Should().Contain("""a"": 1")
        json.Should().Contain(Environment.NewLine)
    End Sub

    <Fact>
    Public Sub ToJsonElement_String_ReturnsStringElement()
        Dim element As JsonElement = "hello".ToJsonElement()

        element.ValueKind.Should().Be(JsonValueKind.String)
        element.GetString().Should().Be("hello")
    End Sub

    Private Shared ReadOnly s_expectation As String() = {"a", "b"}

    <Fact>
    Public Sub JsonElementToDictionary_Object_ReturnsProperties()
        Dim dict As Dictionary(Of String, JsonElement) = Parse("{""a"":1,""b"":""x""}").JsonElementToDictionary()

        dict.Keys.Should().BeEquivalentTo(s_expectation)
        dict(key:="a").GetInt32().Should().Be(expected:=1)
        dict(key:="b").GetString().Should().Be(expected:="x")
    End Sub

    <Fact>
    Public Sub JsonElementToDictionary_NonObject_Throws()
        Dim element As JsonElement = Parse("[1]")

        Dim act As Func(Of Dictionary(Of String, JsonElement)) = Function() element.JsonElementToDictionary()

        act.Should().Throw(Of ArgumentException)()
    End Sub

    <Fact>
    Public Sub ToStringDictionary_Json_ConvertsValues()
        Dim dict As Dictionary(Of String, String) =
            "{""s"":""x"",""n"":1,""t"":true,""o"":{""k"":1},""z"":null}".ToStringDictionary()

        dict(key:="s").Should().Be(expected:="x")
        dict(key:="n").Should().Be(expected:="1")
        dict(key:="t").Should().Be(expected:="True")
        dict(key:="o").Should().Be(expected:="{""k"":1}")
        dict(key:="z").Should().BeEmpty()
    End Sub

    <Fact>
    Public Sub ToStringDictionary_InvalidJson_ReturnsEmpty()
        Dim invalid As String = "not json"

        invalid.ToStringDictionary().Should().BeEmpty()
    End Sub

    <Fact>
    Public Sub ToStringDictionary_Element_SkipsNullsAndKeepsRawText()
        Dim dict As Dictionary(Of String, String) =
            Parse("{""s"":""x"",""n"":1,""t"":true,""o"":{""k"":1},""z"":null}").ToStringDictionary()

        dict(key:="s").Should().Be(expected:="x")
        dict(key:="n").Should().Be(expected:="1")
        dict(key:="t").Should().Be(expected:="True")
        dict(key:="o").Should().Be(expected:="{""k"":1}")
        dict.ContainsKey(key:="z").Should().BeFalse()
    End Sub

    <Fact>
    Public Sub TryGetStringProperty_ExistingProperty_ReturnsTrue()
        Dim value As String = Nothing

        Parse("{""a"":""x""}").TryGetStringProperty("a", value).Should().BeTrue()

        value.Should().Be(expected:="x")
    End Sub

    <Theory>
    <InlineData("{""a"":""x""}", "missing")>
    <InlineData("{""a"":""""}", "a")>
    <InlineData("{""a"":null}", "a")>
    <InlineData("{}", "a")>
    Public Sub TryGetStringProperty_MissingOrEmpty_ReturnsFalse(json As String, propertyName As String)
        Dim value As String = "preset"

        Parse(json).TryGetStringProperty(propertyName, value).Should().BeFalse()

        value.Should().BeNull()
    End Sub

    <Fact>
    Public Sub JsonToDictionary_BlankOrInvalid_ReturnsEmpty()
        Dim nothingJson As String = Nothing

        nothingJson.JsonToDictionary().Should().BeEmpty()
        Dim blank As String = "  "
        Dim invalid As String = "not json"

        blank.JsonToDictionary().Should().BeEmpty()
        invalid.JsonToDictionary().Should().BeEmpty()
    End Sub

    <Fact>
    Public Sub JsonToDictionary_ScalarsAndNestedObjects_AreFlattened()
        Dim json As String = "{""name"":""bob"",""count"":3,""flag"":true,""nested"":{""k"":""v""},""nothing"":null}"

        Dim dict As Dictionary(Of String, String) = json.JsonToDictionary()

        dict(key:="name").Should().Be(expected:="bob")
        dict(key:="count").Should().Be(expected:="3")
        dict(key:="flag").Should().Be(expected:="True")
        dict(key:="nested:k").Should().Be(expected:="v")
        dict(key:="nothing").Should().BeNull()
    End Sub

    <Fact>
    Public Sub JsonToListOfDictionary_BlankOrInvalid_ReturnsEmptyList()
        Dim nothingJson As String = Nothing

        JsonToListOfDictionary(json:=nothingJson).Should().BeEmpty()
        JsonToListOfDictionary(json:="bad").Should().BeEmpty()
    End Sub

End Class
