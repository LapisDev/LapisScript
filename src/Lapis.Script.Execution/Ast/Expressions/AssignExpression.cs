/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : AssignExpression
 * Description : Represents an assignment expression in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents an assignment expression in the syntax tree.
    /// </summary>
    public class AssignExpression : Expression
    {
        /// <summary>
        /// Gets the left expression of the assignment.
        /// </summary>
        /// <value>The left expression of the assignment.</value>
        public Expression Left { get; private set; }

        /// <summary>
        /// Gets the right expression of the assignment.
        /// </summary>
        /// <value>The right expression of the assignment.</value>
        public Expression Right { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="AssignExpression"/> class using the specified left and right expressions.
        /// </summary>
        /// <param name="left">The left expression of the assignment.</param>
        /// <param name="right">The right expression of the assignment.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public AssignExpression(Parser.Lexical.LinePragma linePragma, Expression left, Expression right)
            : base(linePragma)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Left);
            sb.Append(" = ");
            sb.Append(Right);
            return sb.ToString();
        }
    }
}
