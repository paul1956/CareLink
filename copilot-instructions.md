# Markdown File
Always use 2 spaces for each indentation level in xml-doc comments.

Suggest to break lines that are longer than 120 characters in VB files

Always use the latest version VB, currently VB 16.9 features.

Always write code that runs on .NET 10.0.

Use `NameOf` instead of string literals for member names.

Always have the last return statement of a method on its own line.
By default, if asked for XML comments for an entire class:
  * Add XML comments for all public members of the class.
  * Include comments for the class, the constructor, all public properties,
  * public methods, events and protected virtual members, but never fields.

If generally asked for XML comments, note they need to have a structure:
 * Always try to include <see cref="..."/> tags, and <see langword="..."/> tags, where it makes sense.
 * Try to wrap after 100 characters, always wrap after 120 characters.
 * In <remarks/> sections, please always use <para> tags.
 * Always make sure, you use XML compatible character encoding.
 * Always use the <see langword="$..."/> tag for language keywords like <see langword="True"/>, <see langword="False"/> and <see langword="Nothing"/>, and <paramref name="$..."/> for parameters.
 * Use the <c> tag for code snippets that are not langword, and <paramref name="$..."/> for parameters.
 * <see langword="True"/> and <see langword="False"/> should be used in the XML comments, not <see langword="true"/> or <see langword="false"/>.
 * The XML code must use 1-space indentation (see example below) and use triple-quote-style for the Text Lines not a single ':
   ''' <Summary>
   '''  Summary text.
   ''' </Summary>
   ''' <Remarks>
   '''  <Para>This is a sample to guide an LLM to the structuring of short docu-tags.</Para>
   '''  <Para>This paragraph summarizes how to create effective documentation for code comments.</Para>
   '''  <Para>It is important to keep comments clear and concise for better code readability.</Para>
   '''  <Para>
   '''   It supports formatting for structured documentation and aids in clear communication.
   '''  </Para>
   ''' </Remarks>

 * Always use the <see cref="..."/> tag for references to other members,
 * All Boolean values passed to Subs or Functions should use argument name := True or := False.
