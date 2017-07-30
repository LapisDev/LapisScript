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

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Parser.Parsing;
using Lapis.Script.Execution.Ast;
using Lapis.Script.Execution.Ast.Statements;
using Lapis.Script.Execution.Ast.Expressions;
using Lapis.Script.Execution.Ast.Members;

using L = Lapis.Script.Parser.Lexical.LexicalRule;
using Lex = Lapis.Script.Parser.Lexical.Lexeme;
using PE = Lapis.Script.Parser.Parsing.ParsingRule<Lapis.Script.Execution.Ast.Expressions.Expression>;
using PCE = Lapis.Script.Parser.Parsing.ParsingRuleContainer<Lapis.Script.Execution.Ast.Expressions.Expression>;
using PS = Lapis.Script.Parser.Parsing.ParsingRule<Lapis.Script.Execution.Ast.Statements.Statement>;
using PCS = Lapis.Script.Parser.Parsing.ParsingRuleContainer<Lapis.Script.Execution.Ast.Statements.Statement>;
using PM = Lapis.Script.Parser.Parsing.ParsingRule<Lapis.Script.Execution.Ast.Members.Member>;
using PCM = Lapis.Script.Parser.Parsing.ParsingRuleContainer<Lapis.Script.Execution.Ast.Members.Member>;

namespace Lapis.Script.Execution.Parsers
{
    public partial class Parser
    {
        #region Grammar

        #region Expressions

        PCE p_expression = new PCE("exp");
        PCE p_exp_null = new PCE("exp_null");
        PCE p_exp_conditional = new PCE("exp_conditional");
        PCE p_exp_orElse = new PCE("exp_orElse");
        PCE p_exp_andAlso = new PCE("exp_andAlso");
        PCE p_exp_or = new PCE("exp_or");
        PCE p_exp_and = new PCE("exp_and");
        PCE p_exp_xor = new PCE("exp_xor");
        PCE p_exp_equality = new PCE("exp_equality");
        PCE p_exp_compare = new PCE("exp_compare");
        PCE p_exp_shift = new PCE("exp_shift");
        PCE p_exp_add = new PCE("exp_add");
        PCE p_exp_multiply = new PCE("exp_multiply");
        PCE p_exp_unary = new PCE("exp_unary");
        PCE p_exp_primary = new PCE("exp_primary");
        PCE p_exp_atom = new PCE("exp_atom");
        ParsingRuleContainer<ExpressionCollection> p_argList = new ParsingRuleContainer<ExpressionCollection>("argList");
        PCE p_exp_array = new PCE("exp_array");
        PCE p_exp_function = new PCE("exp_function");
        PCE p_exp_object = new PCE("exp_object");
        ParsingRuleContainer<ParameterCollection> p_paraList = new ParsingRuleContainer<ParameterCollection>("paraList");
        PCE p_exp_type = new PCE("exp_type");
        PCE p_exp_new = new PCE("exp_new");

        private void InitializeExpressionParsers()
        {
            var expRest
                = lex_op_assign.GetParsingRule()
                    .Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected),
                        (l, r) => Tuple.Create(0, l, r))
                | (lex_op_plusAssign.GetParsingRule()
                    | lex_op_minusAssign.GetParsingRule()
                    | lex_op_mutiplyAssign.GetParsingRule()
                    | lex_op_divideAssign.GetParsingRule()
                    | lex_op_modAssign.GetParsingRule()
                    | lex_op_xorAssign.GetParsingRule()
                    | lex_op_andAssign.GetParsingRule()
                    | lex_op_orAssign.GetParsingRule()
                    | lex_op_shiftLeftAssign.GetParsingRule()
                    | lex_op_shiftRightAssign.GetParsingRule())
                    .Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected),
                        (l, r) => Tuple.Create(1, l, r));

            p_expression.Content
                = p_exp_null
                .Concat(expRest | ParsingRule<Tuple<int, Token, Expression>>.Empty(Tuple.Create<int, Token, Expression>(-1, null, null)),
                    (l, r) =>
                    {
                        if (r.Item1 == -1)
                            return l;
                        else if (r.Item1 == 0)
                            return (Expression)new AssignExpression(l.LinePragma, l, r.Item3);
                        else if (r.Item1 == 1)
                            return (Expression)new BinaryOperatorExpression(l.LinePragma, l, r.Item2.Text, r.Item3);                        
                        else
                            throw new Exception();
                    });

            var nullRest
                = lex_op_null.GetParsingRule()
                .Concat(p_exp_null.OrFail(ExceptionResource.ExpressionExpected), (t, e) => e);
            p_exp_null.Content
                = p_exp_conditional
                .Concat(nullRest | PE.Empty(null),
                (t, e) =>
                {
                    if (e != null)
                        return (Expression)new BinaryOperatorExpression(t.LinePragma, t, "??", e);
                    else
                        return t;
                });

            var conditionalRest
                = lex_op_question.GetParsingRule()
                .Concat(p_exp_conditional.OrFail(ExceptionResource.ExpressionExpected), (t, e) => e)
                .Concat(lex_op_colon.GetParsingRule().OrFailExpected(":"), (t, u) => t)
                .Concat(p_exp_conditional.OrFail(ExceptionResource.ExpressionExpected), (t, e) => Tuple.Create(t, e));
            p_exp_conditional.Content
                = p_exp_orElse
                .Concat(conditionalRest | ParsingRule<Tuple<Expression,Expression >>.Empty (null) , 
                    (t, e) =>{
                        if (e != null)
                            return (Expression)new TernaryOperatorExpression(t.LinePragma, t, "?", e.Item1, ":", e.Item2);
                        else
                            return t;
                    });

            var orElseRest
                = lex_op_orElse.GetParsingRule()
                .Concat(p_exp_andAlso.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_orElse.Content
                = p_exp_andAlso
                .Concat(
                    orElseRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var andAlsoRest
               = lex_op_andAlso.GetParsingRule()
               .Concat(p_exp_or.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_andAlso.Content
                = p_exp_or
                .Concat(
                    andAlsoRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var orRest
              = lex_op_or.GetParsingRule()
              .Concat(p_exp_xor.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_or.Content
                = p_exp_xor
                .Concat(
                    orRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var xorRest
              = lex_op_xor.GetParsingRule()
              .Concat(p_exp_and.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_xor.Content
                = p_exp_and
                .Concat(
                    xorRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var andRest
              = lex_op_and.GetParsingRule()
              .Concat(p_exp_equality.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_and.Content
                = p_exp_equality
                .Concat(
                    andRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var equalityRest
                  = (lex_op_equal.GetParsingRule()
                    | lex_op_notEqual.GetParsingRule())
                  .Concat(p_exp_compare.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_equality.Content
                = p_exp_compare
                .Concat(
                    equalityRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var compareRest
                 = (lex_op_less.GetParsingRule()
                   | lex_op_greater.GetParsingRule()
                   | lex_op_lessEqual.GetParsingRule()
                   | lex_op_greaterEqual.GetParsingRule()
                   | lex_kw_is.GetParsingRule())
                 .Concat(p_exp_shift.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_compare.Content
                = p_exp_shift
                .Concat(
                    compareRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var shiftRest
                = (lex_op_shiftLeft.GetParsingRule()
                  | lex_op_shiftRight.GetParsingRule())
                .Concat(p_exp_add.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_shift.Content
                = p_exp_add
                .Concat(
                    shiftRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var addRest
                = (lex_op_plus.GetParsingRule()
                  | lex_op_minus.GetParsingRule())
                .Concat(p_exp_multiply.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_add.Content
                = p_exp_multiply
                .Concat(
                    addRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            var multiplyRest
                = (lex_op_multiply.GetParsingRule()
                  | lex_op_divide.GetParsingRule()
                  | lex_op_mod.GetParsingRule())
                .Concat(p_exp_unary.OrFail(ExceptionResource.ExpressionExpected), (o, r) => Tuple.Create(o.Text, r));
            p_exp_multiply.Content
                = p_exp_unary
                .Concat(
                    multiplyRest.Repeat(),
                    (f, i) => i.Aggregate(f, (a, b) => new BinaryOperatorExpression(a.LinePragma, a, b.Item1, b.Item2)));

            p_exp_unary.Content
                = (lex_op_not.GetParsingRule()
                  | lex_op_inverse.GetParsingRule()
                  | lex_op_increment.GetParsingRule()
                  | lex_op_decrement.GetParsingRule()
                  | lex_op_plus.GetParsingRule()
                  | lex_op_minus.GetParsingRule())
                  .Concat(p_exp_unary.OrFail(ExceptionResource.ExpressionExpected), (o, r) => (Expression)new PrefixOperatorExpression(o.LinePragma, o.Text, r))
                | p_exp_primary;
            

            var primaryRest
                = lex_op_dot.GetParsingRule()
                    .Concat(lex_identifer.GetParsingRule().OrFail(ExceptionResource.IdentifierExpected),
                        (dot, member) => Tuple.Create(dot, (object)member))
                | lex_op_leftParenthesis.GetParsingRule()
                    .Concat(
                        p_argList | ParsingRule<ExpressionCollection>.Empty(new ExpressionCollection()),
                        (id, args) => Tuple.Create(id, (object)args))
                    .Concat(
                        lex_op_rightParenthesis.GetParsingRule().OrFailExpected(")"),
                        (e, r) => e)
                | lex_op_leftBracket.GetParsingRule()
                    .Concat(
                        p_argList | ParsingRule<ExpressionCollection>.Empty(new ExpressionCollection()),
                        (id, args) => Tuple.Create(id, (object)args))
                    .Concat(
                        lex_op_rightBracket.GetParsingRule().OrFailExpected("]"),
                        (e, r) => e)
                | (lex_op_increment.GetParsingRule()| lex_op_decrement.GetParsingRule())
                    .Map(op => Tuple.Create(op, (object)null));

            p_exp_primary.Content
                = p_exp_atom
                    .Concat(
                        primaryRest.Repeat(),
                        (e, i) =>
                        {
                            Expression exp = e;
                            foreach (var t in i)
                            {
                                if (t.Item1.Lexeme == lex_op_dot)
                                    exp = new MemberReferenceExpression(exp.LinePragma, exp, ((Token)t.Item2).Text);
                                else if (t.Item1.Lexeme == lex_op_leftParenthesis)
                                    exp = new FunctionInvokeExpression(exp.LinePragma, exp, (ExpressionCollection)t.Item2);
                                else if (t.Item1.Lexeme == lex_op_leftBracket)
                                    exp = new ArrayIndexerExpression(exp.LinePragma, exp, (ExpressionCollection)t.Item2);
                                else if (t.Item1.Lexeme == lex_op_increment ||
                                         t.Item1.Lexeme == lex_op_decrement)
                                    exp = new PostfixOperatorExpression(exp.LinePragma, exp, t.Item1.Text);
                                else
                                    throw new Exception();
                            }
                            return exp;
                        });


            p_exp_new.Content
                = lex_kw_new.GetParsingRule()
                .Concat(p_exp_type.OrFail(ExceptionResource.TypeExpected), (t, e) => Tuple.Create(t, e))
                .Concat(
                    lex_op_leftParenthesis.GetParsingRule()
                    .Concat(
                        p_argList | ParsingRule<ExpressionCollection>.Empty(new ExpressionCollection()),
                        (id, args) => args)
                    .Concat(
                        lex_op_rightParenthesis.GetParsingRule().OrFailExpected(")"),
                        (e, r) => e)
                    | ParsingRule<ExpressionCollection>.Empty(new ExpressionCollection()),
                        (t, p) => (Expression)new NewExpression(t.Item1.LinePragma, t.Item2, p));

            p_exp_atom.Content
                = p_exp_array | p_exp_function | p_exp_object | p_exp_new
                | lex_identifer.GetParsingRule(t => (Expression)new VariableReferenceExpression(t.LinePragma, t.Text))
                | lex_kw_false.GetParsingRule(t => (Expression)new PrimitiveExpression(t.LinePragma, bool.Parse(t.Text)))
                | lex_kw_true.GetParsingRule(t => (Expression)new PrimitiveExpression(t.LinePragma, bool.Parse(t.Text)))
                | lex_li_string1.GetParsingRule(t => (Expression)new PrimitiveExpression(t.LinePragma, t.Text.Trim('\'').Replace("\\\'", "\'")))
                | lex_li_string.GetParsingRule(t =>
                {
                    string str;
                    try
                    {
                        str = t.Text.Trim('\"').ConvertFromEscapeChar();
                    }
                    catch (ParserException ex)
                    {
                        var lp = new LinePragma(t.LinePragma.Line + ex.LinePragma.Line - 1, t.LinePragma.Span + ex.LinePragma.Span);
                        throw new ParserException(lp, ExceptionResource.UnrecognizedEscapeCharacter);
                    }
                    return (Expression)new PrimitiveExpression(t.LinePragma, str);
                })
                | lex_li_num.GetParsingRule(t => (Expression)new PrimitiveExpression(t.LinePragma, double.Parse(t.Text)))
                | lex_kw_null.GetParsingRule(t => (Expression)new PrimitiveExpression(t.LinePragma, null))
                | lex_kw_this.GetParsingRule(t => (Expression)new ThisReferenceExpression(t.LinePragma))
                | lex_kw_super.GetParsingRule(t => (Expression)new SuperReferenceExpression(t.LinePragma))
                | lex_op_leftParenthesis.GetParsingRule()
                    .Concat(
                        p_expression,
                        (l, e) => e)
                    .Concat(
                        lex_op_rightParenthesis.GetParsingRule().OrFailExpected(")"),
                        (e, r) => e);

            p_argList.Content
                = p_expression
                    .Concat(
                        lex_op_comma.GetParsingRule().Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected), (comma, e) => e).Repeat(),
                        (e, i) =>
                        {
                            var list = i.ToList();
                            list.Insert(0, e);
                            return new ExpressionCollection(list);
                        });

            p_exp_array.Content
                = lex_op_leftBracket.GetParsingRule()
                .Concat(
                        p_argList | ParsingRule<ExpressionCollection>.Empty(new ExpressionCollection()),
                        (t, eles) => Tuple.Create(t, eles))
                .Concat(
                        lex_op_rightBracket.GetParsingRule().OrFailExpected("]"),
                        (eles, t) => (Expression)new ArrayExpression(eles.Item1.LinePragma, eles.Item2));

            var para
                = lex_identifer.GetParsingRule()
                .Concat(
                    lex_op_assign.GetParsingRule().Concat(p_expression, (t, e) => e)
                    | PE.Empty(null),
                    (t, e) => e == null ? new Parameter(t.LinePragma, t.Text) : new Parameter(t.LinePragma, t.Text, e));

            p_paraList.Content
                = para.Concat(
                    lex_op_comma.GetParsingRule().Concat(para.OrFail(ExceptionResource.IdentifierExpected), (comma, p) => p).Repeat(),
                    (p, i) =>
                    {
                        var list = i.ToList();
                        list.Insert(0, p);
                        return new ParameterCollection(list);
                    })
                | ParsingRule<ParameterCollection>.Empty(null);
                        

            p_paras.Content
                = lex_op_leftParenthesis.GetParsingRule()
                .Concat(p_paraList, (t, paras) => paras)
                .Concat(lex_op_rightParenthesis.GetParsingRule(), (paras, t) => paras);

            var funcExp
                = lex_kw_function.GetParsingRule()
                .Concat(p_paras.OrFail(ExceptionResource.ParemetersExpected), (t, paras) => Tuple.Create(t, paras))
                .Concat(p_stats.OrFail(ExceptionResource.StatementsExpected), (paras, stats) => (Expression)new FunctionExpression(paras.Item1.LinePragma, paras.Item2, stats));

            var lambda_paras
                = p_paras
                | para.Map(p => new ParameterCollection(p));

            var lambda_stats
                = p_stats
                | p_expression.Map(e => new StatementCollection(new ReturnStatement(e.LinePragma, e)));

            var lambdaExp
                = lambda_paras
                .Concat(lex_op_lambda.GetParsingRule(), (paras, t) => Tuple.Create(paras, t))
                .Concat(lambda_stats.OrFail(ExceptionResource.StatementsExpected), (paras, stats) => (Expression)new FunctionExpression(paras.Item2.LinePragma, paras.Item1, stats));

            p_exp_function.Content
                = funcExp | lambdaExp;

            var objectMember
                = lex_identifer.GetParsingRule()
                .Concat(lex_op_colon.GetParsingRule(), (t, u) => t)
                .Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected), (t, e) => Tuple.Create(t, e));
            var objectMemberList
                = objectMember
                .Concat(
                    lex_op_comma.GetParsingRule()
                    .Concat(objectMember.OrFail(ExceptionResource.IdentifierExpected), (t, m) => m)
                    .Repeat(),
                    (t, i) =>
                    {
                        var list = i.ToList();
                        list.Insert(0, t);
                        return list;
                    });

            p_exp_object.Content
                = lex_op_leftBrace.GetParsingRule()
                .Concat(objectMemberList | ParsingRule<List<Tuple<Token, Expression>>>.Empty(new List<Tuple<Token, Expression>>()), 
                    (t, m) => Tuple.Create(t, m))
                .Concat(lex_op_rightBrace.GetParsingRule().OrFailExpected("}"), (t, u) =>
                {
                    var dict = new List<KeyValuePair<string, Expression>>();
                    foreach (var m in t.Item2)
                        dict.Add(new KeyValuePair<string, Expression>(m.Item1.Text, m.Item2));
                    return (Expression)new ObjectExpression(t.Item1.LinePragma, dict);
                });

            var typeRest
                = lex_op_dot.GetParsingRule()
                .Concat(lex_identifer.GetParsingRule(),
                    (dot, id) => id);
            p_exp_type.Content
                = lex_identifer.GetParsingRule(id => new VariableReferenceExpression(id.LinePragma, id.Text))
                    .Concat(
                        typeRest.Repeat(),
                        (e, i) =>
                        {
                            Expression exp = e;
                            foreach (var t in i)
                                exp = new MemberReferenceExpression(exp.LinePragma, exp, t.Text);
                            return exp;
                        });
        }

        #endregion

        #region Statements                

        PCS p_statement = new PCS("statement");
        PCS p_sta_exp = new PCS("sta_exp");
        PCS p_sta_var = new PCS("sta_var");
        PCS p_sta_func = new PCS("sta_func");
        PCS p_sta_block = new PCS("sta_block");
        PCS p_sta_if = new PCS("sta_if");
        PCS p_sta_while = new PCS("sta_while");
        PCS p_sta_do = new PCS("sta_do");
        PCS p_sta_for = new PCS("sta_for");
        PCS p_sta_switch = new PCS("sta_switch");
        PCS p_sta_goto = new PCS("sta_goto");
        PCS p_sta_label = new PCS("sta_label");
        PCS p_sta_return = new PCS("sta_return");
        PCS p_sta_break = new PCS("sta_break");
        PCS p_sta_continue = new PCS("sta_continue");
        PCS p_statement_semicolon = new PCS("statement_semicolon");

        PCS p_sta_class = new PCS("sta_class");

        ParsingRuleContainer<StatementCollection> p_statList = new ParsingRuleContainer<StatementCollection>("statList");

        ParsingRuleContainer<ParameterCollection> p_paras = new ParsingRuleContainer<ParameterCollection>("paras");
        ParsingRuleContainer<StatementCollection> p_stats = new ParsingRuleContainer<StatementCollection>("stats");
    
        private void InitializeStatementParsers()
        {         
            p_sta_exp.Content
                = p_expression
                .Map(e => (Statement)new ExpressionStatement(e.LinePragma, e));

            p_sta_var.Content
                = lex_kw_var.GetParsingRule()
                .Concat(lex_identifer.GetParsingRule().OrFail(ExceptionResource.IdentifierExpected), (t, id) => Tuple.Create(t, id))
                .Concat(
                    lex_op_assign.GetParsingRule()
                    .Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected), (t, e) => e)
                    | PCE.Empty(null),
                    (t, e) => (Statement)new VariableDeclarationStatement(t.Item1.LinePragma, t.Item2.Text, e));

            p_sta_func.Content
                = lex_kw_function.GetParsingRule()
                .Concat(lex_identifer.GetParsingRule().OrFail(ExceptionResource.IdentifierExpected), (t, id) => Tuple.Create(t, id))
                .Concat(p_paras.OrFail(ExceptionResource.ParemetersExpected), (t, paras) => Tuple.Create(t, paras))
                .Concat(p_stats.OrFail(ExceptionResource.StatementsExpected), (t, stats) => (Statement)new FunctionDeclarationStatement(t.Item1.Item1.LinePragma, t.Item1.Item2.Text, t.Item2, stats));

            p_sta_return.Content
                = lex_kw_return.GetParsingRule()
                .Concat(p_expression | PE.Empty(null),
                    (t, e) => (Statement)new ReturnStatement(t.LinePragma, e));

            p_sta_break.Content
                = lex_kw_break.GetParsingRule(t => (Statement)new BreakStatement(t.LinePragma));

            p_sta_continue.Content
                = lex_kw_continue.GetParsingRule(t => (Statement)new ContinueStatement(t.LinePragma));

            p_sta_goto.Content
              = lex_kw_goto.GetParsingRule()
              .Concat(lex_identifer.GetParsingRule().OrFail(ExceptionResource.IdentifierExpected), (t, id) => (Statement)new GotoStatement(t.LinePragma, id.Text));

            p_sta_label.Content
                = lex_identifer.GetParsingRule()
                .Concat(lex_op_colon.GetParsingRule(), (id, t) => id)
                .Concat(p_statement_semicolon.OrFail(ExceptionResource.StatementExpected), (id, s) => (Statement)new LabelStatement(id.LinePragma, id.Text, s));

            var cons
                = lex_op_leftParenthesis.GetParsingRule().OrFailExpected("(")
                .Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected), (t, e) => e)
                .Concat(lex_op_rightParenthesis.GetParsingRule().OrFailExpected(")"), (t, u) => t);

            var states
                = p_stats
                | p_statement_semicolon.Map(s => s == null ? new StatementCollection() : new StatementCollection(s));

            p_sta_if.Content
                = lex_kw_if.GetParsingRule()
                .Concat(cons, (t, con) => Tuple.Create(t, con))
                .Concat(states.OrFail(ExceptionResource.StatementExpected), (t, s) => Tuple.Create(t, s))
                .Concat(
                    lex_kw_else.GetParsingRule()
                    .Concat(states.OrFail(ExceptionResource.StatementExpected), (t, s) => s)
                    | ParsingRule<StatementCollection>.Empty(null),
                    (t, s) => (Statement)new IfStatement(t.Item1.Item1.LinePragma, t.Item1.Item2, t.Item2, s)
                );                      
           

            p_sta_while.Content
                = lex_kw_while.GetParsingRule()
                .Concat(cons, (t, con) => Tuple.Create(t, con))
                .Concat(states.OrFail(ExceptionResource.StatementExpected),
                    (t, s) => (Statement)new WhileStatement(t.Item1.LinePragma, t.Item2, s));

            p_sta_for.Content
                = lex_kw_for.GetParsingRule()
                .Concat(lex_op_leftParenthesis.GetParsingRule(), (t, s) => t)
                .Concat(p_statement | PS.Empty(null), (t, s) => Tuple.Create(t, s))
                .Concat(lex_op_semicolon.GetParsingRule().OrFailExpected(";"), (t, s) => t)
                .Concat(p_expression | PE.Empty(null), (t, c) => Tuple.Create(t, c))
                .Concat(lex_op_semicolon.GetParsingRule().OrFailExpected(";"), (t, s) => t)
                .Concat(p_statement | PS.Empty(null), (t, s) => Tuple.Create(t, s))
                .Concat(lex_op_rightParenthesis.GetParsingRule().OrFailExpected(")"), (t, u) => t)
                .Concat(p_stats.OrFail(ExceptionResource.StatementExpected),
                    (t, s) =>
                        (Statement)new ForStatement(t.Item1.Item1.Item1.LinePragma, t.Item1.Item1.Item2, t.Item1.Item2, t.Item2, s));

            p_sta_do.Content
               = lex_kw_do.GetParsingRule()
               .Concat(states.OrFail(ExceptionResource.StatementExpected), (t, s) => Tuple.Create(t, s))
               .Concat(lex_kw_while.GetParsingRule().OrFailExpected("while"), (s, t) => s)
               .Concat(cons, (t, c) => (Statement)new DoWhileStatement(t.Item1.LinePragma, c, t.Item2));


            var switch_case
                = lex_kw_case.GetParsingRule()
                .Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected), (t, e) => Tuple.Create(t, e))
                .Concat(lex_op_colon.GetParsingRule().OrFailExpected(":"), (t, u) => t)
                .Concat(p_statList, (t, s) => new SwitchCase(t.Item1.LinePragma, t.Item2, s));      

            var switch_default
                = lex_kw_default.GetParsingRule()
                .Concat(lex_op_colon.GetParsingRule(), (t, u) => t)
                .Concat(p_statList | ParsingRule<StatementCollection>.Empty(null), (t, s) => s);

            p_sta_switch.Content
                = lex_kw_switch.GetParsingRule()
                .Concat(cons, (t, e) => Tuple.Create(t, e))
                .Concat(lex_op_leftBrace.GetParsingRule().OrFailExpected("{"), (t, u) => t)
                .Concat(switch_case.Repeat(), (t, c) => Tuple.Create(t, new SwitchCaseCollection(c)))
                .Concat(switch_default, (t, d) => Tuple.Create(t, d))
                .Concat(lex_op_rightBrace.GetParsingRule().OrFailExpected("}"), (t, r) =>
                    (Statement)new SwitchStatement(t.Item1.Item1.Item1.LinePragma,
                        t.Item1.Item1.Item2, t.Item1.Item2, t.Item2));


            p_statement.Content
                = p_sta_block | p_sta_for | p_sta_if | p_sta_while | p_sta_switch | p_sta_func | p_sta_class | p_sta_label
                | p_sta_var | p_sta_do | p_sta_break | p_sta_continue | p_sta_return | p_sta_goto | p_sta_exp;

            var semicolonInsert
                = PS.Custom(lex =>
                {                    
                    Statement s;
                    if ((p_sta_var | p_sta_do | p_sta_break | p_sta_continue | p_sta_return | p_sta_goto | p_sta_exp)
                        .TryParse(lex, out s))
                    {
                        var t = lex.Peek();
                        if (t == null)
                            return Tuple.Create(true, s);
                        else if (t.Lexeme == lex_op_semicolon)
                        {
                            lex.Read();
                            return Tuple.Create(true, s);
                        }
                        else if (t.Lexeme == lex_op_rightBrace)                       
                            return Tuple.Create(true, s);                       
                        else if (t.LinePragma.Line > s.LinePragma.Line)
                            return Tuple.Create(true, s);
                        else
                            Util.Fail<Statement>(string.Format(ExceptionResource.Expected, ";")).TryParse(lex, out s);
                    }                  
                    return Tuple.Create(false, (Statement)null);
                });


            p_statement_semicolon.Content
                = p_sta_block | p_sta_for | p_sta_if | p_sta_while | p_sta_switch | p_sta_func | p_sta_class | p_sta_label
                | semicolonInsert
                | PS.Empty(null).Concat(lex_op_semicolon.GetParsingRule(),
                        (t, s) => t);

            p_statList.Content
                = p_statement_semicolon.Repeat(i => new StatementCollection(i.Where(s => s != null)));

            p_stats.Content
                = lex_op_leftBrace.GetParsingRule()
                .Concat(p_statList, (t, s) => s)
                .Concat(lex_op_rightBrace.GetParsingRule().OrFailExpected("}"), (s, t) => s);

            p_sta_block.Content
                = lex_op_leftBrace.GetParsingRule()
                .Concat(p_statList, (t, s) => Tuple.Create(t, s))
                .Concat(lex_op_rightBrace.GetParsingRule().OrFailExpected("}"), (s, t) => (Statement)new BlockStatement(s.Item1.LinePragma, s.Item2));

            var classExtends
                = lex_kw_extends.GetParsingRule()
                .Concat(p_exp_type.OrFail(ExceptionResource.TypeExpected), (t, e) => e);
            p_sta_class.Content
                = lex_kw_class.GetParsingRule()
                .Concat(lex_identifer.GetParsingRule().OrFail(ExceptionResource.IdentifierExpected), (t, id) => Tuple.Create(t, id))
                .Concat(classExtends | PE.Empty(null), (t, e) => Tuple.Create(t, e))
                .Concat(lex_op_leftBrace.GetParsingRule().OrFailExpected("{"), (t, u) => t)
                .Concat(p_members.OrFail(ExceptionResource.MemberExpected), (t, m) => Tuple.Create(t, m))
                .Concat(lex_op_rightBrace.GetParsingRule().OrFail(ExceptionResource.MemberExpected),
                    (t, u) => (Statement)new ClassDeclarationStatement(
                        t.Item1.Item1.Item1.LinePragma,
                        t.Item1.Item1.Item2.Text,
                        t.Item1.Item2, t.Item2));        
        }

        #endregion
        
        #region Members

        PCM p_member = new PCM("member");
       
        ParsingRuleContainer<MemberCollection> p_members = new ParsingRuleContainer<MemberCollection>("members");

        ParsingRuleContainer<LinePragma> p_linePragma = new ParsingRuleContainer<LinePragma>("linePragma");
        
        private void InitializeMemberParsers()
        {   
            var modifier
                = lex_kw_public.GetParsingRule(t => Modifier.Public) 
                | lex_kw_private.GetParsingRule(t => Modifier.Private) 
                | lex_kw_protected.GetParsingRule(t=>Modifier.Protected)
                | ParsingRule<Modifier>.Empty(Modifier.Public);

            var mem_modifier
                = lex_kw_static.GetParsingRule(t => true)
                    .Concat(modifier, (t, u) => Tuple.Create(t, u))
                | modifier
                    .Concat(lex_kw_static.GetParsingRule(t => true) | ParsingRule<bool>.Empty(false), (u, t) => Tuple.Create(t, u))
                | ParsingRule<Tuple<bool, Modifier>>.Empty(Tuple.Create(false, Modifier.Public));

            var mem_field
                = (lex_op_assign.GetParsingRule()
                        .Concat(p_expression.OrFail(ExceptionResource.ExpressionExpected), (t, e) => e)
                  | PCE.Empty(null))
                .Map(e => Tuple.Create(0, e, (ParameterCollection)null, (StatementCollection)null,
                        (Tuple<Accessor, Accessor>)null));

            var mem_method
                = p_paras
                    .Concat(p_stats.OrFail(ExceptionResource.StatementsExpected),
                        (t, stats) => Tuple.Create(1, (Expression)null, t, stats,
                            (Tuple<Accessor, Accessor>)null));

            var mem_constructor
                 = lex_kw_constructor.GetParsingRule()
                     .Concat(p_paras.OrFail(ExceptionResource.ParemetersExpected), (t, paras) => paras)
                     .Concat(p_stats.OrFail(ExceptionResource.StatementsExpected), 
                        (t, stats)=> Tuple.Create(2, (Expression)null, t, stats,
                            (Tuple<Accessor, Accessor>)null));

            var modifier1
                = lex_kw_public.GetParsingRule(t => (Modifier?)Modifier.Public)
                | lex_kw_private.GetParsingRule(t => (Modifier?)Modifier.Private)
                | lex_kw_protected.GetParsingRule(t => (Modifier?)Modifier.Protected)
                | ParsingRule<Modifier?>.Empty(null);

            var getter
                = modifier1
                .Concat(lex_kw_get.GetParsingRule(), (t, g) => Tuple.Create(t, g))
                .Concat(
                    lex_op_semicolon.GetParsingRule(t => (StatementCollection)null)
                    | p_stats.OrFail(ExceptionResource.StatementsExpected),
                    (t, stats) => new Accessor(t.Item2.LinePragma, t.Item1, stats));
            var setter
              = modifier1
              .Concat(lex_kw_set.GetParsingRule(), (t, g) => Tuple.Create(t, g))
              .Concat(
                    lex_op_semicolon.GetParsingRule(t => (StatementCollection)null)
                    | p_stats.OrFail(ExceptionResource.StatementsExpected),
                    (t, stats) => new Accessor(t.Item2.LinePragma, t.Item1, stats));
            var prop_stats
                = getter.Concat(setter | ParsingRule<Accessor>.Empty(null), (g, s) => Tuple.Create(g, s))
                | setter.Concat(getter | ParsingRule<Accessor>.Empty(null), (s, g) => Tuple.Create(g, s));

            var mem_property
              = lex_op_leftBrace.GetParsingRule()
               .Concat(prop_stats, (t, s) => s)
               .Concat(lex_op_rightBrace.GetParsingRule().OrFailExpected("}"), (t, b) => t)
               .Map(t =>
               {
                   return Tuple.Create(3, (Expression)null, (ParameterCollection)null, (StatementCollection)null, t);
               });

            ParsingRule<Tuple<int, Expression, ParameterCollection, StatementCollection, Tuple<Accessor, Accessor>>> mem_indexer
                = lex_kw_this.GetParsingRule()
                .Concat(lex_op_leftBracket.GetParsingRule(), (t, b) => t)
                .Concat(p_paraList, (t, p) => p)
                .Concat(lex_op_rightBracket.GetParsingRule().OrFailExpected("]"), (t, b) => t)
                .Concat(lex_op_leftBrace.GetParsingRule().OrFailExpected("{"), (t, b) => t)
                .Concat(prop_stats, (t, s) => Tuple.Create(t, s))
                .Concat(lex_op_rightBrace.GetParsingRule().OrFailExpected("}"), (t, b) => t)
                .Map(t =>
                {
                    return Tuple.Create(4, (Expression)null, t.Item1, (StatementCollection)null, t.Item2);
                });


            p_linePragma.Content
                = ParsingRuleContainer<LinePragma>.Custom(lexer =>
                {
                    var t = lexer.Peek();
                    if (t != null)
                        return Tuple.Create(true, t.LinePragma);
                    else
                        return Tuple.Create(false, (LinePragma)null);
                });

            p_member.Content
                = p_linePragma
                .Concat(mem_modifier, (t, m) => Tuple.Create(t, m))
                .Concat(
                    mem_constructor.Map((t) => Tuple.Create((Token)null, t))
                    | lex_identifer.GetParsingRule()
                            .Concat(mem_method | mem_property | mem_field,
                                (t, m) => Tuple.Create(t, m))
                    | mem_indexer.Map(t => Tuple.Create((Token)null, t)) ,
                    (t, m) =>
                    {
                        var lp = t.Item1;
                        var mod = t.Item2;
                        var type = m.Item2.Item1;
                        var id = m.Item1;
                        var exp = m.Item2.Item2;
                        var para = m.Item2.Item3;
                        var stat = m.Item2.Item4;
                        var getset = m.Item2.Item5;
                        if (type == 0)
                            return (Member)new Field(lp, mod.Item1, mod.Item2, id.Text, exp);
                        else if (type == 1)
                            return (Member)new Method(lp, mod.Item1, mod.Item2, id.Text, para, stat);
                        else if (type == 2)
                            return (Member)new Constructor(lp, mod.Item1, mod.Item2, para, stat);
                        else if (type == 3)
                        {
                            var g = getset.Item1;
                            var s = getset.Item2;
                            return (Member)new Property(lp, mod.Item1, mod.Item2, id.Text, g, s);
                        }
                        else if (type == 4)
                        {
                            var g = getset.Item1;
                            var s = getset.Item2;
                            return (Member)new Indexer(lp, mod.Item1, mod.Item2, para, g, s);
                        }
                        else
                            throw new Exception();
                    });
                
            var member_semicolon
               = PM.Custom(lex =>
               {
                   Member m;
                   if (p_member.TryParse(lex, out m))
                   {
                       if (m is Field)
                       {
                           var t = lex.Peek();
                           if (t == null)
                               return Tuple.Create(true, m);
                           else if (t.Lexeme == lex_op_semicolon)
                           {
                               lex.Read();
                               return Tuple.Create(true, m);
                           }
                           else if (t.Lexeme == lex_op_rightBrace)                           
                               return Tuple.Create(true, m);                           
                           else if (t.LinePragma.Line > m.LinePragma.Line)
                               return Tuple.Create(true, m);
                           else
                               Util.Fail<Member>(string.Format(ExceptionResource.Expected, ";")).TryParse(lex, out m);
                       }
                       else
                           return Tuple.Create(true, m);
                   }
                   return Tuple.Create(false, (Member)null);
               });    			   

            p_members.Content
                = member_semicolon.Repeat(i => new MemberCollection(i));
        }

        #endregion

        #endregion
    }
}
