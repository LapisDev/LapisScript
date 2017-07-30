/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Indexer
 * Description : Represents a indexer declarasion in the syntax tree.
 * Created     : 2015/7/12
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Execution.Ast.Statements;
using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Ast.Members
{
    /// <summary>
    /// Represents a indexer declarasion in the syntax tree.
    /// </summary>
    public class Indexer : Member
    {
        /// <summary>
        /// Gets the parameters of the indexer.
        /// </summary>
        /// <value>The parameters of the indexer.</value>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets the <c>get</c> accessor of the indexer.
        /// </summary>
        /// <value>The <c>get</c> accessor of the indexer.</value>
        public Accessor Getter { get; private set; }

        /// <summary>
        /// Gets the <c>set</c> accessor of the indexer.
        /// </summary>
        /// <value>The <c>set</c> accessor of the indexer.</value>
        public Accessor Setter { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Indexer"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <param name="isStatic">A value indicating whether the member is static.</param>
        /// <param name="modifier">The access modifier of the member.</param>
        /// <param name="parameters">The parameters of the indexer.</param>
        /// <param name="getter">The <c>get</c> accessor of the indexer.</param>
        /// <param name="setter">The <c>set</c> accessor of the indexer.</param>        
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="parameters"/> is <see langword="null"/>.</exception>
        public Indexer(
            LinePragma linePragma, bool isStatic, 
            Modifier modifier,
            ParameterCollection parameters,
            Accessor getter,
            Accessor setter)
            : base(linePragma, isStatic, modifier)
        {
            if (parameters == null)
                throw new ArgumentNullException();
            Parameters = parameters;
            Getter = getter;
            Setter = setter;
        }

        /// <summary>
        /// Returns the string representation of the member.
        /// </summary>
        /// <returns>Returns the string representation of the member.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public override string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            var sb = new StringBuilder();
            sb.Append(ind);
            if (IsStatic)
                sb.Append("static ");
            sb.Append(Modifier.Write()).Append(" ");            
            sb.Append("this")
                .Append("[");
            sb.Append(Parameters);
            sb.Append("]\n");
            sb.Append(ind).Append("{\n");
            var tab = new string(' ', 4);
            if (Getter != null)
            {
                sb.Append(Getter.ToString(true, indentation + 4)).Append("\n");
            }
            else if (Setter != null)
            {
                sb.Append(Setter.ToString(false, indentation + 4)).Append("\n");
            }
            sb.Append(ind).Append("}");
            return sb.ToString();
         }
    }
}
