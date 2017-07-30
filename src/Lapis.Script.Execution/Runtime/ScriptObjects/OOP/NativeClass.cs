/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : NativeClass
 * Description : Represents a native class object.
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
    /// Represents a native class object.
    /// </summary>
    public class NativeClass : ClassObject
    {
        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            var ins = new NativeInstance(this);
            var sup = Super as ScriptClass;
            if (Constructor != null)
                Constructor.Invoke(ins, args);
            return ins;
        }

        /// <summary>
        /// Gets the constructor of the class.
        /// </summary>
        /// <value>The constructor of the class.</value>
        public NativeConstructor Constructor { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="NativeClass"/> class using the specified parameters.
        /// </summary>
        /// <param name="name">The name of the class.</param>
        /// <param name="super">The super class.</param>  
        /// <param name="constructor">The constructor of the class.</param>
        public NativeClass(
            string name,
            ClassObject super,
            NativeConstructor constructor)
            : base(name, super)
        {
            Constructor = constructor;
        }
    }

	/// <summary>
    /// Represents a native class object that wraps a native type.
    /// </summary>
    /// <typeparam name="T">The wrapped native type.</typeparam>
    public class NativeClass<T> : NativeClass
    {
        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            var ins = new NativeInstance<T>(this);
            if (Constructor != null)
                Constructor.Invoke(ins, args);
            return ins;
        }
    
        /// <summary>
        /// Initialize a new instance of the <see cref="NativeClass{T}"/> class using the specified parameters.
        /// </summary>
        /// <param name="name">The name of the class.</param>
        /// <param name="super">The super class.</param>  
        /// <param name="constructor">The constructor of the class.</param>
        public NativeClass(
           string name,
           ClassObject super,
           NativeConstructor constructor)
            : base(name, super, constructor)
        {
        }
    }
}
