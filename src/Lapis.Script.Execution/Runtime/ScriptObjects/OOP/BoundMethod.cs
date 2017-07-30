/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : BoundMethod
 * Description : Represents a method object bound to an object.
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
    /// Represents a method object bound to an object.
    /// </summary>
    public sealed class BoundMethod : IFunctionObject
    {
        /// <summary>
        /// Gets the object to which the method object is bound.
        /// </summary>
        /// <value>The object to which the method object is bound.</value>
        public IScriptObject Target { get; private set; }

        /// <summary>
        /// Gets the method that the method object wrapped.
        /// </summary>
        /// <value>The method that the method object wrapped.</value>
        public ClassMethod Method { get; private set; }

        /// <summary>
        /// Invokes the method with the specified parameters and returns the value.
        /// </summary>
        /// <param name="args">The parameters passed to the method.</param>
        /// <returns>The return value of the method.</returns>
        IScriptObject IFunctionObject.Invoke(List<IScriptObject> args)
        {
            return Method.Invoke(Target, args);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="BoundMethod"/> class using the specified parameter.
        /// </summary>
        /// <param name="target">The object to which the method object is bound.</param>
        /// <param name="method">The method that the method object wrapped.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method"/> is <see langword="null"/>.</exception>
        public BoundMethod(IScriptObject target, ClassMethod method)
        {
            Target = target;
            if (method == null)
                throw new ArgumentNullException();
            Method = method;
        }

        /// <summary>
        /// Specifies whether this script object is equal to another <see cref="IScriptObject"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IScriptObject"/>.</param>
        /// <returns><see langword="true"/> if this script object is equal to the other <see cref="IScriptObject"/>; otherwise, <see langword="false"/>.</returns>
        bool IEquatable<IScriptObject>.Equals(IScriptObject other)
        {
            return base.Equals(other);
        }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return Method.ToString();
        }
    }
}
