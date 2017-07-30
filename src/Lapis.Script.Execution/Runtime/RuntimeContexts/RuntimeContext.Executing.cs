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
using System.Threading;

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
        /// Executes the statements and returns the result.
        /// </summary>
        /// <param name="statements">The statements to be executed.</param>
        /// <returns>The execution result of <paramref name="statements"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="statements"/> is <see langword="null"/>, or contains <see langword="null"/>.</exception>
        /// <exception cref="RuntimeException">An exception occurred during the execution of the statements.</exception>
        public ExecuteResult ExecuteStatements(StatementCollection statements)
        {           
            if (statements == null || statements.Contains(null))
                throw new ArgumentNullException();
            if (Labels == null)
                Labels = new Dictionary<string, LabelStatement>();
            HoistingExecuteStatements(statements);
            ExecuteResult flow;
            for (int i = 0; i < statements.Count; i++)
            {
                CancellationToken.ThrowIfCancellationRequested();
                flow = ExecuteStatement(statements[i]);
            label:
                if (flow.FlowControl == FlowControl.Goto)
                {
                    var go = (string)flow.Data;
                    if (Labels.ContainsKey(go))
                    {
                        i = statements.IndexOf(Labels[go]);
                        flow = ExecuteStatement(Labels[go]);
                        goto label;
                    }
                    else
                    {
                        return flow;
                    }
                }
                else if (flow.FlowControl == FlowControl.Break ||
                    flow.FlowControl == FlowControl.Return ||
                    flow.FlowControl == FlowControl.Continue)
                    return flow;
            }
            Labels.Clear();
            return ExecuteResult.Next;
        }

        internal virtual ExecuteResult ExecuteStatement(Statement statement)
        {
            if (statement is VariableDeclarationStatement)
                return ExecuteStatement((VariableDeclarationStatement)statement);
            else if (statement is FunctionDeclarationStatement)
                return ExecuteStatement((FunctionDeclarationStatement)statement);
            else if (statement is ClassDeclarationStatement)
                return ExecuteStatement((ClassDeclarationStatement)statement);
            else if (statement is ExpressionStatement)
                return ExecuteStatement((ExpressionStatement)statement);
            else if (statement is IfStatement)
                return ExecuteStatement((IfStatement)statement);
            else if (statement is ForStatement)
                return ExecuteStatement((ForStatement)statement);
            else if (statement is WhileStatement)
                return ExecuteStatement((WhileStatement)statement);
            else if (statement is DoWhileStatement)
                return ExecuteStatement((DoWhileStatement)statement);
            else if (statement is SwitchStatement)
                return ExecuteStatement((SwitchStatement)statement);
            else if (statement is BlockStatement)
                return ExecuteStatement((BlockStatement)statement);
            else if (statement is BreakStatement)
                return ExecuteStatement((BreakStatement)statement);
            else if (statement is GotoStatement)
                return ExecuteStatement((GotoStatement)statement);
            else if (statement is ContinueStatement)
                return ExecuteStatement((ContinueStatement)statement);
            else if (statement is ReturnStatement)
                return ExecuteStatement((ReturnStatement)statement);
            else if (statement is LabelStatement)
                return ExecuteStatement((LabelStatement)statement);
            else if (statement == null)
                throw new ArgumentNullException();
            else
                throw new RuntimeException(statement.LinePragma,
                    ExceptionResource.InvalidStatememt);          
        }

        private ExecuteResult ExecuteStatement(VariableDeclarationStatement statement)
        {
            var name = statement.Name;
            IScriptObject value;
            if (statement.InitialExpression != null)
                value = EvaluateExpression(statement.InitialExpression);
            else
                value = ScriptNull.Instance;
            try
            {
                Memory.DecalreVariable(this, name, value);
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuntimeException(statement.LinePragma, ex.Message, ex);
            }           
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(FunctionDeclarationStatement statement)
        {
            var name = statement.Name;
            var paras = statement.Parameters;
            var stats = statement.Statements;
            try
            {
                IFunctionObject value = this.ObjectCreator.CreateFunction(this, paras, stats);
                Memory.DecalreFunction(this, name, value);
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuntimeException(statement.LinePragma, ex.Message, ex);
            }           
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(ClassDeclarationStatement statement)
        {
            var name = statement.Name;
            IClassObject super = null;
            if (statement.Super != null)
            {
                super = this.EvaluateExpression(statement.Super) as IClassObject;
                if (super == null)
                    throw new RuntimeException(statement.Super.LinePragma,
                        ExceptionResource.TypeExpected);
            }            
            try
            {
                IClassObject value = ObjectCreator.CreateClass(this, name, super, statement.Members);
                Memory.DecalreClass(this, name, value);
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuntimeException(statement.LinePragma, ex.Message, ex);
            }           
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(ExpressionStatement statement)
        {
            EvaluateExpression(statement.Expression);
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(IfStatement statement)
        {
            var con = EvaluateExpression(statement.Condition) as IBooleanObject;
            if (con == null)
                throw new RuntimeException(statement.Condition.LinePragma, 
                    ExceptionResource.CannotConvertToBoolean);
            if (con.ToBoolean())
            {
                if (statement.TrueStatements != null)
                {
                    var block = new BlockContext(this, BlockType.If);
                    return block.ExecuteStatements(statement.TrueStatements);
                }
                else
                    return ExecuteResult.Next;
            }
            else
            {
                if (statement.FalseStatements != null)
                {
                    var block = new BlockContext(this, BlockType.If);
                    return block.ExecuteStatements(statement.FalseStatements);
                }
                else
                    return ExecuteResult.Next;
            }
        }

        private ExecuteResult ExecuteStatement(ForStatement statement)
        {
            var init = statement.InitialStatement;
            var test = statement.TestExpression;
            var inc = statement.IncrementStatement;
            var stats = statement.Statements;
            RuntimeContext block = new BlockContext(this, BlockType.Block);
            if (init != null)
            {
                block.HoistingExecuteStatement(init);
                block.ExecuteStatement(init);
            }
            ExecuteResult flow;
            RuntimeContext inblock;
            while (true)
            {
                if (test != null)
                {
                    var con = block.EvaluateExpression(test) as IBooleanObject;
                    if (con == null)
                        throw new RuntimeException(test.LinePragma,
                            ExceptionResource.CannotConvertToBoolean);
                    if (!con.ToBoolean())
                        break;
                }
                if (stats != null)
                {
                    inblock = new BlockContext(block, BlockType.Loop);
                    flow = inblock.ExecuteStatements(stats);
                    if (flow.FlowControl == FlowControl.Break)
                        break;
                    else if (flow.FlowControl == FlowControl.Continue)
                        continue;
                    else if (flow.FlowControl == FlowControl.Goto ||
                        flow.FlowControl == FlowControl.Return)
                        return flow;
                }
                if (inc != null)
                    block.ExecuteStatement(inc);
            }
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(WhileStatement statement)
        {
            var test = statement.TestExpression;
            var stats = statement.Statements;
            RuntimeContext block;
            ExecuteResult flow;
            while (true)
            {
                if (test != null)
                {
                    var con = EvaluateExpression(test) as IBooleanObject;
                    if (con == null)
                        throw new RuntimeException(test.LinePragma,
                            ExceptionResource.CannotConvertToBoolean);
                    if (!con.ToBoolean())
                        break;
                }
                if (stats != null)
                {
                    block = new BlockContext(this, BlockType.Loop);
                    flow = block.ExecuteStatements(stats);
                    if (flow.FlowControl == FlowControl.Break)
                        break;
                    else if (flow.FlowControl == FlowControl.Continue)
                        continue;
                    else if (flow.FlowControl == FlowControl.Goto ||
                        flow.FlowControl == FlowControl.Return)
                        return flow;
                }
            }
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(DoWhileStatement statement)
        {
            var test = statement.TestExpression;
            var stats = statement.Statements;
            RuntimeContext block;
            ExecuteResult flow;
            while (true)
            {
                if (stats != null)
                {
                    block = new BlockContext(this, BlockType.Loop);
                    flow = block.ExecuteStatements(stats);
                    if (flow.FlowControl == FlowControl.Break)
                        break;
                    else if (flow.FlowControl == FlowControl.Continue)
                        continue;
                    else if (flow.FlowControl == FlowControl.Goto ||
                        flow.FlowControl == FlowControl.Return)
                        return flow;
                }
                if (test != null)
                {
                    var con = EvaluateExpression(test) as IBooleanObject;
                    if (con == null)
                        throw new RuntimeException(test.LinePragma,
                            ExceptionResource.CannotConvertToBoolean);
                    if (!con.ToBoolean())
                        break;
                }
            }
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(SwitchStatement statement)
        {
            var exp = EvaluateExpression(statement.Expression);
            var cases = statement.Cases;
            var def = statement.Default;
            RuntimeContext block;
            ExecuteResult flow;
            bool match = false;
            foreach (var ca in cases)
            {
                if (match || exp.Equals(EvaluateExpression((ca.Expression))))
                {
                    match = true;
                    if (ca.Statements != null)
                    {
                        block = new BlockContext(this, BlockType.Switch);
                        flow = block.ExecuteStatements(ca.Statements);
                        if (flow.FlowControl == FlowControl.Break)
                            return ExecuteResult.Next;
                        else if (flow.FlowControl == FlowControl.Continue)
                            return flow;
                        else if (flow.FlowControl == FlowControl.Goto ||
                            flow.FlowControl == FlowControl.Return)
                            return flow;
                    }
                }
            }
            if (def != null)
            {
                block = new BlockContext(this, BlockType.Switch);
                flow = block.ExecuteStatements(def);
                if (flow.FlowControl == FlowControl.Break)
                    return ExecuteResult.Next;
                else if (flow.FlowControl == FlowControl.Continue)
                    return flow;
                else if (flow.FlowControl == FlowControl.Goto ||
                    flow.FlowControl == FlowControl.Return)
                    return flow;
            }
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(BlockStatement statement)
        {
            var stats = statement.Statements;
            RuntimeContext block;
            ExecuteResult flow;
            if (stats != null)
            {
                block = new BlockContext(this, BlockType.Block);
                return flow = block.ExecuteStatements(stats);
            }
            return ExecuteResult.Next;
        }

        private ExecuteResult ExecuteStatement(BreakStatement statement)
        {
            if (this.CanBreak)
                return ExecuteResult.Break;
            else
                throw new ParserException(statement.LinePragma, 
                    ExceptionResource.BreakMustBeInLoopOrSwitch);
        }

        private ExecuteResult ExecuteStatement(GotoStatement statement)
        {
            var label = statement.Label;
            if (CanGoto(label))
                return ExecuteResult.Goto(label);
            else
                throw new RuntimeException(statement.LinePragma,
                    string.Format(ExceptionResource.LabelNotFound, statement.Label));
        }

        private ExecuteResult ExecuteStatement(ContinueStatement statement)
        {
            if (this.CanContinue)
                return ExecuteResult.Continue;
            else
                throw new RuntimeException(statement.LinePragma,
                    ExceptionResource.ContinueMustBeInLoop);
        }

        private ExecuteResult ExecuteStatement(ReturnStatement statement)
        {
            bool hasValue = statement.Expression != null;
            if (this.CanReturn(hasValue))
                throw new RuntimeException(statement.LinePragma,
                    ExceptionResource.ReturnMustBeInFunction);
            if (hasValue)
                return ExecuteResult.Return(EvaluateExpression(statement.Expression));
            else
                return ExecuteResult.Return(ScriptNull.Instance);          
        }

        private ExecuteResult ExecuteStatement(LabelStatement statement)
        {
            return ExecuteStatement(statement.Statement);
        }
    }
}
