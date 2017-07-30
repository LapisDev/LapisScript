/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : VariableDeclarationStatement
 * Description : Represents a variable declaration statement in the syntax tree.
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
    /// Represents a variable declaration statement in the syntax tree.
    /// </summary>
    public class VariableDeclarationStatement : Statement
    {
        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        /// <value>The name of the variable.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the expression of the initial value of the variable.
        /// </summary>
        /// <value>The expression of the initial value of the variable.</value>
        public Expression InitialExpression { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="VariableDeclarationStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="initialExpression">The expression of the initial value of the variable.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public VariableDeclarationStatement(LinePragma linePragma, string name, Expression initialExpression)
            : base(linePragma)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Name = name;
            InitialExpression = initialExpression;
        }

        /// <summary>
        /// Returns the string representation of the <c>case</c> clauset.
        /// </summary>
        /// <returns>Returns the string representation of the <c>case</c> clause.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public override string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            var sb = new StringBuilder();
            sb.Append(ind).Append("var ").Append(Name);
            if (InitialExpression != null)
                sb.Append(" = ").Append(InitialExpression.ToString());
            sb.Append(";");
            return sb.ToString();            
        }
    }
}
