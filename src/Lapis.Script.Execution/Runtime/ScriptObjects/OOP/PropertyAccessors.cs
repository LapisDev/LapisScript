/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : PropertyGetter  PropertySetter
 * Description : Represents a get or set accessor of a property.
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
    /// Represents a <c>get</c> accessor of an property.
    /// </summary>
    public class PropertyGetter : ClassMember
    {
        /// <summary>
        /// Gets the method that the accessor wraps.
        /// </summary>
        /// <value>The method that the accessor wraps.</value>
        public ClassMethod Method { get; private set; }

        /// <summary>
        /// Invokes the <c>get</c> accessor of the property with the specified parameters and returns the value.
        /// </summary>
        /// <param name="target">The target object on which the property is invoked. It should be <see langword="null"/> if the property is a static property.</param>
        /// <returns>The return value of the property.</returns>
        public IScriptObject GetProperty(IScriptObject target)
        {
            return Method.Invoke(target, new List<IScriptObject>());
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="PropertyGetter"/> class using the specified parameter.
        /// </summary>  
        /// <param name="method">The method that the accessor wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public PropertyGetter(ClassMethod method)
        {
            if (method == null)
                throw new ArgumentNullException();
            Method = method;
        }
    }

    /// <summary>
    /// Represents a <c>set</c> accessor of an property.
    /// </summary>
    public class PropertySetter : ClassMember
    {
        /// <summary>
        /// Gets the method that the accessor wraps.
        /// </summary>
        /// <value>The method that the accessor wraps.</value>
        public ClassMethod Method { get; private set; }

        /// <summary>
        /// Invokes the <c>set</c> accessor of the property with the specified parameters and the value to set.
        /// </summary>
		/// <param name="target">The target object on which the property is invoked. It should be <see langword="null"/> if the property is a static property.</param>
        /// <param name="value">The value to set.</param>
        public void SetProperty(IScriptObject target, IScriptObject value)
        {
            Method.Invoke(target, new List<IScriptObject>() { value });
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="PropertySetter"/> class using the specified parameter.
        /// </summary>  
        /// <param name="method">The method that the accessor wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public PropertySetter(ClassMethod method)
        {
            if (method == null)
                throw new ArgumentNullException();
            Method = method;
        }
    }
}
