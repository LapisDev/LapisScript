/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Parameter
 * Description : Represents a parameter in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Ast
{
    /// <summary>
    /// Represents a parameter in the syntax tree.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the expression of the default value for optional parameters.
        /// </summary>
        /// <value>The expression of the default value for optional parameters.</value>
        public Expression DefaultExpression { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the parameter is optional.
        /// </summary>
        /// <value><see langword="true"/> is the parameter is optional; otherwise <see langword="false"/>.</value>
        public bool IsOptional { get; private set; }

        /// <summary>
        /// Gets the position of the parameter in the script code.
        /// </summary>
        /// <value>The position of the parameter in the script code.</value>
        public Parser.Lexical.LinePragma LinePragma { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Parameter"/> class using the secified parameters.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="defaultExpression">The expression of the default value for optional parameters.</param>
        /// <param name="linePragma">The position of the parameter in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is an empty string.</exception>
        public Parameter(Parser.Lexical.LinePragma linePragma, string name, Expression defaultExpression)
            : this(linePragma, name)
        {            
            IsOptional = defaultExpression != null;
            DefaultExpression = defaultExpression;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Parameter"/> class using the secified parameters.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="linePragma">The position of the parameter in the script code.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is an empty string.</exception>
        public Parameter(Parser.Lexical.LinePragma linePragma, string name)
        {
            if (linePragma == null || name == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            LinePragma = linePragma;
            Name = name;
        }

        /// <summary>
        /// Returns the string representation of the parameter.
        /// </summary>
        /// <returns>Returns the string representation of the parameter.</returns>
        public override string ToString()
        {
            if (IsOptional)
                return string.Format("{0} = {1}", Name, DefaultExpression.ToString());
            else
                return Name;
        }
    }
}
