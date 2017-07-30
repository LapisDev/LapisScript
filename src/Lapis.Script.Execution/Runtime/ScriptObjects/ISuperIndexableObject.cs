/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ISuperIndexableObject
 * Description : Represents a script object that supports the indexer access referring to super.
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
    /// Represents a script object that supports the indexer access referring to <c>super</c>.
    /// </summary>
    public interface ISuperIndexableObject
    {
		/// <summary>
        /// Invokes the <c>get</c> accessor of the indexer referring to <c>super</c> with the specified parameters and returns the value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <returns>The return value of the indexer.</returns>
        IScriptObject GetSuperItem(RuntimeContext context, List<IScriptObject> indices);

        /// <summary>
        /// Invokes the <c>set</c> accessor of the indexer referring to <c>super</c> with the specified parameters and the value to set.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <param name="value">The value to set.</param>
        void SetSuperItem(RuntimeContext context, List<IScriptObject> indices, IScriptObject value);  
    }
}
