/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : PrefixOperatorExpression
 * Description : Represents a prefix unary operator expression in the syntax tree.
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
    /// Represents a prefix unary operator expression in the syntax tree.
    /// </summary>
    public class PrefixOperatorExpression : Expression
    {
        /// <summary>
        /// Gets the right expression of the operator.
        /// </summary>
        /// <value>The right expression of the operator.</value>
        public Expression Right { get; private set; }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="PrefixOperatorExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="operator">The operator.</param>
        /// <param name="right">The right expression of the operator.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="operator"/> is empty or white space.</exception>
        public PrefixOperatorExpression(Parser.Lexical.LinePragma linePragma, string @operator, Expression right)
            : base(linePragma)
        {
            if (right == null || @operator == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(@operator))
                throw new ArgumentException(ExceptionResource.InvalidOperator);
            Right = right;
            Operator = @operator;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();          
            sb.Append(Operator);
            if (Right.IsAtomOrPrimitive())
                sb.Append(Right);
            else
                sb.Append("(").Append(Right.ToString()).Append(")");
            return sb.ToString();
        }
    }
}
