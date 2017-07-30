/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Method
 * Description : Represents a method declarasion in the syntax tree.
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
    /// Represents a method declarasion in the syntax tree.
    /// </summary>
    public class Method : Member
    {
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the parameters of the method.
        /// </summary>
        /// <value>The parameters of the method.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the statements in the method body.
        /// </summary>
        /// <value>The statements in the method body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Method"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <param name="statements">The statements in the method body.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public Method(
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
        /// Initialize a new instance of the <see cref="Method"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <param name="isStatic">A value indicating whether the member is static.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <param name="statements">The statements in the method body.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public Method(
            LinePragma linePragma, bool isStatic, string name,
            ParameterCollection parameters,
            StatementCollection statements)
            : base(linePragma, isStatic)
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
        /// Initialize a new instance of the <see cref="Method"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <param name="isStatic">A value indicating whether the member is static.</param>
        /// <param name="modifier">The access modifier of the member.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <param name="statements">The statements in the method body.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public Method(
            LinePragma linePragma, bool isStatic, Modifier modifier, string name,
            ParameterCollection parameters,
            StatementCollection statements)
            : base(linePragma, isStatic, modifier)
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
        /// Returns the string representation of the member.
        /// </summary>
        /// <returns>Returns the string representation of the member.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>public override string ToString(int indentation)
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
            sb.Append(Name)
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
