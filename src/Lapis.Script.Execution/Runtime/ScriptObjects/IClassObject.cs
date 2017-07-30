/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IClassObject
 * Description : Represents a class object.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Runtime.ScriptObjects
{
    /// <summary>
    /// epresents a class object.
    /// </summary>
    public interface IClassObject : IScriptObject
    {
        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>
        IScriptObject Construct(List<IScriptObject> args); 
    } 
}
