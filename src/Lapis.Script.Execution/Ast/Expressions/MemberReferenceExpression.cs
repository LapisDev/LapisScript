/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : MemberReferenceExpression
 * Description : Represents a member reference expression in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents a member reference expression in the syntax tree.
    /// </summary>
    public class MemberReferenceExpression : Expression
    {
        /// <summary>
        /// Gets the expression of the target object.
        /// </summary>
        /// <value>The expression of the target object.</value>
        public Expression Target { get; private set; }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <value>The name of the member.</value>
        public string MemberName { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="MemberReferenceExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="target">The expression of the target object.</param>
        /// <param name="memberName">The name of the member.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="memberName"/> is empty or white space.</exception>
        public MemberReferenceExpression(Parser.Lexical.LinePragma linePragma, Expression target, string memberName)
            : base(linePragma)
        {
            if (target == null || memberName == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(memberName))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Target = target;
            MemberName = memberName;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Target.IsAtomOrPrimitive())
                sb.Append(Target);
            else
                sb.Append("(").Append(Target.ToString()).Append(")");
            sb.Append(".");
            sb.Append(MemberName);
            return sb.ToString();
        }

    }
}
