/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Constructor
 * Description : Represents a constructor in the syntax tree.
 * Created     : 2015/6/3
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Execution.Ast.Statements;
using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Ast.Members
{
    /// <summary>
    /// Represents a constructor in the syntax tree.
    /// </summary>
    public class Constructor : Member
    {
        /// <summary>
        /// Gets the parameters of the constructor.
        /// </summary>
        /// <value>The parameters of the constructor.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the statements in the constructor body.
        /// </summary>
        /// <value>The statements in the constructor body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Constructor"/> class using the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters of the constructor.</param>
        /// <param name="statements">The statements in the constructor body.</param>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public Constructor(
            LinePragma linePragma,
            ParameterCollection parameters,
            StatementCollection statements)
            : base(linePragma, false)
        {
            Parameters = parameters;
            Statements = statements;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Constructor"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <param name="isStatic">A value indicating whether the member is static.</param>
        /// <param name="modifier">The access modifier of the member.</param>
        /// <param name="parameters">The parameters of the constructor.</param>
        /// <param name="statements">The statements in the constructor body.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public Constructor(
            LinePragma linePragma, bool isStatic, Modifier modifier,
            ParameterCollection parameters,
            StatementCollection statements)
            : base(linePragma, isStatic, modifier)
        {
            Parameters = parameters;
            Statements = statements;
        }
        
        /// <summary>
        /// Returns the string representation of the member.
        /// </summary>
        /// <returns>Returns the string representation of the member.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public override string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            var sb = new StringBuilder();
            sb.Append(ind);
            if (IsStatic)
                sb.Append("static ");
            if (Modifier == Modifier.Public)
                sb.Append("public ");
            else if (Modifier == Modifier.Protected)
                sb.Append("protected ");
            else if (Modifier == Modifier.Private)
                sb.Append("private ");
            sb.Append("constructor")
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
