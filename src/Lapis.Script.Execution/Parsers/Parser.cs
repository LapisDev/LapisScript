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
    /// <summary>
    /// Implements a default <see cref="IParser"/>.
    /// </summary>
    public partial class Parser : IParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        public Parser()
        {            
            InitializeLexer();
            InitializeExpressionParsers();
            InitializeStatementParsers();
            InitializeMemberParsers();
        }

        /// <summary>
        /// Parses the specified code and return the created syntax tree.
        /// </summary>
        /// <param name="code">The string contains the specified code.</param>
        /// <returns>The syntax tree created from <paramref name="code"/>.</returns>
        /// <exception cref="LexerException">Thrown when a lexical error occurs.</exception>
        /// <exception cref="ParserException">Thrown when a grammar error occurs.</exception>
        public StatementCollection ParseStatements(string code)
        {
             using (var lexer = lexb.GetBranchedLexer(code))
             {
                 StatementCollection r;
                 p_statList
                     .Concat(
                        PS.Custom(lex =>
                        {
                            var t = lex.Peek();
                            if (t == null)
                                return Tuple.Create<bool, Statement>(true, null);
                            else
                                throw new ParserException(t.LinePragma, ExceptionResource.StatementExpected);
                        }),
                        (sta, t) => sta)
                     .TryParse(lexer, out r);
                 return r;
             }
        }    
    }
}
