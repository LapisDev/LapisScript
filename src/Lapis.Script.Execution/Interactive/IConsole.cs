/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IConsole
 * Description : Represents the console used by the interactive interpreter.
 * Created     : 2015/8/3
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
    /// Represents the console used by the interactive interpreter.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Prints the specified script objects.
        /// </summary>
        /// <param name="objects">The specified script objects to be printed.</param>
        void Print(params IScriptObject[] objects);

        /// <summary>
        /// Prints the specified exceptions.
        /// </summary>
        /// <param name="exceptions">The specified script exceptions to be printed.</param>
        void PrintError(params Exception[] exceptions);        
    }
}
