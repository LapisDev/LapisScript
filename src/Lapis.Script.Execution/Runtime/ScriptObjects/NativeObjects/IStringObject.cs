/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IStringObject
 * Description : Represents a string object.
 * Created     : 2015/10/23
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects
{    
    /// <summary>
    /// Represents a string object.
    /// </summary>
    public interface IStringObject : IScriptObject
    {
        /// <summary>
        /// Gets the value wrapped by the onject.
        /// </summary>
        /// <value>The value wrapped by the onject.</value>  
        string Value { get; }
    }
}
