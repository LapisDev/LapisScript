/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ObjectExpression
 * Description : Represents an object expression in the syntax tree.
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
    /// Represents an object expression in the syntax tree.
    /// </summary>
    public class ObjectExpression : Expression, IEnumerable<KeyValuePair<string, Expression>>
    {
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ObjectExpression"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="ObjectExpression"/>.</returns>
        public IEnumerator<KeyValuePair<string, Expression>> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ObjectExpression"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="ObjectExpression"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="members">The collection whose elements are the pairs of the member name and the value expression.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public ObjectExpression(Parser.Lexical.LinePragma linePragma, IEnumerable<KeyValuePair<string, Expression>> members)
            : base(linePragma)
        {
            _members = new List<KeyValuePair<string,Expression>>(members);
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            foreach (var t in _members)
            {
                sb.Append(t.Key).Append(" : ").Append(t.Value.ToString()).Append(", ");
            }
            if (sb.Length >= 4)
                sb.Length -= 2;
            sb.Append(" }");
            return sb.ToString();
        }

        internal ObjectExpression(Parser.Lexical.LinePragma linePragma, List<KeyValuePair<string, Expression>> members)
            : base(linePragma)
        {
            _members = members;
        }

        private List<KeyValuePair<string, Expression>> _members;      
    }
}
