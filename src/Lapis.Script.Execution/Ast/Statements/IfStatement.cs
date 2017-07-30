/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IfStatement
 * Description : Represents a if statement in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Ast.Statements
{
    /// <summary>
    /// Represents a <c>if</c> if statement in the syntax tree.
    /// </summary>
    public class IfStatement : Statement
    {
        /// <summary>
        /// Gets the expression that is tested as the condition.
        /// </summary>
        /// <value>The expression that is tested as the condition.</value>
        public Expression Condition { get; private set; }

        /// <summary>
        /// Gets the statements to be executed when the <see cref="Condition"/> is tested <see langword="true"/>.
        /// </summary>
        /// <value>The statements to be executed when the <see cref="Condition"/> is tested <see langword="true"/>.</value>
        public StatementCollection TrueStatements { get; private set; }

        /// <summary>
        /// Gets the statements to be executed when the <see cref="Condition"/> is tested <see langword="false"/>.
        /// </summary>
        /// <value>The statements to be executed when the <see cref="Condition"/> is tested <see langword="false"/>.</value>
        public StatementCollection FalseStatements { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="IfStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="condition">The expression that is tested as the condition.</param>
        /// <param name="trueStatements">The statements to be executed when the <see cref="Condition"/> is tested <see langword="true"/>.</param>
        /// <param name="falseStatements">The statements to be executed when the <see cref="Condition"/> is tested <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="condition"/> is <see langword="null"/>.</exception>
        public IfStatement(
            LinePragma linePragma,
            Expression condition,
            StatementCollection trueStatements,
            StatementCollection falseStatements)
            : base(linePragma)
        {
            if (condition == null)
                throw new ArgumentNullException();
            Condition = condition;
            TrueStatements = trueStatements;
            FalseStatements = falseStatements;
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
            sb.Append(ind).Append("if").Append(" ")
                .Append("(").Append(Condition.ToString()).Append(")\n");
            if (TrueStatements != null)
                sb.Append(ind).Append("{\n")
                    .Append(TrueStatements.ToString(indentation + 4)).Append("\n")
                    .Append(ind).Append("}");  
            else
                sb.Append(ind).Append("{ }");
            if (FalseStatements != null)
            {
                sb.Append("\n").Append(ind).Append("else\n")
                     .Append(ind).Append("{\n")
                     .Append(FalseStatements.ToString(indentation + 4)).Append("\n")
                     .Append(ind).Append("}");
            }
            return sb.ToString();
        }
    }
}
