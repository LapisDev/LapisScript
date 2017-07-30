/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : GotoStatement
 * Description : Represents a goto statement in the syntax tree.
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
    /// Represents a <c>goto</c> statement in the syntax tree.
    /// </summary>
    public class GotoStatement : Statement
    {
        /// <summary>
        /// Get the label name to skip to.
        /// </summary>
        /// <value>The label name to skip to.</value>
        public string Label { get; private set; }

		/// <summary>
        /// Initialize a new instance of the <see cref="GotoStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="label">The label name to skip to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="label"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="label"/> is empty or white space.</exception>
        public GotoStatement(LinePragma linePragma, string label)
            : base(linePragma)
        {
            if (label == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Label = label;
        }

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
            return ind + "goto " + Label + ";";
        }
    }
}
