/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : FunctionExpression
 * Description : Represents a function expression in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast.Statements;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents a function expression in the syntax tree.
    /// </summary>
    public class FunctionExpression : Expression
    {
        /// <summary>
        /// Gets the parameters of the function expression.
        /// </summary>
        /// <value>The parameters of the function expression.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the statements in the function body.
        /// </summary>
        /// <value>The statements in the function body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="FunctionExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters of the function expression.</param>
        /// <param name="statements">The statements in the function body.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public FunctionExpression(
            Parser.Lexical.LinePragma linePragma, 
            ParameterCollection parameters, 
            StatementCollection statements)
            : base(linePragma)
        {
            Parameters = parameters;
            Statements = statements;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {            
            var sb = new StringBuilder();
            sb.Append("function").Append(" ")
                .Append("(");
            if (Parameters != null)
                sb.Append(Parameters);
            sb.Append(")");
            if (Statements != null)
                sb.Append("\n{\n")
                    .Append(Statements.ToString(4)).Append("\n")
                    .Append("}");
            else
                sb.Append("{ }");
            return sb.ToString();
        }
    }
}
