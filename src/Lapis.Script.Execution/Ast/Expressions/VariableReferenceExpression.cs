/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : VariableReferenceExpression
 * Description : Represents the expression that refers to a variable in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents the expression that refers to a variable in the syntax tree.
    /// </summary>
    public class VariableReferenceExpression : Expression
    {
        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        /// <value>The name of the variable.</value>
        public string VariableName { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="VariableReferenceExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="variableName"/> is empty or white space.</exception>
        public VariableReferenceExpression(Parser.Lexical.LinePragma linePragma, string variableName)
            : base(linePragma)
        {
            if (variableName == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            VariableName = variableName;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            return VariableName;
        }
    }
}
