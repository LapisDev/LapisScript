/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Util
 * Description : Provides commonly used methods.
 * Created     : 2015/6/20
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Parser.Parsing;
using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution
{
    static class Util
    {
        public static bool IsAtomOrPrimitive(this Expression expression)
        {
            return
                expression is VariableReferenceExpression ||
                expression is ArrayExpression ||
                expression is ArrayIndexerExpression ||
                expression is FunctionInvokeExpression ||
                expression is ThisReferenceExpression ||
                expression is SuperReferenceExpression ||
                expression is NewExpression ||
                expression is MemberReferenceExpression ||
                expression is PrimitiveExpression ||
                expression is ObjectExpression;
        }

        public static string ConvertFromEscapeChar(this string str)
        {
            using (var reader = new BranchedReader(str))
            {
                var sb = new StringBuilder();
                char c;
                while ((c = reader.Peek()) != '\0')
                {
                    c = reader.Read();
                    if (c == '\\')
                    {
                        c = reader.Peek();
                        if (c == 'n')
                            c = '\n';
                        else if (c == 't')
                            c = '\t';
                        else if (c == 'v')
                            c = '\v';
                        else if (c == 'b')
                            c = '\b';
                        else if (c == 'f')
                            c = '\f';
                        else if (c == 'r')
                            c = '\r';
                        else if (c == 'a')
                            c = '\a';
                        else if (c == '\'')
                            c = '\'';
                        else if (c == '\"')
                            c = '\"';
                        else if (c == '\\')
                            c = '\\';
                        else if (c == 'x')
                        {
                            int ascii = 0;
                            for (int j = 0; j < 2; j++)
                            {
                                c = reader.Peek();
                                if (char.IsDigit(c))
                                    ascii = ascii * 16 + c - '0';
                                else if (c >= 'A' && c <= 'Z')
                                    ascii = ascii * 16 + c - 'A' + 10;
                                else if (c >= 'a' && c <= 'z')
                                    ascii = ascii * 16 + c - 'a' + 10;
                                else
                                {
                                    if (j == 0)
                                        goto fail;
                                    break;
                                }
                                c = reader.Read();
                            }
                            c = (char)ascii;
                        }
                        else if (char.IsDigit(c))
                        {
                            int ascii = 0;
                            for (int j = 0; j < 3; j++)
                            {
                                c = reader.Peek();
                                if (char.IsDigit(c))
                                    ascii = ascii * 8 + c - '0';
                                else
                                    break;
                                c = reader.Read();
                            }
                            c = (char)ascii;
                        }
                        else
                            goto fail;
                    }
                    sb.Append(c);
                }
                return sb.ToString();
            fail:
                throw new ParserException(new LinePragma(reader.Line, reader.Span), ExceptionResource.UnrecognizedEscapeCharacter);
            }
        }


        public static string ConvertToEscapeChar(this string str)
        {

            var sb = new StringBuilder();
            string s;
            foreach (char c in str)
            {
                if (c == '\n')
                    s = "\\n";
                else if (c == '\t')
                    s = "\\t";
                else if (c == '\v')
                    s = "\\v";
                else if (c == '\b')
                    s = "\\b";
                else if (c == '\f')
                    s = "\\f";
                else if (c == '\r')
                    s = "\\r";
                else if (c == '\a')
                    s = "\\a";
                else if (c == '\'')
                    s = "\\'";
                else if (c == '\"')
                    s = "\\\"";
                else if (c == '\\')
                    s = "\\\\";
                else
                    s = c.ToString();
                sb.Append(s);
            }
            return sb.ToString();
        }


        public static ParsingRule<T> Fail<T>(string message)
        {
            return ParsingRule<T>.Custom(t =>
            {
                if (t.Peek() != null)
                    throw new ParserException(t.Peek().LinePragma, message);
                else
                    throw new ParserException(LinePragma.EOF, message);
            });
        }

        public static ParsingRule<T> OrFail<T>(this ParsingRule<T> rule, string message)
        {
            return rule | Fail<T>(message);
        }

        public static ParsingRule<T> OrFailExpected<T>(this ParsingRule<T> rule, string text)
        {
            return rule.OrFail<T>(string.Format(ExceptionResource.Expected, text));
        } 

    }
}
