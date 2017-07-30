/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : StringObject
 * Description : Represents a string object.
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
    /// Represents a string object.
    /// </summary>
    /// <seealso cref="StringClass"/>
    public sealed partial class StringObject : NativeInstance<string>, IStringObject, IBooleanObject
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
        /// Initialize a new instance of the <see cref="StringObject"/> class.
        /// </summary>
        public StringObject()
            : base(StringClass.Instance)
        {
            Value = string.Empty;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="StringObject"/> class using the specified value.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to be wrapped.</param>
        public StringObject(string value)
            : base(StringClass.Instance)
        {
            if (value != null)
                this.Value = value;
            else
                Value = string.Empty;
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
            var value = other as StringObject;
            if (value == null)
                return false;
            return this.Value == value.Value;
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="String"/> to a <see cref="StringObject"/>.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to be wrapped.</param>
        /// <returns>The <see cref="StringObject"/> that wraps <paramref name="value"/>.</returns>
        public static implicit operator StringObject(string value)
        {
            return new StringObject(value);
        }
    }
}
