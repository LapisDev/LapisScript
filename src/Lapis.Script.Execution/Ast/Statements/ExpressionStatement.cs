/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ExpressionStatement
 * Description : Represents a statement containing an expression in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Ast.Statements
{
    /// <summary>
    /// Represents a statement containing an expression in the syntax tree.
    /// </summary>
    public class ExpressionStatement : Statement
    {
        /// <summary>
        /// Gets the expression in the statement.
        /// </summary>
        /// <value>The expression in the statement.</value>
        public Expression Expression { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ExpressionStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="expression">The expression in the statement.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="expression"/> is <see langword="null"/>.</exception>
        public ExpressionStatement(LinePragma linePragma, Expression expression)
            : base(linePragma)
        {
            if (expression == null)
                throw new ArgumentNullException();
            Expression = expression;
        }

        /// <summary>
        /// Returns the string representation of the statement.
        /// </summary>
        /// <returns>Returns the string representation of the statement.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public override string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            return ind + Expression + ";";
        }
    }
}
