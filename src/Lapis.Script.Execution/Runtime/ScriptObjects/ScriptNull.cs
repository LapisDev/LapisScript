/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ScriptNull
 * Description : Represents the null reference.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Runtime.ScriptObjects
{
    /// <summary>
    /// Represents the <see langword="null"/> reference.
    /// </summary>
    public sealed class ScriptNull : IScriptObject, IBooleanObject
    {
        /// <summary>
        /// Represents the <see langword="null"/> reference.
        /// </summary>
        public static readonly ScriptNull Instance = new ScriptNull();

        /// <summary>
        /// Converts the object to <see langword="false"/>.
        /// </summary>
        /// <returns>The Boolean <see langword="false"/>.</returns>
        bool IBooleanObject.ToBoolean()
        {
            return false;
        }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return "null";
        }

        /// <summary>
        /// Specifies whether this script object is equal to another <see cref="IScriptObject"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IScriptObject"/>.</param>
        /// <returns><see langword="true"/> if this script object is equal to the other <see cref="IScriptObject"/>; otherwise, <see langword="false"/>.</returns>
        bool IEquatable<IScriptObject>.Equals(IScriptObject other)
        {
            return other is ScriptNull;
        }

        private ScriptNull() { }
    }
}
