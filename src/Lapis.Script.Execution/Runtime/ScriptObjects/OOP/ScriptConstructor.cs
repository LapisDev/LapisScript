/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ScriptConstructor
 * Description : Represents a constructor defined in the script.
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
    /// Represents a constructor defined in the script.
    /// </summary>
    public class ScriptConstructor
    {
        /// <summary>
        /// Gets the closure scope of the constructor.
        /// </summary>
        /// <value>The closure scope of the constructor.</value>
        public RuntimeContext Closure { get; private set; }

        /// <summary>
        /// Gets the class that the constructor belongs to.
        /// </summary>
        /// <value>The class that the constructor belongs to.</value>     
        public ScriptClass Class { get; private set; }

        /// <summary>
        /// Gets the parameters of the constructor.
        /// </summary>
        /// <value>The parameters of the constructor.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the statements in the constructor body.
        /// </summary>
        /// <value>The statements in the constructor body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Gets the parameters passed to the constructor of the super class.
        /// </summary>
        /// <value>The parameters passed to the constructor of the super class.</value>
        public ExpressionCollection SuperCall { get; private set; }

        /// <summary>
        /// Invokes the constructor on the specified target with the specified parameters and returns the value.
        /// </summary>
        /// <param name="target">The target on which the constructor is invoked.</param>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <exception cref="ArgumentException"><paramref name="args"/> is invalid.</exception>
        public void Invoke(InstanceObject target, List<IScriptObject> args)
        {
            try
            { 
                FunctionHelper.CheckArguments(Parameters, args); 
            }
            catch (ArgumentException)
            {
                throw new ArgumentException(
                    string.Format(ExceptionResource.ConstructorParemetersNotMatch,
                        Class.Name));
            }
            MethodContext context = new MethodContext(Class, target, Closure);
            FunctionHelper.SetArguments(context, Parameters, args);
            if (Class.Super != null && Class.Super.Constructor != null)
            {
                var list = new List<IScriptObject>();
                if (Parameters != null)
                    foreach (var p in SuperCall)
                    {
                        list.Add(context.EvaluateExpression(p));
                    }
                Class.Super.Constructor.Invoke(target, list);
            }
            IScriptObject value;
            foreach (var field in Class.FieldInitializers)
            {
                if (field.Value != null)
                    value = Closure.EvaluateExpression(field.Value);
                else
                    value = ScriptNull.Instance;
                if (target.Fields.ContainsKey(field.Key))
                    target.Fields.Add(field.Key, value);
                else
                    target.Fields[field.Key] = value;
            }
            if (Statements != null)
                context.ExecuteStatements(Statements);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ScriptConstructor"/> class using the specified parameter.
        /// </summary>  
        /// <param name="cls">The class that the constructor belongs to.</param>
        /// <param name="parameters">The parameters of the constructor.</param>
        /// <param name="statements">The statements in the constructor body.</param>
        /// <param name="closure">The closure scope of the constructor.</param>
        /// <exception cref="ArgumentNullException"><paramref name="closure"/> or <paramref name="cls"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameters"/> is invalid.</exception>
        public ScriptConstructor(
            ScriptClass cls,
            ParameterCollection parameters,
            StatementCollection statements,       
            RuntimeContext closure)
        {
            if (closure == null || cls == null)
                throw new ArgumentNullException();
            FunctionHelper.CheckParameters(parameters);
            Class = cls;
            Parameters = parameters;    
            Closure = closure;
            Statements = statements;
            if (statements != null && statements.Count > 0)
            {
                var expStat = statements[0] as ExpressionStatement;
                if (expStat != null)
                {
                    var funcInv = expStat.Expression as FunctionInvokeExpression;
                    if (funcInv != null)
                    {
                        if (funcInv.Target is SuperReferenceExpression)
                        {                            
                            var list = statements.ToList();
                            list.RemoveAt(0);
                            Statements = new StatementCollection(list);
                            SuperCall = funcInv.Parameters;
                        }
                    }
                }
            }
        }
    }
}
