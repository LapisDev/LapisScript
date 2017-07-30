/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IObjectCreator
 * Description : Represents an object creator used by the runtime context.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast;
using Lapis.Script.Execution.Ast.Statements;
using Lapis.Script.Execution.Ast.Expressions;
using Lapis.Script.Execution.Ast.Members;
using Lapis.Script.Execution.Runtime.ScriptObjects;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Runtime
{
    /// <summary>
    /// Represents an object creator used by the runtime context.
    /// </summary>
    public interface IObjectCreator
    {
        /// <summary>
        /// Create an object of the primitive type.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="value">The value of the object.</param>
        /// <returns>The primitive object with the value <paramref name="value"/>.</returns>
        IScriptObject CreatePrimitive(RuntimeContext context, object value);

        /// <summary>
        /// Create a function object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="parameters">The parameters of the function.</param>
        /// <param name="statements">The statements in the function body.</param>
        /// <returns>The function object with the parameters <paramref name="parameters"/> and the statements <paramref name="statements"/>.</returns>
        IFunctionObject CreateFunction(RuntimeContext context, ParameterCollection parameters, StatementCollection statements);

        /// <summary>
        /// Create an object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="members">The collection whose elements are the pairs of the member name and the value expression.</param>
        /// <returns>The object with the members <paramref name="members"/>.</returns>
        IScriptObject CreateObject(RuntimeContext context, IEnumerable<KeyValuePair<string, Expression>> members);

        /// <summary>
        /// Create a class object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the class.</param>
        /// <param name="super">The super class.</param>
        /// <param name="members">The members of the class.</param>
        /// <returns>The class object with the name <paramref name="name"/>, the super class <paramref name="super"/> and the members <paramref name="members"/>.</returns>    
        IClassObject CreateClass(RuntimeContext context, string name, IClassObject super, MemberCollection members);

        /// <summary>
        /// Create an array object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="items">The elements in the array.</param>
        /// <returns>The array object with the elements <paramref name="items"/>.</returns>
        IScriptObject CreateArray(RuntimeContext context, List<IScriptObject> items);
    }
}
