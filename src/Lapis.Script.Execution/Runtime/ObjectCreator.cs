/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ObjectCreator
 * Description : Implements a default IObjectCreator.
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
using Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects;
using Lapis.Script.Execution.Runtime.ScriptObjects.OOP;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Runtime
{
    /// <summary>
    /// Implements a default <see cref="IObjectCreator"/>.
    /// </summary>
    public class ObjectCreator : IObjectCreator
    {
        /// <summary>
        /// Create an object of the primitive type.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="value">The value of the object.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not of a primitive type.</exception>
        public virtual IScriptObject CreatePrimitive(RuntimeContext context, object value)
        {
            if (value == null)
                return ScriptNull.Instance;
            else if (value is bool)
                return new BooleanObject((bool)value);
            else if (value is double || value is int)
                return new NumberObject((double)value);
            else if (value is string)
                return new StringObject((string)value);
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Create a function object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="parameters">The parameters of the function.</param>
        /// <param name="statements">The statements in the function body.</param>
        /// <returns>The function object with the parameters <paramref name="parameters"/> and the statements <paramref name="statements"/>.</returns>
        public virtual IFunctionObject CreateFunction(RuntimeContext context, ParameterCollection parameters, StatementCollection statements)
        {
            return new ScriptFunction(parameters, statements, context);
        }

        /// <summary>
        /// Create an object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="members">The collection whose elements are the pairs of the member name and the value expression.</param>
        /// <returns>The object with the members <paramref name="members"/>.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public virtual IScriptObject CreateObject(RuntimeContext context, IEnumerable<KeyValuePair<string, Expression>> members)
        {
            if (members == null)
                throw new ArgumentNullException();
            IMemberObject obj = new ExpandoObject();       
            foreach (var member in members)
            {   
                obj.SetMember(context, member.Key, context.EvaluateExpression(member.Value));
            }
            return obj;  
        }

        /// <summary>
        /// Create a class object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the class.</param>
        /// <param name="super">The super class.</param>
        /// <param name="members">The members of the class.</param>
        /// <returns>The class object with the name <paramref name="name"/>, the super class <paramref name="super"/> and the members <paramref name="members"/>.</returns>    
        public virtual IClassObject CreateClass(RuntimeContext context, string name, IClassObject super, MemberCollection members)
        {            
            var clscrt = new ScriptClassCreator(context, name, super);
            clscrt.DeclareMembers(members);
            return clscrt.Class;
        }

        /// <summary>
        /// Create an array object.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="items">The elements in the array.</param>
        /// <returns>The array object with the elements <paramref name="items"/>.</returns>
        public virtual IScriptObject CreateArray(RuntimeContext context, List<IScriptObject> items)
        {
            return new ArrayObject(items);
        }
    }
}
