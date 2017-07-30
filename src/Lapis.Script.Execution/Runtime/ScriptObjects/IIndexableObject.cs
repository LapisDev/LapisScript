/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IIndexableObject
 * Description : Represents a script object that supports the indexer.
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
    /// Represents a script object that supports the indexer.
    /// </summary>
    public interface IIndexableObject : IScriptObject
    {
        /// <summary>
        /// Invokes the <c>get</c> accessor of the indexer with the specified parameters and returns the value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <returns>The return value of the indexer.</returns>
        IScriptObject GetItem(RuntimeContext context, List<IScriptObject> indices);

        /// <summary>
        /// Invokes the <c>set</c> accessor of the indexer with the specified parameters and the value to set.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <param name="value">The value to set.</param>
        void SetItem(RuntimeContext context, List<IScriptObject> indices, IScriptObject value);  
    }
}
