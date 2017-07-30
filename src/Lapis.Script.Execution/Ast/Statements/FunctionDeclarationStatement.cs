/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : FunctionDeclarationStatement
 * Description : Represents a function declaration statement in the syntax tree.
 * Created     : 2015/6/3
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
    /// Represents a function declaration statement in the syntax tree.
    /// </summary>
    public class FunctionDeclarationStatement : Statement
    {
        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        /// <value>The name of the function.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the parameters of the function.
        /// </summary>
        /// <value>The parameters of the function.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the statements in the function body.
        /// </summary>
        /// <value>The statements in the function body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="FunctionDeclarationStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="parameters">The parameters of the function.</param>
        /// <param name="statements">The statements in the function body.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public FunctionDeclarationStatement(
            LinePragma linePragma, string name, 
            ParameterCollection parameters, 
            StatementCollection statements)
            : base(linePragma)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Name = name;
            Parameters = parameters;
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
            sb.Append(ind).Append("function").Append(" ").Append(Name)
                .Append("(");
            if (Parameters != null)
                sb.Append(Parameters);
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
