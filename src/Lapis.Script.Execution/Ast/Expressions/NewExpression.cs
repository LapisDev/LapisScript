/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : NewExpression
 * Description : Represents an object creation expression in the syntax tree.
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
    /// Represents an object creation expression in the syntax tree.
    /// </summary>
    public class NewExpression : Expression
    {
        /// <summary>
        /// Gets the expression of the type of the object to be created.
        /// </summary>
        /// <value>The expression of the type of the object to be created.</value>
        public Expression Type { get; private set; }

        /// <summary>
        /// Gets the expressions of the parameters for the constructor invocation.
        /// </summary>
        /// <value>The expressions of the parameters for the constructor invocation.</value>
        public ExpressionCollection Parameters { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="NewExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="type">The expression of the type of the object to be created.</param>
        /// <param name="parameters">The expressions of the parameters for the constructor invocation.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <paramref name="type"/> is <see langword="null"/>.</exception>
        public NewExpression(Parser.Lexical.LinePragma linePragma, Expression type, ExpressionCollection parameters)
            : base(linePragma)
        {
            if (type == null)
                throw new ArgumentNullException();
            Type = type;
            Parameters = parameters;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("new ");
            if (Type.IsAtomOrPrimitive())
                sb.Append(Type.ToString());
            else
                sb.Append("(").Append(Type.ToString()).Append(")");
            sb.Append("(");
            if (Parameters != null)
                sb.Append(Parameters);
            sb.Append(")");           
            return sb.ToString();
        }
    }
}
