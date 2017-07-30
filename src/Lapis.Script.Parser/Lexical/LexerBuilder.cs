/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : LexerBuilder
 * Description : Provides methods for creating lexical analyzers.
 * Created     : 2015/5/27
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Parser.Lexical
{
    /// <summary>
    /// Provides methods for creating lexical analyzers.
    /// </summary>
    public class LexerBuilder
    {
        /// <summary>
        /// Creates a lexeme using the specified identifier and lexical rule.
        /// </summary>
        /// <param name="id">The specified identifier.</param>
        /// <param name="rule">The specified lexical rule.</param>
        /// <returns>The lexeme created using the specified identifier and lexical rule.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public Lexeme DefineLexeme(object id, LexicalRule rule)
        {
            if (id == null || rule == null)
                throw new ArgumentNullException();
            Lexeme lexeme = new Lexeme(id, rule);
            _lexemes.Add(lexeme);
            return lexeme;
        }

        /// <summary>
        /// Creates a lexeme using the specified identifier and lexical rule.
        /// </summary>
        /// <param name="id">The specified identifier.</param>
        /// <param name="isSkippable">A value that indicates whether the lexeme is skipped when the lexical analysis is performed.</param>
        /// <param name="rule">The specified lexical rule.</param>
        /// <returns>The lexeme created using the specified identifier and lexical rule.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public Lexeme DefineLexeme(object id, bool isSkippable, LexicalRule rule)
        {
            if (id == null || rule == null)
                throw new ArgumentNullException();
            Lexeme lexeme = new Lexeme(id, isSkippable, rule);
            _lexemes.Add(lexeme);
            return lexeme;
        }
       
        /// <summary>
        /// Creates a lexer for the specified string.
        /// </summary>
        /// <param name="s">The string for lexical analysis.</param>
        /// <returns>The lexer created for the specified string.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public Lexer GetLexer(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            return new Lexer(s, new HashSet<Lexeme>(_lexemes));
        }

        /// <summary>
        /// Creates a lexer that can create branches for the specified string.
        /// </summary>
        /// <param name="s">The string for lexical analysis.</param>
        /// <returns>The lexer created for the specified string.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public BranchedLexer GetBranchedLexer(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            return new BranchedLexer(s, new HashSet<Lexeme>(_lexemes));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LexerBuilder"/> class.
        /// </summary>
        public LexerBuilder() 
        {
            _lexemes = new HashSet<Lexeme>();
        }


        #region Private

        private HashSet<Lexeme> _lexemes;

        #endregion

    }
}
