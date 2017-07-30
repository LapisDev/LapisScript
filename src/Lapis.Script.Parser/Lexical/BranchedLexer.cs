/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : BranchedLexer
 * Description : A lexical analyzer that can create branches.
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
    /// Represents a lexical analyzer that can create branches.
    /// </summary>
    public sealed class BranchedLexer : IDisposable
    {
        /// <summary>
        /// Returns the next available <see cref="Lexical.Token"/>，but does not consume it.
        /// </summary>
        /// <returns>The next <see cref="Lexical.Token"/> to be read, or <see langword="null"/> if there are no <see cref="Lexical.Token"/> to be read.</returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the object was disposed.</exception>
        /// <exception cref="LexerException">No matched lexeme was found.</exception>
        public Token Peek()
        {
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            Token t;
            if (_buffer.Count == 0)
                t = _lexer.Peek();
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
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            Token t;
            if (_buffer.Count == 0)
                t = ReadAndBuffer();
            else
                t = _buffer.Dequeue();          
            return t;
        }

        /// <summary>
        /// Creates a new branch from the lexer. 
        /// </summary>   
        /// <returns>The branch created from the lexer.</returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the object was disposed.</exception>
        public BranchedLexer NewBranch()
        {
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            return new BranchedLexer(this);
        }

        /// <summary>
        /// Returns a value indicating whether the lexer has the same root reader as another specified branch. 
        /// </summary>   
        /// <param name="branch">The specified branch.</param>
        /// <returns><see langword="true"/> if the lexer has the same root reader as <paramref name="branch"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the object was disposed.</exception>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public bool HasSameRoot(BranchedLexer branch)
        {
            if (branch == null)
                throw new ArgumentNullException();
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            else if (!_branches.Contains(branch))
                throw new ObjectDisposedException(branch.ToString());
            return this._lexer.Equals(branch._lexer);
        }

        /// <summary>
        /// Merges the lexer to the state of another specified branch.  
        /// </summary> 
        /// <param name="branch">The specified branch.</param>
        /// <exception cref="ObjectDisposedException">Methods were called after the object was disposed.</exception>
        /// <exception cref="InvalidOperationException">The lexer has a different root with the specified branch.</exception>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public void Merge(BranchedLexer branch)
        {
            if (branch == null)
                throw new ArgumentNullException();
            if (HasSameRoot(branch))
            {
                _buffer = new Queue<Token>(branch._buffer);
            }
            else
                throw new InvalidOperationException(ExceptionResource.DifferentRoot);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="BranchedLexer"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="BranchedLexer"/>. The <see cref="Dispose"/> method leaves the <see cref="BranchedLexer"/> in an unusable state.</remarks>
        public void Dispose()
        {
            if (_branches.Contains(this))
            {
                if (_branches.Count == 1)
                    _lexer.Dispose();
                _branches.Remove(this);
            }
        }


        #region Non-public

        internal BranchedLexer(string s, HashSet<Lexeme> lexemes)
        {
            _lexer = new Lexer(s, lexemes);
            _buffer = new Queue<Token>();
            _branches = new HashSet<BranchedLexer>();
            _branches.Add(this);
        } 
    

        private Queue<Token> _buffer;     

        private HashSet<BranchedLexer> _branches;

        private Lexer _lexer;

        private BranchedLexer(BranchedLexer parent)       
        {
            _branches = parent._branches;
            _branches.Add(this);
            _buffer = new Queue<Token>(parent._buffer);
            _lexer = parent._lexer;
        }

        private Token ReadAndBuffer()
        {
            Token t;
            t = _lexer.Read();
            if (t != null)
                foreach (var b in _branches)
                    if (!this.Equals(b))
                        b._buffer.Enqueue(t);
            return t;
        }

        #endregion

    }
}
