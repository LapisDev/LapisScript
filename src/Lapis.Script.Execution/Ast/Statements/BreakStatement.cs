/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : BreakStatement
 * Description : Represents a break statement in the syntax tree.
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
    /// Represents a <c>break</c> statement in the syntax tree.
    /// </summary>
    public class BreakStatement : Statement
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="BreakStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public BreakStatement(LinePragma linePragma) : base(linePragma) { }

        /// <summary>
        /// Returns the string representation of the statement.
        /// </summary>
        /// <returns>Returns the string representation of the statement.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public override string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            return ind + "break;";
        }
    }
}
