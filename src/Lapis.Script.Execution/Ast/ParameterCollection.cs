/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ParameterCollection
 * Description : Represents a collection of parameters in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast
{
    /// <summary>
    /// Represents a collection of parameters in the syntax tree.
    /// </summary>
    public class ParameterCollection : IEnumerable<Parameter>
    {
        /// <summary>
        /// Gets the <see cref="Parameter"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Parameter"/> to get.</param>
        /// <returns>The <see cref="Parameter"/> at the specified index. </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>, or is equal to or greater than <see cref="Count"/>.</exception>
        public Parameter this[int index]
        {
            get { return _parameters[index]; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ParameterCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="ParameterCollection"/>.</value>
        public int Count { get { return _parameters.Count; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class that contains elements copied from the specified <see cref="IEnumerable{Parameter}"/>.
        /// </summary>
        /// <param name="parameters">The collection whose elements are copied to the new <see cref="ParameterCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public ParameterCollection(IEnumerable<Parameter> parameters)
        {
            if (parameters == null || parameters.Contains(null))
                throw new ArgumentNullException();   
            _parameters = parameters.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class that contains elements copied from the specified <see cref="Parameter"/> array.
        /// </summary>
        /// <param name="parameters">The array whose elements are copied to the new <see cref="ParameterCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public ParameterCollection(params Parameter[] parameters)
            : this((IEnumerable<Parameter>)parameters) { }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ParameterCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="ParameterCollection"/>.</returns>
        public IEnumerator<Parameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ParameterCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="ParameterCollection"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of the parameters.
        /// </summary>
        /// <returns>Returns the string representation of the parameters.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in _parameters)
            {
                sb.Append(p.ToString()).Append(", ");
            }
            if (sb.Length > 2)
                sb.Length -= 2;
            return sb.ToString();
        }

        internal ParameterCollection(List<Parameter> parameters)
        {
            _parameters = parameters;
        }

        private List<Parameter> _parameters;
    }
}
