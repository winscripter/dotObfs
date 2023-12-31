# dotObfs
dotObfs is a library that can help you obfuscate up to 7 programming languages with .NET.

*Warning: **The library has an MIT license, found in the file LICENSE. This means you're allowed to copy the source code, as long as you include the license for dotObfs. Also, please keep in mind that if you corrupt the source code/library, resulting in unintended results, we're NOT RESPONSIBLE FOR ANY DEBUGGING OR FIXES!***

In most cases, you really don't need to obfuscate .NET. Not a lot of developers do so. Obfuscating languages like JavaScript makes sense, since anyone can see
the source with their browser. Most .NET developers make their projects open-source.

If you still need .NET obfuscators, here's how to use dotObfs.

dotObfs lets you obfuscate these programming languages:
- C
- C++
- Java
- JavaScript
- C#
- Visual Basic
- F#

# One example (with C#)
```csharp
using System;
using System.Collections.Generic;
using dotObfs.Core.CSharp;
using dotObfs.Core;

namespace DotObfsClientDemo
{
    class Program
    {
        static void Main()
        {
            string CSharpCode = @"using System;
class Program
{
    static void Main()
    {
        Console.WriteLine(""Hello, World!"");
        string foo = ""bar"";
    }
}";
            CSharpLexer lexer = new CSharpLexer(CSharpCode);
            List<Token> tokens = new List<Token>();
            Token last;
            do
            {
                last = lexer.GetNextToken();
                tokens.Add(last);
            }
            while (last.type != TokenType.EOF);
            
            Console.WriteLine(CSObfuscator.Obfuscate(
                ObfuscatorRule.RandomString,
                tokens));
        }
    }
}
```
The program outputs:
`using YRqHwQnUomOuLLW;class LvVxhRXxEOD{static void MvOWOr(){RYRfWxngiMUjFo.dFYAxOjLMCEouiyiVSRpG("Hello, World!");string APSChtxasF="bar";}}`
As you can see, it has obfuscated every single identifier. This is not great, you don't want to obfuscate everything.
In this case, there's a way to add **ignores**. They allow you to obfuscate anything except the specified identifiers.

Now if we add this:
```csharp
            Ignore.AllIdentifierIgnores.Add("System");
            Ignore.AllIdentifierIgnores.Add("Program");
            Ignore.AllIdentifierIgnores.Add("Main");
            Ignore.AllIdentifierIgnores.Add("Console");
            Ignore.AllIdentifierIgnores.Add("WriteLine");
            
            Console.WriteLine(CSObfuscator.Obfuscate(
                ObfuscatorRule.RandomString,
                tokens));
```
The result is:
`using System;class Program{static void Main(){Console.WriteLine("Hello, World!");string ctyIwlLmXSQb="bar";}}`
There's also a feature called **obfuscator rule**. They specify how to obfuscate your code.
As you can see, the obfuscator rule is set to `RandomString`.
If we change that to `XOnly`:
```csharp
 Console.WriteLine(CSObfuscator.Obfuscate(
                ObfuscatorRule.XOnly, // notice this - ObfuscatorRule.XOnly
                tokens));
```
We get:
`using System;class Program{static void Main(){Console.WriteLine("Hello, World!");string XXXXXXXXXXX="bar";}}`
However, this is not recommended, as it can often cause duplicate identifiers, resulting in a `throw` statement.
Setting `ObfuscatorRule.RandomString` is a much more robust way to prevent getting the `throw` statement, although the performance may degrade a little bit.

# How does dotObfs work?
dotObfs works by using its lexer for that programming language, and then it replaces the identifiers. It ensures they're not in the ignore list and were not obfuscated before.
