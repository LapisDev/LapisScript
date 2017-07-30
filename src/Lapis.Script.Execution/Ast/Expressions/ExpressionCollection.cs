/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ExpressionCollection
 * Description : Represents a collection of expressions in the syntax tree.
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
    /// Represents a collection of expressions in the syntax tree.
    /// </summary>
    public class ExpressionCollection : IEnumerable<Expression>
    {
        /// <summary>
        /// Gets the <see cref="Expression"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Expression"/> to get.</param>
        /// <returns>The <see cref="Expression"/> at the specified index. </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>, or is equal to or greater than <see cref="Count"/>.</exception>
        public Expression this[int index]
        {
            get { return _expressions[index]; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ExpressionCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="ExpressionCollection"/>.</value>
        public int Count { get { return _expressions.Count; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionCollection"/> class that contains elements copied from the specified <see cref="IEnumerable{Expression}"/>.
        /// </summary>
        /// <param name="expressions">The collection whose elements are copied to the new <see cref="ExpressionCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public ExpressionCollection(IEnumerable<Expression> expressions)
        {
            if (expressions == null || expressions.Contains(null))
                throw new ArgumentNullException(); 
            _expressions = expressions.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionCollection"/> class that contains elements copied from the specified <see cref="Expression"/> array.
        /// </summary>
        /// <param name="expressions">The array whose elements are copied to the new <see cref="ExpressionCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public ExpressionCollection(params Expression[] expressions)
            : this((IEnumerable<Expression>)expressions) { }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ExpressionCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="ExpressionCollection"/>.</returns>
        public IEnumerator<Expression> GetEnumerator()
        {
            return _expressions.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ExpressionCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="ExpressionCollection"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _expressions.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of the expressions.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var e in _expressions)
                sb.Append(e.ToString()).Append(", ");
            if (sb.Length >= 2)
                sb.Length -= 2;
            return sb.ToString();
        }

        internal ExpressionCollection(List<Expression> expressions)
        {
            _expressions = expressions;
        }

        private List<Expression> _expressions;        
    }
}
