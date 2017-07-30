/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : NativeConstructor
 * Description : Represents a native constructor.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    /// <summary>
    /// Represents a native constructor.
    /// </summary>
    public partial class NativeConstructor
    {
        /// <summary>
        /// Gets the delegate that the object wraps.
        /// </summary>
        /// <value>The delegate that the object wraps.</value>
        public Action<NativeInstance, List<IScriptObject>> Action { get; private set; }

        /// <summary>
        /// Invokes theconstructor on the specified target with the specified parameters and returns the value.
        /// </summary>
        /// <param name="target">The target on which the constructor is invoked.</param>
        /// <param name="args">The parameters passed to the constructor.</param>
        public void Invoke(NativeInstance target, List<IScriptObject> args)
        {
            Action(target, args);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="NativeConstructor"/> class using the specified parameter.
        /// </summary>  
        /// <param name="action">The delegate that the object wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public NativeConstructor(Action<NativeInstance, List<IScriptObject>> action)
        {
            if (action == null)
                throw new ArgumentNullException();
            Action = action;
        }
    }   
}
