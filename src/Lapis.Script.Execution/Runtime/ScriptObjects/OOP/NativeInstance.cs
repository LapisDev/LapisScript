/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : NativeInstance
 * Description : Represents a native instance object.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    /// <summary>
    /// Represents a native instance object.
    /// </summary>
    public class NativeInstance : InstanceObject
    {
        /// <summary>
        /// Gets or sets the value that the object wraps.
        /// </summary>
        /// <value>The value that the object wraps.</value>     
        public object Value { get; set; }
        
        /// <summary>
        /// Initialize a new instance of the <see cref="NativeInstance"/> class using the specified parameter.
        /// </summary>
        /// <param name="cls">The class that the instance belongs to.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public NativeInstance(NativeClass cls)
            : base(cls)
        { }
    }

    /// <summary>
    /// Represents a native instance object that wraps a native type.
    /// </summary>
    /// <typeparam name="T">The wrapped native type.</typeparam>
    public class NativeInstance<T> : NativeInstance
    {
        /// <summary>
        /// Gets or sets the value that the object wraps.
        /// </summary>
        /// <value>The value that the object wraps.</value>     
        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="NativeInstance{T}"/> class using the specified parameter.
        /// </summary>
        /// <param name="cls">The class that the instance belongs to.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public NativeInstance(NativeClass<T> cls)
            : base(cls)
        {
            Value = default(T);
        }
    }
}
