# The Parser Examples

This is an example of arithmetic calculator base on **Lapis.Script.Parser**.

```csharp
using Lapis.Script.Parser.Lexical;
using Lapis.Script.Parser.Parsing;

class ArithmeticCalculator
{
    public ArithmeticCalculator()
    {
        DefineParsingRules();
    }

    private LexerBuilder _lexerBuilder;
    private ParsingRule<double> _parsingRule;
    
    private void DefineParsingRules() 
    {            
        var lexb = new LexerBuilder();

        var digit = LexicalRule.Range('0', '9');
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

        var expr = new ParsingRuleContainer<double>();
        var term = new ParsingRuleContainer<double>();
        var factor = new ParsingRuleContainer<double>();            

        expr.Content
            = term.Concat((plu.Concat(term, (t, y) => y) | min.Concat(term, (t, y) => -y)).Repeat(i => i.Sum()), (x, y) => x + y)
            | term;

        term.Content
            = factor.Concat((mul.Concat(term, (s, y) => y) | (div.Concat(term, (s, y) => 1 / y))).Repeat(t => t.Count() == 0 ? 1 : t.Aggregate((x, y) => x * y)), (x, y) => x * y)
            | factor;

        factor.Content
            = br.Concat(expr, (s, x) => x).Concat(ke, (x, s) => x)
            | num;

        _lexerBuilder = lexb;
        _parsingRule = expr;
    }

    public bool TryEvaluate(string str, out double value)
    {
        var lexer = _lexerBuilder.GetBranchedLexer(str);
        double r;
        return _parsingRule.TryParse(lexer, out value);            
    }
}

var calculator = new ArithmeticCalculator();
string str = "1 * 5 + 2 * 3 / 5 - 3";
double result;
calculator.TryEvaluate(str, out result);
Console.WriteLine(result == 1.0 * 5.0 + 2.0 * 3.0 / 5.0 - 3.0); // True
```
