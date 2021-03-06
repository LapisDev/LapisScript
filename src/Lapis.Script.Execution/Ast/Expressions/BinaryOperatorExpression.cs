﻿/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : BinaryOperatorExpression
 * Description : Represents a binary operator expression in the syntax tree.
 * Created     : 2015/4/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Ast.Expressions
{
    /// <summary>
    /// Represents a binary operator expression in the syntax tree.
    /// </summary>
    public class BinaryOperatorExpression : Expression
    {
        /// <summary>
        /// Gets the left expression of the operator.
        /// </summary>
        /// <value>The left expression of the operator.</value>
        public Expression Left { get; private set; }

        /// <summary>
        /// Gets the right expression of the operator.
        /// </summary>
        /// <value>The right expression of the operator.</value>
        public Expression Right { get; private set; }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="BinaryOperatorExpression"/> class using the specified parameters.
        /// </summary>
        /// <param name="left">The left expression of the operator.</param>
        /// <param name="operator">The operator.</param>
        /// <param name="right">The right expression of the operator.</param>
        /// <param name="linePragma">The position of the expression in the script code.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="operator"/> is empty or white space.</exception>
        public BinaryOperatorExpression(Parser.Lexical.LinePragma linePragma, Expression left, string @operator, Expression right)
            : base(linePragma)
        {
            if (left == null || right == null || @operator == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(@operator))
                throw new ArgumentException(ExceptionResource.InvalidOperator);
            Left = left;
            Right = right;
            Operator = @operator;
        }

        /// <summary>
        /// Returns the string representation of the expression.
        /// </summary>
        /// <returns>Returns the string representation of the expression.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Left.IsAtomOrPrimitive())
                sb.Append(Left);
            else
                sb.Append("(").Append(Left.ToString()).Append(")");
            sb.Append(" ").Append(Operator).Append(" ");
            if (Right.IsAtomOrPrimitive())
                sb.Append(Right);
            else
                sb.Append("(").Append(Right.ToString()).Append(")");
            return sb.ToString();
        }
    }
}
