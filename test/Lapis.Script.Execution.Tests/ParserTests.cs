using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lapis.Script.Execution.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Parsers.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void ParserTest()
        {        
            var parser = new Parser();   
            
            var input = System.IO.File.ReadAllText("test_data/1.input.txt");
            var expected = System.IO.File.ReadAllText("test_data/1.expected.txt");            
            
            string actual;
            {
                var stat = parser.ParseStatements(input);
                actual = stat.ToString();                 
            }
            if (expected != actual)
            {
                System.IO.File.WriteAllText("test_data/1.actual.txt", actual);
                Assert.Fail();
            }
        }
    }
}
