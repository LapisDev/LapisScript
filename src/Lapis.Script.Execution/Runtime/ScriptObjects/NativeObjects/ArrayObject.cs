/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ArrayObject
 * Description : Represents an array object.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects.OOP;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects
{
    /// <summary>
    /// Represents an array object.
    /// </summary>
    /// <seealso cref="ArrayClass"/>
    public partial class ArrayObject : NativeInstance<List<IScriptObject>>, IScriptObject, IEnumerable<IScriptObject>
    {
        /// <summary>
        /// Gets the items of the array object.
        /// </summary>
        /// <value>The items of the array object.</value>
        public List<IScriptObject> Items
        {
            get { return Value; }
            private set { Value = value; } 
        }         

        internal ArrayObject()
            : base(ArrayClass.Instance) 
        { }

        /// <summary>
        /// Initialize a new instance of the <see cref="ArrayObject"/> class using the specified parameter.
        /// </summary>
        /// <param name="items">The items of the array object.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public ArrayObject(List<IScriptObject> items)
            : base(ArrayClass.Instance)
        {
            if (items == null)
                throw new ArgumentNullException();
            Items = items;           
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ArrayObject"/> class using the specified parameter.
        /// </summary>
        /// <param name="items">The collection containing the items of the array object.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public ArrayObject(IEnumerable<IScriptObject> items)
            : this(new List<IScriptObject>(items))
        { }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return string.Format("Array [{0}]", Items.Count);
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="List{IScriptObject}"/> to an <see cref="ArrayObject"/>.
        /// </summary>
        /// <param name="items">The items of the array object.</param>
        /// <returns>The <see cref="ArrayObject"/> thar wraps <paramref name="items"/>.</returns>
        public static implicit operator ArrayObject(List<IScriptObject> items)
        {
            if (items == null)
                return null;
            return new ArrayObject(items);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ArrayObject"/>.
        /// </summary>
        /// <returnsAn enumerator for the <see cref="ArrayObject"/>.</returns>
        public IEnumerator<IScriptObject> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ArrayObject"/>.
        /// </summary>
        /// <returnsAn enumerator for the <see cref="ArrayObject"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }                            
    }
}
