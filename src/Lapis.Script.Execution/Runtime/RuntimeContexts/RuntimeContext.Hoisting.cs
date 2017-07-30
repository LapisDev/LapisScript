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
        private void HoistingExecuteStatements(StatementCollection statements)
        {
            foreach (var stat in statements)
            {
                HoistingExecuteStatement(stat);                              
            }
        }

        private void HoistingExecuteStatement(Statement statement)
        {
            if (statement is LabelStatement)
                HoistingExecuteStatement((LabelStatement)statement);
            else if (statement is VariableDeclarationStatement)
                HoistingExecuteStatement((VariableDeclarationStatement)statement);
            else if (statement is FunctionDeclarationStatement)
                HoistingExecuteStatement((FunctionDeclarationStatement)statement);
            else if (statement is ClassDeclarationStatement)
                HoistingExecuteStatement((ClassDeclarationStatement)statement);
        }

        private void HoistingExecuteStatement(VariableDeclarationStatement statement)
        {
            var name = statement.Name;            
            try
            {
                Memory.HoistingDecalreVariable(this, name);
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuntimeException(statement.LinePragma, ex.Message, ex);
            }
        }
		
        private void HoistingExecuteStatement(FunctionDeclarationStatement statement)
        {
            var name = statement.Name;
            try
            {
                Memory.HoistingDecalreFunction(this, name);
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuntimeException(statement.LinePragma, ex.Message, ex);
            }
        }
		
        private void HoistingExecuteStatement(ClassDeclarationStatement statement)
        {
            var name = statement.Name;
            try
            {
                Memory.HoistingDecalreClass(this, name);
            }
            catch (RuntimeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RuntimeException(statement.LinePragma, ex.Message, ex);
            }
        }

        private void HoistingExecuteStatement(LabelStatement statement)
        {
            if (Labels.ContainsKey(statement.Label))
            {
                throw new RuntimeException(statement.LinePragma,
                    string.Format(ExceptionResource.LabelAlreadyExists, statement.Label));
            }
            Labels.Add(statement.Label, statement);
            HoistingExecuteStatement(statement.Statement);
        }
    }
}
