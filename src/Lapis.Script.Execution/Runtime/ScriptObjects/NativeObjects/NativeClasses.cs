/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ObjectClass
 * Description : Represents the root class of all the script classes.
 * Created     : 2015/8/2
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Runtime.ScriptObjects.OOP;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects
{
    /// <summary>
    /// Represents the root class object of all the script classes.
    /// </summary>
    public class ObjectClass : ScriptClass
    {
        /// <summary>
        /// Represents the root class object of all the script classes.
        /// </summary>
        /// <value>The root class object of all the script classes.</value>
        public static readonly ObjectClass Instance = new ObjectClass();

        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            if (args.Count == 0)
                return new ExpandoObject();
            throw new ArgumentException(string.Format(ExceptionResource.ConstructorParemetersNotMatch, "Object"));
        }

        private ObjectClass()
            : base("Object", null, null)
        {
            Initialize();
        }

        private void Initialize()
        {
            InstanceMembers.Add("equals",
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 1)
                        return new BooleanObject(t.Equals(args[0]));
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("toString",
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 0)
                        return new StringObject(string.Format("instance of {0}", t.Class.Name));
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
        }
    }

    /// <summary>
    /// Represents the class object of the array object.
    /// </summary>
    /// <seealso cref="ArrayObject"/>
    public class ArrayClass : NativeClass<List<IScriptObject>>
    {
        /// <summary>
        /// Represents the class object of the array object.
        /// </summary>
        /// <value>The class object of the array object.</value>
        public static readonly ArrayClass Instance = new ArrayClass();

        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            var ins = new ArrayObject();
            if (Constructor != null)
                Constructor.Invoke(ins, args);
            return ins;
        }

        private ArrayClass()
            : base("Array", ObjectClass.Instance,
            new NativeConstructor((t, args) =>
            {
                if (args.Count == 1)
                {
                    var num = args[0] as INumberObject;
                    if (num != null)
                    {
                        var dbl = num.Value;
                        if (dbl >= 0 && dbl <= int.MaxValue)
                        {
                            var i = (int)dbl;
                            if (dbl == i)
                            {
                                t.Value = new List<IScriptObject>(i);
                                return;
                            }
                        }
                        throw new ArgumentOutOfRangeException(ExceptionResource.ArrayLengthMustBePosInteger, innerException: null);
                    }
                }
                t.Value = args;
            }))
        {
            Initialize();
        }

        private void Initialize()
        {          
            InstanceMembers.Add("toString",
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 0)
                        return new StringObject(string.Format("Array [{0}]", t.Count));
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add(ClassHelper.RenamePropertyGetter("length"), new PropertyGetter(
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 0)
                        return new NumberObject(t.Count);
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            InstanceMembers.Add(ClassHelper.RenameIndexerGetter, new IndexerGetter(
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var idx = args[0] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                    return t[i];
                            }
                        }
                    }
                    throw new ArgumentOutOfRangeException(ExceptionResource.ArrayIndexMustBePosInteger, innerException: null);
                })));
            InstanceMembers.Add(ClassHelper.RenameIndexerSetter, new IndexerSetter(
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 2)
                    {
                        var idx = args[1] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                {
                                    t[i] = args[0];
                                    return ScriptNull.Instance;
                                }
                            }
                        }
                    }
                    throw new ArgumentOutOfRangeException(ExceptionResource.ArrayIndexMustBePosInteger, innerException: null);
                })));
            InstanceMembers.Add("add",
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    foreach (var item in args)
                    {
                        t.Add(item);
                    }
                    return ScriptNull.Instance;
                }));
            InstanceMembers.Add("contains",
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        return new BooleanObject(t.Contains(args[0]));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("clone",
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new ArrayObject(new List<IScriptObject>(t));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("insert",
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 2)
                    {
                        var idx = args[0] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                {
                                    t.Insert(i, args[1]);
                                    return ScriptNull.Instance;
                                }
                            }
                        }
                        throw new ArgumentOutOfRangeException(ExceptionResource.ArrayIndexMustBePosInteger, innerException: null);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("indexOf",
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        return new NumberObject(t.IndexOf(args[0]));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("remove",
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        t.Remove(args[0]);
                        return ScriptNull.Instance;
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
        }

    }

    /// <summary>
    /// Represents the class object of the string object.
    /// </summary>
    /// <seealso cref="StringObject"/>
    public class StringClass : NativeClass<string>
    {
        /// <summary>
        /// Represents the class object of the string object.
        /// </summary>
        /// <value>The class object of the string object.</value>
        public static readonly StringClass Instance = new StringClass();

        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>        
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            var ins = new StringObject();
            if (Constructor != null)
                Constructor.Invoke(ins, args);
            return ins;
        }

        private StringClass()
            : base("String", ObjectClass.Instance,
            new NativeConstructor((t, args) =>
            {
                var sb = new StringBuilder();
                foreach (var arg in args)
                    sb.Append(arg.ToString());
                t.Value = sb.ToString();
            }))
        {
            Initialize();
        }

        private void Initialize()
        {           
            InstanceMembers.Add("toString",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                        return new StringObject(t.ToString());
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("equals",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var num = args[0] as IStringObject;
                        return new BooleanObject(num != null && t == num.Value);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add(ClassHelper.RenamePropertyGetter("length"), new PropertyGetter(
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                        return new NumberObject(t.Length);
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            InstanceMembers.Add(ClassHelper.RenameIndexerGetter, new IndexerGetter(
                new NativeMethod<List<IScriptObject>>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var idx = args[0] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                    return t[i];
                            }
                        }
                    }
                    throw new ArgumentOutOfRangeException(ExceptionResource.ArrayIndexMustBePosInteger, innerException: null);
                })));
            InstanceMembers.Add("contains",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        return new BooleanObject(t.Contains(args[0].ToString()));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("endsWith",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        return new BooleanObject(t.EndsWith(args[0].ToString()));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("indexOf",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var s = args[0] as IStringObject;
                        if (s != null)
                            return new NumberObject(t.IndexOf(s.Value));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("insert",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 2)
                    {
                        var idx = args[0] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                {
                                    return new StringObject(
                                        t.Insert(i, args[1].ToString()));
                                }
                            }
                        }
                        throw new ArgumentException();
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("replace",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 2)
                    {
                        return new StringObject(
                            t.Replace(args[0].ToString(), args[1].ToString()));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("split",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        return new ArrayObject(
                            t.Split(args[0].ToString().ToCharArray())
                            .Cast<StringObject>());
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("startsWith",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        return new BooleanObject(t.StartsWith(args[0].ToString()));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("substring",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var idx = args[0] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                {
                                    return new StringObject(
                                        t.Substring(i));
                                }
                            }
                        }
                        throw new ArgumentException();
                    }
                    if (args.Count == 2)
                    {
                        var idx0 = args[0] as INumberObject;
                        var idx1 = args[1] as INumberObject;
                        if (idx0 != null && idx1 != null)
                        {
                            var dbl0 = idx0.Value;
                            var dbl1 = idx1.Value;
                            if (dbl0 >= 0 && dbl0 <= int.MaxValue &&
                                dbl1 >= 0 && dbl1 <= int.MaxValue)
                            {
                                var i0 = (int)dbl0;
                                var i1 = (int)dbl1;
                                if (dbl0 == i0 && dbl1 == i1)
                                {
                                    return new StringObject(
                                        t.Substring(i0, i1));
                                }
                            }
                        }
                        throw new ArgumentException();
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("toCharArray",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new ArrayObject(
                            t.ToCharArray()
                            .Cast<StringObject>());
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("toLower",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new StringObject(
                            t.ToLower());
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("toUpper",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new StringObject(
                            t.ToUpper());
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("trim",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new StringObject(
                            t.Trim());
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("trimStart",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new StringObject(
                            t.TrimStart());
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("trimEnd",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new StringObject(
                            t.TrimEnd());
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("padLeft",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var idx = args[0] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                {
                                    return new StringObject(
                                        t.PadLeft(i));
                                }
                            }
                        }
                        throw new ArgumentException();
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("padRight",
                new NativeMethod<string>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var idx = args[0] as INumberObject;
                        if (idx != null)
                        {
                            var dbl = idx.Value;
                            if (dbl >= 0 && dbl <= int.MaxValue)
                            {
                                var i = (int)dbl;
                                if (dbl == i)
                                {
                                    return new StringObject(
                                        t.PadRight(i));
                                }
                            }
                        }
                        throw new ArgumentException();
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));

            StaticMembers.Add(ClassHelper.RenamePropertyGetter("empty"), new PropertyGetter(
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new StringObject(string.Empty);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            StaticMembers.Add("compare",
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 2)
                    {
                        return new NumberObject(
                            string.Compare(args[0].ToString(), args[1].ToString()));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            StaticMembers.Add("concat",
                new NativeMethod((t, args) =>
                {
                    var sb = new StringBuilder();
                    foreach (var arg in args)
                        sb.Append(arg.ToString());
                    return new StringObject(sb.ToString());
                }));
            StaticMembers.Add("format",
                new NativeMethod((t, args) =>
                {
                    if (args.Count >= 1)
                    {
                        var s = args[0];
                        args.RemoveAt(0);
                        var array = args.ToArray();
                        return new StringObject(string.Format(s.ToString(), array));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
        }
    }

    /// <summary>
    /// Represents the class object of the number object.
    /// </summary>
    /// <seealso cref="NumberObject"/>
    public class NumberClass : NativeClass<double>
    {
        /// <summary>
        /// Represents the class object of the number object.
        /// </summary>
        /// <value>The class object of the number object.</value>
        public static readonly NumberClass Instance = new NumberClass();

        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>        
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            var ins = new NumberObject();
            if (Constructor != null)
                Constructor.Invoke(ins, args);
            return ins;
        }

        private NumberClass()
            : base("Number",ObjectClass.Instance,
            new NativeConstructor((t, args) =>
            {
                if (args.Count == 0)
                {
                    return;
                }
                throw new ArgumentException(string.Format(ExceptionResource.ConstructorParemetersNotMatch, "Number"));
            }))
        {
            Initialize();
        }
        private void Initialize()
        {           
            InstanceMembers.Add("toString",
                new NativeMethod<double>((t, args) =>
                {
                    if (args.Count == 0)
                        return new StringObject(t.ToString());
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("equals",
                new NativeMethod<double>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var num = args[0] as INumberObject;
                        return new BooleanObject(num != null && t == num.Value);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add(ClassHelper.RenamePropertyGetter("isInfinity"), new PropertyGetter(
                new NativeMethod<double>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new BooleanObject(double.IsInfinity(t));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            InstanceMembers.Add(ClassHelper.RenamePropertyGetter("isNaN"), new PropertyGetter(
                new NativeMethod<double>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new BooleanObject(double.IsNaN(t));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            InstanceMembers.Add(ClassHelper.RenamePropertyGetter("isPosInf"), new PropertyGetter(
                new NativeMethod<double>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new BooleanObject(double.IsPositiveInfinity(t));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            InstanceMembers.Add(ClassHelper.RenamePropertyGetter("isNegInf"), new PropertyGetter(
                new NativeMethod<double>((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new BooleanObject(double.IsNegativeInfinity(t));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            StaticMembers.Add(ClassHelper.RenamePropertyGetter("PosInf"), new PropertyGetter(
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new NumberObject(double.PositiveInfinity);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            StaticMembers.Add(ClassHelper.RenamePropertyGetter("NegInf"), new PropertyGetter(
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new NumberObject(double.NegativeInfinity);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            StaticMembers.Add(ClassHelper.RenamePropertyGetter("NaN"), new PropertyGetter(
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new NumberObject(double.NaN);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            StaticMembers.Add(ClassHelper.RenamePropertyGetter("MaxValue"), new PropertyGetter(
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new NumberObject(double.MaxValue);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            StaticMembers.Add(ClassHelper.RenamePropertyGetter("MinValue"), new PropertyGetter(
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 0)
                    {
                        return new NumberObject(double.MinValue);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                })));
            StaticMembers.Add("parse",
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var str = args[0] as IStringObject;
                        if (str != null)
                            return new NumberObject(double.Parse(str.Value));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));          
        }
    }

    /// <summary>
    /// Represents the class object of the Boolean object.
    /// </summary>
    /// <seealso cref="BooleanObject"/>
    public class BooleanClass : NativeClass<bool>
    {
        /// <summary>
        /// Represents the class object of the Boolean object.
        /// </summary>
        /// <value>The class object of the Boolean object.</value>
        public static readonly BooleanClass Instance = new BooleanClass();

        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>        
        public override IScriptObject Construct(List<IScriptObject> args)
        {
            var ins = new BooleanObject();
            if (Constructor != null)
                Constructor.Invoke(ins, args);
            return ins;
        }

        private BooleanClass()
            : base("Boolean", ObjectClass.Instance,
            new NativeConstructor((ins, args) =>
            {
                if (args.Count == 0)
                {
                    ins.Value = new bool();
                    return;
                }
                if (args.Count == 1)
                {
                    var boo = args[0] as IBooleanObject;
                    if (boo != null)
                    {
                        ins.Value = boo.ToBoolean();
                        return;
                    }
                }
                throw new ArgumentException(string.Format(ExceptionResource.ConstructorParemetersNotMatch, "Boolean"));
            }))
        {
            Initialize();
        }

        private void Initialize()
        {
            InstanceMembers.Add("toString",
                new NativeMethod<bool>((t, args) =>
                {
                    if (args.Count == 0)
                        return new StringObject(t ? "true" : "false");
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            InstanceMembers.Add("equals",
                new NativeMethod<bool>((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var boo = args[0] as BooleanObject;
                        return new BooleanObject(boo != null && t == boo.Value);
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
            StaticMembers.Add("parse",
                new NativeMethod((t, args) =>
                {
                    if (args.Count == 1)
                    {
                        var str = args[0] as IStringObject;
                        if (str != null)
                            return new BooleanObject(bool.Parse(str.Value));
                    }
                    throw new ArgumentException(ExceptionResource.ParemetersNotMatch);
                }));
        }
    }
}
