/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : Token
 * Description : Represents a lexical unit.
 * Created     : 2015/4/6
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Parser.Lexical
{
    /// <summary>
    /// Represents a lexical unit.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Get the definition of the lexical unit.
        /// </summary>
        /// <value>The definition of the lexical unit.</value>
        public Lexeme Lexeme { get; private set; }

        /// <summary>
        /// Get the text in the lexical unit.
        /// </summary>
        /// <value>The text in the lexical unit.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Get the position of the lexical unit.
        /// </summary>
        /// <value>The position of the lexical unit.</value>
        public LinePragma LinePragma { get; private set; }

        /// <summary>
        /// Returns a string that represents the current <see cref="Token"/>.
        /// </summary>
        /// <returns>The string representation of the current <see cref="Token"/>.</returns>
        public override string ToString()
        {
            return LinePragma.ToString() + ", Id = " + Lexeme.Id.ToString() + ", Text = " + Text;
        }

        internal Token(LinePragma linePragma, Lexeme lexeme, string text)
        {
            LinePragma = linePragma;
            Lexeme = lexeme;
            Text = text;
        }
    }
}
