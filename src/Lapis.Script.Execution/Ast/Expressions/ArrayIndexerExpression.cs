/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ArrayIndexerExpression
 * Description : Represents an array indexer expression in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents an array indexer expression in the syntax tree.
    /// </summary>
    public class ArrayIndexerExpression : Expression
    {
        /// <summary>
        /// Gets the expression of the target object of the array indexer.
        /// </summary>
        /// <value>The expression of the target object of the array indexer.</value>
        public Expression Target { get; private set; }

        /// <summary>
        /// Gets the expressions of the parameters of the array indexer.
        /// </summary>
        /// <value>The expressions of the parameters of the array indexer.</value>
        public ExpressionCollection Indices { get; private set; }

        /// <summary>
        /// Initialize an new instance of the <see cref="ArrayIndexerExpression"/> class using the specified target and parameters.
        /// </summary>
        /// <param name="target">The expression of the target object of the array indexer.</param>
        /// <param name="indices">The expressions of the parameters of the array indexer.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public ArrayIndexerExpression(LinePragma linePragma, Expression target, ExpressionCollection indices)
            : base(linePragma)
        {
            if (target == null || indices == null)
                throw new ArgumentNullException();
            Target = target;
            Indices = indices;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Target.IsAtomOrPrimitive())
                sb.Append(Target);
            else
                sb.Append("(").Append(Target.ToString()).Append(")");
            sb.Append("[").Append(Indices.ToString()).Append("]");
            return sb.ToString();
        }
    }
}
