/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : RuntimeContext
 * Description : Represents a runtime context.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects;
using System.Threading;

namespace Lapis.Script.Execution.Runtime.RuntimeContexts
{
    /// <summary>
    /// Represents a runtime context.
    /// </summary>
    public partial class RuntimeContext
    {     
        /// <summary>
        /// Gets the parent scope of the context.
        /// </summary>
        /// <value>The parent scope of the context.</value>
        public virtual RuntimeContext Scope { get { return null; } }

        /// <summary>
        /// Gets the class to which the scope belongs.
        /// </summary>
        /// <value>The class of the scope.</value>
        public virtual IClassObject Class { get { return null; } }

        /// <summary>
        /// Gets the <c>this</c> reference bound in the scope.
        /// </summary>
        /// <value>The <c>this</c> reference bound in the scope.</value>
        public virtual IScriptObject This { get { return null; } }

        /// <summary>
        /// Gets the calculater containing the operators used by the context.
        /// </summary>
        /// <value>The calculater containing the operators used by the context.</value>
        public IOperatorCalculator Operators { get; private set; }

        /// <summary>
        /// Gets the variable memory used by the context.
        /// </summary>
        /// <value>The variable memory used by the context.</value>
        public IVariableMemory Memory { get; private set; }

        /// <summary>
        /// Gets the variable memory creator used by the context.
        /// </summary>
        /// <value>The variable memory creator used by the context.</value>
        public IMemoryCreator MemoryCreator { get; private set; }

        /// <summary>
        /// Gets the object creator used by the context.
        /// </summary>
        /// <value>The object creator used by the context.</value>
        public IObjectCreator ObjectCreator { get; private set;}

        internal virtual bool CanReturn(bool hasValue) { return false; }

        internal virtual bool CanBreak { get { return false; } }

        internal virtual bool CanContinue { get { return false; } }

        internal virtual bool CanGoto(string label) { return this.Labels != null && this.Labels.ContainsKey(label); }

        internal Dictionary<string, Ast.Statements.LabelStatement> Labels { get; private set; }

        internal CancellationToken CancellationToken { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="RuntimeContext"/> class using the specified parameters.
        /// </summary>
        /// <param name="memoryCreator">The variable memory creator used by the context.</param>
        /// <param name="objectCreator">The object creator used by the context.</param>
        /// <param name="operators">The calculater containing the operators used by the context.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public RuntimeContext(
            IMemoryCreator memoryCreator,
            IObjectCreator objectCreator,
            IOperatorCalculator operators)
        {
            if (memoryCreator == null ||
                objectCreator == null ||
                operators == null)
                throw new ArgumentNullException();
            MemoryCreator = memoryCreator;
            Memory = memoryCreator.Create();
            ObjectCreator = objectCreator;
            Operators = operators;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RuntimeContext"/> class using the specified parameters.
        /// </summary>
        /// <param name="memory">The variable memory used by the context.</param>
        /// <param name="memoryCreator">The variable memory creator used by the context.</param>
        /// <param name="objectCreator">The object creator used by the context.</param>
        /// <param name="operators">The calculater containing the operators used by the context.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public RuntimeContext(
            IVariableMemory memory,
            IMemoryCreator memoryCreator,
            IObjectCreator objectCreator,
            IOperatorCalculator operators)
        {
            if (memory == null ||
                memoryCreator == null ||
                objectCreator == null ||
                operators == null)
                throw new ArgumentNullException();
            MemoryCreator = memoryCreator;
            Memory = memory;
            ObjectCreator = objectCreator;
            Operators = operators;
        }

        internal static IMemoryCreator CheckNullAndGetMemoryCreator(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException();
            else
                return context.MemoryCreator;
        }
    }
}
