/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ExpandoObject
 * Description : Represents an object whose members can be dynamically added and removed.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects.OOP;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects
{
    /// <summary>
    /// Represents an object whose members can be dynamically added and removed.
    /// </summary>
    /// <seealso cref="ObjectClass"/>
    public sealed partial class ExpandoObject : InstanceObject, IMemberObject
    {        
        /// <summary>
        /// Returns the value of the member of the specified name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <returns>The value of the member <paramref name="name"/>.</returns>
        IScriptObject IMemberObject.GetMember(RuntimeContexts.RuntimeContext context, string name)
        {
            IScriptObject value;
            if (Fields.TryGetValue(name, out value))
                return value;
            else
            {
                var member = Class.GetMember(context, name, false);
                var mtd = member as ClassMethod;
                if (mtd != null)
                {
                    return new BoundMethod(this, mtd);
                }
                var proget = member as PropertyGetter;
                if (proget != null)
                {
                    return proget.GetProperty(this);
                }
            }
            throw new InvalidOperationException(
                string.Format(ExceptionResource.MemberNotFound, name));
        }

        /// <summary>
        /// Sets the value of the member of the specified name to the specified value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="value">The value to set.</param>
        void IMemberObject.SetMember(RuntimeContexts.RuntimeContext context, string name, IScriptObject value)
        {
            if (Fields.ContainsKey(name))
                Fields[name] = value;
            else
                Fields.Add(name, value);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ExpandoObject"/> class.
        /// </summary>
        public ExpandoObject()
            : base(ObjectClass.Instance)
        {
           
        }     
    }  
}