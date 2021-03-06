﻿/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : LexerException
 * Description : The exception that is thrown during the lexical analysis.
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
    /// The exception that is thrown during the lexical analysis.
    /// </summary>
    public class LexerException : Exception
    {
        /// <summary>
        /// Gets the position where the exception is thrown.
        /// </summary>
        /// <value>The position where the exception is thrown.</value>
        public LinePragma LinePragma { get; private set; }
             
        /// <summary>
        /// Initializes a new instance of the <see cref="LexerException"/> class with the specified <see cref="LinePragma"/> and error message.
        /// </summary>
        /// <param name="linePragma">The position where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public LexerException(LinePragma linePragma, string message)
            : base(message)
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LexerException"/> class with the specified <see cref="LinePragma"/>.
        /// </summary>
        /// <param name="linePragma">The position where the exception is thrown.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public LexerException(LinePragma linePragma)
            : base(ExceptionResource.SyntaxError)
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value>The error message that explains the reason for the exception.</value>
        public override string Message
        {
            get
            {
                return LinePragma.ToString() + " : " + base.Message;
            }
        }
    }
}
