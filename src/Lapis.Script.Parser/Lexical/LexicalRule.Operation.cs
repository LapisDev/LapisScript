/********************************************************************************
 * Module      : Lapis.Script.Parser
 * Class       : LexicalRule
 * Description : Represents the lexical rules.
 * Created     : 2015/5/27
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Parser.Lexical
{
    public abstract partial class LexicalRule
    {
        /// <summary>
        /// Creates a lexical rule that matches the specified character.
        /// </summary>
        /// <param name="c">The specified character.</param>
        /// <returns>The created lexical rule that matches <paramref name="c"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="c"/> is <c>'\0'</c>.</exception>

        public static LexicalRule Char(char c)
        {
            if (c != '\0')
                return new CharSetLexicalRule(new HashSet<char>() { c });
            else
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Creates a lexical rule that matches the specified set of characters.
        /// </summary>
        /// <param name="s">The specified set of characters.</param>
        /// <returns>The created lexical rule that matches <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="s"/> is <see langword="null"/> or an empty string, or it contains <c>'\0'</c>.</exception>
        public static LexicalRule Chars(string s)
        {
            if (string.IsNullOrEmpty(s) || s.IndexOf('\0') >= 0)
                throw new ArgumentException();
            return new CharSetLexicalRule(new HashSet<char>(s.ToCharArray()));
        }

        /// <summary>
        /// Creates a lexical rule that matches characters except the specified character.
        /// </summary>
        /// <param name="c">The specified character.</param>
        /// <returns>The created lexical rule that matches characters except <paramref name="c"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="c"/> is <c>'\0'</c>.</exception>
        public static LexicalRule NotChar(char c)
        {
            if (c != '\0')
                return new NegativeCharSetLexicalRule(new HashSet<char>() { c });
            else
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Creates a lexical rule that matches characters not in the specified set of characters.
        /// </summary>
        /// <param name="s">The specified set of characters.</param>
        /// <returns>The created lexical rule that matches characters not in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="s"/> is <see langword="null"/> or an empty string, or it contains <c>'\0'</c>.</exception>
        public static LexicalRule NotChars(string s)
        {
             if (string.IsNullOrEmpty(s) || s.IndexOf('\0') >= 0)
                throw new ArgumentException();
             return new NegativeCharSetLexicalRule(new HashSet<char>(s.ToCharArray()));          
        }

        /// <summary>
        /// Creates a lexical rule that matches the specified range of characters.
        /// </summary>
        /// <param name="from">The first character of the specified range.</param>
        /// <param name="to">The last character of the specified range..</param>
        /// <returns>The created lexical rule that matches characters in the specified range.</returns>
        /// <exception cref="ArgumentException">The parameter is <c>'\0'</c>, or <paramref name="from"/> is greater than <paramref name="to"/>.</exception>
        public static LexicalRule Range(char from, char to)
        {
            if (from != '\0' && from < to)
                return new CharRangeLexicalRule(from, to);
            else if (from == to)
                return Char(from);
            else
                throw new ArgumentException(ExceptionResource.InvalidCharRange);
        }

        /// <summary>
        /// Creates a lexical rule that matches characters not in the specified range.
        /// </summary>
        /// <param name="from">The first character of the specified range.</param>
        /// <param name="to">The last character of the specified range..</param>
        /// <returns>The created lexical rule that matches characters not in the specified range.</returns>
        /// <exception cref="ArgumentException">The parameter is <c>'\0'</c>, or <paramref name="from"/> is greater than <paramref name="to"/>.</exception>
        public static LexicalRule NotRange(char from, char to)
        {
            if (from != '\0' && from < to)
                return new NegativeCharRangeLexicalRule(from, to);
            else if (from == to)
                return NotChar(from);
            else
                throw new ArgumentException(ExceptionResource.InvalidCharRange);
        }

        /// <summary>
        /// Gets the lexical rule that is a placeholder for an empty string.
        /// </summary>
        /// <value>The lexical rule that is a placeholder for an empty string.</value>
        public static LexicalRule Empty
        {
            get { return EmptyLexicalRule.Instance; }
        }

        /// <summary>
        /// Gets the lexical rule that matches any character except <c>'\0'</c>.
        /// </summary>
        /// <value>The lexical rule that matches any character except <c>'\0'</c>.</value>
        public static LexicalRule AnyChar
        {
            get { return AnyCharLexicalRule.Instance; }
        }

        /// <summary>
        /// Creates a lexical rule that matches the specified string literally.
        /// </summary>
        /// <param name="s">The specified string.</param>
        /// <returns>The created lexical rule that matches <paramref name="s"/> literally.</returns>
        /// <exception cref="ArgumentException"><paramref name="s"/> is <see langword="null"/> or an empty string, or it contains <c>'\0'</c>.</exception>
        public static LexicalRule Literal(string s)
        {
            if (string.IsNullOrEmpty(s) || s.IndexOf('\0') >= 0)
                throw new ArgumentException();
            LexicalRule r = Empty;
            foreach (char c in s)
                r = r.Concat(Char(c));
            return r;
        }

        /// <summary>
        /// Creates the concatenation of the current lexical rule and another.
        /// </summary>
        /// <param name="rule">The other lexical rule.</param>
        /// <returns>The concatenation of the current lexical rule and the other.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="rule"/> is <see langword="null"/>.</exception>
        public LexicalRule Concat(LexicalRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException();
            else if (this is EmptyLexicalRule)
                return rule;
            else if (rule is EmptyLexicalRule)
                return this;
            else
                return new ConcatenateLexicalRule(this, rule);
        }

        /// <summary>
        /// Creates the union of the current lexical rule and another.
        /// </summary>
        /// <param name="rule">The other lexical rule.</param>
        /// <returns>The union of the current lexical rule and the other.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="rule"/> is <see langword="null"/>.</exception>
        public LexicalRule Or(LexicalRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException();
            else if (this is CharSetLexicalRule && rule is CharSetLexicalRule)
                return new CharSetLexicalRule(new HashSet<char>(
                   ((CharSetLexicalRule)this).CharSet.Union(
                   ((CharSetLexicalRule)rule).CharSet)
                   ));
            else if (this is NegativeCharSetLexicalRule && rule is NegativeCharSetLexicalRule)
                return new CharSetLexicalRule(new HashSet<char>(
                    ((CharSetLexicalRule)this).CharSet.Intersect(
                    ((CharSetLexicalRule)rule).CharSet)
                    ));
            else
                return new AlternateLexicalRule(this, rule);
        }

        /// <summary>
        /// Creates the concatenation of two lexical rules.
        /// </summary>
        /// <param name="left">The first lexical rule.</param>
        /// <param name="right">The second lexical rule.</param>
        /// <returns>The concatenation of the two lexical rules.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public static LexicalRule operator +(LexicalRule left, LexicalRule right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            else
                return left.Concat(right);
        }

        /// <summary>
        /// Creates the union of two lexical rules.
        /// </summary>
        /// <param name="left">The first lexical rule.</param>
        /// <param name="right">The second lexical rule.</param>
        /// <returns>The union of the two lexical rules.</returns>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public static LexicalRule operator |(LexicalRule left, LexicalRule right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            else
                return left.Or(right);
        }

        /// <summary>
        /// Creates the repetition of the current lexical rule with no limits of repetition times.
        /// </summary>
        /// <returns>The repetition of the current lexical rule with no limits of repetition times.</returns>
        public LexicalRule Repeat()
        {
            return new RepeatingLexicalRule(this, 0, 0); ;
        }

        /// <summary>
        /// Creates the repetition of the current lexical rule with the specified lower and upper limits of repetition times.
        /// </summary>
        /// <param name="min">The minimum times of the repetition. </param>
        /// <param name="max">The maximum times of the repetition. <c>0</c> if there is no upper limit.</param>
        /// <returns>The repetition of the current lexical rule with repetition times of the minimum <paramref name="min"/> and the maximum <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentException">The parameter is negative, or <paramref name="min"/> is greater than <paramref name="max"/> when <paramref name="max"/> is not <c>0</c>.</exception>
        public LexicalRule Repeat(int min, int max)
        {
            if (min < 0 || max < 0)
                goto fail;
            if (min <= max || max == 0)
                return new RepeatingLexicalRule(this, min, max);
        fail:
            throw new ArgumentException();
        }

        /// <summary>
        /// Creates a lexical rule that matches characters that meets the specified criteria.
        /// </summary>
        /// <param name="predicate">The specified criteria that the character should meet.</param>
        /// <returns>The created lexical rule that matches characters that meets <paramref name="predicate"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static LexicalRule CharWhen(Predicate<char> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException();
            else
                return new PredicateLexicalRule(predicate);
        }

        /// <summary>
        /// Creates the same lexical rule as the current lexical rule but it is optional.
        /// </summary>
        /// <returns>The same lexical rule as the current lexical rule but it is optional.</returns>
        public LexicalRule Optional()
        {
            return this.Or(Empty);
        }
    }
}
