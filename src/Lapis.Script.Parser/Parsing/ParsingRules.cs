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
    /// <summary>
    /// Represents a container of a parsing rule. The actual parsing rule can be set as the content after the instance of the <see cref="ParsingRuleContainer{TResult}"/> class is created.
    /// </summary>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class ParsingRuleContainer<TResult> : ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Content"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            if (_content == null)
                throw new InvalidOperationException(ExceptionResource.ContentNull);
            TResult r;
            if (Content.TryParse(lexer, out r))
            {
                result = r; // if (Id != null) System.Diagnostics.Debug.WriteLine(Id + " : " + result);
                return true;
            }
            else
            {
                result = r;
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the actual parsing rule that the container has. The content of the container can be set only once.
        /// </summary>
        /// <value>The actual parsing rule that the container has.</value>
        /// <exception cref="ArgumentNullException"><see cref="Content"/> is set to <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Content"/> has already been set.</exception>
        public ParsingRule<TResult> Content 
        {
            get { return _content; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (_content == null) 
                    _content = value;
                else 
                    throw new InvalidOperationException(ExceptionResource.ContentHasAlreadySet);
            }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ParsingRuleContainer{TResult}"/> class.
        /// </summary>
        public ParsingRuleContainer() { }        
      
        /// <summary>
        /// Initialize a new instance of the <see cref="ParsingRuleContainer{TResult}"/> class using the specified identifier.
        /// </summary>
        /// <param name="id">The specified identifier.</param>
        public ParsingRuleContainer(object id)
            : base(id) { }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            if (Content != null)
                return "[" + Content.ToString() + "]";
            return base.ToString();
        }

        private ParsingRule<TResult> _content;     
    }

    /// <summary>
    /// Represents a cparsing rule. This class is abstract.
    /// </summary>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    // [System.Diagnostics.DebuggerDisplay("{Id} {ToString()}")]
    public abstract partial class ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public abstract bool TryParse(BranchedLexer lexer, out TResult result);

        /// <summary>
        /// Initialize a new instance of the <see cref="ParsingRule{TResult}"/> class.
        /// </summary>
        public ParsingRule() { }

        /// <summary>
        /// Gets the identifier of the parsing rule.
        /// </summary>
        /// <value>The identifier of the parsing rule.</value>
        public object Id { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ParsingRule{TResult}"/> class using the specified identifier.
        /// </summary>
        /// <param name="id">The specified identifier.</param>
        public ParsingRule(object id)
        {
            Id = id;
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="ParsingRule{TResult}"/>.
        /// </summary>
        /// <returns>The string representation of the current <see cref="ParsingRule{TResult}"/>.</returns>
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            return base.ToString();
        }

    }

    /// <summary>
    /// Represents a grammar rule that doesn't consume <see cref="Lexical.Token"/> and returns the specified result.
    /// </summary>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class EmptyParsingRule<TResult> : ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            result = Result; 
            return true;
        }

        /// <summary>
        /// 获取匹配成功时返回的语法分析结果.
        /// </summary>
        /// <value>匹配成功时返回的语法分析结果.</value>
        public TResult Result { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            return "Empty";
        }

        internal EmptyParsingRule(TResult result)
        {
            Result = result;
        }
    }

    /// <summary>
    /// Represents a grammar rule that matches a <see cref="Lexical.Token"/> of the specified <see cref="Lexeme"/>, and converts the matched <see cref="Lexical.Token"/> to the parsing result using the specified converter.
    /// </summary>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class TokenParsingRule<TResult> : ParsingRule<TResult>
    { 
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            var t = lexer.Read();
            if (t != null && t.Lexeme.Equals(Lexeme))
            {
                result = Func(t);
                return true;
            }
            else
            {
                result = default(TResult); 
                return false;
            }
        }

        /// <summary>
        /// Gets the <see cref="Lexeme"/> of the <see cref="Lexical.Token"/> to match.
        /// </summary>
        public Lexeme Lexeme { get; private set; }

        /// <summary>
        /// Gets the converter from the matched <see cref="Lexical.Token"/> to the parsing result.
        /// </summary>
        public Func<Token, TResult> Func { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            return "Token";
        }

        internal TokenParsingRule(Lexeme lexeme, Func<Token, TResult> func)
            : base(lexeme.Id)
        {
            Lexeme = lexeme;
            Func = func;
        }

    }

    /// <summary>
    /// Represents the concatenation of two parsing rules.
    /// </summary>
    /// <typeparam name="T1">The type of the result of the first parsing rule.</typeparam>
    /// <typeparam name="T2">The type of the result of the second parsing rule.</typeparam>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class ConcatenateParsingRule<T1, T2, TResult> : ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            T1 r1; T2 r2;         
            if (First.TryParse(lexer, out r1) &&
                Second.TryParse(lexer, out r2))
            {
                result = Func(r1, r2);
                return true;
            }
            else
            {
                result = default(TResult);
                return false;
            }
        }

        /// <summary>
        /// Gets the first parsing rule.
        /// </summary>
        /// <value>The first parsing rule.</value>
        public ParsingRule<T1> First { get; private set; }

        /// <summary>
        /// Gets the second parsing rule.
        /// </summary>
        /// <value>The second parsing rule.</value>
        public ParsingRule<T2> Second { get; private set; }

        /// <summary>
        /// Gets the converter that combines the results from the two parsing rules to the final parsing result.
        /// </summary>
        /// <value>The converter that combines the results from the two parsing rules to the final parsing result.</value>
        public Func<T1, T2, TResult> Func { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            return "(" + First.ToString() + ")(" + Second.ToString() + ")";
        }

        internal ConcatenateParsingRule(ParsingRule<T1> first, ParsingRule<T2> second, Func<T1, T2, TResult> func)
        {
            First = first;
            Second = second;
            Func = func;
        }

    }

    /// <summary>
    /// Represents the union of two parsing rules.
    /// </summary>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class AlternateParsingRule<TResult> : ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            TResult r;
            BranchedLexer branch;
            branch = lexer.NewBranch();
            if (First.TryParse(branch, out r))
            {
                lexer.Merge(branch);
                branch.Dispose();
                result = r;
                return true;
            }
            branch.Dispose();
            branch = lexer.NewBranch();
            if (Second.TryParse(branch, out r))
            {
                lexer.Merge(branch);
                branch.Dispose();
                result = r;
                return true;
            }
            else
            {
                branch.Dispose();
                result = default(TResult); 
                return false;
            }
        }

        /// <summary>
        /// Gets the first parsing rule.
        /// </summary>
        /// <value>The first parsing rule.</value>
        public ParsingRule<TResult> First { get; private set; }

        /// <summary>
        /// Gets the second parsing rule.
        /// </summary>
        /// <value>The second parsing rule.</value>
        public ParsingRule<TResult> Second { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            return "(" + First.ToString() + ")|(" + Second.ToString() + ")";
        }

        internal AlternateParsingRule(ParsingRule<TResult> first, ParsingRule<TResult> second)
        {
            First = first;
            Second = second;
        }
    }

    /// <summary>
    /// Represents the repetition of a parsing rule.
    /// </summary>
    /// <typeparam name="T">The type of the result of the inner parsing rule.</typeparam>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class RepeatingParsingRule<T, TResult> : ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            T loopr;
            List<T> list = new List<T>();
            BranchedLexer branch0, branch1;
            branch0 = lexer.NewBranch();
            bool check = true;
            int count = 0;
            while (true)
            {
                branch1 = branch0.NewBranch();
                check = Content.TryParse(branch1, out loopr);
                if (check)
                {
                    branch0.Dispose();
                    branch0 = branch1;
                    list.Add(loopr);
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
                result = default(TResult);
                return false;
            }
            else
            {
                lexer.Merge(branch0);
                branch0.Dispose();
                result = Func(list);
                return true;
            }
        }

        /// <summary>
        /// Gets the converter that combines the results from the repeated parsing rule to the final parsing result.
        /// </summary>
        /// <value>The converter that combines the results from the repeated parsing rule to the final parsing result.</value>
        public Func<IEnumerable<T>, TResult> Func { get; private set; }

        /// <summary>
        /// Gets the parsing rule to be repeated.
        /// </summary>
        /// <value>The parsing rule to be repeated.</value>
        public ParsingRule<T> Content { get; private set; }

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

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            return "(" + Content.ToString() + "){" + Min + "," + Max + "}";
        }

        internal RepeatingParsingRule(
            ParsingRule<T> content, 
            Func<IEnumerable<T>, TResult> func, 
            int min, int max)
        {
            Content = content;
            Func = func;
            Min = min;
            Max = max;
        }
    }


    /// <summary>
    /// Reprensents a grammar rule that wraps the specified parsing rule and converts its result.
    /// </summary>
    /// <typeparam name="T">The type of the result of the wrapped parsing rule.</typeparam>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class MappingParsingRule<T, TResult> : ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            T r;
            if (Content.TryParse(lexer, out r))
            {
                result = Func(r);
                return true;
            }
            else
            {             
                result = default(TResult);
                return false;
            }
        }

        /// <summary>
        /// Gets the converter that maps the result from the inner parsing rule to the final parsing result.
        /// </summary>
        /// <value>The converter that maps the result from the inner parsing rule to the final parsing result.</value>
        public Func<T, TResult> Func { get; private set; }

        /// <summary>
        /// Gets the inner parsing rule.
        /// </summary>
        /// <value>The inner parsing rule.</value>
        public ParsingRule<T> Content { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();
            if (Content != null)
                return "[" + Content.ToString() + "]";
            return base.ToString();
        }

        internal MappingParsingRule(
            ParsingRule<T> content,
            Func<T, TResult> func)
        {
            Content = content;
            Func = func;          
        }
    }

    /// <summary>
    /// Represents a grammar rule that uses the custom parsing rule and result converter.
    /// </summary>
    /// <typeparam name="TResult">The type of the parsing result.</typeparam>
    public class CustomParsingRule<TResult> : ParsingRule<TResult>
    {
        /// <summary>
        /// Returns a value indicating whether the input <see cref="Lexical.Token"/> matches the grammar rule.
        /// </summary>
        /// <param name="lexer">The <see cref="BranchedLexer"/> that provides the input <see cref="Lexical.Token"/>.</param>
        /// <param name="result">When this method returns, contains the parsing result if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise, is <c>default(TResult)</c></param>
        /// <returns><see langword="true"/> if the input <see cref="Lexical.Token"/> matches the grammar rule; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lexer"/> is <see langword="null"/>.</exception>
        public override bool TryParse(BranchedLexer lexer, out TResult result)
        {
            if (lexer == null)
                throw new ArgumentNullException();
            var r = Func(lexer);
            result = r.Item2;
            return r.Item1;
        }

        /// <summary>
        /// Gets the delegate that the parsing rule uses.
        /// </summary>
        /// <value>The delegate that represents the parsing rule. It consumes tokens from the lexer, and returns a <see cref="Tuple{Boolean, TResult}"/> with the first item is a <see cref="Boolean"/> that indicating whether the match, and the second item that is the parsing result.</value>
        public Func<BranchedLexer, Tuple<bool, TResult>> Func { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Id != null)
                return Id.ToString();            
            return "Custom";
        }

        internal CustomParsingRule(
            Func<BranchedLexer, Tuple<bool, TResult>> func)
        {
            Func = func;
        }
    }

}
