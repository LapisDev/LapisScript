/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ClassObject
 * Description : Represents a class object.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Execution.Ast.Expressions;
using Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    /// <summary>
    /// Represents a class object.
    /// </summary>
    public abstract partial class ClassObject : IClassObject, IMemberObject, IIndexableObject
    {
        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the super class.
        /// </summary>
        /// <value>The super class.</value>
        public ClassObject Super { get; private set; }

        /// <summary>
        /// Gets the instance members of the class.
        /// </summary>
        /// <value>The instance members of the class.</value>
        public Dictionary<string, ClassMember> InstanceMembers { get; private set; }

        /// <summary>
        /// Gets the static fields of the class.
        /// </summary>
        /// <value>The static fields of the class.</value>
        public Dictionary<string, IScriptObject> StaticFields { get; private set; }

        //// <summary>
        /// Gets the static methods of the class.
        /// </summary>
        /// <value>The static methods of the class.</value>
        public Dictionary<string, ClassMember> StaticMembers { get; private set; }

        /// <summary>
        /// Constructs an instance of the class with the specified parameters.
        /// </summary>
        /// <param name="args">The parameters passed to the constructor.</param>
        /// <returns>The created instance of the class.</returns>
        public abstract IScriptObject Construct(List<IScriptObject> args);               

        /// <summary>
        /// Returns the value of the member of the specified name.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <returns>The value of the member <paramref name="name"/>.</returns>
        IScriptObject IMemberObject.GetMember(RuntimeContexts.RuntimeContext context, string name)
        {
            IScriptObject value;
            string rename;
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                rename = ClassHelper.RenamePrivate(name, cls.Name);
                if (StaticFields.TryGetValue(rename, out value))
                    return value;
            }
            {
                rename = name;
                if (StaticFields.TryGetValue(rename, out value))
                    return value;
            }
            if (cls == this || ClassHelper.IsExtendedFrom(this, cls) || ClassHelper.IsExtendedFrom(cls, this))
            {
                rename = ClassHelper.RenameProtected(name);
                if (StaticFields.TryGetValue(rename, out value))
                    return value;
            }
            {
                var member = GetMember(context, name, true);
                var mtd = member as ClassMethod;
                if (mtd != null)
                {
                    return new BoundMethod(null, mtd);
                }
                var proget = member as PropertyGetter;
                if (proget != null)
                {
                    return proget.GetProperty(null);
                }
            }
            throw new InvalidOperationException(
                string.Format(ExceptionResource.MemberNotFound, name));
        }

        internal ClassMember GetMember(RuntimeContexts.RuntimeContext context, string name, bool isStatic)
        {
            var members = isStatic ? StaticMembers : InstanceMembers;
            ClassMember value;
            string rename;
            string propget = ClassHelper.RenamePropertyGetter(name);
            string propset = ClassHelper.RenamePropertySetter(name);
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                rename = ClassHelper.RenamePrivate(name, cls.Name);
                if (members.TryGetValue(rename, out value) && value is ClassMethod)
                    return value;
                rename = ClassHelper.RenamePrivate(propget, cls.Name);
                if (members.TryGetValue(rename, out value) && value is PropertyGetter)
                    return value;
                rename = ClassHelper.RenamePrivate(propset, cls.Name);
                if (members.TryGetValue(rename, out value) && value is PropertySetter)
                    return ClassHelper.ThrowPropertyIsWriteonly(name);
            }
            {
                rename = name;
                if (members.TryGetValue(rename, out value) && value is ClassMethod)
                    return value;
                rename = propget;
                if (members.TryGetValue(rename, out value) && value is PropertyGetter)
                    return value;
                rename = propset;
                if (members.TryGetValue(rename, out value) && value is PropertySetter)
                    return ClassHelper.ThrowPropertyIsWriteonly(name);
            }
            if (cls == this || ClassHelper.IsExtendedFrom(this, cls) || ClassHelper.IsExtendedFrom(cls, this))
            {
                rename = ClassHelper.RenameProtected(name);
                if (members.TryGetValue(rename, out value) && value is ClassMethod)
                    return value;
                rename = ClassHelper.RenameProtected(propget);
                if (members.TryGetValue(rename, out value) && value is PropertyGetter)
                    return value;
                rename = ClassHelper.RenameProtected(propset);
                if (members.TryGetValue(rename, out value) && value is PropertySetter)
                    return ClassHelper.ThrowPropertyIsWriteonly(name);
            }
            if (Super != null)
            {                
                return Super.GetMember(context, name, isStatic);
            }
            return ClassHelper.ThrowMemberNotFound(name);
        }

        /// <summary>
        /// Sets the value of the member of the specified name to the specified value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="value">The value to set.</param>
        void IMemberObject.SetMember(RuntimeContexts.RuntimeContext context, string name, IScriptObject value)
        {
            string rename;
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                rename = ClassHelper.RenamePrivate(name, cls.Name);
                if (StaticFields.ContainsKey(rename))
                {
                    StaticFields[rename] = value;
                    return;
                }
            }
            {
                rename = name;
                if (StaticFields.ContainsKey(rename))
                {
                    StaticFields[rename] = value;
                    return;
                }
            }
            if (cls == this || ClassHelper.IsExtendedFrom(this, cls) || ClassHelper.IsExtendedFrom(cls, this))
            {
                rename = ClassHelper.RenameProtected(name);
                if (StaticFields.ContainsKey(rename))
                {
                    StaticFields[rename] = value;
                    return;
                }
            }
            {
                var member = SetMember(context, name, true);
                var proset = member as PropertySetter;
                if (proset != null)
                {
                    proset.SetProperty(this, value);
                    return;
                }
            }
            throw new InvalidOperationException(
                string.Format(ExceptionResource.MemberNotFound, name));
        }

        internal ClassMember SetMember(RuntimeContexts.RuntimeContext context, string name, bool isStatic)
        {
            var members = isStatic ? StaticMembers : InstanceMembers;
            ClassMember value;
            string rename;
            string propget = ClassHelper.RenamePropertyGetter(name);
            string propset = ClassHelper.RenamePropertySetter(name);
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                rename = ClassHelper.RenamePrivate(name, cls.Name);
                if (members.TryGetValue(rename, out value) && value is ClassMethod)
                    return ClassHelper.ThrowCannotAssignMethod(name);
                rename = ClassHelper.RenamePrivate(propset, cls.Name);
                if (members.TryGetValue(rename, out value) && value is PropertySetter)
                    return value;
                rename = ClassHelper.RenamePrivate(propget, cls.Name);
                if (members.TryGetValue(rename, out value) && value is PropertyGetter)
                    return ClassHelper.ThrowPropertyIsReadonly(name);
            }
            {
                rename = name;
                if (members.TryGetValue(rename, out value) && value is ClassMethod)
                    return ClassHelper.ThrowCannotAssignMethod(name);
                rename = propset;
                if (members.TryGetValue(rename, out value) && value is PropertySetter)
                    return value;
                rename = propget;
                if (members.TryGetValue(rename, out value) && value is PropertyGetter)
                    return ClassHelper.ThrowPropertyIsReadonly(name);
            }
            if (cls == this || ClassHelper.IsExtendedFrom(this, cls) || ClassHelper.IsExtendedFrom(cls, this))
            {
                rename = ClassHelper.RenameProtected(name);
                if (members.TryGetValue(rename, out value) && value is ClassMethod)
                    return value;
                rename = ClassHelper.RenameProtected(propset);
                if (members.TryGetValue(rename, out value) && value is PropertySetter)
                    return value;
                rename = ClassHelper.RenameProtected(propget);
                if (members.TryGetValue(rename, out value) && value is PropertyGetter)
                    return ClassHelper.ThrowPropertyIsReadonly(name);
            }
            if (Super != null)
            {
                return Super.SetMember(context, name, isStatic);
            }
            return ClassHelper.ThrowMemberNotFound(name);
        }

        /// <summary>
        /// Invokes the <c>get</c> accessor of the indexer with the specified parameters and returns the value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <returns>The return value of the indexer.</returns>
        IScriptObject IIndexableObject.GetItem(RuntimeContexts.RuntimeContext context, List<IScriptObject> indices)
        {
            var member = GetItem(context, true);
            var idxget = member as IndexerGetter;
            if (idxget != null)
            {
                return idxget.GetItem(null, indices);
            }
            throw new InvalidOperationException(
                ExceptionResource.IndexerNotSupported);
        }

        internal ClassMember GetItem(RuntimeContexts.RuntimeContext context, bool isStatic)
        {
            var members = isStatic ? StaticMembers : InstanceMembers;
            ClassMember value;
            string rename;
            string idxget = ClassHelper.RenameIndexerGetter;
            string idxset = ClassHelper.RenameIndexerSetter;
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                rename = ClassHelper.RenamePrivate(idxget, cls.Name);
                if (members.TryGetValue(rename, out value) && value is IndexerGetter)
                    return value;
                rename = ClassHelper.RenamePrivate(idxset, cls.Name);
                if (members.TryGetValue(rename, out value) && value is IndexerSetter)
                    return ClassHelper.ThrowIndexerIsWriteonly();
            }
            {
                rename = idxget;
                if (members.TryGetValue(rename, out value) && value is IndexerGetter)
                    return value;
                rename = idxset;
                if (members.TryGetValue(rename, out value) && value is IndexerSetter)
                    return ClassHelper.ThrowIndexerIsWriteonly();
            }
            if (cls == this || ClassHelper.IsExtendedFrom(this, cls) || ClassHelper.IsExtendedFrom(cls, this))
            {
                rename = ClassHelper.RenameProtected(idxget);
                if (members.TryGetValue(rename, out value) && value is IndexerGetter)
                    return value;
                rename = ClassHelper.RenameProtected(idxset);
                if (members.TryGetValue(rename, out value) && value is IndexerSetter)
                    return ClassHelper.ThrowIndexerIsWriteonly();
            }
            if (Super != null)
                return Super.GetItem(context, isStatic);
            return ClassHelper.ThrowIndexerNotSupported();
        }

        /// <summary>
        /// Invokes the <c>set</c> accessor of the indexer with the specified parameters and the value to set.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <param name="value">The value to set.</param>
        void IIndexableObject.SetItem(RuntimeContexts.RuntimeContext context, List<IScriptObject> indices, IScriptObject value)
        {
            var member = SetItem(context, true);
            var idxset = member as IndexerSetter;
            if (idxset != null)
            {
                idxset.SetItem(null, indices, value);
                return;
            }
            throw new InvalidOperationException(
                ExceptionResource.IndexerNotSupported);
        }

        internal ClassMember SetItem(RuntimeContexts.RuntimeContext context, bool isStatic)
        {
            var members = isStatic ? StaticMembers : InstanceMembers;
            ClassMember value;
            string rename;
            string idxget = ClassHelper.RenameIndexerGetter;
            string idxset = ClassHelper.RenameIndexerSetter;
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                rename = ClassHelper.RenamePrivate(idxset, cls.Name);
                if (members.TryGetValue(rename, out value) && value is IndexerSetter)
                    return value;
                rename = ClassHelper.RenamePrivate(idxget, cls.Name);
                if (members.TryGetValue(rename, out value) && value is IndexerGetter)
                    return ClassHelper.ThrowIndexerIsReadonly();
            }
            {
                rename = idxset;
                if (members.TryGetValue(rename, out value) && value is IndexerSetter)
                    return value;
                rename = idxget;
                if (members.TryGetValue(rename, out value) && value is IndexerGetter)
                    return ClassHelper.ThrowIndexerIsReadonly();
            }
            if (cls == this || ClassHelper.IsExtendedFrom(this, cls) || ClassHelper.IsExtendedFrom(cls, this))
            {
                rename = ClassHelper.RenameProtected(idxset);
                if (members.TryGetValue(rename, out value) && value is IndexerSetter)
                    return value;
                rename = ClassHelper.RenameProtected(idxget);
                if (members.TryGetValue(rename, out value) && value is IndexerGetter)
                    return ClassHelper.ThrowIndexerIsReadonly();
            }
            if (Super != null)
                return Super.SetItem(context, isStatic);
            return ClassHelper.ThrowIndexerNotSupported();
        }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            return string.Format("class {0}", Name);
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
         
        internal ClassObject(
            string name,
            ClassObject super)
        {
            Name = name;
            Super = super;
            InstanceMembers = new Dictionary<string, ClassMember>();
            StaticFields = new Dictionary<string, IScriptObject>();
            StaticMembers = new Dictionary<string, ClassMember>();
        }
    }

    internal static class ClassHelper
    {
        public static bool IsExtendedFrom(this ClassObject value, ClassObject other)
        {
            if (value == null)
                return false;
            ClassObject cls = value.Super;
            while (cls != null)
                if (cls == other)
                    return true;
                else
                    cls = cls.Super;
            return false;
        }

        public static string RenamePrivate(
            string memberName,
            string className)
        {
            return string.Format("##__{0}__{1}", className, memberName);
        }
        public static string RenameProtected(
            string memberName)
        {
            return string.Format("#_{0}", memberName);
        }
        public static string RenamePropertyGetter(
            string memberName)
        {
            return string.Format("#get_{0}", memberName);
        }
        public static string RenamePropertySetter(
               string memberName)
        {
            return string.Format("#set_{0}", memberName);
        }
        public static string RenameIndexerGetter
        {
            get { return "#get_#indexer"; }
        }
        public static string RenameIndexerSetter
        {
            get { return "#set_#indexer"; }
        }

        public static ClassMember ThrowMemberNotFound(string name)
        {
            throw new InvalidOperationException(
                string.Format(ExceptionResource.MemberNotFound, name));
        }
        public static ClassMember ThrowCannotAssignMethod(string name)
        {
            throw new InvalidOperationException(
                string.Format(ExceptionResource.CannotAssignMethod, name));
        }
        public static ClassMember ThrowPropertyIsReadonly(string name)
        {
            throw new InvalidOperationException(
                string.Format(ExceptionResource.PropertyIsReadonly, name));
        }
        public static ClassMember ThrowPropertyIsWriteonly(string name)
        {
            throw new InvalidOperationException(
                string.Format(ExceptionResource.PropertyIsWriteonly, name));
        }
        public static ClassMember ThrowIndexerIsReadonly()
        {
            throw new InvalidOperationException(
                ExceptionResource.IndexerIsReadonly);
        }
        public static ClassMember ThrowIndexerIsWriteonly()
        {
            throw new InvalidOperationException(
                ExceptionResource.IndexerIsWriteonly);
        }
        public static ClassMember ThrowIndexerNotSupported()
        {
            throw new InvalidOperationException(
                ExceptionResource.IndexerNotSupported);
        }
    }
}
