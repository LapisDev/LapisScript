/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : PrimitiveExpression
 * Description : Represents an expression of the primitive value in the syntax tree.
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
    /// Represents an expression of the primitive value in the syntax tree.
    /// </summary>
    public class PrimitiveExpression : Expression
    {
        /// <summary>
        /// Gets the value of the primitive expression.
        /// </summary>
        /// <value>The value of the primitive expression.</value>
        public object Value { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="PrimitiveExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="value">The value of the primitive expression.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public PrimitiveExpression(Parser.Lexical.LinePragma linePragma, object value)
            : base(linePragma)
        {
            Value = value;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            if (Value == null)
                return "null";
            else if (Value is string)
                return "\"" + ((string)Value).ConvertToEscapeChar() + "\"";
            else
                return Value.ToString();
        }
    }
}
