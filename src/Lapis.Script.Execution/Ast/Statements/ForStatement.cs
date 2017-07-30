/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ForStatement
 * Description : Represents a for statement in the syntax tree.
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
    /// Represents a <c>for</c> statement in the syntax tree.
    /// </summary>
    public class ForStatement : Statement
    {
        /// <summary>
        /// Gets the initialization statement before the loop.
        /// </summary>
        /// <value>The initialization statement before the loop.</value>
        public Statement InitialStatement { get; private set; }

        /// <summary>
        /// Gets the expression that is tested as the condition for continuation of the loop.
        /// </summary>
        /// <value>The expression that is tested as the condition for continuation of the loop.</value>
        public Expression TestExpression { get; private set; }      

        /// <summary>
        /// Gets the statement that is executed after each cycle.
        /// </summary>
        /// <value>The statement that is executed after each cycle.</value>
        public Statement IncrementStatement { get; private set; }

        /// <summary>
        /// Gets the statements to be executed in the loop.
        /// </summary>
        /// <value>The statements to be executed in the loop.</value>
        public StatementCollection Statements { get; private set; }
      
        /// <summary>
        /// Initialize a new instance of the <see cref="ForStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="initialStatement">The initialization statement before the loop.</param>
        /// <param name="testExpression">The expression that is tested as the condition for continuation of the loop.</param>
        /// <param name="incrementStatement">The statement that is executed after each cycle.</param>
        /// <param name="statements">The statements to be executed in the loop.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public ForStatement(
            LinePragma linePragma,
            Statement  initialStatement,
            Expression testExpression,
            Statement incrementStatement,
            StatementCollection statements
            )
            : base(linePragma)
        {
            InitialStatement = initialStatement;
            TestExpression = testExpression;
            IncrementStatement = incrementStatement;
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
            sb.Append(ind).Append("for").Append(" (");
            if (InitialStatement != null)
                sb.Append(InitialStatement.ToString()).Append(" ");
            else
                sb.Append("; ");
            if (TestExpression != null)
                sb.Append(TestExpression.ToString()).Append("; ");
            else
                sb.Append("; ");
            if (IncrementStatement != null)
                sb.Append(IncrementStatement.ToString().TrimEnd(';'));
            sb.Append(")\n");
            if (Statements != null)
                sb.Append(ind).Append("{\n")
                    .Append(Statements.ToString(indentation + 4)).Append("\n")
                    .Append(ind).Append("}");
            else
                sb.Append(ind).Append("{ }");            
            return sb.ToString();            
        }
    }
}
