/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : RuntimeException
 * Description : The exception that is thrown in the runtime context.
 * Created     : 2015/6/21
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;

namespace Lapis.Script.Execution.Runtime
{
    /// <summary>
    /// The exception that is thrown in the runtime context.
    /// </summary>
    public class RuntimeException : Exception
    {
        /// <summary>
        /// Gets the position where the exception is thrown.
        /// </summary>
        /// <value>The position where the exception is thrown.</value>
        public LinePragma LinePragma { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeException"/> class with the specified <see cref="LinePragma"/> and error message.
        /// </summary>
        /// <param name="linePragma">The position where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public RuntimeException(LinePragma linePragma, string message)
            : base(message)
        {
            if (linePragma == null)
                throw new ArgumentNullException();
            LinePragma = linePragma;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeException"/> class with the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> is <see langword="null"/>.</exception>
        public RuntimeException(LinePragma linePragma, string message, Exception innerException)
            : base(message, innerException)
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
