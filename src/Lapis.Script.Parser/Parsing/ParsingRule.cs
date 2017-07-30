/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : ParsingRule
 * Description : Represents the grammar rules.
 * Created     : 2015/5/30
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;

namespace Lapis.Script.Parser.Parsing
{
    /// <summary>
    /// Provides static methods for creating the grammar rules. 
    /// </summary>
    public static partial class ParsingRule
    {
        /// <summary>
        /// Creates a grammar rule that matches a <see cref="Lexical.Token"/> of the specified <see cref="Lexeme"/>.
        /// </summary>
        /// <param name="lexeme">The specified <see cref="Lexeme"/> of the <see cref="Lexical.Token"/> to match.</param>
        /// <returns>The grammar rule created that matches a <see cref="Lexical.Token"/> of <paramref name="lexeme"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexeme"/> is <see langword="null"/>.</exception>
        public static ParsingRule<Token> Token(Lexeme lexeme)
        {
            if (lexeme == null)
                throw new ArgumentNullException();
            else
                return new TokenParsingRule<Token>(lexeme, t => t);
        }

        /// <summary>
        /// Creates a grammar rule that matches a <see cref="Lexical.Token"/> of the specified <see cref="Lexeme"/>.
        /// </summary>
        /// <param name="lexeme">The specified <see cref="Lexeme"/> of the <see cref="Lexical.Token"/> to match.</param>
        /// <returns>The grammar rule created that matches a <see cref="Lexical.Token"/> of <paramref name="lexeme"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexeme"/> is <see langword="null"/>.</exception>
        public static ParsingRule<Token> GetParsingRule(
            this Lexeme lexeme)
        {
            if (lexeme == null)
                throw new ArgumentNullException();
            else
                return new TokenParsingRule<Token>(lexeme, t => t);
        }

        /// <summary>
        /// Creates a grammar rule that matches a <see cref="Lexical.Token"/> of the specified <see cref="Lexeme"/>, and converts the matched <see cref="Lexical.Token"/> to the parsing result using the specified converter.
        /// </summary>
        /// <typeparam name="TResult">The type of the parsing result.</typeparam>
        /// <param name="lexeme">The specified <see cref="Lexeme"/> of the <see cref="Lexical.Token"/> to match.</param>
        /// <param name="func">The converter from the matched <see cref="Lexical.Token"/> to the parsing result.</param>
        /// <returns>The grammar rule created that matches a <see cref="Lexical.Token"/> of <paramref name="lexeme"/>.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public static ParsingRule<TResult> GetParsingRule<TResult>(
            this Lexeme lexeme, Func<Token, TResult> func)
        {
            if (lexeme == null || func == null)
                throw new ArgumentNullException();
            return ParsingRule<TResult>.Token(lexeme, func);
        }              

        /// <summary>
        /// Creates a grammar rule that doesn't consume <see cref="Lexical.Token"/> and returns the specified result.
        /// </summary>
        /// <typeparam name="TResult">The type of the parsing result.</typeparam>
        /// <param name="result">The parsing result to return.</param>
        /// <returns>The created grammar rule that doesn't consume <see cref="Lexical.Token"/> and returns <paramref name="result"/>.</returns>
        public static ParsingRule<TResult> Empty<TResult>(TResult result)
        {
            return new EmptyParsingRule<TResult>(result);
        }          
   
        /// <summary>
        /// Creates the union of two grammar rules.
        /// </summary>
        /// <param name="left">The first grammar rule.</param>
        /// <param name="right">The second grammar rule.</param>
        /// <returns>The union of the two grammar rules.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public static ParsingRule<TResult> Or<TResult>(ParsingRule<TResult> left, ParsingRule<TResult> right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            else
                return new AlternateParsingRule<TResult>(left, right);
        }                

        /// <summary>
        /// Creates a grammar rule that uses the custom parsing rule and result converter.
        /// </summary>   
        /// <typeparam name="TResult">The type of the parsing result.</typeparam>
        /// <param name="func">A delegate that represents the parsing rule. It consumes tokens from the lexer, and returns a <see cref="Tuple{Boolean, TResult}"/> with the first item is a <see cref="Boolean"/> that indicating whether the match, and the second item that is the parsing result.</param>
        /// <returns>The created grammar rule that uses <paramref name="func"/> 的语法规则.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public static CustomParsingRule<TResult> Custom<TResult>(Func<BranchedLexer, Tuple<bool, TResult>> func)
        {
            if (func == null)
                throw new ArgumentNullException();
            return new CustomParsingRule<TResult>(func);
        }
    }
}
