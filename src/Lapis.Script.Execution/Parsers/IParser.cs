/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : IParser
 * Description : Represents a script parser.
 * Created     : 2015/8/2
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast.Statements;

namespace Lapis.Script.Execution.Parsers
{
    /// <summary>
    /// Represents a script parser.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Parses the specified code and return the created syntax tree.
        /// </summary>
        /// <param name="code">The string contains the specified code.</param>
        /// <returns>The syntax tree created from <paramref name="code"/>.</returns>
        StatementCollection ParseStatements(string code);
    }
}
