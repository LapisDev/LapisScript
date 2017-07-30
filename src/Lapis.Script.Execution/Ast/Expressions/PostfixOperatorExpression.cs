/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : PostfixOperatorExpression
 * Description : Represents a postfix unary operator expression in the syntax tree.
 * Created     : 2015/6/20
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents a postfix unary operator expression in the syntax tree.
    /// </summary>
    public class PostfixOperatorExpression : Expression
    {
        /// <summary>
        /// Gets the left expression of the operator.
        /// </summary>
        /// <value>The left expression of the operator.</value>
        public Expression Left { get; private set; }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="PrefixOperatorExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="operator">The operator.</param>
        /// <param name="left">The left expression of the operator.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="operator"/> is empty or white space.</exception>
        public PostfixOperatorExpression(Parser.Lexical.LinePragma linePragma, Expression left, string @operator)
            : base(linePragma)
        {
            if (left == null || @operator == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(@operator))
                throw new ArgumentException(ExceptionResource.InvalidOperator);
            Left = left;
            Operator = @operator;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(); 
            if (Left.IsAtomOrPrimitive())
                sb.Append(Left);
            else
                sb.Append("(").Append(Left.ToString()).Append(")");
            sb.Append(Operator);
            return sb.ToString();
        }
    }
}
