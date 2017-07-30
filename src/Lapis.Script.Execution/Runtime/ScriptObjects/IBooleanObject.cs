/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IBooleanObject
 * Description : Represents a script object that that can be converted to a Boolean value.
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
    /// Represents a script object that that can be converted to a Boolean value.
    /// </summary>
    public interface IBooleanObject : IScriptObject
    {
        /// <summary>
        /// Converts the object to a Boolean value.
        /// </summary>
        /// <returns>The Boolean value converted from the object.</returns>
        bool ToBoolean();
    }
}
