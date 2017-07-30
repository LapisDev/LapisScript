using System;
using System.IO;
using Lapis.Script.Execution;
using Lapis.Script.Execution.Interactive;
using Lapis.Script.Execution.Runtime.ScriptObjects;

namespace ScriptSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                var code = File.ReadAllText(args[0]);

                var stw = new System.Diagnostics.Stopwatch();
                var interpreter = new InteractiveInterpreter(ConsoleProxy);
                stw.Start();
                try
                {               
                    interpreter.Run(code);                
                }            
                finally
                {
                    stw.Stop();
                    Console.WriteLine("[End] " + stw.ElapsedMilliseconds + " ms");
                }
            }
            else
            {
                Console.WriteLine("Script path?");
            }
        }

        static IConsole ConsoleProxy = new ConsoleProxy();
    }

    class ConsoleProxy : IConsole
    {
        public void Print(params IScriptObject[] objects)
        {
            foreach (var obj in objects)
            {
                Console.WriteLine("[Out] " + obj.ToString());
            }
        }

        public void PrintError(params Exception[] exceptions)
        {
            foreach (var ex in exceptions)
            {
                Console.WriteLine("[Err] " + ex.Message);
            }
        }
    }
}
