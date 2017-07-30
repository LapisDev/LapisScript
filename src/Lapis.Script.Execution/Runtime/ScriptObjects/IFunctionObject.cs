/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IFunctionObject
 * Description : Represents a function object.
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
    /// Represents a function object.
    /// </summary>
    public interface IFunctionObject : IScriptObject
    {
        /// <summary>
        /// Invokes the function with the specified parameters and returns the value.
        /// </summary>
        /// <param name="args">The parameters passed to the function.</param>
        /// <returns>The return value of the function.</returns>
        IScriptObject Invoke(List<IScriptObject> args);       
    }
}
