/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : Lexer
 * Description : Represents a lexical analyzer.
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
    /// Represents a lexical analyzer.
    /// </summary>
    public sealed class Lexer : IDisposable
    {
        /// <summary>
        /// Returns the next available <see cref="Lexical.Token"/>，but does not consume it.
        /// </summary>
        /// <returns>The next <see cref="Lexical.Token"/> to be read, or <see langword="null"/> if there are no <see cref="Lexical.Token"/> to be read.</returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the object was disposed.</exception>
        /// <exception cref="LexerException">No matched lexeme was found.</exception>
        public Token Peek()
        {
            if (_disposed)
                throw new ObjectDisposedException(ToString());
            Token t;
            if (_buffer.Count == 0)
            {
                t = Match();
                _buffer.Enqueue(t);
            }
            else
                t = _buffer.Peek();
            return t;
        }

        /// <summary>
        /// Reads the next <see cref="Lexical.Token"/> from the input string.  
        /// </summary>
        /// <returns>The next <see cref="Lexical.Token"/> to be read, or <see langword="null"/> if there are no <see cref="Lexical.Token"/> to be read.</returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the object was disposed.</exception>
        /// <exception cref="LexerException">No matched lexeme was found.</exception>
        public Token Read()
        {
            if (_disposed)
                throw new ObjectDisposedException(ToString());
            Token t;
            if (_buffer.Count == 0)
            {
                t = Match();
            }
            else
                t = _buffer.Dequeue();
            return t;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Lexer"/> object. 
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Lexer"/>. The <see cref="Dispose"/> method leaves the <see cref="Lexer"/> in an unusable state.</remarks>
        public void Dispose()
        {
            if (_disposed == false)
            {
                _reader.Dispose();
                _disposed = true;
            }
        }
        

        #region Non-public

        internal Lexer(string s, HashSet<Lexeme> lexemes)
        {
            _reader = new BranchedReader(s);
            _lexemes = lexemes;
            _buffer = new Queue<Token>();
            _disposed = false;
        }    


        private HashSet<Lexeme> _lexemes;

        private BranchedReader _reader;

        private bool _disposed;

        private Queue<Token> _buffer;

        private Token Match()
        {
            if (_reader.Peek() == '\0')
                return null;
            LinePragma line = new LinePragma(_reader.Line, _reader.Span + 1);
            BranchedReader leading = null, branch;
            Lexeme lexeme = null;
            foreach (var t in _lexemes)
            {
                branch = _reader.NewBranch();
                if (t.Rule.Match(branch) &&
                     branch.Position >= _reader.Position)
                {
                    if (leading != null)
                        if (branch.Position >= leading.Position)
                        {
                            leading.Dispose();
                            leading = branch;
                            lexeme = t;
                        }
                        else
                            branch.Dispose();
                    else
                    {
                        leading = branch;
                        lexeme = t;
                    }
                }
                else
                    branch.Dispose();
            }
            if (leading == null)
                throw new LexerException(line, ExceptionResource.MatchFailed);
            else
            {
                string text = _reader.Merge(leading);
                leading.Dispose();
                if (lexeme.IsSkippable)
                    return Match();
                else
                    return new Token(line, lexeme, text);
            }
        }

        #endregion

    }
}
