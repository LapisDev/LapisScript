/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : NativeFunction
 * Description : Represents a native function object.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Runtime.ScriptObjects
{
    /// <summary>
    /// Represents a native function object.
    /// </summary>
    public sealed partial class NativeFunction : IFunctionObject
    {
        /// <summary>
        /// Gets the delegate that the object wraps.
        /// </summary>
        /// <value>The delegate that the object wraps.</value>
        public Func<List<IScriptObject>, IScriptObject> Func { get; private set; }

        /// <summary>
        /// Invokes the function with the specified parameters and returns the value.
        /// </summary>
        /// <param name="args">The parameters passed to the function.</param>
        /// <returns>The return value of the function.</returns>
        IScriptObject IFunctionObject.Invoke(List<IScriptObject> args)
        {
            return Func(args);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="NativeFunction"/> class using the specified parameter.
        /// </summary>
        /// <param name="func">The delegate that the object wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public NativeFunction(Func<List<IScriptObject>, IScriptObject> func)
        {
            if (func == null)
                throw new ArgumentNullException();
            Func = func;
        }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return "native function";
        }

        /// <summary>
        /// Specifies whether this script object is equal to another <see cref="IScriptObject"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IScriptObject"/>.</param>
        /// <returns><see langword="true"/> if this script object is equal to the other <see cref="IScriptObject"/>; otherwise, <see langword="false"/>.</returns>
        bool IEquatable<IScriptObject>.Equals(IScriptObject other)
        {
            return base.Equals(other);
        }
    }

    public partial class NativeFunction
    {
        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 0)
                    return func();
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 1)
                    return func(args[0]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 2)
                    return func(args[0], args[1]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 3)
                    return func(args[0], args[1], args[2]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 4)
                    return func(args[0], args[1], args[2], args[3]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 5)
                    return func(args[0], args[1], args[2], args[3], args[4]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 6)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 7)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 8)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 9)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                else
                    return Throw();
            });
        }
   
        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 10)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 11)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                else
                    return Throw();
            });
        }
   
        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 12)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                else
                    return Throw();
            });
        }
 
        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 13)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 14)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 15)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                else
                    return Throw();
            });
        }
 
        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeFunction(Func<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 16)
                    return func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                else
                    return Throw();
            });
        }

   
        /// <summary>
        /// Defines an implicit conversion of <see cref="Action"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 0)
                {
                    action();
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 1)
                {
                    action(args[0]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 2)
                {
                    action(args[0], args[1]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 3)
                {
                    action(args[0], args[1], args[2]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 4)
                {
                    action(args[0], args[1], args[2], args[3]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 5)
                {
                    action(args[0], args[1], args[2], args[3], args[4]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 6)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 7)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 8)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 9)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 10)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 11)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 12)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 13)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 14)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 15)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeFunction"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be converted.</param>
        /// <returns>The <see cref="NativeFunction"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeFunction(Action<IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeFunction(args =>
            {
                if (args.Count == 16)
                {
                    action(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }
		
        private static IScriptObject Throw()
        {
            throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
        }
    }
}
