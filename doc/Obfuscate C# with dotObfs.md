# How to obfuscate the C# programming language dynamically with dotObfs?
Hello, developers! Thank you for learning how to use dotObfs.

We will demonstrate how can you easily obfuscate C# with dotObfs.

# Example
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
Running the program above yields the result:
```cs
using YRqHwQnUomOuLLW;class LvVxhRXxEOD{static void MvOWOr(){RYRfWxngiMUjFo.dFYAxOjLMCEouiyiVSRpG("Hello, World!");string APSChtxasF="bar";}}
```

This is nice, but you can notice dotObfs obfuscates any identifier it sees.
This is why there's a feature called **obfuscator ignores**, which allow you to
choose which identifiers dotObfs should not obfuscate.

By modifying our code to this:
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

The output slightly differs:
```cs
using System;class Program{static void Main(){Console.WriteLine("Hello, World!");string ctyIwlLmXSQb="bar";}}
```

As you can see, dotObfs will never obfuscate the identifiers *System*, *Program*, *Main*, *Console*, and *WriteLine*.
However, these identifiers must not conflict with the actual keywords, i.e. `yield`, `return`, etc. dotObfs will not skip those identifiers.
