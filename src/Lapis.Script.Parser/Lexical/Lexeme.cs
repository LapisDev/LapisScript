/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : Lexeme
 * Description : Represents the definition of a basic lexical unit.
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
    /// Represents the definition of a basic lexical unit.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToString()} {Id}")]
    public class Lexeme
    {
        /// <summary>
        /// Gets the idntifier of the lexeme.
        /// </summary>
        /// <value>The idntifier of the lexeme.</value>
        public object Id { get; private set; }

        /// <summary>
        /// Gets the lexical rule of the lexeme.
        /// </summary>
        /// <value>The lexical rule of the lexeme.</value>
        public LexicalRule Rule { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the lexeme is skipped when the lexical analysis is performed.
        /// </summary>   
        /// <value><see langword="true"/> the lexeme is skipped when the lexical analysis is performed; otherwise <see langword="false"/>.</value>
        public bool IsSkippable { get; private set; }


        #region Non-public

        internal Lexeme(object id, LexicalRule rule)
        {
            Id = id;
            Rule = rule;
        }

        internal Lexeme(object id, bool isSkippable, LexicalRule rule)
        {
            Id = id;
            IsSkippable = isSkippable;
            Rule = rule;
        }

        #endregion

    }
   
}
