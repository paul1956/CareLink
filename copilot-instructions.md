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
 * Always try to include <see cref="..."/> tags, where it makes sense.
 * Always wrap after 100 characters.
 * In <remarks/> sections, please always use <para> tags.
 * Always make sure, you use XML compatible character encoding.
 * Always use the <c> tag for code snippets, and <paramref name="$..."/> for parameters.
 * Always use the <see langword="$..."/> tag for language keywords like True, False and Nothing, and <paramref name="$..."/> for parameters.
 * True and False should be used in the XML comments, not true or false.
 * The XML code must use 1-space indentation (see example below) and use triple-quote-style for the Text Lines not a single ':
   ''' <Summary>
   '''  Summary text.
   ''' </Summary>
   ''' <Remarks>
   '''  <Para>
   '''   This is a sample paragraph to guide an LLM to the structuring of docu-tags.
   '''  </Para>
   '''  <Para>
   '''   It also handles the processing of paragraphs within a conversation,
   '''   identifying and managing code listings.
   '''  </Para>
   ''' </Remarks>

 * Always use the <see cref="..."/> tag for references to other members,
 * All Boolean values passed to Subs or Functions should use argument name := True or := False.
