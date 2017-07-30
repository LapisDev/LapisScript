/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : MethodContext
 * Description : Represents a runtime context in a method.
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
    /// Represents a runtime context in a method.
    /// </summary>
    public class MethodContext : FunctionContext
    {
        /// <summary>
        /// Gets the class to which the scope belongs.
        /// </summary>
        /// <value>The class of the scope.</value>
        public override IClassObject Class { get { return _class; } }

        private IClassObject _class;

        /// <summary>
        /// Gets the <c>this</c> reference bound in the scope, or <see langword="null"/> if the method is a static method.
        /// </summary>
        /// <value>The <c>this</c> reference bound in the scope.</value>
        public override IScriptObject This { get { return _this; } }

        private IScriptObject _this;

        /// <summary>
        /// Gets a value indicating whether the method is a static method.
        /// </summary>
        /// <value><see langword="true"/> if the method is a static method; otherwise, <see langword="false"/>.</value>
        public bool IsStatic { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="MethodContext"/> class with the specified parameters.
        /// </summary>
        /// <param name="cls">The class of the scope.</param>
        /// <param name="closure">The closure scope of the context.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cls"/> or <paramref name="closure"/> is <see langword="null"/>.</exception>
        public MethodContext(
            IClassObject cls,
            RuntimeContext closure)
            : this(cls, null, closure)
        { }

        /// <summary>
        /// Initialize a new instance of the <see cref="MethodContext"/> class with the specified parameters.
        /// </summary>
        /// <param name="cls">The class of the scope.</param>
        /// <param name="self">The <c>this</c> reference bound in the scope.</param>
        /// <param name="closure">The closure scope of the context.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cls"/> or <paramref name="closure"/> is <see langword="null"/>.</exception>
        public MethodContext(
            IClassObject cls,
            IScriptObject self,
            RuntimeContext closure)
            : base(closure)
        {
            if (cls == null)
                throw new ArgumentNullException();
            _class = cls;
            _this = self;
            IsStatic = self == null;
        }
    }
}
