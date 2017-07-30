/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : RuntimeContext
 * Description : Represents a runtime context.
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

namespace Lapis.Script.Execution.Runtime.RuntimeContexts
{
    public partial class RuntimeContext
    {
        /// <summary>
        /// Performs an assignment.
        /// </summary>
        /// <param name="expression">The left expression.</param>
        /// <param name="value">The right value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
        /// <exception cref="RuntimeException">An exception occurred during the assignment.</exception>
        public void Assign(Expression expression, IScriptObject value)
        {
            if (expression is VariableReferenceExpression)
                Assign((VariableReferenceExpression)expression, value);
            else if (expression is MemberReferenceExpression)
                Assign((MemberReferenceExpression)expression, value);
            else if (expression is ArrayIndexerExpression)
                Assign((ArrayIndexerExpression)expression, value);
            else if (expression is ArrayExpression)
                Assign((ArrayExpression)expression, value);
            else if (expression == null)
                throw new ArgumentNullException();
            else
                throw new RuntimeException(expression.LinePragma,
                    ExceptionResource.LeftExpressionExpected);
        }

        private void Assign(VariableReferenceExpression expression, IScriptObject value)
        {     
            var name = expression.VariableName;
            try
            {
                this.Memory.SetVariable(this, name, value);
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuntimeException(expression.LinePragma, ex.Message, ex);
            }            
        }

        private void Assign(MemberReferenceExpression expression, IScriptObject value)
        {
            var name = expression.MemberName;
            if (expression.Target is SuperReferenceExpression)
            {
                var t = This as ISuperMemberObject;
                if (t != null)
                {
                    try
                    {
                        t.SetSuperMember(this, name, value);
                        return;
                    }
                    catch (RuntimeException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new RuntimeException(expression.LinePragma, ex.Message, ex);
                    }
                }
            }
            {
                var target = EvaluateExpression(expression.Target);
                if (target is ScriptNull)
                    throw new RuntimeException(expression.Target.LinePragma,
                        ExceptionResource.NullReference);              
                var mem = target as IMemberObject;
                if (mem == null)
                    throw new RuntimeException(expression.LinePragma,
                        string.Format(ExceptionResource.MemberNotFound, name));
                try
                {
                    mem.SetMember(this, name, value);
                }
                catch (RuntimeException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new RuntimeException(expression.LinePragma, ex.Message, ex);
                }
            }
        }

        private void Assign(ArrayIndexerExpression expression, IScriptObject value)
        {
            if (expression.Target is SuperReferenceExpression)
            {
                var t = This as ISuperIndexableObject;
                if (t != null)
                {
                    var args = new List<IScriptObject>();
                    foreach (var idx in expression.Indices)
                    {
                        args.Add(EvaluateExpression(idx));
                    }
                    try
                    {
                        t.SetSuperItem(this, args, value);
                        return;
                    }
                    catch (RuntimeException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new RuntimeException(expression.LinePragma, ex.Message, ex);
                    }
                }
            }
            {
                var target = EvaluateExpression(expression.Target);
                if (target is ScriptNull)
                    throw new RuntimeException(expression.Target.LinePragma,
                        ExceptionResource.NullReference);
                var ind = target as IIndexableObject;
                if (ind == null)
                    throw new RuntimeException(expression.LinePragma,
                        ExceptionResource.IndexerNotSupported);
                var args = new List<IScriptObject>();
                foreach (var idx in expression.Indices)
                {
                    args.Add(EvaluateExpression(idx));
                }
                try
                {
                    ind.SetItem(this, args, value);
                }
                catch (RuntimeException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new RuntimeException(expression.LinePragma, ex.Message, ex);
                }
            }
        }

        private void Assign(ArrayExpression expression, IScriptObject value)
        {            
            var array = value as IEnumerable<IScriptObject>;
            if (array != null && expression.Items.Count > 0 &&
                expression.Items.Count <= array.Count())
                for (int i = 0; i < expression.Items.Count; i++)
                {
                    Assign(expression.Items[i], array.ElementAt(i));
                }             
            else
                throw new RuntimeException(expression.LinePragma,
                    ExceptionResource.ArrayAssignNotMatch);
        }
    }
}
