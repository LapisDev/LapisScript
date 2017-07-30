/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IOperatorCalculator
 * Description : Represents a calculater containing the operators used by the runtime context.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast.Expressions;
using Lapis.Script.Execution.Runtime.ScriptObjects;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Runtime
{
    /// <summary>
    /// Represents a calculater containing the operators used by the runtime context.
    /// </summary>
    public interface IOperatorCalculator
    {
        /// <summary>
        /// Evaluates the specified binary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>The result of the expression.</returns>
        IScriptObject CalculateBinaryOperator(RuntimeContext context, string oper, Expression left, Expression right);
        
        /// <summary>
        /// Evaluates the specified prefix unary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>The result of the expression.</returns>
        IScriptObject CalculatePrefixOperators(RuntimeContext context, string oper, Expression right);

        /// <summary>
        /// Evaluates the specified postfix unary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="left">The left expression.</param>
        /// <returns>The result of the expression.</returns>
        IScriptObject CalculatePostfixOperators(RuntimeContext context, string oper, Expression left);

        /// <summary>
        /// Evaluates the specified binary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper1">The first operator.</param>
        /// <param name="oper2">The second operator.</param>
        /// <param name="operand1">The first expression.</param>
        /// <param name="operand2">The second expression.</param>
        /// <param name="operand3">The third expression.</param>
        /// <returns>The result of the expression.</returns>
        IScriptObject CalculateTernaryOperators(RuntimeContext context, string oper1, string oper2, Expression operand1, Expression operand2, Expression operand3);   
    }
}
