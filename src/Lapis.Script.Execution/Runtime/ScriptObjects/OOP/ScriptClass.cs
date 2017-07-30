/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ScriptClass
 * Description : Represents a class object defined in the script.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    /// <summary>
    /// Represents a class object defined in the script.
    /// </summary>
    public class ScriptClass : ClassObject
    {
        /// <summary>
        /// Gets the constructor of the class.
        /// </summary>
        /// <value>The constructor of the class.</value>
        public ScriptConstructor Constructor { get; internal set; }

        /// <summary>
        /// Gets the super class.
        /// </summary>
        /// <value>The super class.</value>
        public new ScriptClass Super { get; private set; }

        /// <summary>
        /// Gets the expressions of the instance fields of the class.
        /// </summary>
        /// <value>The expressions of the instance fields of the class.</value>
        public Dictionary<string, Expression> FieldInitializers { get; private set; }

        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            var ins = new InstanceObject(this);
            if (Constructor != null)
                Constructor.Invoke(ins, args);
            return ins;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ScriptClass"/> class using the specified parameters.
        /// </summary>
        /// <param name="name">The name of the class.</param>
        /// <param name="super">The super class.</param>  
        /// <param name="constructor">The constructor of the class.</param>
        public ScriptClass(
            string name,
            ScriptClass super,
            ScriptConstructor constructor)
            : base(name, super)
        {
            Super = super;
            FieldInitializers = new Dictionary<string, Expression>();
            Constructor = constructor;
        }      
    }
}
