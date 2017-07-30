/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Interpreter
 * Description : Represents a script interpreter.
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
using Lapis.Script.Execution.Parsers;
using Lapis.Script.Execution.Runtime;
using Lapis.Script.Execution.Runtime.ScriptObjects;
using Lapis.Script.Execution.Runtime.ScriptObjects.OOP;
using Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution
{
    /// <summary>
    /// Represents a script interpreter.
    /// </summary>
    public class Interpreter
    {
        /// <summary>
        /// Executes the specified code.
        /// </summary>
        /// <param name="code">The string that contains the specified code.</param>
        /// <exception cref="LexerException">Thrown when a lexical error occurs.</exception>
        /// <exception cref="ParserException">Thrown when a grammar error occurs.</exception>
        /// <exception cref="RuntimeException">Thrown when a execution error occurs.</exception>
        public virtual void Run(string code)
        {         
            var stats = Parser.ParseStatements(code);         
            RuntimeContext.ExecuteStatements(stats);           
        }

        /// <summary>
        /// Gets the global runtime context of the interpreter.
        /// </summary>
        /// <value>The global runtime context of the interpreter.</value>
        public RuntimeContext RuntimeContext { get; private set; }

        /// <summary>
        /// Gets the parser used by the interpreter.
        /// </summary>
        /// <value>The parser used by the interpreter.</value>       
        public IParser Parser { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Interpreter"/> class.
        /// </summary>
        public Interpreter()
        {
            InitializeParser();
            InitializeRuntimeContext();
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Interpreter"/> class using the specified parameters.
        /// </summary>
        /// <param name="context">The global runtime context of the interpreter.</param>
        /// <param name="parser">The parser used by the interpreter.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public Interpreter(IParser parser, RuntimeContext context)
        {
            if (parser == null || context == null)
                throw new ArgumentNullException();
            Parser = parser;
            RuntimeContext = context; 
        }

        private void InitializeParser()
        {
            Parser = new Parsers.Parser();
        }

        private void InitializeRuntimeContext()
        {
            ObjectCreator objCreator = new ObjectCreator();
            VariableDictionaryCreator varDictCreator = new VariableDictionaryCreator();
            OperatorDictionary operators = OperatorDictionary.Default;
            var run = new RuntimeContext(varDictCreator, objCreator, operators);
            var memory = run.Memory as VariableDictionary;
            memory.Classes.Add("Object", ObjectClass.Instance);
            memory.Classes.Add("Array", ArrayClass.Instance);
            memory.Classes.Add("Boolean", BooleanClass.Instance);
            memory.Classes.Add("Number", NumberClass.Instance);
            memory.Classes.Add("String", StringClass.Instance);
            RuntimeContext = run;
        }
    }
}
