/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ExecuteResult
 * Description : Represents the execution result of a statement.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace Lapis.Script.Execution.Runtime.RuntimeContexts
{
    /// <summary>
    /// Represents the execution result of a statement.
    /// </summary>
    public class ExecuteResult
    {
        /// <summary>
        /// Gets the flow control type of the execution result.
        /// </summary>
        /// <value>The flow control type of the execution result.</value>
        public FlowControl FlowControl { get; private set; }

        /// <summary>
        /// Gets the return value of the execution result.
        /// </summary>
        /// <value>The return value of the execution result.</value>
        public object Data { get; private set; }

        /// <summary>
        /// Represents the execution result of seqential statements.
        /// </summary>
        public static readonly ExecuteResult Next = new ExecuteResult(FlowControl.Next);

        /// <summary>
        /// Represents the execution result of a <c>break</c> statement.
        /// </summary>
        public static readonly ExecuteResult Break = new ExecuteResult(FlowControl.Break);

        /// <summary>
        /// Represents the execution result of a <c>continue</c> statement.
        /// </summary>
        public static readonly ExecuteResult Continue = new ExecuteResult(FlowControl.Continue);

        /// <summary>
        /// Returns a execution result of a <c>return</c> statement.
        /// </summary>
        /// <param name="value">The return value of the <c>return</c> statement.</param>
        /// <returns>The execution result of <c>return</c> <paramref name="value"/>.</returns>
        public static ExecuteResult Return(IScriptObject value)
        {
            return new ExecuteResult(FlowControl.Return, value);
        }

        /// <summary>
        /// Returns a execution result of a <c>goto</c> statement.
        /// </summary>
        /// <param name="label">The label of the <c>goto</c> statement.</param>
        /// <returns>The execution result of <c>goto</c> <paramref name="label"/>.</returns>
        public static ExecuteResult Goto(string label)
        {
            return new ExecuteResult(FlowControl.Goto, label);
        }

        private ExecuteResult(FlowControl flowControl)
        {
            FlowControl = flowControl;
        }

        private ExecuteResult(FlowControl flowControl, object data)
            : this(flowControl)
        {
            Data = data;
        }
    }

    /// <summary>
    /// Represents the flow control type of the execution result.
    /// </summary>
    public enum FlowControl
    {
        /// <summary>
        /// Represents the seqential statements.
        /// </summary>
        Next,
        /// <summary>
        /// Represents the <c>break</c> statement.
        /// </summary>
        Break,
        /// <summary>
        /// Represents the <c>continue</c> statement.
        /// </summary>
        Continue, 
        /// <summary>
        /// Represents the <c>goto</c> statement.
        /// </summary>
        Goto, 
        /// <summary>
        /// Represents the <c>return</c> statement.
        /// </summary>
        Return
    }
}
