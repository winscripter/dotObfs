# How to obfuscate the Visual Basic programming language dynamically with dotObfs?
Hello, developers! Thank you for learning how to use dotObfs.

We will demonstrate how can you easily obfuscate VB with dotObfs.

# Example
```csharp
using System;
using System.Collections.Generic;
using dotObfs.Core.VisualBasic;
using dotObfs.Core;

namespace DotObfsClientDemo
{
    class Program
    {
        static void Main()
        {
            string VBCode = @"Imports System

Module Demo
    Sub Main()
        Console.WriteLine(""foo bar"")
    End Sub
End Module";
            VisualBasicLexer lexer = new VisualBasicLexer(VBCode);
            List<Token> tokens = new List<Token>();
            Token last;
            do
            {
                last = lexer.GetNextToken();
                tokens.Add(last);
            }
            while (last.type != TokenType.EOF);
            
            Console.WriteLine(VBObfuscator.Obfuscate(
                ObfuscatorRule.RandomString,
                tokens));
        }
    }
}
```
Running the program above yields the result:
```vb
Imports yTRT Module bXwQfZ Sub QLzOOq() Console.WriteLine("Foo, bar!") End Sub End Module
```

This is nice, but you can notice dotObfs obfuscates any identifier it sees.
This is why there's a feature called **obfuscator ignores**, which allow you to
choose which identifiers dotObfs should not obfuscate.

By modifying our code to this:
```csharp
Ignore.AllIdentifierIgnores.Add("System");
Ignore.AllIdentifierIgnores.Add("Demo");
Ignore.AllIdentifierIgnores.Add("Main");
Ignore.AllIdentifierIgnores.Add("Console");
Ignore.AllIdentifierIgnores.Add("WriteLine");

Console.WriteLine(CSObfuscator.Obfuscate(
                  ObfuscatorRule.RandomString,
                  tokens));
```

The output slightly differs:
```vb
Imports System Module Demo Sub Main() Console.WriteLine("Foo, bar!") End Sub End Module
```

As you can see, dotObfs will never obfuscate the identifiers *System*, *Demo*, *Main*, *Console*, and *WriteLine*.
However, these identifiers must not conflict with the actual keywords, i.e. `Imports`, `CStr`, etc. dotObfs will not skip those identifiers.
