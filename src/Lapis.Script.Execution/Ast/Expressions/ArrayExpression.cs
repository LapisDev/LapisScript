/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ArrayExpression
 * Description : Represents an array expression in the syntax tree.
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
    /// Represents an array expression in the syntax tree.
    /// </summary>
    public class ArrayExpression : Expression
    {
        /// <summary>
        /// Gets the expressions of the elements in the array.
        /// </summary>
        /// <returns>The expressions of the elements in the array.</returns>
        public ExpressionCollection Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="items">The collection that contains the expressions of the elements in the array.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public ArrayExpression(LinePragma linePragma, ExpressionCollection items)
            : base(linePragma)
        {
            Items = items;            
        }   

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[ ");
            if (Items != null)
                sb.Append(Items.ToString());
            sb.Append(" ]");
            return sb.ToString();
        }
    }
}
