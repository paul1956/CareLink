# Markdown File
Always use 2 spaces for each indentation level in xml-doc comments.

Suggest to break lines that are longer than 120 characters in VB files

Always use the latest version VB, currently VB 16.9 features.

Always write code that runs on .NET 10.0.

Use `NameOf` instead of string literals for member names.

Always have the last return statement of a method on its own line.
By default, if asked for XML comments for an entire class:
  * Add XML comments for all public members of the class.
  * Include comments for the class, the constructor, all Public properties,
  * Public methods, events and protected virtual members, but never fields.
  * Never modify file_header unless it does not match file_header_template in .editorconfig

If generally asked for XML comments, note they need to have a structure:
 * Always try to include <see cref="..."/> tags, and <see langword="..."/> tags, where it makes sense.
 * Prefer <see langword="..."/> tags over <see cref="..."/> tags where both can be used
 * In <remarks/> sections, please always use <para> tags if total line lenght would exceed 119 characters.
 * Always make sure, you use XML compatible character encoding.
 * Always use the <see langword="$..."/> tag for language keywords like <see langword="True"/>, <see langword="False"/> and <see langword="Nothing"/>, and <paramref name="$..."/> for parameters.
 * Use the <c/> tag for code snippets that are not <see langword="$..."/>, and <paramref name="$..."/> for parameters.
 * In the XML comments <see langword="True"/> and <see langword="False"/> should be used, not <see langword="true"/> or <see langword="false"/>.
 * Try to wrap after 100 characters, always wrap after 120 characters, except <param name="..."/> sections which should never be wrapped.
 * Only for XML Comments use 1-space indentation (see example below) and use triple-quote-style for the Text Lines not a single-quote:
   ''' <summary>
   '''  Summary text.
   ''' </summary>
   ''' <param name="SomeName">This is a sample of a parameter, is is never wrapped.</param>
   ''' <remarks>
   '''  <para>This is a sample to guide an LLM to the structuring of short docu-tags.</para>
   '''  <para>This paragraph summarizes how to create effective documentation for code comments.</para>
   '''  <para>It is important to keep comments clear and concise for better code readability.</Para>
   '''  <para>
   '''   It supports formatting for structured documentation and aids in clear communication.
   '''  </para>
   ''' </remarks>

 * Always use the <see cref="..."/> tag for references to other members.
 * All Boolean values passed to Subs or Functions should use argument name := True or := False.
