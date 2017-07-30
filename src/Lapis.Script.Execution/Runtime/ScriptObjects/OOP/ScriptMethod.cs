/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ScriptMethod
 * Description : Represents a cmethod defined in the script.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Parser.Parsing;
using Lapis.Script.Execution.Ast;
using Lapis.Script.Execution.Ast.Statements;
using Lapis.Script.Execution.Ast.Expressions;
using Lapis.Script.Execution.Ast.Members;
using Lapis.Script.Execution.Runtime.ScriptObjects;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    /// <summary>
    /// Represents a method defined in the script.
    /// </summary>
    public class ScriptMethod : ClassMethod
    {
        /// <summary>
        /// Gets the closure scope of the method.
        /// </summary>
        /// <value>The closure scope of the method.</value>
        public RuntimeContext Closure { get; private set; }

        /// <summary>
        /// Gets the parameters of the method.
        /// </summary>
        /// <value>The parameters of the method.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the statements in the method body.
        /// </summary>
        /// <value>The statements in the method body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Gets the class that the method belongs to.
        /// </summary>
        /// <value>The class that the method belongs to.</value>     
        public IClassObject Class { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ScriptMethod"/> class using the specified parameter.
        /// </summary>  
        /// <param name="cls">The class that the method belongs to.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <param name="statements">The statements in the method body.</param>
        /// <param name="closure">The closure scope of the method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="closure"/> or <paramref name="cls"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameters"/> is invalid.</exception>
        public ScriptMethod(
            IClassObject cls,
            ParameterCollection parameters,
            StatementCollection statements,
            RuntimeContext closure)
        {
            if (closure == null || cls == null)
                throw new ArgumentNullException();
            FunctionHelper.CheckParameters(parameters);
            Class = cls;
            Parameters = parameters;
            Statements = statements;
            Closure = closure;
        }

        /// <summary>
        /// Invokes the method on the specified target with the specified parameters and returns the value.
        /// </summary>
        /// <param name="target">The target on which the method is invoked. It should be <see langword="null"/> if the method is a static method.</param>
        /// <param name="args">The parameters passed to the method.</param>
        /// <exception cref="ArgumentException"><paramref name="args"/> is invalid.</exception>
		public override IScriptObject Invoke(IScriptObject target, List<IScriptObject> args)
        {
            FunctionHelper.CheckArguments(Parameters, args);
            MethodContext context = new MethodContext(Class, target, Closure);
            FunctionHelper.SetArguments(context, Parameters, args);
            ExecuteResult result = context.ExecuteStatements(Statements);
            if (result.FlowControl == FlowControl.Return)
                return (IScriptObject)result.Data;
            else
                return ScriptNull.Instance;
        }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return FunctionHelper.ToString(Parameters, Statements);
        }
    }
}
