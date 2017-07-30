/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : MemberCollection
 * Description : Represents a collection of members in the syntax tree.
 * Created     : 2015/6/3
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Members
{
    /// <summary>
    /// Represents a collection of members in the syntax tree.
    /// </summary>
    public class MemberCollection : IEnumerable<Member>
    {
        /// <summary>
        /// Gets the <see cref="Member"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Member"/> to get.</param>
        /// <returns>The <see cref="Member"/> at the specified index. </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>, or is equal to or greater than <see cref="Count"/>.</exception>
        public Member this[int index]
        {
            get { return _members[index]; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="MemberCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="MemberCollection"/>.</value>
        public int Count { get { return _members.Count; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberCollection"/> class that contains elements copied from the specified <see cref="IEnumerable{Member}"/>.
        /// </summary>
        /// <param name="members">The collection whose elements are copied to the new <see cref="MemberCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public MemberCollection(IEnumerable<Member> members)
        {
            if (members == null || members.Contains(null))
                throw new ArgumentNullException();            
            _members = members.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberCollection"/> class that contains elements copied from the specified <see cref="Member"/> array.
        /// </summary>
        /// <param name="members">The array whose elements are copied to the new <see cref="MemberCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public MemberCollection(params Member[] members)
            : this((IEnumerable<Member>)members) { }
     
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="MemberCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="MemberCollection"/>.</returns>
        public IEnumerator<Member> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="MemberCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="MemberCollection"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of the members.
        /// </summary>
        /// <returns>Returns the string representation of the members.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var sb = new StringBuilder();
            foreach (var m in _members)
            {
                sb.Append(m.ToString(indentation)).Append("\n");
            }
            if (sb.Length > 1)
                sb.Length -= 1;
            return sb.ToString();
        }

        /// <summary>
        /// Returns the string representation of the members.
        /// </summary>
        /// <returns>Returns the string representation of the members.</returns>
        public override string ToString()
        {
            return ToString(0);
        }

        internal MemberCollection(List<Member> members)
        {
            _members = members;
        }

        private List<Member> _members;    
    }
}
