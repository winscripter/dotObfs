using System;
using System.Collections.Generic;
using System.Text;
using dotObfs.Core;
using dotObfs.Core.Java;
using LDA = dotObfs.Core.LanguageDefinitionAssociations;

namespace dotObfs.Core.Java
{
    public class JavaObfuscator
    {
        private static Random r = new Random();
        
        public static string Obfuscate(
            ObfuscatorRule rule,
            List<Token> tokens
        )
        {
            StringBuilder output = new StringBuilder();
            foreach (Token t in tokens)
            {
                if (t.type == TokenType.Identifier)
                {
                    if (Ignore.AllIdentifierIgnores.Contains(t.value))
                    {
                        output.Append(t.value);
                        continue;
                    }
                    
                    if (!LDA.AssociationStore.ContainsKey(t.value))
                    {
                        LDA.SetAssociation(t.value, RandomStringByRule(rule));
                        output.Append(LDA.AssociationStore[t.value]);
                    }
                    else
                        output.Append(LDA.AssociationStore[t.value]);
                }
                else if (t.type == TokenType.Text)
                    output.Append("\"" + t.value + "\"");
                else if (t.type == TokenType.Character)
                    output.Append("'" + t.value);
                else
                    if (t.value != null)
                        output.Append(t.value);
                    else
                        output.Append(t.type != TokenType.EOF ? t.type.ToString().ToLower() + " " : null);
            }
            
            return output.ToString();
        }

        private static string RandomStringByRule(ObfuscatorRule rule)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
            StringBuilder sb = new StringBuilder();

            switch (rule)
            {
                case ObfuscatorRule.RandomString:
                    for (int i = 0; i < r.Next(1, 64); i++)
                        sb.Append(chars[r.Next(1, chars.Length) - 1]);
                    return sb.ToString();

                case ObfuscatorRule.XOnly:
                default:
                    for (int i = 0; i < r.Next(1, 64); i++)
                        if (r.Next(1, 2) == 1)
                            sb.Append("X");
                        else
                            sb.Append("x");
                    return sb.ToString();
            }
        }
    }
}
