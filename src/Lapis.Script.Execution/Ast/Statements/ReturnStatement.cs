/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ReturnStatement
 * Description : Represents a return statement in the syntax tree.
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
    /// Represents a <c>return</c> statement in the syntax tree.
    /// </summary>
    public class ReturnStatement : Statement
    {
        /// <summary>
        /// Gets the expression of the return value.
        /// </summary>
        /// <value>The expression of the return value.</value>
        public Expression Expression { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ReturnStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="expression">The expression of the return value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="label"/> is <see langword="null"/>.</exception>
        public ReturnStatement(LinePragma linePragma, Expression expression)
            : base(linePragma)
        {
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
            if (Expression != null)
                return ind + "return " + Expression.ToString() + ";";
            else
                return ind + "return;";
        }
    }
}
