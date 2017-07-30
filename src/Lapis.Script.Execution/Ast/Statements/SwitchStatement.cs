/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : SwitchStatement
 * Description : Represents a switch statement in the syntax tree.
 * Created     : 2015/6/20
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Execution.Ast.Expressions;

namespace Lapis.Script.Execution.Ast.Statements
{
    /// <summary>
    /// Represents a <c>switch</c> statement in the syntax tree.
    /// </summary>
    public class SwitchStatement : Statement
    {
        /// <summary>
        /// Gets the expression to be evaluated.
        /// </summary>
        /// <value>The expression to be evaluated.</value>
        public Expression Expression { get; private set; }

        /// <summary>
        /// Gets the <c>case</c> clauses.
        /// </summary>
        /// <value>The <c>case</c> clauses.</value>
        public SwitchCaseCollection Cases { get; private set; }

        /// <summary>
        /// Gets the <c>default</c> clause.
        /// </summary>
        /// <value>The <c>default</c> clause.</value>        
        public StatementCollection Default { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ISwitchStatement"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the statement in the script code.</param>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <param name="cases">The <c>case</c> clauses.</param>
        /// <param name="default">The <c>default</c> clause.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="expression"/> is <see langword="null"/>.</exception>
        public SwitchStatement(
            LinePragma linePragma,
            Expression expression,
            SwitchCaseCollection cases,
            StatementCollection @default)
            : base(linePragma)
        {
            if (expression == null)
                throw new ArgumentNullException();
            Expression = expression;
            Cases = cases;
            Default  = @default;
        }

        /// <summary>
        /// Returns the string representation of the statement.
        /// </summary>
        /// <returns>Returns the string representation of the statement.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public override string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            var sb = new StringBuilder();
            sb.Append(ind).Append("switch (").Append(Expression.ToString()).Append(")\n").Append(ind).Append("{\n");
            if (Cases != null)
                sb.Append(Cases.ToString(indentation + 4)).Append("\n");
            if (Default != null)
                sb.Append(new string(' ', indentation + 4)).Append("default:\n")
                    .Append(Default.ToString(indentation + 8));
            sb.Append(ind).Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// Represents a <c>case</c> clause in the <c>switch</c> statement.
    /// </summary>
    public class SwitchCase
    {
        /// <summary>
        /// Gets the expression to be matched.
        /// </summary>
        /// <value>The expression to be matched.</value>
        public Expression Expression { get; private set; }

        /// <summary>
        /// Gets the statements to be executed when the <see cref="SwitchCase.Expression"/> is matched.
        /// </summary>
        /// <value>The statements to be executed when the <see cref="SwitchCase.Expression"/> is matched.</value>
        public StatementCollection Statements { get; private set; }

        /// <summary>
        /// Gets the position of the <c>case</c> clause in the script code.
        /// </summary>
        /// <value>The position of the <c>case</c> clause in the script code.</value>
        public LinePragma LinePragma { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="SwitchCase"/> class using the specified parameters.
        /// </summary>
        /// <param name="linePragma">The position of the <c>case</c> clause in the script code.</param>
        /// <param name="expression">The expression to be matched.</param>
        /// <param name="statements">The statements to be executed when the <paramref name="expression"/> is matched.</param>
        /// <exception cref="ArgumentNullException"><paramref name="linePragma"/> or <pararef name="expression"/> is <see langword="null"/>.</exception>
        public SwitchCase(
            LinePragma linePragma,
            Expression expression,
            StatementCollection statements)
        {
            if (expression == null)
                throw new ArgumentNullException();
            Expression = expression;
            Statements = statements;
        }

        /// <summary>
        /// Returns the string representation of the <c>case</c> clauset.
        /// </summary>
        /// <returns>Returns the string representation of the <c>case</c> clause.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();
            var ind = new string(' ', indentation);
            if (Statements != null)
                return ind + "case " + Expression.ToString() + ":\n"
                    + Statements.ToString(indentation + 4);
            else
                return ind + "case " + Expression.ToString() + ":";
        }

        /// <summary>
        /// Returns the string representation of the <c>case</c> clauset.
        /// </summary>
        /// <returns>Returns the string representation of the <c>case</c> clause.</returns>
		public override string ToString()
        {
            return ToString(0);
        }
    }

    /// <summary>
    /// Represents a collection of <c>case</c> clauses in the <c>switch</c> statement.
    /// </summary>
    public class SwitchCaseCollection: IEnumerable<SwitchCase>
    {
        /// <summary>
        /// Gets the <see cref="SwitchCase"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="SwitchCase"/> to get.</param>
        /// <returns>The <see cref="SwitchCase"/> at the specified index. </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>, or is equal to or greater than <see cref="Count"/>.</exception>
        public SwitchCase this[int index]
        {
            get { return _switchCases[index]; }
        }
        
        /// <summary>
        /// Gets the number of elements contained in the <see cref="SwitchCaseCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="SwitchCaseCollection"/>.</value>
        public int Count { get { return _switchCases.Count; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchCaseCollection"/> class that contains elements copied from the specified <see cref="IEnumerable{SwitchCase}"/>.
        /// </summary>
        /// <param name="switchCases">The collection whose elements are copied to the new <see cref="SwitchCaseCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public SwitchCaseCollection(IEnumerable<SwitchCase> switchCases)
        {
            if (switchCases == null || switchCases.Contains(null))
                throw new ArgumentNullException();            
            _switchCases = switchCases.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchCaseCollection"/> class that contains elements copied from the specified <see cref="SwitchCase"/> array.
        /// </summary>
        /// <param name="switchCases">The array whose elements are copied to the new <see cref="SwitchCaseCollection"/>.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>, or contains <see langword="null"/> elements.</exception>
        public SwitchCaseCollection(params SwitchCase[] switchCases)
            : this((IEnumerable<SwitchCase>)switchCases) { }
     
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SSwitchCaseCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="SwitchCaseCollection"/>.</returns>
        public IEnumerator<SwitchCase> GetEnumerator()
        {
            return _switchCases.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SSwitchCaseCollection"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="SwitchCaseCollection"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _switchCases.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of the <c>case</c> clauses.
        /// </summary>
        /// <returns>Returns the string representation of the <c>case</c> clauses.</returns>
		/// <param name="indentation">The number of indented characters.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indentation"/> is negative.</exception>
        public string ToString(int indentation)
        {
            if (indentation < 0)
                throw new ArgumentOutOfRangeException();  
            var sb = new StringBuilder();
            foreach (var c in _switchCases)
            {
                sb.Append(c.ToString(indentation)).Append("\n");
            }
            if (sb.Length > 1)
                sb.Length -= 1;
            return sb.ToString();           
        }

        /// <summary>
        /// Returns the string representation of the <c>case</c> clauses.
        /// </summary>
        /// <returns>Returns the string representation of the <c>case</c> clauses.</returns>
		public override string ToString()
        {
            return ToString(0);
        }

        internal SwitchCaseCollection(List<SwitchCase> switchCases)
        {
            _switchCases = switchCases;
        }

        private List<SwitchCase> _switchCases;
    }
}
