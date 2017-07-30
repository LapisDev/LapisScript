/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : INumberObject
 * Description : Represents a number object.
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
    /// Represents a number object.
    /// </summary>
    public interface INumberObject : IScriptObject
    {
        /// <summary>
        /// Gets the value wrapped by the onject.
        /// </summary>
        /// <value>The value wrapped by the onject.</value>  
        double Value { get; }
    }
}
