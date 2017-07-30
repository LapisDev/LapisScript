/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : BranchedReader
 * Description : A string reader that can create branches.
 * Created     : 2015/5/27
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lapis.Script.Parser.Lexical
{
    /// <summary>
    /// Represents a string reader that can create branches.
    /// </summary>
    public sealed class BranchedReader : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchedReader"/> class that reads from the specified string.
        /// </summary>
        /// <param name="s">The string to which the <see cref="BranchedReader"/> should be initialized.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public BranchedReader(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            _reader = new Reader(s);
            _buffer = new Queue<char>();
            _branches = new HashSet<BranchedReader>();
            _branches.Add(this);
            Position = 0;
            Line = 1;
            Span = 0;
        }      

        /// <summary>
        /// Returns the next available character but does not consume it.
        /// </summary>
        /// <returns>The next character to be read, or <c>'\0'</c> if no more characters are available.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        public char Peek()
        {
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            char c;
            if (_buffer.Count == 0)
                c = _reader.Peek();
            else
                c = _buffer.Peek();
            return c;
        }

        /// <summary>
        /// Reads the next character from the input string and advances the character position by one character.
        /// </summary>
        /// <returns>The next character from the underlying string, or <c>'\0'</c> if no more characters are available.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        public char Read()
        {
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            char c;
            if (_buffer.Count == 0)
                c = ReadAndBuffer();
            else
                c = _buffer.Dequeue();
            if (c != '\0')
            {
                if (c == '\n')
                {
                    Line++;
                    Span = 0;
                }
                else
                    Span++;
                Position++;
            }
            return c;
        }

        /// <summary>
        /// Reads a block of characters from the input string and advances the character position by <paramref name="length"/>.
        /// </summary>
        /// <param name="length">The number of characters to read.</param>
        /// <returns>A string that contains the characters read from the current source.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is zero or negative.</exception>
        public string Read(int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException();
            StringBuilder sb = new StringBuilder();
            char c;
            int i = 0;
            while (i < length && (c = Read()) != '\0')
            {
                sb.Append(c);
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a new branch from the reader. 
        /// </summary>   
        /// <returns>The branch created from the reader.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        public BranchedReader NewBranch()
        {
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            return new BranchedReader(this);
        }

        /// <summary>
        /// Returns a value indicating whether the reader has the same root reader as another specified branch. 
        /// </summary>   
        /// <param name="branch">The specified branch.</param>
        /// <returns><see langword="true"/> if the lexer has the same root reader as <paramref name="branch"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public bool HasSameRoot(BranchedReader branch)
        {
            if (branch == null)
                throw new ArgumentNullException();
            if (!_branches.Contains(this))
                throw new ObjectDisposedException(ToString());
            else if (!_branches.Contains(branch))
                throw new ObjectDisposedException(branch.ToString());
            return this._reader.Equals(branch._reader);
        }

        /// <summary>
        /// Merges the reader to the state of another specified branch, and returns a string that contains the characters between the positions of the current reader and <paramref name="branch"/>.  
        /// </summary>   
        /// <param name="branch">The specified branch.</param>
        /// <returns>A string that contains the characters between the positions of the current reader and <paramref name="branch"/> if the current reader is ahead of <paramref name="branch"/>; A empty string if the current reader and <paramref name="branch"/> are in the same position; <see langword="null"/> if the current reader is behind <paramref name="branch"/>.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        /// <exception cref="InvalidOperationException">The reader has a different root with the specified branch.</exception>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public string Merge(BranchedReader branch)
        {
            if (branch == null)
                throw new ArgumentNullException();      
            if (HasSameRoot(branch))
            {
                if (Position <= branch.Position)
                {
                    var sb = new StringBuilder();
                    while (Position < branch.Position)
                        sb.Append(Read());
                    return sb.ToString();
                }                
                else
                {
                    _buffer = new Queue<char>(branch._buffer);
                    Line = branch.Line;
                    Span = branch.Span;
                    Position = branch.Position;
                    return null;
                }
            }
            else
                throw new InvalidOperationException(ExceptionResource.DifferentRoot);
        }

        /// <summary>
        /// Gets the position of the current reader in the string.
        /// </summary>
        /// <value>The position of the current reader in the string.</value>
        public int Position { get; private set; }

        /// <summary>
        /// Gets the line number of the current reader in the string.
        /// </summary>
        /// <value>The line number of the current reader in the string.</value>
        public int Line { get; private set; }

        /// <summary>
        /// Gets the position of the current reader in the current line of the string.
        /// </summary>
        /// <value>The position of the current reader in the current line of the string.</value>
        public int Span { get; private set; }

        /// <summary>
        /// Releases all resources used by the <see cref="BranchedReader"/>.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="BranchedReader"/>. The <see cref="Dispose"/> method leaves the <see cref="BranchedReader"/> in an unusable state.</remarks>
        public void Dispose()
        {
            if (_branches.Contains(this))
            {                
                if (_branches.Count == 1)
                    _reader.Dispose();
                _branches.Remove(this);
            }
        }


        #region Private

        private Queue<char> _buffer;    

        private Reader _reader;

        private HashSet<BranchedReader> _branches;

        private BranchedReader(BranchedReader parent)
        {
            _reader = parent._reader;
            _branches = parent._branches;
            _branches.Add(this);
            _buffer = new Queue<char>(parent._buffer);
            Line = parent.Line;
            Span = parent.Span;
            Position = parent.Position;
        }

        private char ReadAndBuffer()
        {
            char c;
            c = _reader.Read();
            if (c != '\0')
                foreach (var b in _branches)
                    if (!this.Equals(b))
                        b._buffer.Enqueue(c);
            return c;
        }

        #endregion

    }   

}
