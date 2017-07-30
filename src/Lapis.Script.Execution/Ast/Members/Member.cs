/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : Member
 * Description : Represents a member declarasion in the syntax tree.
 * Created     : 2015/6/3
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;

namespace Lapis.Script.Execution.Ast.Members
{
    /// <summary>
    /// Represents a member declarasion in the syntax tree. This class is abstract.
    /// </summary>
    public abstract class Member
    {   
        /// <summary>
        /// Gets a value indicating whether the member is static.
        /// </summary>
        /// <value><see langword="true"/> if the member is static; otherwise <see langword="false"/>.</value>
        public bool IsStatic { get; private set; }

        /// <summary>
        /// Gets the access modifier of the member.
        /// </summary>
        /// <value>The access modifier of the member.</value>
        public Modifier Modifier { get; private set; }

        /// <summary>
        /// Gets the position of the member in the script code.
        /// </summary>
        /// <value>The position of the member in the script code.</value>
        public LinePragma LinePragma { get; private set; }

        /// <summary>
        /// Returns the string representation of the member.
        /// </summary>
        /// <returns>Returns the string representation of the member.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public abstract string ToString(int indentation);

        /// <summary>
        /// Returns the string representation of the member.
        /// </summary>
        /// <returns>Returns the string representation of the member.</returns>
        public override string ToString()
        {
            return ToString(0);
        }

        internal Member(LinePragma linePragma, bool isStatic)
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;
            IsStatic = isStatic;
        }

        internal Member(LinePragma linePragma, bool isStatic, Modifier modifier)
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;
            IsStatic = isStatic;
            Modifier = modifier;
        }

        internal Member(LinePragma linePragma)
            : this(linePragma, false) { }

    }

    /// <summary>
    /// Represents an access modifier that indicates that the member is public, protected, or private.
    /// </summary>
    public enum Modifier
    {
        /// <summary>
        /// The value that indicates that the member is public.
        /// </summary>
        Public, 
        /// <summary>
        /// The value that indicates that the member is protected.
        /// </summary>
        Protected,
        /// <summary>
        /// The value that indicates that the member is private.
        /// </summary>
        Private
    }

    internal static class ModifierExtensions
    {
        public static string Write(this Modifier value)
        {
            if (value == Modifier.Protected)
                return "protected";
            else if (value == Modifier.Private)
                return "private";
            else if (value == Modifier.Public)
                return "public";
            else
                throw new ArgumentException();
        }

        public static bool IsMoreStrictThan(this Modifier value, Modifier other)
        {
            return value > other;            
        }
    }
}
