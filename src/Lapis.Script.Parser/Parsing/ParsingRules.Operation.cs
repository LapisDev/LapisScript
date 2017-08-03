/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : ParsingRule
 * Description : Represents the grammar rules.
 * Created     : 2015/5/27
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;

namespace Lapis.Script.Parser.Parsing
{
    public abstract partial class ParsingRule<TResult>
    {       

        /// <summary>
        /// Creates a grammar rule that matches a <see cref="Lexical.Token"/> of the specified <see cref="Lexeme"/>, and converts the matched <see cref="Lexical.Token"/> to the parsing result using the specified converter.
        /// </summary>
        /// <param name="lexeme">The specified <see cref="Lexeme"/> of the <see cref="Lexical.Token"/> to match.</param>
        /// <param name="func">The converter from the matched <see cref="Lexical.Token"/> to the parsing result.</param>
        /// <returns>The grammar rule created that matches a <see cref="Lexical.Token"/> of <paramref name="lexeme"/>.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public static ParsingRule<TResult> Token(Lexeme lexeme, Func<Token, TResult> func)
        {
            if (lexeme == null || func == null)
                throw new ArgumentNullException();
            else
                return new TokenParsingRule<TResult>(lexeme, func);
        }

        /// <summary>
        /// Creates a grammar rule that doesn't consume <see cref="Lexical.Token"/> and returns the specified result.
        /// </summary>
        /// <param name="result">The parsing result to return.</param>
        /// <returns>The created grammar rule that doesn't consume <see cref="Lexical.Token"/> and returns <paramref name="result"/>.</returns>
        public static ParsingRule<TResult> Empty(TResult result)
        {
            return new EmptyParsingRule<TResult>(result);
        }

        /// <summary>
        /// Creates the concatenation of the current parsing rule and another.
        /// </summary>
        /// <typeparam name="T2">The type of the result of the other parsing rule.</typeparam>
        /// <typeparam name="TR">The type of the final parsing result.</typeparam>
        /// <param name="rule">The other parsing rule.</param>
        /// <param name="func">The converter that combines the results from the two parsing rules to the final parsing result.</param>
        /// <returns>The concatenation of the current parsing rule and the other.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public ParsingRule<TR> Concat<T2, TR>(ParsingRule<T2> rule, Func<TResult, T2, TR> func)
        {
            if (rule == null || func == null)
                throw new ArgumentNullException();
            else
                return new ConcatenateParsingRule<TResult, T2, TR>(this, rule, func);
        }

        /// <summary>
        /// Creates the union of the current parsing rule and another.
        /// </summary>
        /// <param name="rule">The other parsing rule.</param>
        /// <returns>The union of the current parsing rule and the other.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public ParsingRule<TResult> Or(ParsingRule<TResult> rule)
        {
            if (rule == null)
                throw new ArgumentNullException();
            else
                return new AlternateParsingRule<TResult>(this, rule);
        }

        /// <summary>
        /// Creates the union of two parsing rules.
        /// </summary>
        /// <param name="left">The first parsing rule.</param>
        /// <param name="right">The second parsing rule.</param>
        /// <returns>The union of the two parsing rules.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public static ParsingRule<TResult> operator |(ParsingRule<TResult> left, ParsingRule<TResult> right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            else
                return new AlternateParsingRule<TResult>(left, right);
        }

        /// <summary>
        /// Creates the repetition of the current parsing rule with the specified lower and upper limits of repetition times.
        /// </summary>
        /// <typeparam name="T">The type of the final parsing result.</typeparam>
        /// <param name="func">The converter that combines the results from the repeated parsing rule to the final parsing result.</param>
        /// <param name="min">The minimum times of the repetition. </param>
        /// <param name="max">The maximum times of the repetition. <c>0</c> if there is no upper limit.</param>
        /// <returns>The repetition of the current parsing rule with repetition times of the minimum <paramref name="min"/> and the maximum <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The parameter is negative, or <paramref name="min"/> is greater than <paramref name="max"/> when <paramref name="max"/> is not <c>0</c>.</exception>
        public ParsingRule<T> Repeat<T>(Func<IEnumerable<TResult>, T> func, int min, int max)
        {
            if (func == null)
                throw new ArgumentNullException();
            if (min < 0 || max < 0)
                goto fail;
            if (min <= max || max == 0)
                return new RepeatingParsingRule<TResult, T>(this, func, min, max);
        fail:
            throw new ArgumentException();
        }

        /// <summary>
        /// Creates the repetition of the current parsing rule with no limits of repetition times.
        /// </summary>
        /// <typeparam name="T">The type of the final parsing result.</typeparam>
        /// <param name="func">The converter that combines the results from the repeated parsing rule to the final parsing result.</param>
        /// <returns>The repetition of the current parsing rule with no limits of repetition times.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public ParsingRule<T> Repeat<T>(Func<IEnumerable<TResult>, T> func)
        {
            if (func == null)
                throw new ArgumentNullException();
            else
                return new RepeatingParsingRule<TResult, T>(this, func, 0, 0);
        }

        /// <summary>
        /// Creates the repetition of the current parsing rule with no limits of repetition times.
        /// </summary>
        /// <returns>The repetition of the current parsing rule with no limits of repetition times.</returns>
        public ParsingRule<IEnumerable<TResult>> Repeat()
        {
            return new RepeatingParsingRule<TResult, IEnumerable<TResult>>(this, i => i, 0, 0);
        }

        /// <summary>
        /// Creates the repetition of the current parsing rule with the specified lower and upper limits of repetition times.
        /// </summary>
        /// <param name="min">The minimum times of the repetition. </param>
        /// <param name="max">The maximum times of the repetition. <c>0</c> if there is no upper limit.</param>
        /// <returns>The repetition of the current parsing rule with repetition times of the minimum <paramref name="min"/> and the maximum <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentException">The parameter is negative, or <paramref name="min"/> is greater than <paramref name="max"/> when <paramref name="max"/> is not <c>0</c>.</exception>
        public ParsingRule<IEnumerable<TResult>> Repeat(int min, int max)
        {
            return new RepeatingParsingRule<TResult, IEnumerable<TResult>>(this, i => i, min, max);
        }

        /// <summary>
        /// Creates a grammar rule that wraps the specified parsing rule and converts its result.
        /// </summary>    
        /// <typeparam name="T">The type of the parsing result.</typeparam>
        /// <param name="func">The converter that maps the result from the inner parsing rule to the final parsing result.</param>
        /// <returns>The mapping of the current parsing rule with the converter <paramref name="func"/>.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public ParsingRule<T> Map<T>(Func<TResult, T> func)
        {
            if (func == null)
                throw new ArgumentNullException();
            return new MappingParsingRule<TResult, T>(this, func);
        }

        /// <summary>
        /// Creates a grammar rule that uses the custom parsing rule and result converter.
        /// </summary>   
        /// <param name="func">The delegate that represents the parsing rule. It consumes tokens from the lexer, and returns a <see cref="Tuple{Boolean, TResult}"/> with the first item is a <see cref="Boolean"/> that indicating whether the match, and the second item that is the parsing result.</param>
        /// <returns>The created grammar rule that uses <paramref name="func"/>.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public static CustomParsingRule<TResult> Custom(Func<BranchedLexer, Tuple<bool, TResult>> func)
        {
            if (func == null)
                throw new ArgumentNullException();
            return new CustomParsingRule<TResult>(func);
        }
        
        /// <summary>
        /// Creates the same parsing rule as the current parsing rule but it is optional.
        /// </summary>
        /// <param name="result">The result to return when the parsing rule matches empty</param>
        /// <returns>The the same parsing rule as the current parsing rule but it is optional.</returns>
        public ParsingRule<TResult> Optional(TResult result)
        {
            return this.Or(Empty(result));
        }
    }
}
