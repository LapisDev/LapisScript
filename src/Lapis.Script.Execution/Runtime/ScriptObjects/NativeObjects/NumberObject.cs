/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : NumberObject
 * Description : Represents a number object.
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
    /// Represents a number object.
    /// </summary>
    /// <seealso cref="NumberClass"/>
    public partial class NumberObject : NativeInstance<double>, INumberObject, IBooleanObject
    {

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="NumberObject"/> class.
        /// </summary>
        public NumberObject()
            : base(NumberClass.Instance)
        { }

        /// <summary>
        /// Initialize a new instance of the <see cref="NumberObject"/> class using the specified value.
        /// </summary>
        /// <param name="value">The <see cref="Double"/> to be wrapped.</param>
        public NumberObject(double value)
            : base(NumberClass.Instance)
        {
            this.Value = value;
        }

        /// <summary>
        /// Converts the object to a Boolean value.
        /// </summary>
        /// <returns>The Boolean value converted from the object.</returns>
        bool IBooleanObject.ToBoolean()
        {
            return Convert.ToBoolean(Value);
        }

        /// <summary>
        /// Specifies whether this script object is equal to another <see cref="IScriptObject"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IScriptObject"/>.</param>
        /// <returns><see langword="true"/> if this script object is equal to the other <see cref="IScriptObject"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(IScriptObject other)
        {
            var value = other as NumberObject;
            if (value == null)
                return false;
            return this.Value == value.Value;
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Double"/> to a <see cref="NumberObject"/>.
        /// </summary>
        /// <param name="value">The <see cref="Double"/> to be wrapped.</param>
        /// <returns>The <see cref="NumberObject"/> that wraps <paramref name="value"/>.</returns>        
        public static implicit operator NumberObject(double value)
        {
            return new NumberObject(value);
        }
    }
}
