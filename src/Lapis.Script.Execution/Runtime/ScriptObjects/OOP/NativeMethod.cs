/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : NativeMethod
 * Description : Represents a native method.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    /// <summary>
    /// Represents a native method.
    /// </summary>
    public partial class NativeMethod : ClassMethod
    {
        /// <summary>
        /// Gets the delegate that the object wraps.
        /// </summary>
        /// <value>The delegate that the object wraps.</value>
        public Func<InstanceObject, List<IScriptObject>, IScriptObject> Func { get; private set; }

        /// <summary>
        /// Invokes the method on the specified target with the specified parameters and returns the value.
        /// </summary>
        /// <param name="target">The target on which the method is invoked. It should be <see langword="null"/> if the method is a static method.</param>
        /// <param name="args">The parameters passed to the method.</param>
        /// <exception cref="ArgumentException"><paramref name="args"/> is invalid.</exception>
        /// <exception cref="NotSupportedException"><paramref name="target"/> is neither <see cref="InstanceObject"/> nor <see langword="null"/>。</exception>
        public override IScriptObject Invoke(IScriptObject target, List<IScriptObject> args)
        {
            if (target == null)
                return Func(null, args);
            var ins = target as InstanceObject;
            if (ins != null)
            {
                return Func(ins, args);
            }
            throw new NotSupportedException();
        }
  
        /// <summary>
        /// Initialize a new instance of the <see cref="NativeMethod"/> class using the specified parameter.
        /// </summary>  
        /// <param name="func">The delegate that the object wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public NativeMethod(Func<InstanceObject, List<IScriptObject>, IScriptObject> func)
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

    }

    /// <summary>
    /// Represents a native method that is targeted at a wrapped native type.
    /// </summary>
    /// <typeparam name="T">The wrapped native type.</typeparam>
    public partial class NativeMethod<T> : NativeMethod
    {
        /// <summary>
        /// Gets the delegate that the object wraps.
        /// </summary>
        /// <value>The delegate that the object wraps.</value>
        public new Func<T, List<IScriptObject>, IScriptObject> Func { get; private set; }

        /// <summary>
        /// Invokes the method on the specified target with the specified parameters and returns the value.
        /// </summary>
        /// <param name="target">The target on which the method is invoked. It should be <see langword="null"/> if the method is a static method.</param>
        /// <param name="args">The parameters passed to the method.</param>
        /// <exception cref="ArgumentException"><paramref name="args"/> is invalid.</exception>
        /// <exception cref="NotSupportedException"><paramref name="target"/> is not <see cref="NativeInstance"/>, or <see cref="NativeInstance.Value"/> is not <typeparamref name="T"/>.</exception>
        public override IScriptObject Invoke(IScriptObject target, List<IScriptObject> args)
        {
            if (target == null)
                return Func(default(T), args);
            var native = target as NativeInstance;
            if (native != null)
            {
                if (native.Value is T)
                {
                    return Func((T)native.Value, args);
                }
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="NativeMethod{T}"/> class using the specified parameter.
        /// </summary>  
        /// <param name="func">The delegate that the object wraps.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public NativeMethod(Func<T, List<IScriptObject>, IScriptObject> func)
            : base(CheckNullAndPack(func))
        {
            Func = func;
        }

        private static Func<InstanceObject, List<IScriptObject>, IScriptObject> CheckNullAndPack(Func<T, List<IScriptObject>, IScriptObject> func)
        {
            if (func == null)
                throw new ArgumentNullException();
            return (target, args) =>
             {
                 var native = target as NativeInstance;
                 if (native != null)
                 {
                     if (native.Value is T)
                     {
                         return func((T)native.Value, args);
                     }
                 }
                 throw new NotSupportedException();
             };
        }
    }

    public partial class NativeMethod<T>
    {
        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 0)
                    return func(t);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 1)
                    return func(t, args[0]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 2)
                    return func(t, args[0], args[1]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 3)
                    return func(t, args[0], args[1], args[2]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 4)
                    return func(t, args[0], args[1], args[2], args[3]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 5)
                    return func(t, args[0], args[1], args[2], args[3], args[4]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 6)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 7)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 8)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 9)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 10)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 11)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 12)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 13)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 14)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="func">The <see cref="System.Func{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="func"/>.</returns>
        public static implicit operator NativeMethod<T>(Func<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> func)
        {
            if (func == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 15)
                    return func(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                else
                    return Throw();
            });
        }


        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 0)
                {
                    action(t);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 1)
                {
                    action(t, args[0]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 2)
                {
                    action(t, args[0], args[1]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 3)
                {
                    action(t, args[0], args[1], args[2]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 4)
                {
                    action(t, args[0], args[1], args[2], args[3]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 5)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 6)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 7)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 8)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 9)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 10)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 11)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 12)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 13)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 14)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                    return ScriptNull.Instance;
                }
                else
                    return Throw();
            });
        }

        /// <summary>
        /// Defines an implicit conversion of <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to <see cref="NativeMethod{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject}"/> to be wrapped.</param>
        /// <returns>The <see cref="NativeMethod{T}"/> that wraps <paramref name="action"/>.</returns>
        public static implicit operator NativeMethod<T>(Action<T, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject, IScriptObject> action)
        {
            if (action == null)
                return null;
            return new NativeMethod<T>((t, args) =>
            {
                if (args.Count == 15)
                {
                    action(t, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
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
