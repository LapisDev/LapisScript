/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Expression
 * Description : Represents a expression in the syntax tree.
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
    /// Represents a expression in the syntax tree. This class is abstract.
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// Gets the position of the expression in the script code.
        /// </summary>
        /// <value>The position of the expression in the script code.</value>
        public LinePragma LinePragma { get; private set; }
              
        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public abstract override string ToString();

        internal Expression(LinePragma linePragma)
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;
        }
    }
}
