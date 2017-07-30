/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : LabelStatement
 * Description : Represents a statement with a label in the syntax tree.
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
    /// Represents a statement with a label in the syntax tree.
    /// </summary>
    public class LabelStatement : Statement
    {
        /// <summary>
        /// Get the label name.
        /// </summary>
        /// <value>The label name.</value>
        public string Label { get; private set; }

        /// <summary>
        /// Get the labeled statement.
        /// </summary>
        /// <value>The labeled statement.</value>
        public Statement Statement { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="LabelStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="label">The label name.</param>
        /// <param name="statement">The labeled statement.</param>
        /// <exception cref="ArgumentNullException">The parmeter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="label"/> is empty or white space.</exception>
        public LabelStatement(LinePragma linePragma, string label, Statement statement)
            : base(linePragma)
        {
            if (label == null || statement == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);         
            Label = label;
            Statement = statement;
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
            var sb = new StringBuilder();
            if (indentation > 4)
                sb.Append(new string(' ', indentation - 4))
                    .Append(Label).Append(": ");
            else
                sb.Append(Label).Append(": ");
            sb.Append("\n")
                .Append(Statement.ToString(indentation));
            return sb.ToString();
        }
    }
}
