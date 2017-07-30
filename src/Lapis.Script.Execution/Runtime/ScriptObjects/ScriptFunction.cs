/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ScriptFunction
 * Description : Represents a function object defined in the script.
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

namespace Lapis.Script.Execution.Runtime.ScriptObjects
{
    /// <summary>
    /// Represents a function object defined in the script.
    /// </summary>
    public class ScriptFunction : IFunctionObject
    {
        /// <summary>
        /// Gets the closure scope of the function.
        /// </summary>
        /// <value>The closure scope of the function.</value>
        public RuntimeContext Closure { get; private set; }

        /// <summary>
        /// Gets the parameters of the function.
        /// </summary>
        /// <value>The parameters of the function.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the statements in the function body.
        /// </summary>
        /// <value>The statements in the function body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ScriptFunction"/> class using the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters of the function.</param>
        /// <param name="statements">The statements in the function body.</param>
        /// <param name="closure">The closure scope of the function.</param>
        /// <exception cref="ArgumentNullException"><paramref name="closure"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameters"/> is invalid.</exception>
        public ScriptFunction(
            ParameterCollection parameters,
            StatementCollection statements,
            RuntimeContext closure)
        {
            if (closure == null)
                throw new ArgumentNullException();
            FunctionHelper.CheckParameters(parameters);
            Parameters = parameters;
            Statements = statements;
            Closure = closure;
        }

        /// <summary>
        /// Invokes the function with the specified parameters and returns the value.
        /// </summary>
        /// <param name="args">The parameters passed to the function.</param>
        /// <returns>The return value of the function.</returns>
        /// <exception cref="ArgumentException"><paramref name="args"/> is invalid.</exception>
        IScriptObject IFunctionObject.Invoke(List<IScriptObject> args)
        {
            FunctionHelper.CheckArguments(Parameters, args);
            FunctionContext context = new FunctionContext(Closure);
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

        /// <summary>
        /// Specifies whether this script object is equal to another <see cref="IScriptObject"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IScriptObject"/>.</param>
        /// <returns><see langword="true"/> if this script object is equal to the other <see cref="IScriptObject"/>; otherwise, <see langword="false"/>.</returns>
        bool IEquatable<IScriptObject>.Equals(IScriptObject other)
        {
            return base.Equals(other);
        }
    }

    internal static class FunctionHelper
    {
        internal static void CheckParameters(ParameterCollection parameters)
        {
            if (parameters != null)
            {
                var names = new HashSet<string>();
                var hasOptional = false;
                foreach (var para in parameters)
                {
                    if (names.Contains(para.Name))
                        throw new ArgumentException(
                            string.Format(ExceptionResource.ParameterAlreadyExists, para.Name));
                    else
                        names.Add(para.Name);
                    if (para.IsOptional)
                    {
                        if (!hasOptional)
                            hasOptional = true;
                    }
                    else
                    {
                        if (hasOptional)
                            throw new ArgumentException(
                                string.Format(ExceptionResource.OptionalParameterMustBeAtLast));
                    }
                }
            }
        }

        internal static void CheckArguments(ParameterCollection parameters, List<IScriptObject> args)
        {
            var pc = parameters != null ? parameters.Count : 0;
            var ac = args != null ? args.Count : 0;
            int i = 0;
            while (i < pc)
            {
                if (parameters[i].IsOptional)
                    break;
                i++;
            }
            if (ac <= pc && ac >= i)
                return;
            else
                throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
        }

        internal static void SetArguments(RuntimeContext context, ParameterCollection parameters, List<IScriptObject> args)
        {
            var pc = parameters != null ? parameters.Count : 0;
            var ac = args != null ? args.Count : 0;
            int i = 0;
            while (i < ac)
            {
                context.Memory.HoistingDecalreVariable(context, parameters[i].Name);
                context.Memory.DecalreVariable(context, parameters[i].Name, args[i]);
                i++;
            }
            IScriptObject opArg;
            while (i < pc)
            {
                opArg = context.EvaluateExpression(parameters[i].DefaultExpression);
                context.Memory.HoistingDecalreVariable(context, parameters[i].Name);
                context.Memory.DecalreVariable(context, parameters[i].Name, opArg);
                i++;
            }
        }

        internal static string ToString(ParameterCollection Parameters, StatementCollection statements)
        {
            var sb = new StringBuilder();
            sb.Append("function").Append(" ")
                .Append("(");
            if (Parameters != null)
                sb.Append(Parameters);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
