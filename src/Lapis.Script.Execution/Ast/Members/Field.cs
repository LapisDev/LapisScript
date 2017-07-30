/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Field
 * Description : Represents a field declarasion in the syntax tree.
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
    /// Represents a field declarasion in the syntax tree.
    /// </summary>
    public class Field : Member
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the expression of the initial value of the field.
        /// </summary>
        /// <value>The expression of the initial value of the field.</value>
        public Expression InitialExpression { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Field"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <param name="isStatic">A value indicating whether the member is static.</param>
        /// <param name="modifier">The access modifier of the member.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="initialExpression">The expression of the initial value of the field.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public Field(LinePragma linePragma, bool isStatic, Modifier modifier,
            string name, Expression initialExpression)
            : base(linePragma, isStatic, modifier)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Name = name;
            InitialExpression = initialExpression;
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
            sb.Append(Name);
            if (InitialExpression != null)
                sb.Append(" = ").Append(InitialExpression.ToString());
            sb.Append(";");
            return sb.ToString();
        }
    }
}
