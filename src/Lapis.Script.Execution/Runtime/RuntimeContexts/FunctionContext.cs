/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : FunctionContext
 * Description : Represents a runtime context in a function.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace Lapis.Script.Execution.Runtime.RuntimeContexts
{
    /// <summary>
    /// Represents a runtime context in a function.
    /// </summary>
    public class FunctionContext : RuntimeContext
    {
        /// <summary>
        /// Gets the parent scope of the context.
        /// </summary>
        /// <value>The parent scope of the context.</value>
        public override RuntimeContext Scope
        {
            get { return Closure; }
        }

        /// <summary>
        /// Gets the closure scope of the context.
        /// </summary>
        /// <value>The closure scope of the context.</value>
        public RuntimeContext Closure { get; private set; }

        /// <summary>
        /// Gets the class to which the scope belongs.
        /// </summary>
        /// <value>The class of the scope.</value>
        public override IClassObject Class
        {
            get { return Closure.Class; }
        }

        /// <summary>
        /// Gets the <c>this</c> reference bound in the scope.
        /// </summary>
        /// <value>The <c>this</c> reference bound in the scope.</value>
        public override IScriptObject This
        {
            get { return Closure.This; }
        }
        
        /// <summary>
        /// Initialize a new instance of the <see cref="FunctionContext"/> class with the specified parameters.
        /// </summary>
        /// <param name="closure">The closure scope of the context.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public FunctionContext(
            RuntimeContext closure)
            : base(CheckNullAndGetMemoryCreator(closure), closure.ObjectCreator, closure.Operators)
        {
            Closure = closure;
        }
    }
}
