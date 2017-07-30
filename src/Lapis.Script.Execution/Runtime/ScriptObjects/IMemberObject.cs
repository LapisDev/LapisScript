/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IMemberObject
 * Description : Represents a script object that supports member access.
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
    /// Represents a script object that supports member access.
    /// </summary>
    public interface IMemberObject : IScriptObject
    {   
        /// <summary>
        /// Returns the value of the member of the specified name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <returns>The value of the member <paramref name="name"/>.</returns>
        IScriptObject GetMember(RuntimeContext context, string name);

        /// <summary>
        /// Sets the value of the member of the specified name to the specified value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="value">The value to set.</param>
        void SetMember(RuntimeContext context, string name, IScriptObject value);
    }
}
