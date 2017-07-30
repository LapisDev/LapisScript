/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : FunctionInvokeExpression
 * Description : Represents a function invocation expression in the syntax tree.
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
    /// Represents a function invocation expression in the syntax tree.
    /// </summary>
    public class FunctionInvokeExpression : Expression
    {
        /// <summary>
        /// Gets the expression of the target function.
        /// </summary>
        /// <value>The expression of the target function.</value>
        public Expression Target { get; private set; }

        /// <summary>
        /// Gets the expressions of the parameters for the function invocation.
        /// </summary>
        /// <value>The expressions of the parameters for the function invocation.</value>
        public ExpressionCollection Parameters { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="FunctionInvokeExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="target">The expression of the target function.</param>
        /// <param name="parameters">The expressions of the parameters for the function invocation.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is<see langword="null"/>, or <paramref name="target"/> is <see langword="null"/>.</exception>
        public FunctionInvokeExpression(
            Parser.Lexical.LinePragma linePragma,
            Expression target,
            ExpressionCollection parameters)
            : base(linePragma)
        {
            if (target == null)
                throw new ArgumentNullException();
            Target = target;
            Parameters = parameters;
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
            sb.Append("(").Append(Parameters.ToString()).Append(")");
            return sb.ToString();
        }
    }
}
