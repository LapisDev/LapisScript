/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : VariableDictionary
 * Description : Represents a dictionary containing the variables used by the runtime context.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.RuntimeContexts;
using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace Lapis.Script.Execution.Runtime
{
    /// <summary>
    /// Represents a dictionary containing the variables used by the runtime context.
    /// </summary>
    public class VariableDictionary : IVariableMemory
    {
        /// <summary>
        /// Gets the dictionary containing the variables.
        /// </summary>
        /// <value>The dictionary containing the variables.</value>
        public Dictionary<string, IScriptObject> Variables { get; private set; }

        /// <summary>
        /// Gets the dictionary containing the functions.
        /// </summary>
        /// <value>The dictionary containing the functions.</value>
        public Dictionary<string, IFunctionObject> Functions { get; private set; }

        /// <summary>
        /// Gets the dictionary containing the classes.
        /// </summary>
        /// <value>The dictionary containing the classes.</value>
        public Dictionary<string, IClassObject> Classes { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="VariableDictionary"/> class.
        /// </summary>
        public VariableDictionary()
        {
            Variables = new  Dictionary<string,IScriptObject>();
            Functions = new Dictionary<string, IFunctionObject>();
            Classes = new Dictionary<string, IClassObject>();        
        }

        /// <summary>
        /// Returns the value of the variable with the specified name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the variable.</param>
        /// <returns>The value of the variable with the name <paramref name="name"/>.</returns>
        /// <exception cref="InvalidOperationException">No variable with the name <paramref name="name"/> is found.</exception>
        IScriptObject IVariableMemory.GetVariable(RuntimeContext context, string name)
        {
            return GetVariable(context, name);
        }
		
        /// <summary>
        /// Returns the value of the variable with the specified name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the variable.</param>
        /// <returns>The value of the variable with the name <paramref name="name"/>.</returns>
        /// <exception cref="InvalidOperationException">No variable with the name <paramref name="name"/> is found.</exception>
        protected virtual IScriptObject GetVariable(RuntimeContext context, string name)
        {
            IScriptObject v;
            if (this.Variables.TryGetValue(name, out v))
            {
                if (v == null)
                    goto fail;
                return v;
            }
            IFunctionObject func;
            if (this.Functions.TryGetValue(name, out func))
            {
                if (func == null)
                    goto fail;
                return func;
            }
            IClassObject cls;
            if (this.Classes.TryGetValue(name, out cls))
            {
                if (cls == null)
                    goto fail;
                return cls;
            }
            if (context.Scope != null)
                return context.Scope.Memory.GetVariable(context.Scope, name);
        fail:
            throw new InvalidOperationException(
                string.Format(ExceptionResource.Undefined, name));
        }

        /// <summary>
        /// Sets the value of the variable with the specified name to the specified value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="InvalidOperationException">No variable with the name <paramref name="name"/> is found.</exception>
        void IVariableMemory.SetVariable(RuntimeContext context, string name, IScriptObject value)
        {
            SetVariable(context, name, value);
        }
        
		/// <summary>
        /// Sets the value of the variable with the specified name to the specified value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="InvalidOperationException">No variable with the name <paramref name="name"/> is found.</exception>
        protected virtual void SetVariable(RuntimeContext context, string name, IScriptObject value)
        {
            IScriptObject v;
            if (this.Variables.TryGetValue(name, out v))
            {
                if (v == null)
                    goto fail;
                this.Variables[name] = value;
                return;
            }
            IFunctionObject func;
            if (this.Functions.TryGetValue(name, out func))
            {
                if (func == null)
                    goto fail;
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.CannotAssignFunction, name));
            }
            IClassObject cls;
            if (this.Classes.TryGetValue(name, out cls))
            {
                if (cls == null)
                    goto fail;
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.CannotAssignClass, name));
            }
            if (context.Scope != null)
            {
                context.Scope.Memory.SetVariable(context.Scope, name, value);
                return;
            }
        fail:
            throw new InvalidOperationException(
                string.Format(ExceptionResource.Undefined, name));
        }
    
        /// <summary>
        /// Declares a variable with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The initial value of the variable.</param>
        /// <exception cref="InvalidOperationException">The variable has not been declared in hoisting. <see cref="HoistingDecalreVariable"/> should be called first.</exception>     
        void IVariableMemory.DecalreVariable(RuntimeContext context, string name, IScriptObject value)
        {
            DecalreVariable(context, name, value);
        }
        
		/// <summary>
        /// Declares a variable with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The initial value of the variable.</param>
        /// <exception cref="InvalidOperationException">The variable has not been declared in hoisting. <see cref="HoistingDecalreVariable"/> should be called first.</exception>     
        protected virtual void DecalreVariable(RuntimeContext context, string name, IScriptObject value)
        {
            if (this.Variables.ContainsKey(name))
            {
                this.Variables[name] = value;
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.Undefined, name));
            }
        }

        /// <summary>
        /// Declares a function with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        /// <param name="value">The function object.</param>
        /// <exception cref="InvalidOperationException">The function has not been declared in hoisting. <see cref="HoistingDecalreFunction"/> should be called first.</exception>     
        void IVariableMemory.DecalreFunction(RuntimeContext context, string name, IFunctionObject value)
        {
            DecalreFunction(context, name, value);
        }
        
		/// <summary>
        /// Declares a function with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        /// <param name="value">The function object.</param>
        /// <exception cref="InvalidOperationException">The function has not been declared in hoisting. <see cref="HoistingDecalreFunction"/> should be called first.</exception>     
        protected virtual void DecalreFunction(RuntimeContext context, string name, IFunctionObject value)
        {
            if (this.Functions.ContainsKey(name))
            {
                this.Functions[name] = value;
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.Undefined, name));
            }
        }
       
        /// <summary>
        /// Declares a class with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        /// <param name="value">The class object.</param>
        /// <exception cref="InvalidOperationException">The class has not been declared in hoisting. <see cref="HoistingDecalreClass"/> should be called first.</exception>     
        void IVariableMemory.DecalreClass(RuntimeContext context, string name, IClassObject value)
        {
            DecalreClass(context, name, value);
        }
        
		/// <summary>
        /// Declares a class with the specified a name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        /// <param name="value">The class object.</param>
        /// <exception cref="InvalidOperationException">The class has not been declared in hoisting. <see cref="HoistingDecalreClass"/> should be called first.</exception>     
        protected virtual void DecalreClass(RuntimeContext context, string name, IClassObject value)
        {
            if (this.Classes.ContainsKey(name))
            {
                this.Classes[name] = value;
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.Undefined, name));
            }
        }        
       
        /// <summary>
        /// Declares a variable with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        /// <exception cref="InvalidOperationException"><paramref name="name"/> has been already declared.</exception>     
        void IVariableMemory.HoistingDecalreVariable(RuntimeContext context, string name)
        {
            HoistingDecalreVariable(context, name);
        }
        
		/// <summary>
        /// Declares a variable with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The variable name.</param>
        /// <exception cref="InvalidOperationException"><paramref name="name"/> has been already declared.</exception>     
        protected virtual void HoistingDecalreVariable(RuntimeContext context, string name)
        {
            if (this.Variables.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.VariableAlreadyExists, name));
            }
            else if (this.Functions.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.FunctionAlreadyExists, name));
            }
            else if (this.Classes.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.ClassAlreadyExists, name));
            }
            else
            {
                this.Variables.Add(name, null);
            }
        }

        /// <summary>
        /// Declares a function with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        /// <exception cref="InvalidOperationException"><paramref name="name"/> has been already declared.</exception>     
        void IVariableMemory.HoistingDecalreFunction(RuntimeContext context, string name)
        {
            HoistingDecalreFunction(context, name);
        }
        
		/// <summary>
        /// Declares a function with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The function name.</param>
        /// <exception cref="InvalidOperationException"><paramref name="name"/> has been already declared.</exception>     
        protected virtual void HoistingDecalreFunction(RuntimeContext context, string name)
        {
            if (this.Variables.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.VariableAlreadyExists, name));
            }
            else if (this.Functions.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.FunctionAlreadyExists, name));
            }
            else if (this.Classes.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.ClassAlreadyExists, name));
            }
            else
            {
                this.Functions.Add(name, null);
            }
        }

        /// <summary>
        /// Declares a class with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        /// <exception cref="InvalidOperationException"><paramref name="name"/> has been already declared.</exception>     
        void IVariableMemory.HoistingDecalreClass(RuntimeContext context, string name)
        {
            HoistingDecalreClass(context, name);
        }
        
		/// <summary>
        /// Declares a class with the specified a name in hoisting.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The class name.</param>
        /// <exception cref="InvalidOperationException"><paramref name="name"/> has been already declared.</exception>     
        protected virtual void HoistingDecalreClass(RuntimeContext context, string name)
        {
            if (this.Variables.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.VariableAlreadyExists, name));
            }
            else if (this.Functions.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.FunctionAlreadyExists, name));
            }
            else if (this.Classes.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionResource.ClassAlreadyExists, name));
            }
            else
            {
                this.Classes.Add(name, null);
            }
        }
    }

    /// <summary>
    /// Implements a <see cref="IMemoryCreator"/> that creates instances of the <see cref="VariableDictionary"/> class.
    /// </summary>
    public class VariableDictionaryCreator : IMemoryCreator
    {
        /// <summary>
        /// Creates a new instance of the <see cref="VariableDictionary"/> class.
        /// </summary>
        /// <returns>The <see cref="VariableDictionary"/> instance created.</returns>
        public IVariableMemory Create()
        {
            return new VariableDictionary();
        }
    }
}
