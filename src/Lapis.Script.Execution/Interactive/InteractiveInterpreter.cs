/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : InteractiveInterpreter
 * Description : Represents an interactive script interpreter.
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
using Lapis.Script.Execution.Parsers;
using Lapis.Script.Execution.Runtime;
using Lapis.Script.Execution.Runtime.ScriptObjects;
using Lapis.Script.Execution.Runtime.ScriptObjects.OOP;
using Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Interactive
{
    /// <summary>
    /// Represents an interactive script interpreter.
    /// </summary>
    public class InteractiveInterpreter : Interpreter
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="InteractiveInterpreter"/> class using the specified parameters.
        /// </summary>
        /// <param name="console">The console used by the interpreter.</param>
        public InteractiveInterpreter(IConsole console)
            :base(
            new Parsers.Parser(),
            new InteractiveContext( 
                new InteractiveMemory(),
                new VariableDictionaryCreator(),
                new ObjectCreator(),
                OperatorDictionary.Default,
                console))
        {
            Console = console;
            var memory = RuntimeContext.Memory as VariableDictionary;
            memory.Classes.Add("Object", ObjectClass.Instance);
            memory.Classes.Add("Array", ArrayClass.Instance);
            memory.Classes.Add("Boolean", BooleanClass.Instance);
            memory.Classes.Add("Number", NumberClass.Instance);
            memory.Classes.Add("String", StringClass.Instance);
            memory.Functions.Add("print", new NativeFunction(args =>
            {
                if (Console != null)
                    Console.Print(args.ToArray());
                return ScriptNull.Instance;
            }));
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="InteractiveInterpreter"/> class using the specified parameters.
        /// </summary>
        /// <param name="context">The global runtime context of the interpreter.</param>
        /// <param name="parser">The parser used by the interpreter.</param>
        /// <param name="console">The console used by the interpreter.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public InteractiveInterpreter(
            IParser parser,
            InteractiveContext context,
            IConsole console)
            : base(parser, context)
        {
            Console = console;
        }

        /// <summary>
        /// Gets the console used by the interpreter.
        /// </summary>
        /// <value>The console used by the interpreter.</value>
        public IConsole Console { get; private set; }

        /// <summary>
        /// Executes the specified code.
        /// </summary>
        /// <param name="code">The string that contains the specified code.</param>
        public override void Run(string code)
        {
            try
            {
                base.Run(code);
            }
            catch (RuntimeException ex)
            {
                if (Console != null)
                    Console.PrintError(ex);
            }
            catch (ParserException ex)
            {
                if (Console != null)
                    Console.PrintError(ex);
            }
            catch (LexerException ex)
            {
                if (Console != null)
                    Console.PrintError(ex);
            }       
        }
    }
}
