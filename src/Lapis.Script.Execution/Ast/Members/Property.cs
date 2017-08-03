/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Property
 * Description : Represents a property declarasion in the syntax tree.
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
    /// Represents a property declarasion in the syntax tree.
    /// </summary>
    public class Property : Member
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the <c>get</c> accessor of the property.
        /// </summary>
        /// <value>The <c>get</c> accessor of the property.</value>
        public Accessor Getter { get; private set; }

        /// <summary>
        /// Gets the <c>set</c> accessor of the property.
        /// </summary>
        /// <value>The <c>set</c> accessor of the property.</value>
        public Accessor Setter { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Property"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the member in the script code.</param>
        /// <param name="isStatic">A value indicating whether the member is static.</param>
        /// <param name="modifier">The access modifier of the member.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="getter">The <c>get</c> accessor of the property.</param>
        /// <param name="setter">The <c>set</c> accessor of the property.</param>        
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="parameters"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="name"/> is empty or white space.</exception>
        public Property(
            LinePragma linePragma, bool isStatic, 
            Modifier modifier,
            string name,
            Accessor getter,
            Accessor setter)
            : base(linePragma, isStatic, modifier)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionResource.InvalidIdentifier);
            Name = name;
            Getter = getter;
            Setter = setter;
        }

        /// <summary>
        /// Returns the string representation of the member.
        /// </summary>
        /// <returns>Returns the string representation of the member.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>public override string ToString(int indentation)
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
            sb.Append(Name);
            sb.Append("\n");
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

    /// <summary>
    /// Represents an accessor of the property or indexer.
    /// </summary>
    public class Accessor
    {
        /// <summary>
        /// Gets the position of the accessor in the script code.
        /// </summary>
        /// <value>The position of the accessor in the script code.</value>
        public LinePragma LinePragma { get; private set; }

        /// <summary>
        /// Gets the statements in the accessor body.
        /// </summary>
        /// <value>The statements in the accessor body.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Gets the access modifier of the accessor.
        /// </summary>
        /// <value>The access modifier of the accessor.</value>
        public Modifier? Modifier { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Accessor"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the accessor in the script code.</param>
		/// <param name="modifier">The access modifier of the accessor.</param>
        /// <param name="statements">The statements in the accessor body.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public Accessor(
            LinePragma linePragma, 
            Modifier? modifier,            
            StatementCollection statements)                  
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;
            Modifier = modifier;
            Statements = statements;
        }
                
        /// <summary>
        /// Returns the string representation of the member.
        /// </summary>
        /// <returns>Returns the string representation of the member.</returns>
		/// <param name="indentation">The number of indented characters.</param>
		/// <param name="isGetter">A value indicating whether the accessor is a <c>get</c> or <c>set</c> accessor.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>public override string ToString(int indentation)
        public string ToString(bool isGetter, int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            var sb = new StringBuilder();
            sb.Append(ind);

            if (Modifier.HasValue)
                sb.Append(Modifier.Value.Write()).Append(" ");
            if (isGetter)
                sb.Append("get");
            else
                sb.Append("set");
            
            if (Statements != null)
            {
                sb.Append("\n")
                    .Append(ind).Append("{\n")
                    .Append(Statements.ToString(indentation + 4)).Append("\n")
                    .Append(ind).Append("}");
            }
            else 
            {
                sb.Append(";");
            }          
            return sb.ToString();
        }

        /// <summary>
        /// Returns the string representation of the accessor.
        /// </summary>
        /// <returns>Returns the string representation of the accessor.</returns>
        public override string ToString()
        {           
            var sb = new StringBuilder();           
            if (Modifier.HasValue)
                sb.Append(Modifier.Value.Write()).Append(" "); 
            if (Statements != null)
            {
                sb.Append("\n")
                    .Append("{\n")
                    .Append(Statements.ToString()).Append("\n")
                    .Append("}");
            }
            else
            {
                sb.Append(";");
            }
            return sb.ToString();
        }
    }
}
