/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : InteractiveMemory
 * Description : Represents the variable memory used by the interactive interpreter.
 * Created     : 2015/8/3
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime;
using Lapis.Script.Execution.Runtime.RuntimeContexts;
using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace Lapis.Script.Execution.Interactive
{
    /// <summary>
    /// Represents the variable memory used by the interactive interpreter.
    /// </summary>
    public class InteractiveMemory : VariableDictionary
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="InteractiveMemory"/> class.
        /// </summary>
        public InteractiveMemory()
        {
                  
        }

        /// <summary>
        /// Declares a variable with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The initial value of the variable.</param>
        protected override void DecalreVariable(RuntimeContext context, string name, IScriptObject value)
        {
            if (this.Variables.ContainsKey(name))
            {
                this.Variables[name] = value;
            }
            else if (this.Functions.ContainsKey(name))
            {
                Functions.Remove(name);
                this.Variables.Add(name, value);
            }
            else if (this.Classes.ContainsKey(name))
            {
                Classes.Remove(name);
                this.Variables.Add(name, value);
            }
            else
            {
                this.Variables.Add(name, value);
            }           
        }

        /// <summary>
        /// Declares a function with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        /// <param name="value">The function object.</param>
        protected override void DecalreFunction(RuntimeContext context, string name, IFunctionObject value)
        {
            if (this.Functions.ContainsKey(name))
            {
                this.Functions[name] = value;
            }
            else if (this.Variables.ContainsKey(name))
            {
                Variables.Remove(name);
                this.Functions.Add(name, value);
            }
            else if (this.Classes.ContainsKey(name))
            {
                Classes.Remove(name);
                this.Functions.Add(name, value);
            }
            else
            {
                this.Functions.Add(name, value);
            }           
        }

        /// <summary>
        /// Declares a class with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        /// <param name="value">The class object.</param>
        protected override void DecalreClass(RuntimeContext context, string name, IClassObject value)
        {
            if (this.Classes.ContainsKey(name))
            {
                this.Classes[name] = value;
            }
            else if (this.Variables.ContainsKey(name))
            {
                Variables.Remove(name);
                this.Classes.Add(name, value);
            }
            else if (this.Functions.ContainsKey(name))
            {
                Functions.Remove(name);
                this.Classes.Add(name, value);
            }
            else
            {
                this.Classes.Add(name, value);
            }
        }

        /// <summary>
        /// Declares a variable with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        protected override void HoistingDecalreVariable(RuntimeContext context, string name)
        {
            
        }

        /// <summary>
        /// Declares a function with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        protected override void HoistingDecalreFunction(RuntimeContext context, string name)
        {
            
        }

        /// <summary>
        /// Declares a class with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        protected override void HoistingDecalreClass(RuntimeContext context, string name)
        {
           
        }
    }
}
