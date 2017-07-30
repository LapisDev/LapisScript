/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ClassMethod
 * Description : Represents a method of a class object.
 * Created     : 2015/7/18
 * Note        : Converted from interface IClassMethod, 2015/8/2.
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    /// <summary>
    /// Represents a method of a class object.
    /// </summary>
    public abstract class ClassMethod : ClassMember
    {
        /// <summary>
        /// Invokes the method on the specified target with the specified parameters and returns the value.
        /// </summary>
        /// <param name="target">The target on which the method is invoked. It should be <see langword="null"/> if the method is a static method.</param>
        /// <param name="args">The parameters passed to the method.</param>
        /// <returns>The return value of the method.</returns>
        public abstract IScriptObject Invoke(IScriptObject target, List<IScriptObject> args);

        /// <summary>
        /// Initialize a new instance of the <see cref="ClassMethod"/> class.
        /// </summary>
        protected ClassMethod() { }
    }
}
