/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Statement
 * Description : Represents a statement in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;

namespace Lapis.Script.Execution.Ast.Statements
{
    /// <summary>
    /// Represents a statement in the syntax tree. This class is abstract.
    /// </summary>
    public abstract class Statement
    {
        /// <summary>
        /// Gets the position of the statement in the script code.
        /// </summary>
        /// <value>The position of the statement in the script code.</value>
        public LinePragma LinePragma { get; private set; }               

        /// <summary>
        /// Returns the string representation of the statement.
        /// </summary>
        /// <returns>Returns the string representation of the statement.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public abstract string ToString(int indentation);

        /// <summary>
        /// Returns the string representation of the statement.
        /// </summary>
        /// <returns>Returns the string representation of the statement.</returns>
        public override string ToString()
        {
            return ToString(0);
        }

        internal Statement(LinePragma linePragma)
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;
        }
    }
}
