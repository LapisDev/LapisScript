/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : StatementCollection
 * Description : Represents a collection of statements in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Statements
{
    /// <summary>
    /// Represents a collection of statements in the syntax tree.
    /// </summary>
    public class StatementCollection : IEnumerable<Statement>
    {
        /// <summary>
        /// Gets the <see cref="Statement"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Statement"/> to get.</param>
        /// <returns>The <see cref="Statement"/> at the specified index. </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>, or is equal to or greater than <see cref="Count"/>.</exception>
        public Statement this[int index]
        {
            get { return _statements[index]; }
        }

        /// <summary>
        /// Searches for the specified <see cref="Statement"/> and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="statement">The <see cref="Statement"/> to locate in the <see cref="StatementCollection"/>.</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="statement"/>, if found; otherwise, <c>–1</c>.</returns>
        public int IndexOf(Statement statement)
        {
            return _statements.IndexOf(statement);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="StatementCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="StatementCollection"/>.</value>
        public int Count { get { return _statements.Count; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementCollection"/> class that contains elements copied from the specified <see cref="IEnumerable{Statement}"/>.
        /// </summary>
        /// <param name="statements">The collection whose elements are copied to the new <see cref="StatementCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public StatementCollection(IEnumerable<Statement> statements)
        {
            if (statements == null || statements.Contains(null))
                throw new ArgumentNullException();            
            _statements = statements.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementCollection"/> class that contains elements copied from the specified <see cref="Statement"/> array.
        /// </summary>
        /// <param name="statements">The array whose elements are copied to the new <see cref="StatementCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public StatementCollection(params Statement[] statements)
            : this((IEnumerable<Statement>)statements) { }
     
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="StatementCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="StatementCollection"/>.</returns>
        public IEnumerator<Statement> GetEnumerator()
        {
            return _statements.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="StatementCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="StatementCollection"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _statements.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of the statements.
        /// </summary>
        /// <returns>Returns the string representation of the statements.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();  
            var sb = new StringBuilder();            
            foreach (var s in _statements)
            {
                sb.Append(s.ToString(indentation)).Append("\n");
            }
            if (sb.Length > 1)
                sb.Length -= 1;
            return sb.ToString();
        }

        /// <summary>
        /// Returns the string representation of the statements.
        /// </summary>
        /// <returns>Returns the string representation of the statements.</returns>
        public override string ToString()
        {
            return ToString(0);
        }

        internal StatementCollection(List<Statement> statements)
        {
            _statements = statements;
        }

        private List<Statement> _statements;    
    }
}
