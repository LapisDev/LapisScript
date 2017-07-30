/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : TernaryOperatorExpression
 * Description : Represents a ternary operator expression in the syntax tree.
 * Created     : 2015/6/3
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents a ternary operator expression in the syntax tree.
    /// </summary>
    public class TernaryOperatorExpression : Expression
    {
        /// <summary>
        /// Gets the first expression of the operator.
        /// </summary>
        /// <value>The first expression of the operator.</value>
        public Expression Operand1 { get; private set; }

        /// <summary>
        /// Gets the second expression of the operator.
        /// </summary>
        /// <value>The second expression of the operator.</value>
        public Expression Operand2 { get; private set; }

        /// <summary>
        /// Gets the thrid expression of the operator.
        /// </summary>
        /// <value>The third expression of the operator.</value>
        public Expression Operand3 { get; private set; }

        /// <summary>
        /// Gets the first operator.
        /// </summary>
        /// <value>The first operator.</value>
        public string Operator1 { get; private set; }

        /// <summary>
        /// Gets the second operator.
        /// </summary>
        /// <value>The second operator.</value>
        public string Operator2 { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="TernaryOperatorExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="operand1">The first expression of the operator.</param>
        /// <param name="operator1">The first operator.</param>
        /// <param name="operand2">The second expression of the operator.</param>
        /// <param name="operator2">The second operator.</param>
        /// <param name="operand3">The third expression of the operator.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><pararef name="operator1"/> or <pararef name="operator2"/> is empty or white space.</exception>
        public TernaryOperatorExpression(Parser.Lexical.LinePragma linePragma, 
            Expression operand1, string operator1, Expression operand2, string operator2, Expression operand3)
            : base(linePragma)
        {
            if (operand1 == null || operand2 == null || operand3 == null || operator1 == null || operator2 == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(operator1) || string.IsNullOrWhiteSpace(operator2))
                throw new ArgumentException(ExceptionResource.InvalidOperator);
            Operand1 = operand1;
            Operand2 = operand2;
            Operand3 = operand3;
            Operator1 = operator1;
            Operator2 = operator2;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Operand1.IsAtomOrPrimitive())
                sb.Append(Operand1);
            else
                sb.Append("(").Append(Operand1.ToString()).Append(")");
            sb.Append(" ").Append(Operator1).Append(" ");
            if (Operand2.IsAtomOrPrimitive())
                sb.Append(Operand2);
            else
                sb.Append("(").Append(Operand2.ToString()).Append(")");
            sb.Append(" ").Append(Operator2).Append(" ");
            if (Operand3.IsAtomOrPrimitive())
                sb.Append(Operand3);
            else
                sb.Append("(").Append(Operand3.ToString()).Append(")");
            return sb.ToString();
        }
    }
}
