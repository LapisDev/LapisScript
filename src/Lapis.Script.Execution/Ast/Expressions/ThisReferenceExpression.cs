/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ThisReferenceExpression
 * Description : Represents the expression that refers to the current instance in the syntax tree.
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
    /// Represents the expression that refers to the current instance in the syntax tree.
    /// </summary>
    public class ThisReferenceExpression : Expression
    {        
        /// <summary>
        /// Initialize a new instance of the <see cref="ThisReferenceExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public ThisReferenceExpression(Parser.Lexical.LinePragma linePragma)
            : base(linePragma) { }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            return "this";
        }
    }
}
