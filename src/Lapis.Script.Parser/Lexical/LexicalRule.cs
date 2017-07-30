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
    /// <summary>
    /// Represents a lexical rule. This class is abstract.
    /// </summary>
    public abstract partial class LexicalRule
    {
        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public abstract bool Match(BranchedReader reader);


        internal LexicalRule() { }     
    }

    /// <summary>
    /// Represents the concatenation of two lexical rules.
    /// </summary>
    public class ConcatenateLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the first lexical rule.
        /// </summary>
        /// <value>The first lexical rule.</value>
        public LexicalRule First { get; private set; }

        /// <summary>
        /// Gets the second lexical rule.
        /// </summary>
        /// <value>The second lexical rule.</value>
        public LexicalRule Second { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            return First.Match(reader) && Second.Match(reader);
        }

        internal ConcatenateLexicalRule(LexicalRule first, LexicalRule second)
        {
            First = first;
            Second = second;
        }
    }

    /// <summary>
    /// Represents the union of two lexical rules.
    /// </summary>
    public class AlternateLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the first lexical rule.
        /// </summary>
        /// <value>The first lexical rule.</value>
        public LexicalRule First { get; private set; }

        /// <summary>
        /// Gets the second lexical rule.
        /// </summary>
        /// <value>The second lexical rule.</value>
        public LexicalRule Second { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            BranchedReader branch1 = reader.NewBranch();
            BranchedReader branch2 = reader.NewBranch();
            bool check1 = First.Match(branch1);
            bool check2 = Second.Match(branch2);
            if (!check1 && !check2)
            {
                branch1.Dispose();
                branch2.Dispose();
                return false;
            }
            if (check1 && check2)
                reader.Merge(branch1.Position > branch2.Position ? branch1 : branch2);
            else if (check1)
                reader.Merge(branch1);
            else if (check2)
                reader.Merge(branch2);
            branch1.Dispose();
            branch2.Dispose();
            return true;
        }

        internal AlternateLexicalRule(LexicalRule first, LexicalRule second)
        {
            First = first;
            Second = second;
        }
    }

    /// <summary>
    /// Represents the repetition of a lexical rule.
    /// </summary>
    public class RepeatingLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the maximum times of the repetition. <c>0</c> if there is no upper limit.
        /// </summary>
        /// <value>The maximum times of the repetition. <c>0</c> if there is no upper limit.</value>
        public int Max { get; private set; }

        /// <summary>
        /// Gets the minimum times of the repetition. 
        /// </summary>
        /// <value>The minimum times of the repetition. </value>
        public int Min { get; private set; }

        /// <summary>
        /// Gets the lexical rule to be repeated.
        /// </summary>
        /// <value>The lexical rule to be repeated.</value>
        public LexicalRule Content { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            BranchedReader branch0, branch1;
            branch0 = reader.NewBranch();
            bool check = true;
            int count = 0;
            while (true)
            {
                branch1 = branch0.NewBranch();
                check = Content.Match(branch1);
                if (check)
                {
                    branch0.Dispose();
                    branch0 = branch1;
                    count++;
                }
                else
                    break;
                if (Max > 0 && count == Max)
                    break;
            }
            branch1.Dispose();
            if (count < Min)
            {
                branch0.Dispose();
                return false;
            }
            else
            {
                reader.Merge(branch0);
                branch0.Dispose();
                return true;
            }
        }

        internal RepeatingLexicalRule(LexicalRule content, int min, int max)
        {
            Content = content;
            Min = min;
            Max = max;
        }
    }

    /// <summary>
    /// Represents the lexical rule that is a placeholder for an empty string.
    /// </summary>
    public class EmptyLexicalRule : LexicalRule
    {
        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            return true;
        }

        internal static readonly EmptyLexicalRule Instance = new EmptyLexicalRule();

        internal EmptyLexicalRule() { }

    }

    /// <summary>
    /// Represents a lexical rule that matches a set of characters.
    /// </summary>
    public class CharSetLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the characters that the lexical rule matches.
        /// </summary>
        /// <value>A string that contains the characters that the lexical rule matches.</value>
        public new string Chars { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            char c = reader.Read();
            if (c == '\0')
                return false;
            if (CharSet.Contains(c))
                return true;
            else
                return false;
        }

        internal CharSetLexicalRule(HashSet<char> chars)
        {
            CharSet = chars;
            Chars = new string(chars.ToArray());
        }

        internal CharSetLexicalRule(string chars)
        {
            CharSet = new HashSet<char>(chars.ToCharArray());
            Chars = chars;
        }

        internal HashSet<char> CharSet;
    }

    /// <summary>
    /// Represents a lexical rule that matches characters not in a character set.
    /// </summary>
    public class NegativeCharSetLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the characters that the lexical rule doesn't match.
        /// </summary>
        /// <value>A string that contains the characters that the lexical rule doesn't match.</value>
        public new string Chars { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            char c = reader.Read();
            if (c == '\0')           
                return false;
            if (!CharSet.Contains(c))           
                return true;
            else
                return false;
        }

        internal NegativeCharSetLexicalRule(HashSet<char> chars)
        {
            CharSet = chars;
            Chars = new string(chars.ToArray());
        }

        internal NegativeCharSetLexicalRule(string chars)
        {
            CharSet = new HashSet<char>(chars.ToCharArray());
            Chars = chars;
        }

        internal HashSet<char> CharSet { get; private set; }
    }

    /// <summary>
    /// Represents a lexical rule that matches a range of characters.
    /// </summary>
    public class CharRangeLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the first character of the range that the lexical rule matches.
        /// </summary>
        /// <value>The first character of the range that the lexical rule matches.</value>
        public char From { get; private set; }

        /// <summary>
        /// Gets the last character of the range that the lexical rule matches.
        /// </summary>
        /// <value>The last character of the range that the lexical rule matches.</value>
        public char To { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            char c = reader.Read();
            if (c == '\0')        
                return false;            
            if (From <= c && c <= To)          
                return true;          
            else
                return false;
        }

        internal CharRangeLexicalRule(char from, char to)
        {
            From = from;
            To = to;
        }
    }

    /// <summary>
    /// Represents a lexical rule that matches characters not in a range.
    /// </summary>
    public class NegativeCharRangeLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the first character of the range that the lexical rule doesn't match.
        /// </summary>
        /// <value>The first character of the range that the lexical rule doesn't match.</value>
        public char From { get; private set; }

        /// <summary>
        /// Gets the last character of the range that the lexical rule doesn't match.
        /// </summary>
        /// <value>The last character of the range that the lexical rule doesn't match.</value>
        public char To { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            char c = reader.Read();
            if (c == '\0')           
                return false;
            if (!(From <= c && c <= To))
                return true;
            else
                return false;
        }

        internal NegativeCharRangeLexicalRule(char from, char to)
        {
            From = from;
            To = to;
        }
    }

    /// <summary>
    /// Represents the lexical rule that matches any character except <c>'\0'</c>.
    /// </summary>
    public class AnyCharLexicalRule : LexicalRule
    {
        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            char c = reader.Read();
            if (c == '\0')
                return false;
            else
                return true;
        }

        internal static readonly AnyCharLexicalRule Instance = new AnyCharLexicalRule();

        internal AnyCharLexicalRule() { }
    }

    /// <summary>
    /// Represents a lexical rule that matches characters that meets the specified criteria.
    /// </summary>
    public class PredicateLexicalRule : LexicalRule
    {
        /// <summary>
        /// Gets the criteria that the character should meet.
        /// </summary>
        /// <value>The criteria that the character should meet.</value>
        public Predicate<char> Predicate { get; private set; }

        /// <summary>
        /// Returns a value indicating whether the next character matches the lexical rule.
        /// </summary>
        /// <param name="reader">The <see cref="BranchedReader"/> that provides the input character.</param>
        /// <returns><see langword="true"/> if the next character matches the lexical rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
        public override bool Match(BranchedReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            char c = reader.Read();
            if (Predicate(c))
                return true;
            else
                return false;
        }

        internal PredicateLexicalRule(Predicate<char> predicate) 
        {
            Predicate = predicate;
        }
    }
}