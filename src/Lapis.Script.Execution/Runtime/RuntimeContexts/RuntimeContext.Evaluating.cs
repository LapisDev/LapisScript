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
        /// Evaluates the expression and returns the result.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>The result of <paramref name="expression"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/>.</exception>
        /// <exception cref="RuntimeException">An exception occurred during the evaluation.</exception>
        public IScriptObject EvaluateExpression(Expression expression)
        {
            if (expression is ArrayExpression)
                return EvaluateExpression((ArrayExpression)expression);
            else if (expression is ArrayIndexerExpression)
                return EvaluateExpression((ArrayIndexerExpression)expression);
            else if (expression is AssignExpression)
                return EvaluateExpression((AssignExpression)expression);
            else if (expression is BinaryOperatorExpression)
                return EvaluateExpression((BinaryOperatorExpression)expression);
            else if (expression is FunctionExpression)
                return EvaluateExpression((FunctionExpression)expression);
            else if (expression is FunctionInvokeExpression)
                return EvaluateExpression((FunctionInvokeExpression)expression);
            else if (expression is MemberReferenceExpression)
                return EvaluateExpression((MemberReferenceExpression)expression);
            else if (expression is NewExpression)
                return EvaluateExpression((NewExpression)expression);
            else if (expression is ObjectExpression)
                return EvaluateExpression((ObjectExpression)expression);
            else if (expression is PostfixOperatorExpression)
                return EvaluateExpression((PostfixOperatorExpression)expression);
            else if (expression is PrefixOperatorExpression)
                return EvaluateExpression((PrefixOperatorExpression)expression);
            else if (expression is PrimitiveExpression)
                return EvaluateExpression((PrimitiveExpression)expression);
            else if (expression is SuperReferenceExpression)
                return EvaluateExpression((SuperReferenceExpression)expression);
            else if (expression is ThisReferenceExpression)
                return EvaluateExpression((ThisReferenceExpression)expression);
            else if (expression is TernaryOperatorExpression)
                return EvaluateExpression((TernaryOperatorExpression)expression);
            else if (expression is VariableReferenceExpression)
                return EvaluateExpression((VariableReferenceExpression)expression);
            else if (expression == null)
                throw new ArgumentNullException();
            else
                throw new RuntimeException(expression.LinePragma,
                    ExceptionResource.InvalidExpression);
        }

        private IScriptObject EvaluateExpression(ArrayExpression expression)
        {
            var items = new List<IScriptObject>();
            if (expression.Items != null)
                foreach (var i in expression.Items)
                {
                    items.Add(EvaluateExpression(i));
                }
            try
            {
                return this.ObjectCreator.CreateArray(this, items);
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

        private IScriptObject EvaluateExpression(ArrayIndexerExpression expression)
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
                        return t.GetSuperItem(this, args);
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
                    return ind.GetItem(this, args);
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

        private IScriptObject EvaluateExpression(AssignExpression expression)
        {
            var left = expression.Left;
            var right = EvaluateExpression(expression.Right);
            Assign(left, right);
            return right;
        }

        private IScriptObject EvaluateExpression(BinaryOperatorExpression expression)
        {
            var left = expression.Left;
            var right = expression.Right;
            var oper = expression.Operator;
            try
            {
                return Operators.CalculateBinaryOperator(this, oper, left, right);
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

        private IScriptObject EvaluateExpression(FunctionExpression expression)
        {
            try
            {
                return this.ObjectCreator.CreateFunction(this, expression.Parameters, expression.Statements);
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

        private IScriptObject EvaluateExpression(FunctionInvokeExpression expression)
        {
            if (expression.Target is SuperReferenceExpression)
                throw new RuntimeException(expression.Target.LinePragma,
                    ExceptionResource.SuperInvoke);
            var target = EvaluateExpression(expression.Target);            
            if (target is ScriptNull)
                throw new RuntimeException(expression.Target.LinePragma,
                    ExceptionResource.NullReference);
            var func = target as IFunctionObject;
            if (func == null)              
                throw new RuntimeException(expression.LinePragma,
                    ExceptionResource.FunctionExpectedToInvoke);
            var args = new List<IScriptObject>();
            if (expression.Parameters != null)
                foreach (var p in expression.Parameters)
                {
                    args.Add(EvaluateExpression(p));
                }
            try
            {
                return func.Invoke(args) ?? ScriptNull.Instance;
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

        private IScriptObject EvaluateExpression(MemberReferenceExpression expression)
        {
            var name = expression.MemberName;
            if (expression.Target is SuperReferenceExpression)
            {
                var t = This as ISuperMemberObject;
                if (t != null)
                {
                    try
                    {
                        return t.GetSuperMember(this, name);
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
                    return mem.GetMember(this, name);
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

        private IScriptObject EvaluateExpression(NewExpression expression)
        {
            var type = EvaluateExpression(expression.Type);
            var cls = type as IClassObject;
            if (cls != null)
            {
                var args = new List<IScriptObject>();
                if (expression.Parameters != null)
                    foreach (var arg in expression.Parameters)
                        args.Add(EvaluateExpression(arg));
                try
                {
                    return cls.Construct(args);
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
            throw new RuntimeException(expression.LinePragma,
                ExceptionResource.ClassExpectedForNewExpression);
        }

        private IScriptObject EvaluateExpression(ObjectExpression expression)
        {            
            try
            {
                return this.ObjectCreator.CreateObject(this, expression);
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

        private IScriptObject EvaluateExpression(PostfixOperatorExpression expression)
        {
            var left = expression.Left;
            var oper = expression.Operator;            
            try
            {
                return Operators.CalculatePostfixOperators(this, oper, left);
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

        private IScriptObject EvaluateExpression(PrefixOperatorExpression expression)
        {
            var right = expression.Right;
            var oper = expression.Operator;
            try
            {
                return Operators.CalculatePrefixOperators(this, oper, right);
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

        private IScriptObject EvaluateExpression(PrimitiveExpression expression)
        {
            var value = expression.Value;
            try
            {
                return this.ObjectCreator.CreatePrimitive(this, value);
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

        private IScriptObject EvaluateExpression(SuperReferenceExpression expression)
        {
            throw new RuntimeException(expression.LinePragma,
                ExceptionResource.InvalidSuper);
        }

        private IScriptObject EvaluateExpression(TernaryOperatorExpression expression)
        {
            var left = expression.Operand1;
            var middle = expression.Operand2;
            var right = expression.Operand3;
            var oper1 = expression.Operator1;
            var oper2 = expression.Operator2;
            try
            {
                return Operators.CalculateTernaryOperators(this, oper1, oper2, left, middle, right);
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

        private IScriptObject EvaluateExpression(ThisReferenceExpression expression)
        {
            if (this.This != null)
                return this.This;            
            throw new RuntimeException(expression.LinePragma,
                ExceptionResource.ThisMustBeInInstanceMethod);
        }

        private IScriptObject EvaluateExpression(VariableReferenceExpression expression)
        {        
            var name = expression.VariableName;
            try
            {
                return Memory.GetVariable(this, name);
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
}
