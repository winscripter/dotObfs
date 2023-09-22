# How to obfuscate the Visual Basic programming language dynamically with dotObfs?
Hello, developers! Thank you for learning how to use dotObfs.

We will demonstrate how can you easily obfuscate VB with dotObfs.

# Example
```csharp
using System;
using System.Collections.Generic;
using dotObfs.Core.FSharp;
using dotObfs.Core;

namespace DotObfsClientDemo
{
    class Program
    {
        static void Main()
        {
            string FSharpCode = @"let fooBar = ""foo bar""
printfn ""%s"" fooBar";
            FSharpLexer lexer = new FSharpLexer(FSharpCode);
            List<Token> tokens = new List<Token>();
            Token last;
            do
            {
                last = lexer.GetNextToken();
                tokens.Add(last);
            }
            while (last.type != TokenType.EOF);
            
            Console.WriteLine(FSObfuscator.Obfuscate(
                ObfuscatorRule.RandomString,
                tokens));
        }
    }
}
```
Running the program above yields the result:
```fs
let PaQVUzT="foo bar"foEuocQSmJFTK"%s"PaQVUzT
```

This is nice, but you can notice dotObfs obfuscates any identifier it sees.
This is why there's a feature called **obfuscator ignores**, which allow you to
choose which identifiers dotObfs should not obfuscate.

By modifying our code to this:
```csharp
Ignore.AllIdentifierIgnores.Add("fooBar");

Console.WriteLine(CSObfuscator.Obfuscate(
                  ObfuscatorRule.RandomString,
                  tokens));
```

The output slightly differs:
```fs
let fooBar="foo bar"printfn"%s"fooBar
```

As you can see, dotObfs will never obfuscate the identifiers *printfn* and *fooBar*.
However, these identifiers must not conflict with the actual keywords, i.e. `let`, `while`, etc. dotObfs will not skip those identifiers.
