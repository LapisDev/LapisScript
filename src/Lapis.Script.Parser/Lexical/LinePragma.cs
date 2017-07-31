/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : LinePragma
 * Description : Represents the position of a token.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Parser.Lexical
{
    /// <summary>
    /// Represents the position of a token.
    /// </summary>
    public class LinePragma
    {
        /// <summary>
        /// Gets the line number of the position.
        /// </summary>
        /// <value>The line number of the position.</value>
        public int Line { get; private set; }

        /// <summary>
        /// Gets the character position in the current line.
        /// </summary>
        /// <value>The character position in the current line.</value>
        public int Span { get; private set; }

        /// <summary>
        /// Returns a string that represents the current <see cref="LinePragma"/>.
        /// </summary>
        /// <returns>The string representation of the current <see cref="LinePragma"/>.</returns>
        public override string ToString()
        {
            if (this.Equals(EOF))
                return "EOF";
            return "Line " + Line.ToString() + ", " + "Span " + Span.ToString();
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LinePragma"/> class with the specified parameters.
        /// </summary>
        /// <param name="line">The line number. </param>
        /// <param name="span">The character position in the current line.</param>
        /// <exception cref="ArgumentOutOfRangeException">The parameter is zero or negative.</exception>
        public LinePragma(int line, int span)
        {
            if (line <= 0 || span <= 0)
                throw new ArgumentOutOfRangeException();
            Line = line;
            Span = span;
        }

        /// <summary>
        /// Gets the position that represents the end of file.
        /// </summary>
        /// <value>The position that represents the end of file.</value>
        public static readonly LinePragma EOF = new LinePragma(int.MaxValue, int.MaxValue);
    }
}
