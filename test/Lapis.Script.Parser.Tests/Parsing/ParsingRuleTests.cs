using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lapis.Script.Parser.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lapis.Script.Parser.Lexical;

namespace Lapis.Script.Parser.Parsing.Tests
{
    [TestClass()]
    public class ParsingRuleTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            LexicalRule digit = LexicalRule.Range('0', '9');
            LexerBuilder lexb = new LexerBuilder();
           
            var blank = lexb.DefineLexeme(0, true, LexicalRule.Chars(" \n\t\r").Repeat());
            var number = lexb.DefineLexeme(1, digit.Repeat() + (LexicalRule.Char('.') + digit.Repeat() | LexicalRule.Empty));
            var plus = lexb.DefineLexeme(2, LexicalRule.Char('+'));
            var minus = lexb.DefineLexeme(2, LexicalRule.Char('-'));
            var times = lexb.DefineLexeme(2, LexicalRule.Char('*'));
            var divide = lexb.DefineLexeme(2, LexicalRule.Char('/'));
            var bra = lexb.DefineLexeme(3, LexicalRule.Char('('));
            var ket = lexb.DefineLexeme(3, LexicalRule.Char(')'));

            var plu = plus.GetParsingRule();
            var min = minus.GetParsingRule();
            var mul = times.GetParsingRule();
            var div = divide.GetParsingRule();
            var br = bra.GetParsingRule();
            var ke = ket.GetParsingRule();
            var num = number.GetParsingRule(i => double.Parse(i.Text));

            ParsingRuleContainer<double> expr = new ParsingRuleContainer<double>();
            ParsingRuleContainer<double> term = new ParsingRuleContainer<double>();
            ParsingRuleContainer<double> factor = new ParsingRuleContainer<double>();
            // ParsingRuleContainer<int, double> bracket = new ParsingRuleContainer<int, double>();

            expr.Content
                = term.Concat((plu.Concat(term, (t, y) => y) | min.Concat(term, (t, y) => -y)).Repeat(i => i.Sum()), (x, y) => x + y)
                | term;

            term.Content
                = factor.Concat((mul.Concat(term, (s, y) => y) | (div.Concat(term, (s, y) => 1 / y))).Repeat(t => t.Count() == 0 ? 1 : t.Aggregate((x, y) => x * y)), (x, y) => x * y)
                | factor;

            factor.Content
                = br.Concat(expr, (s, x) => x).Concat(ke, (x, s) => x)
                | num;

            string str = "1 * 5 + 2 * 3 / 5 - 3";
            BranchedLexer lexer = lexb.GetBranchedLexer(str);
            double r;
            expr.TryParse(lexer, out r);
            Assert.AreEqual(1.0 * 5.0 + 2.0 * 3.0 / 5.0 - 3.0, r);
        }
    }
}
