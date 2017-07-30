/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ClassDeclarationStatement
 * Description : Represents a class declaration statement in the syntax tree.
 * Created     : 2015/6/3
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Execution.Ast.Members;

namespace Lapis.Script.Execution.Ast.Statements
{
    /// <summary>
    /// Represents a class declaration statement in the syntax tree.
    /// </summary>
    public class ClassDeclarationStatement : Statement
    {
        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type expression of the super class.
        /// </summary>
        /// <value>The type expression of the super class.</value>
        public Expressions.Expression Super { get; private set; }

        /// <summary>
        /// Gets the members of the class.
        /// </summary>
        /// <value>The members of the class.</value>
        public MemberCollection Members { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ClassDeclarationStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="name">The name of the class.</param>
        /// <param name="super">The type expression of the super class.</param>
        /// <param name="members">The members of the class.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public ClassDeclarationStatement(
            LinePragma linePragma, string name, Expressions.Expression super,
            MemberCollection members)
            : base(linePragma)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Name = name;
            Super = super;
            Members = members;            
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ClassDeclarationStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="name">The name of the class.</param>
        /// <param name="members">The members of the class.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public ClassDeclarationStatement(
            LinePragma linePragma, string name,
            MemberCollection members)
            : base(linePragma)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Name = name;
            Members = members;
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
            sb.Append(ind).Append("class ").Append(Name);
            if (Super != null)
                sb.Append(" extends ").Append(Super.ToString());
            sb.Append("\n");
            if (Members != null)
                sb.Append(ind).Append("{\n")
                    .Append(Members.ToString(indentation + 4)).Append("\n")
                    .Append(ind).Append("}");
            else
                sb.Append(ind).Append("{ }");
            return sb.ToString();
        }
    }
}
