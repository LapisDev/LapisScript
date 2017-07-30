/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : OperatorDictionary
 * Description : Represents a dictionary containing the operators used by the runtime context.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast.Expressions;
using Lapis.Script.Execution.Runtime.ScriptObjects;
using Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Runtime
{
    /// <summary>
    /// Represents a dictionary containing the operators used by the runtime context.
    /// </summary>
    public sealed partial class OperatorDictionary : IOperatorCalculator
    {
        /// <summary>
        /// Gets the dictionary containing the binary operators.
        /// </summary>
        /// <value>The dictionary containing the binary operators.</value>
        public Dictionary<string, Func<RuntimeContext, Expression, Expression, IScriptObject>> BinaryOperators { get; private set; }

        /// <summary>
        /// Gets the dictionary containing the prefix unary operators.
        /// </summary>
        /// <value>The dictionary containing the prefix unary operators.</value>
        public Dictionary<string, Func<RuntimeContext, Expression, IScriptObject>> PrefixOperators { get; private set; }

        /// <summary>
        /// Gets the dictionary containing the postfix unary operators.
        /// </summary>
        /// <value>The dictionary containing the postfix unary operators.</value>
        public Dictionary<string, Func<RuntimeContext, Expression, IScriptObject>> PostfixOperators { get; private set; }

        /// <summary>
        /// Gets the dictionary containing the ternary operators. The key for a ternary operator is the symbols of the two operators joined by a white space (<c>' '</c>).
        /// </summary>
        /// <value>The dictionary containing the ternary operators.</value>
        public Dictionary<string, Func<RuntimeContext, Expression, Expression, Expression, IScriptObject>> TernaryOperators { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="OperatorDictionary"/> class.
        /// </summary>
        public OperatorDictionary()
        {
            BinaryOperators = new Dictionary<string, Func<RuntimeContext, Expression, Expression, IScriptObject>>();
            PrefixOperators = new Dictionary<string, Func<RuntimeContext, Expression, IScriptObject>>();
            PostfixOperators = new Dictionary<string, Func<RuntimeContext, Expression, IScriptObject>>();
            TernaryOperators = new Dictionary<string, Func<RuntimeContext, Expression, Expression, Expression, IScriptObject>>();
        }

        /// <summary>
        /// Evaluates the specified binary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>The result of the expression.</returns>
        /// <exception cref="ArithmeticException">No operator is matched.</exception>
        IScriptObject IOperatorCalculator.CalculateBinaryOperator(RuntimeContext context, string oper, Expression left, Expression right)
        {
            Func<RuntimeContext, Expression, Expression, IScriptObject> bin;
            if (BinaryOperators.TryGetValue(oper, out bin))
                return bin(context, left, right);
            throw new ArithmeticException(
                string.Format(ExceptionResource.OperatorNotSupported, oper));
        }

        /// <summary>
        /// Evaluates the specified prefix unary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>The result of the expression.</returns>
        /// <exception cref="ArithmeticException">No operator is matched.</exception>
        IScriptObject IOperatorCalculator.CalculatePrefixOperators(RuntimeContext context, string oper, Expression right)
        {
            Func<RuntimeContext, Expression, IScriptObject> pre;
            if (PrefixOperators.TryGetValue(oper, out pre))
                return pre(context, right);
            throw new ArithmeticException(
                string.Format(ExceptionResource.OperatorNotSupported, oper));
        }

        /// <summary>
        /// Evaluates the specified postfix unary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="left">The left expression.</param>
        /// <returns>The result of the expression.</returns>
        /// <exception cref="ArithmeticException">No operator is matched.</exception>
        IScriptObject IOperatorCalculator.CalculatePostfixOperators(RuntimeContext context, string oper, Expression left)
        {
            Func<RuntimeContext, Expression, IScriptObject> pos;
            if (PostfixOperators.TryGetValue(oper, out pos))
                return pos(context, left);
            throw new ArithmeticException(
                string.Format(ExceptionResource.OperatorNotSupported, oper));
        }

        /// <summary>
        /// Evaluates the specified binary operator expression and returns the result.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="oper1">The first operator.</param>
        /// <param name="oper2">The second operator.</param>
        /// <param name="operand1">The first expression.</param>
        /// <param name="operand2">The second expression.</param>
        /// <param name="operand3">The third expression.</param>
        /// <returns>The result of the expression.</returns>
        /// <exception cref="ArithmeticException">No operator is matched.</exception>
        IScriptObject IOperatorCalculator.CalculateTernaryOperators(RuntimeContext context, string oper1, string oper2, Expression operand1, Expression operand2, Expression operand3)
        {
            var oper = string.Format("{0} {1}", oper1, oper2);
            Func<RuntimeContext, Expression, Expression, Expression, IScriptObject> ter;
            if (TernaryOperators.TryGetValue(oper, out ter))
                return ter(context, operand1, operand2, operand3);
            throw new ArithmeticException(
                string.Format(ExceptionResource.OperatorNotSupported, oper));
        }

        internal OperatorDictionary(OperatorDictionary dictionary)
        {
            BinaryOperators = new Dictionary<string, Func<RuntimeContext, Expression, Expression, IScriptObject>>(dictionary.BinaryOperators);
            PrefixOperators = new Dictionary<string, Func<RuntimeContext, Expression, IScriptObject>>(dictionary.PrefixOperators);
            PostfixOperators = new Dictionary<string, Func<RuntimeContext, Expression, IScriptObject>>(dictionary.PostfixOperators);
            TernaryOperators = new Dictionary<string, Func<RuntimeContext, Expression, Expression, Expression, IScriptObject>>(dictionary.TernaryOperators);
        }
    }

    public partial class OperatorDictionary
    {
        /// <summary>
        /// Gets an <see cref="OperatorDictionary"/> instance that contains the default built-in operators.
        /// </summary>
        /// <value>The default <see cref="OperatorDictionary"/>.</value>
        public static OperatorDictionary Default 
        {
            get { return new OperatorDictionary(_builtInOperators); } 
        }

        private static readonly OperatorDictionary _builtInOperators = InitializeBuiltInOperators();

        private static OperatorDictionary InitializeBuiltInOperators()
        {
            var dict = new OperatorDictionary();

            var bin = dict.BinaryOperators;            
            bin.Add("+", Pack(Addition));
            bin.Add("-", Pack(Subtraction));
            bin.Add("*", Pack(Multiply));
            bin.Add("/", Pack(Division));
            bin.Add("%", Pack(Modulus));
            bin.Add("&", Pack(BitwiseAnd));
            bin.Add("|", Pack(BitwiseOr));
            bin.Add("^", Pack(BitwiseExclusiveOr));
            bin.Add("<", Pack(LessThan));
            bin.Add(">", Pack(GreaterThan));
            bin.Add("<<", Pack(LeftShift));
            bin.Add(">>", Pack(RightShift));
            bin.Add("==", Pack(Equality));
            bin.Add("!=", Pack(Inequality));
            bin.Add("<=", Pack(LessThanOrEqual));
            bin.Add(">=", Pack(GreaterThanOrEqual));
            bin.Add("&&", LogicalAnd);
            bin.Add("||", LogicalOr);
            bin.Add("+=", AdditionAssignment);
            bin.Add("-=", SubtractionAssignment);
            bin.Add("*=", MultiplyAssignment);
            bin.Add("/=", DivisionAssignment);
            bin.Add("%=", ModulusAssignment);
            bin.Add("&=", BitwiseAndAssignment);
            bin.Add("|=", BitwiseOrAssignment);
            bin.Add("^=", BitwiseExclusiveOrAssignment);
            bin.Add("<<=", LeftShiftAssignment);
            bin.Add(">>=", RightShiftAssignment);
            bin.Add("??", NullCoalescing);

            var pre = dict.PrefixOperators;
            pre.Add("+", Pack(UnaryPlus));
            pre.Add("-", Pack(UnaryNegation));
            pre.Add("!", Pack(LogicalNot));
            pre.Add("~", Pack(OnesComplement));
            pre.Add("++", PrefixIncrement);
            pre.Add("--", PrefixDecrement);

            var pos = dict.PostfixOperators;
            pos.Add("++", PostfixIncrement);
            pos.Add("--", PostfixDecrement);

            var ter = dict.TernaryOperators;
            ter.Add("? :", Condition);

            return dict;
        }

        private static Func<RuntimeContext, Expression, Expression, IScriptObject> 
            Pack(Func<IScriptObject, IScriptObject, IScriptObject> binary)
        {
            return (context, left, right) =>
            {
                var x = context.EvaluateExpression(left);
                var y = context.EvaluateExpression(right);
                return binary(x, y);
            };
        }
      
        private static Func<RuntimeContext, Expression, IScriptObject>
           Pack(Func<IScriptObject, IScriptObject> unary)
        {
            return (context, operand) =>
            {
                var x = context.EvaluateExpression(operand);                
                return unary(x);
            };
        }
    
             
        private static IScriptObject UnaryPlus(IScriptObject x)
        {            
            if (x is INumberObject)
                return x;      
            // TODO overload
            return Throw("+");
        }

        private static IScriptObject UnaryNegation(IScriptObject x)
        {
            if (x is INumberObject)
            {
                var num = (INumberObject)x;
                return new NumberObject(-num.Value);
            }
            // TODO overload
            return Throw("-");
        }

        private static IScriptObject LogicalNot(IScriptObject x)
        {
            if (x is IBooleanObject)
            {
                var boo = (IBooleanObject)x;
                return new BooleanObject(!boo.ToBoolean());
            }
            // TODO overload
            return Throw("!");
        }

        private static IScriptObject OnesComplement(IScriptObject x)
        {
            // TODO overload
            return Throw("~");
        }

        private static IScriptObject Addition(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new NumberObject(num1.Value + num2.Value);
            }
            if (x is IStringObject)
            {
                var str1 = (IStringObject)x;
                var str2 = y.ToString();
                return new StringObject(str1 + str2);
            }
            // TODO overload
            return Throw("+");
        }

        private static IScriptObject Subtraction(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new NumberObject(num1.Value - num2.Value);
            }        
            // TODO overload
            return Throw("-");
        }

        private static IScriptObject Multiply(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new NumberObject(num1.Value * num2.Value);
            }
            // TODO overload
            return Throw("*");
        }

        private static IScriptObject Division(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new NumberObject(num1.Value / num2.Value);
            }
            // TODO overload
            return Throw("/");
        }

        private static IScriptObject Modulus(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new NumberObject(num1.Value % num2.Value);
            }
            // TODO overload
            return Throw("%");
        }

        private static IScriptObject BitwiseAnd(IScriptObject x, IScriptObject y)
        {
            if (x is IBooleanObject && y is IBooleanObject)
            {
                var boo1 = (IBooleanObject)x;
                var boo2 = (IBooleanObject)y;
                return new BooleanObject(boo1.ToBoolean() & boo2.ToBoolean());
            }
            // TODO overload
            return Throw("&");
        }

        private static IScriptObject BitwiseOr(IScriptObject x, IScriptObject y)
        {
            if (x is IBooleanObject && y is IBooleanObject)
            {
                var boo1 = (IBooleanObject)x;
                var boo2 = (IBooleanObject)y;
                return new BooleanObject(boo1.ToBoolean() | boo2.ToBoolean());
            }
            // TODO overload
            return Throw("|");
        }

        private static IScriptObject BitwiseExclusiveOr(IScriptObject x, IScriptObject y)
        {
            if (x is IBooleanObject && y is IBooleanObject)
            {
                var boo1 = (IBooleanObject)x;
                var boo2 = (IBooleanObject)y;
                return new BooleanObject(boo1.ToBoolean() ^ boo2.ToBoolean());
            }
            // TODO overload
            return Throw("^");
        }
  
        private static IScriptObject LessThan(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new BooleanObject(num1.Value < num2.Value);
            }
            // TODO overload
            return Throw("<");
        }

        private static IScriptObject GreaterThan(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new BooleanObject(num1.Value > num2.Value);
            }
            // TODO overload
            return Throw(">");
        }

        private static IScriptObject LeftShift(IScriptObject x, IScriptObject y)
        {           
            // TODO overload
            return Throw("<<");
        }

        private static IScriptObject RightShift(IScriptObject x, IScriptObject y)
        {
            // TODO overload
            return Throw(">>");
        }

        private static IScriptObject Equality(IScriptObject x, IScriptObject y)
        {
            var equ = x as IEquatable<IScriptObject>;
            if (equ == null)
                Throw("==");
            return new BooleanObject(equ.Equals(y));            
            // TODO overload           
        }

        private static IScriptObject Inequality(IScriptObject x, IScriptObject y)
        {
            var equ = x as IEquatable<IScriptObject>;
            if (equ == null)
                Throw("!=");
            return new BooleanObject(!equ.Equals(y));
            // TODO overload           
        }

        private static IScriptObject LessThanOrEqual(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new BooleanObject(num1.Value <= num2.Value);
            }
            // TODO overload
            return Throw("<=");
        }

        private static IScriptObject GreaterThanOrEqual(IScriptObject x, IScriptObject y)
        {
            if (x is INumberObject && y is INumberObject)
            {
                var num1 = (INumberObject)x;
                var num2 = (INumberObject)y;
                return new BooleanObject(num1.Value >= num2.Value);
            }
            // TODO overload
            return Throw(">=");
        }

        private static IScriptObject PrefixIncrement(RuntimeContext context, Expression operand)
        {
            var x = context.EvaluateExpression(operand);
            IScriptObject result;
            if (x is INumberObject)
            {
                var num = (INumberObject)x;
                result = new NumberObject(num.Value + 1);
            }
            // TODO overload
            else
                return Throw("++");
            context.Assign(operand, result);
            return result;
        }

        private static IScriptObject PostfixIncrement(RuntimeContext context, Expression operand)
        {
            var x = context.EvaluateExpression(operand);
            IScriptObject result;
            if (x is INumberObject)
            {
                var num = (INumberObject)x;
                result = new NumberObject(num.Value + 1);
            }
            // TODO overload
            else
                return Throw("++");
            context.Assign(operand, result);
            return x;
        }

        private static IScriptObject PrefixDecrement(RuntimeContext context, Expression operand)
        {
            var x = context.EvaluateExpression(operand);
            IScriptObject result;
            if (x is INumberObject)
            {
                var num = (INumberObject)x;
                result = new NumberObject(num.Value - 1);
            }
            // TODO overload
            else
                return Throw("--");
            context.Assign(operand, result);
            return result;
        }

        private static IScriptObject PostfixDecrement(RuntimeContext context, Expression operand)
        {
            var x = context.EvaluateExpression(operand);
            IScriptObject result;
            if (x is INumberObject)
            {
                var num = (INumberObject)x;
                result = new NumberObject(num.Value - 1);
            }
            // TODO overload
            else
                return Throw("--");
            context.Assign(operand, result);
            return x;
        }
 
        private static IScriptObject LogicalAnd(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left) as IBooleanObject;            
            if (x != null)
            {
                if(x.ToBoolean())
                {
                    var y = context.EvaluateExpression(right) as IBooleanObject;
                    if (y != null)
                    {
                        return new BooleanObject(y.ToBoolean());
                    }
                }
                else
                    return new BooleanObject(false);
            }
            return Throw("&&");           
        }

        private static IScriptObject LogicalOr(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left) as IBooleanObject;
            if (x != null)
            {
                if (x.ToBoolean())
                    return new BooleanObject(true);               
                else
                {
                    var y = context.EvaluateExpression(right) as IBooleanObject;
                    if (y != null)
                    {
                        return new BooleanObject(y.ToBoolean());
                    }
                }
            }
            return Throw("||");
        }

        private static IScriptObject AdditionAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = Addition(x, y);            
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject SubtractionAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = Subtraction(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject MultiplyAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = Multiply(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject DivisionAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = Division(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject ModulusAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = Modulus(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject BitwiseAndAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = BitwiseAnd(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject BitwiseOrAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = BitwiseOr(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject BitwiseExclusiveOrAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = BitwiseExclusiveOr(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject LeftShiftAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = LeftShift(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject RightShiftAssignment(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            var y = context.EvaluateExpression(right);
            IScriptObject result = RightShift(x, y);
            context.Assign(left, result);
            return result;
        }

        private static IScriptObject NullCoalescing(RuntimeContext context, Expression left, Expression right)
        {
            var x = context.EvaluateExpression(left);
            if (x is ScriptNull)
                return context.EvaluateExpression(right);
            else
                return x;
        }

        private static IScriptObject Condition(RuntimeContext context, Expression operand1, Expression operand2, Expression operand3)
        {
            var con = context.EvaluateExpression(operand1) as IBooleanObject;
            if (con != null)
            {
                if (con.ToBoolean())
                    return context.EvaluateExpression(operand2);
                else
                    return context.EvaluateExpression(operand3);
            }
            return Throw("? :");
        }

        private static IScriptObject Throw(string oper)
        {
            throw new ArithmeticException(
                string.Format(ExceptionResource.OperatorNotSupported, oper));
        }
    }
}
