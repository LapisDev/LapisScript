using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lapis.Script.Parser.Lexical;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Lapis.Script.Parser.Lexical.Tests
{
    [TestClass()]
    public class LexerTests
    {
        [TestMethod()]
        public void LexerTest()
        {
            LexicalRule letter = LexicalRule.Range('A', 'Z') | LexicalRule.Range('a', 'z');
            LexicalRule digit = LexicalRule.Range('0', '9');   

            LexerBuilder lexb = new LexerBuilder();
            Lexeme blank = lexb.DefineLexeme(0, true, LexicalRule.Chars(" \n\t\r").Repeat());
            Lexeme id = lexb.DefineLexeme(1, letter + (letter | digit).Repeat());
            Lexeme keyword = lexb.DefineLexeme(2, LexicalRule.Literal("var") | LexicalRule.Literal("function") |
                LexicalRule.Literal("new") | LexicalRule.Literal("this") | LexicalRule.Literal("for") | 
                LexicalRule.Literal("return"));
            Lexeme number = lexb.DefineLexeme(3, digit.Repeat() + (LexicalRule.Char('.') + digit.Repeat() | LexicalRule.Empty));
            Lexeme inc = lexb.DefineLexeme(4, LexicalRule.Literal("++"));
            Lexeme oper = lexb.DefineLexeme(4, LexicalRule.Chars("+-*/^=<>"));
            Lexeme str = lexb.DefineLexeme(5, LexicalRule.Char('\'') + 
                (LexicalRule.NotChar('\'') | LexicalRule.Literal(@"\'")).Repeat() + LexicalRule.Char('\''));
            Lexeme bracket = lexb.DefineLexeme(6, LexicalRule.Chars("()[]{}"));
            Lexeme deli = lexb.DefineLexeme(7, LexicalRule.Chars(",;:"));
            Lexeme comm = lexb.DefineLexeme(10, true, LexicalRule.Literal("//") + 
                LexicalRule.NotChars("\n\r").Repeat() + LexicalRule.Chars("\n\r"));
            Lexeme commul = lexb.DefineLexeme(10, true, LexicalRule.Literal("/*") + 
                (LexicalRule.Char('/') | LexicalRule.Char('*').Repeat() + LexicalRule.NotChars("/*")).Repeat() + 
                LexicalRule.Char('*') + LexicalRule.Char('/'));                     

            var input = System.IO.File.ReadAllText("test_data/1.input.txt");
            var expected = System.IO.File.ReadAllText("test_data/1.expected.txt");
            string actual;
            {
                var sb = new System.Text.StringBuilder();
                BranchedLexer blexer = lexb.GetBranchedLexer(input);
                Token t;
                while ((t = blexer.Read()) != null)
                {
                    sb.AppendLine(t.ToString());
                }
                actual = sb.ToString();
            }
            if (expected != actual)
            {
                System.IO.File.WriteAllText("test_data/1.actual.txt", actual);
                Assert.Fail();
            }
        }     
    }  
}
