/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : InteractiveContext
 * Description : Represents the global runtime context of the interactive interpreter.
 * Created     : 2015/8/2
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
using Lapis.Script.Execution.Runtime;
using Lapis.Script.Execution.Runtime.RuntimeContexts;
using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace Lapis.Script.Execution.Interactive
{
    /// <summary>
    /// Represents the global runtime context of the interactive interpreter.
    /// </summary>    
    public class InteractiveContext : RuntimeContext
    {
        /// <summary>
        /// Gets the console used by the context.
        /// </summary>
        /// <value>The console used by the context.</value>
        public IConsole Console { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="InteractiveContext"/> class using the specified parameters.
        /// </summary>
        /// <param name="memory">The variable memory used by the context.</param>
        /// <param name="memoryCreator">The variable memory creator used by the context.</param>
        /// <param name="objectCreator">The object creator used by the context.</param>
        /// <param name="operators">The calculater containing the operators used by the context.</param>
        /// <param name="console">The console used by the context.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public InteractiveContext(
            IVariableMemory memory,
            IMemoryCreator memoryCreator,
            IObjectCreator objectCreator,
            IOperatorCalculator operators,
            IConsole console)
            : base(memory, memoryCreator, objectCreator, operators)
        {
            Console = console;
        }

        internal override ExecuteResult ExecuteStatement(Statement statement)
        {
            if (statement is ExpressionStatement)
                return ExecuteStatement((ExpressionStatement)statement);
            else
                return base.ExecuteStatement(statement);
        }

        private ExecuteResult ExecuteStatement(ExpressionStatement statement)
        {
            var result = EvaluateExpression(statement.Expression);
            if (Console != null)
                if (statement.Expression is FunctionInvokeExpression &&
                    result is ScriptNull)
                { }
                else
                    Console.Print(result);
            return ExecuteResult.Next;
        }
    }
}
