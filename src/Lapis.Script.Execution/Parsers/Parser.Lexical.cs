/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Parser
 * Description : Implements a default IParser.
 * Created     : 2015/6/20
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Parser.Parsing;
using Lapis.Script.Execution.Ast;
using Lapis.Script.Execution.Ast.Statements;
using Lapis.Script.Execution.Ast.Expressions;

using L = Lapis.Script.Parser.Lexical.LexicalRule;
using Lex = Lapis.Script.Parser.Lexical.Lexeme;
using PE = Lapis.Script.Parser.Parsing.ParsingRule<Lapis.Script.Execution.Ast.Expressions.Expression>;
using PCE = Lapis.Script.Parser.Parsing.ParsingRuleContainer<Lapis.Script.Execution.Ast.Expressions.Expression>;

namespace Lapis.Script.Execution.Parsers
{
    public partial class Parser
    {
        #region Lexical

        LexerBuilder lexb;

        Lex lex_white;
        Lex lex_comment;

        Lex lex_identifer;

        #region Keywords

        Lex lex_kw_future;
        Lex lex_kw_break;
        Lex lex_kw_case;
        Lex lex_kw_continue;
        Lex lex_kw_default;
        Lex lex_kw_do;
        Lex lex_kw_else;
        Lex lex_kw_false;
        Lex lex_kw_for;
        Lex lex_kw_goto;
        Lex lex_kw_if;
        Lex lex_kw_is;
        Lex lex_kw_new;
        Lex lex_kw_null;
        Lex lex_kw_return;
        Lex lex_kw_switch;
        Lex lex_kw_this;
        Lex lex_kw_true;
        Lex lex_kw_while;
        Lex lex_kw_class;
        Lex lex_kw_function;
        Lex lex_kw_var;
        Lex lex_kw_super;
        Lex lex_kw_extends;
        Lex lex_kw_public;
        Lex lex_kw_private;
        Lex lex_kw_static;
        Lex lex_kw_constructor;
        Lex lex_kw_protected;
        Lex lex_kw_get;
        Lex lex_kw_set;

        #endregion

        #region Literal

        Lex lex_li_num;
        Lex lex_li_string;
        Lex lex_li_string1;

        #endregion

        #region Operators and Punctuators

        Lex lex_op_leftBrace;
        Lex lex_op_rightBrace;
        Lex lex_op_leftBracket;
        Lex lex_op_rightBracket;
        Lex lex_op_leftParenthesis;
        Lex lex_op_rightParenthesis;
        Lex lex_op_dot;
        Lex lex_op_comma;
        Lex lex_op_colon;
        Lex lex_op_semicolon;
        Lex lex_op_plus;
        Lex lex_op_minus;
        Lex lex_op_multiply;
        Lex lex_op_divide;
        Lex lex_op_mod;
        Lex lex_op_and;
        Lex lex_op_or;
        Lex lex_op_xor;
        Lex lex_op_not;
        Lex lex_op_inverse;
        Lex lex_op_assign;
        Lex lex_op_less;
        Lex lex_op_greater;
        Lex lex_op_question;
        Lex lex_op_increment;
        Lex lex_op_decrement;
        Lex lex_op_andAlso;
        Lex lex_op_orElse;
        Lex lex_op_shiftLeft;
        Lex lex_op_shiftRight;
        Lex lex_op_equal;
        Lex lex_op_notEqual;
        Lex lex_op_lessEqual;
        Lex lex_op_greaterEqual;
        Lex lex_op_plusAssign;
        Lex lex_op_minusAssign;
        Lex lex_op_mutiplyAssign;
        Lex lex_op_divideAssign;
        Lex lex_op_modAssign;
        Lex lex_op_andAssign;
        Lex lex_op_orAssign;
        Lex lex_op_xorAssign;
        Lex lex_op_shiftLeftAssign;
        Lex lex_op_shiftRightAssign;
        Lex lex_op_lambda;
        Lex lex_op_null;

        #endregion

        private void InitializeLexer()
        {
            lexb = new LexerBuilder();

            L l_newline
                = L.Chars("\u000D\u000A\u2028\u2029");
            L l_whitespace
                = L.Chars("\u0009\u000B\u000C")
                | L.CharWhen(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator);
            lex_white = lexb.DefineLexeme("blank", true, l_whitespace | l_newline);

            L l_singleLineComment = L.Literal("//") + L.NotChars("\n\r").Repeat() + (L.Chars("\n\r") | L.CharWhen(c =>
            {
                return c == '\0';
            }));
            L l_delimitedComment = L.Literal("/*") + (L.Char('/') | L.Char('*').Repeat() + L.NotChars("/*")).Repeat() + L.Char('*') + L.Char('/');
            lex_comment = lexb.DefineLexeme("comment", true, l_singleLineComment | l_delimitedComment);

            L l_letter = L.CharWhen(c => char.IsLetter(c));
            L l_digit = L.CharWhen(c => char.IsDigit(c));
            L l_identifier = (l_letter | L.Char('_')) + (l_letter | l_digit | L.Char('_')).Repeat();
            lex_identifer = lexb.DefineLexeme("identifier", l_identifier);

            // keywords
            lex_kw_break = lexb.DefineLexeme("kw_break", L.Literal("break"));
            lex_kw_case = lexb.DefineLexeme("kw_case", L.Literal("case"));
            lex_kw_continue = lexb.DefineLexeme("kw_continue", L.Literal("continue"));
            lex_kw_default = lexb.DefineLexeme("kw_default", L.Literal("default"));
            lex_kw_do = lexb.DefineLexeme("kw_do", L.Literal("do"));
            lex_kw_else = lexb.DefineLexeme("kw_else", L.Literal("else"));
            lex_kw_false = lexb.DefineLexeme("kw_false", L.Literal("false"));
            lex_kw_for = lexb.DefineLexeme("kw_for", L.Literal("for"));
            lex_kw_goto = lexb.DefineLexeme("kw_goto", L.Literal("goto"));
            lex_kw_if = lexb.DefineLexeme("kw_if", L.Literal("if"));
            lex_kw_is = lexb.DefineLexeme("kw_is", L.Literal("is"));
            lex_kw_new = lexb.DefineLexeme("kw_new", L.Literal("new"));
            lex_kw_null = lexb.DefineLexeme("kw_null", L.Literal("null"));
            lex_kw_return = lexb.DefineLexeme("kw_return", L.Literal("return"));
            lex_kw_switch = lexb.DefineLexeme("kw_switch", L.Literal("switch"));
            lex_kw_this = lexb.DefineLexeme("kw_this", L.Literal("this"));
            lex_kw_true = lexb.DefineLexeme("kw_true", L.Literal("true"));
            lex_kw_while = lexb.DefineLexeme("kw_while", L.Literal("while"));
            lex_kw_class = lexb.DefineLexeme("kw_class", L.Literal("class"));
            lex_kw_var = lexb.DefineLexeme("kw_var", L.Literal("var"));
            lex_kw_function = lexb.DefineLexeme("kw_function", L.Literal("function"));
            lex_kw_super = lexb.DefineLexeme("kw_super", L.Literal("super"));
            lex_kw_extends = lexb.DefineLexeme("kw_extends", L.Literal("extends"));
            lex_kw_public = lexb.DefineLexeme("kw_public", L.Literal("public"));
            lex_kw_private = lexb.DefineLexeme("kw_private", L.Literal("private"));
            lex_kw_static = lexb.DefineLexeme("kw_static", L.Literal("static"));
            lex_kw_constructor = lexb.DefineLexeme("kw_constructor", L.Literal("constructor"));
            lex_kw_protected = lexb.DefineLexeme("kw_protected", L.Literal("protected"));
            lex_kw_get = lexb.DefineLexeme("kw_get", L.Literal("get"));
            lex_kw_set = lexb.DefineLexeme("kw_set", L.Literal("set"));

            lex_kw_future = lexb.DefineLexeme("kw_future",
                L.Literal("abstract") | L.Literal("as") | L.Literal("base") | L.Literal("bool")
                | L.Literal("byte") | L.Literal("catch") | L.Literal("char") | L.Literal("checked")
                | L.Literal("const") | L.Literal("decimal") | L.Literal("delegate")
                | L.Literal("double") | L.Literal("enum") | L.Literal("event") | L.Literal("explicit")
                | L.Literal("extern") | L.Literal("finally") | L.Literal("fixed") | L.Literal("float")
                | L.Literal("foreach") | L.Literal("implicit") | L.Literal("in") | L.Literal("int")
                | L.Literal("interface") | L.Literal("internal") | L.Literal("lock")
                | L.Literal("long") | L.Literal("namespace") | L.Literal("object") | L.Literal("operator")
                | L.Literal("out") | L.Literal("override") | L.Literal("params")
                | L.Literal("readonly") | L.Literal("ref")
                | L.Literal("sbyte") | L.Literal("sealed") | L.Literal("short") | L.Literal("sizeof")
                | L.Literal("stackalloc") | L.Literal("string") | L.Literal("struct")
                | L.Literal("throw") | L.Literal("try") | L.Literal("typeof") | L.Literal("uint")
                | L.Literal("ulong") | L.Literal("uncheck") | L.Literal("unsafe") | L.Literal("ushort")
                | L.Literal("using") | L.Literal("virtual") | L.Literal("void") | L.Literal("volatile")
                );
            
            L l_num_int = l_digit.Repeat(1, 0);
            L l_num_dec = L.Char('.') + l_digit.Repeat(1, 0);
            L l_num_ind = L.Chars("eE") + L.Chars("+-").Optional() + l_digit.Repeat(1, 0);
            L l_num
                = (l_num_int + l_num_dec.Optional() | l_num_dec)
                .Concat(l_num_ind.Optional());
            lex_li_num = lexb.DefineLexeme("li_number", l_num);
            L l_string1
                = L.Char('\'')
                + (L.NotChar('\'') | L.Char('\\') + L.AnyChar).Repeat()
                + L.Char('\'');
            lex_li_string1 = lexb.DefineLexeme("li_string1", l_string1);
            L l_string
                = L.Char('\"')
                + (L.NotChar('\"') | L.Char('\\') + L.AnyChar).Repeat()
                + LexicalRule.Char('\"');
            lex_li_string = lexb.DefineLexeme("li_string", l_string);

            // Operators and delimiters
            lex_op_and = lexb.DefineLexeme("op_&", L.Char('&'));
            lex_op_andAlso = lexb.DefineLexeme("op_&&", L.Literal("&&"));
            lex_op_andAssign = lexb.DefineLexeme("op_+=", L.Literal("+="));
            lex_op_assign = lexb.DefineLexeme("op_=", L.Char('='));
            lex_op_colon = lexb.DefineLexeme("op_:", L.Char(':'));
            lex_op_comma = lexb.DefineLexeme("op_,", L.Char(','));
            lex_op_decrement = lexb.DefineLexeme("op_--", L.Literal("--"));
            lex_op_divide = lexb.DefineLexeme("op_/", L.Char('/'));
            lex_op_divideAssign = lexb.DefineLexeme("op_/=", L.Literal("/="));
            lex_op_dot = lexb.DefineLexeme("op_.", L.Char('.'));
            lex_op_equal = lexb.DefineLexeme("op_==", L.Literal("=="));
            lex_op_greater = lexb.DefineLexeme("op_>", L.Char('>'));
            lex_op_greaterEqual = lexb.DefineLexeme("op_>=", L.Literal(">="));
            lex_op_increment = lexb.DefineLexeme("op_++", L.Literal("++"));
            lex_op_inverse = lexb.DefineLexeme("op_~", L.Char('~'));
            lex_op_leftBrace = lexb.DefineLexeme("op_{", L.Char('{'));
            lex_op_leftBracket = lexb.DefineLexeme("op_[", L.Char('['));
            lex_op_leftParenthesis = lexb.DefineLexeme("op_(", L.Char('('));
            lex_op_less = lexb.DefineLexeme("op_<", L.Char('<'));
            lex_op_lessEqual = lexb.DefineLexeme("op_<=", L.Literal("<="));
            lex_op_minus = lexb.DefineLexeme("op_-", L.Char('-'));
            lex_op_minusAssign = lexb.DefineLexeme("op_-=", L.Literal("-="));
            lex_op_mod = lexb.DefineLexeme("op_%", L.Char('%'));
            lex_op_modAssign = lexb.DefineLexeme("op_%=", L.Literal("%="));
            lex_op_multiply = lexb.DefineLexeme("op_*", L.Char('*'));
            lex_op_mutiplyAssign = lexb.DefineLexeme("op_*=", L.Literal("*="));
            lex_op_not = lexb.DefineLexeme("op_!", L.Char('!'));
            lex_op_notEqual = lexb.DefineLexeme("op_!=", L.Literal("!="));
            lex_op_or = lexb.DefineLexeme("op_|", L.Char('|'));
            lex_op_orAssign = lexb.DefineLexeme("op_|=", L.Literal("|="));
            lex_op_orElse = lexb.DefineLexeme("op_||", L.Literal("||"));
            lex_op_plus = lexb.DefineLexeme("op_+", L.Char('+'));
            lex_op_plusAssign = lexb.DefineLexeme("op_+=", L.Literal("+="));
            lex_op_lambda = lexb.DefineLexeme("op_=>", L.Literal("=>"));
            lex_op_question = lexb.DefineLexeme("op_?", L.Char('?'));
            lex_op_rightBrace = lexb.DefineLexeme("op_}", L.Char('}'));
            lex_op_rightBracket = lexb.DefineLexeme("op_]", L.Char(']'));
            lex_op_rightParenthesis = lexb.DefineLexeme("op_)", L.Char(')'));
            lex_op_semicolon = lexb.DefineLexeme("op_;", L.Char(';'));
            lex_op_shiftLeft = lexb.DefineLexeme("op_<<", L.Literal("<<"));
            lex_op_shiftLeftAssign = lexb.DefineLexeme("op_<<=", L.Literal("<<="));
            lex_op_shiftRight = lexb.DefineLexeme("op_>>", L.Literal(">>"));
            lex_op_shiftRightAssign = lexb.DefineLexeme("op_>>=", L.Literal(">>="));
            lex_op_xor = lexb.DefineLexeme("op_^", L.Char('^'));
            lex_op_xorAssign = lexb.DefineLexeme("op_^=", L.Literal("^="));
            lex_op_null = lexb.DefineLexeme("op_??", L.Literal("??"));  
        }

        #endregion 
    }
}
