/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IVariableMemory
 * Description : Represents a variable memory used by the runtime context.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.RuntimeContexts;
using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace Lapis.Script.Execution.Runtime
{
    /// <summary>
    /// Represents a variable memory used by the runtime context.
    /// </summary>
    public interface IVariableMemory
    {
        /// <summary>
        /// Returns the value of the variable with the specified name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the variable.</param>
        /// <returns>The value of the variable with the name <paramref name="name"/>.</returns>
        IScriptObject GetVariable(RuntimeContext context, string name);

        /// <summary>
        /// Sets the value of the variable with the specified name to the specified value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value to set.</param>
        void SetVariable(RuntimeContext context, string name, IScriptObject value);

        /// <summary>
        /// Declares a variable with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The initial value of the variable.</param>
        void DecalreVariable(RuntimeContext context, string name, IScriptObject value);

        /// <summary>
        /// Declares a function with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        /// <param name="value">The function object.</param>
        void DecalreFunction(RuntimeContext context, string name, IFunctionObject value);

        /// <summary>
        /// Declares a class with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        /// <param name="value">The class object.</param>
        void DecalreClass(RuntimeContext context, string name, IClassObject value);

        /// <summary>
        /// Declares a variable with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        void HoistingDecalreVariable(RuntimeContext context, string name);

        /// <summary>
        /// Declares a function with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        void HoistingDecalreFunction(RuntimeContext context, string name);

        /// <summary>
        /// Declares a class with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        void HoistingDecalreClass(RuntimeContext context, string name);
    }

    /// <summary>
    /// Represents a variable memory creator used by the context.
    /// </summary>
    public interface IMemoryCreator
    {
        /// <summary>
        /// Creates a variable memory.
        /// </summary>
        /// <returns>The variable memory created.</returns>
        IVariableMemory Create();
    }
}
