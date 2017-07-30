/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : BooleanObject
 * Description : Represents a Boolean object.
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
    /// Represents a Boolean object.
    /// </summary>
    /// <seealso cref="BooleanClass"/>
    public sealed partial class BooleanObject : NativeInstance<bool>, IBooleanObject
    {
        /// <summary>
        /// Converts the object to a Boolean value.
        /// </summary>
        /// <returns>The Boolean value converted from the object.</returns>
        bool IBooleanObject.ToBoolean()
        {
            return Value;
        }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return Value ? "true" : "false";
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="BooleanObject"/> class.
        /// </summary>
        public BooleanObject()
            : base(BooleanClass.Instance) { }

        /// <summary>
        /// Initialize a new instance of the <see cref="BooleanObject"/> class using the specified value.
        /// </summary>
        /// <param name="value">The <see cref="Boolean"/> to be wrapped.</param>
        public BooleanObject(bool value)
            : base(BooleanClass.Instance)
        {
            Value = value;
        }

        /// <summary>
        /// Specifies whether this script object is equal to another <see cref="IScriptObject"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IScriptObject"/>.</param>
        /// <returns><see langword="true"/> if this script object is equal to the other <see cref="IScriptObject"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(IScriptObject other)
        {
            var value = other as BooleanObject;
            if (value == null)
                return false;
            return this.Value == value.Value;     
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Boolean"/> to a <see cref="BooleanObject"/>.
        /// </summary>
        /// <param name="value">The <see cref="Boolean"/> to be wrapped.</param>
        /// <returns>The <see cref="BooleanObject"/> that wraps <paramref name="value"/>.</returns>
        public static implicit operator BooleanObject(bool value)
        {
            return new BooleanObject(value);
        }
    }  
}
