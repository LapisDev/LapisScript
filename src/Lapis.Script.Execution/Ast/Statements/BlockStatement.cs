/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : BlockStatement
 * Description : Represents a block of statements in the syntax tree.
 * Created     : 2015/6/3
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
    /// Represents a block of statements in the syntax tree.
    /// </summary>
    public class BlockStatement : Statement
    {
        /// <summary>
        /// Gets the statements in the block.
        /// </summary>
        /// <value>The statements in the block.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="BlockStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="statements">The statements in the block.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public BlockStatement(
            LinePragma linePragma,
            StatementCollection statements)
            : base(linePragma)
        {
            if (statements == null)
                throw new ArgumentNullException();
            Statements = statements;
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
            var sb = new StringBuilder();
            sb.Append(ind).Append("{\n")
                .Append(Statements.ToString(indentation + 4)).Append("\n")
                .Append(ind).Append("}");
            return sb.ToString();
        }
    }
}
