/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : InstanceObject
 * Description : Represents an instance of a class.
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
    /// Represents an instance of a class.
    /// </summary>
    public class InstanceObject : IMemberObject, IIndexableObject, ISuperMemberObject, ISuperIndexableObject 
    {
        /// <summary>
        /// Gets the class that the instance belongs to.
        /// </summary>
        /// <value>The class that the instance belongs to.</value>     
        public ClassObject Class { get; private set; }

        /// <summary>
        /// Gets the instance fields of the object.
        /// </summary>
        /// <value>The instance fields of the object.</value>
        public Dictionary<string, IScriptObject> Fields { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="InstanceObject"/> class using the specified parameters.
        /// </summary>
        /// <param name="cls">The class that the instance belongs to.</param>
        /// <exception cref="ArgumentNullException">The parameter is <see langword="null"/>.</exception>
        public InstanceObject(ClassObject cls)
        {
            if (cls == null)
                throw new ArgumentNullException();
            Class = cls;
            Fields = new Dictionary<string, IScriptObject>();
        }

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
                if (Fields.TryGetValue(rename, out value))
                    return value;                
            }
            {
                rename = name;
                if (Fields.TryGetValue(rename, out value))
                    return value;  
            }    
            if (cls == this.Class || ClassHelper.IsExtendedFrom(this.Class, cls) || ClassHelper.IsExtendedFrom(cls, this.Class))
            {
                rename = ClassHelper.RenameProtected(name);
                if (Fields.TryGetValue(rename, out value))
                    return value;
            }
            {
                var member = Class.GetMember(context, name, false);
                var mtd = member as ClassMethod;
                if (mtd != null)
                {
                    return new BoundMethod(this, mtd);
                }
                var proget = member as PropertyGetter;
                if (proget != null)
                {
                    return proget.GetProperty(this);
                }               
            }
            throw new InvalidOperationException(
                string.Format(ExceptionResource.MemberNotFound, name));
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
                if (Fields.ContainsKey(rename))
                {
                    Fields[rename] = value;
                    return;
                }
            }
            {
                rename = name;
                if (Fields.ContainsKey(rename))
                {
                    Fields[rename] = value;
                    return;
                }
            }            
            if (cls == this.Class || ClassHelper.IsExtendedFrom(this.Class, cls) || ClassHelper.IsExtendedFrom(cls, this.Class))
            {
                rename = ClassHelper.RenameProtected(name);
                if (Fields.ContainsKey(rename))
                {
                    Fields[rename] = value;
                    return;
                }
            }           
            {
                var member = Class.SetMember(context, name, false);
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

        /// <summary>
        /// Invokes the <c>get</c> accessor of the indexer with the specified parameters and returns the value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <returns>The return value of the indexer.</returns>
        IScriptObject IIndexableObject.GetItem(RuntimeContexts.RuntimeContext context, List<IScriptObject> indices)
        {
            var member = Class.GetItem(context, false);
            var idxget = member as IndexerGetter;
            if (idxget != null)
            {
                return idxget.GetItem(this, indices);
            }
            throw new InvalidOperationException(
                ExceptionResource.IndexerNotSupported);
        }

        /// <summary>
        /// Invokes the <c>set</c> accessor of the indexer with the specified parameters and the value to set.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <param name="value">The value to set.</param>
        void IIndexableObject.SetItem(RuntimeContexts.RuntimeContext context, List<IScriptObject> indices, IScriptObject value)
        {
            var member = Class.SetItem(context, false);
            var idxset = member as IndexerSetter;
            if (idxset != null)
            {
                idxset.SetItem(this, indices, value);
                return;
            }
            throw new InvalidOperationException(
                ExceptionResource.IndexerNotSupported);
        }

        /// <summary>
        /// Returns the value of the member of the specified name referring to <c>super</c>.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <returns>The value of the member <paramref name="name"/>.</returns>
        IScriptObject ISuperMemberObject.GetSuperMember(RuntimeContexts.RuntimeContext context, string name)
        {
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                var super = cls.Super;
                if (ClassHelper.IsExtendedFrom(this.Class, super))
                {
                    var member = super.GetMember(context, name, false);
                    var mtd = member as ClassMethod;
                    if (mtd != null)
                    {
                        return new BoundMethod(this, mtd);
                    }
                    var proget = member as PropertyGetter;
                    if (proget != null)
                    {
                        return proget.GetProperty(this);
                    }
                    throw new InvalidOperationException(
                        string.Format(ExceptionResource.MemberNotFound, name));
                }
            }
            throw new InvalidOperationException(
                ExceptionResource.SuperMustBeInExtendedClass);

        }

        /// <summary>
        /// Sets the value of the member of the specified name referring to <c>super</c> to the specified value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="value">The value to set.</param>
        void ISuperMemberObject.SetSuperMember(RuntimeContexts.RuntimeContext context, string name, IScriptObject value)
        {
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                var super = cls.Super;
                if (ClassHelper.IsExtendedFrom(this.Class, super))
                {
                    var member = super.SetMember(context, name, false);
                    var proset = member as PropertySetter;
                    if (proset != null)
                    {
                        proset.SetProperty(this, value);
                        return;
                    }
                    throw new InvalidOperationException(
                        string.Format(ExceptionResource.MemberNotFound, name));
                }
            }
            throw new InvalidOperationException(
                ExceptionResource.SuperMustBeInExtendedClass);
        }

        /// <summary>
        /// Invokes the <c>get</c> accessor of the indexer referring to <c>super</c> with the specified parameters and returns the value.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <returns>The return value of the indexer.</returns>
        IScriptObject ISuperIndexableObject.GetSuperItem(RuntimeContexts.RuntimeContext context, List<IScriptObject> indices)
        {
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                var super = cls.Super;
                if (ClassHelper.IsExtendedFrom(this.Class, super))
                {
                    var member = super.GetItem(context, false);
                    var idxget = member as IndexerGetter;
                    if (idxget != null)
                    {
                        return idxget.GetItem(this, indices);
                    }
                    throw new InvalidOperationException(
                        ExceptionResource.IndexerNotSupported);
                }
            }
            throw new InvalidOperationException(
                ExceptionResource.SuperMustBeInExtendedClass);
        }

        /// <summary>
        /// Invokes the <c>set</c> accessor of the indexer referring to <c>super</c> with the specified parameters and the value to set.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        /// <param name="indices">The parameters passed to the indexer.</param>
        /// <param name="value">The value to set.</param>
        void ISuperIndexableObject.SetSuperItem(RuntimeContexts.RuntimeContext context, List<IScriptObject> indices, IScriptObject value)
        {
            var cls = context.Class as ClassObject;
            if (cls != null)
            {
                var super = cls.Super;
                if (ClassHelper.IsExtendedFrom(this.Class, super))
                {
                    var member = super.SetItem(context, false);
                    var idxset = member as IndexerSetter;
                    if (idxset != null)
                    {
                        idxset.SetItem(this, indices, value);
                        return;
                    }
                    throw new InvalidOperationException(
                        ExceptionResource.IndexerNotSupported);
                }
            }
            throw new InvalidOperationException(
                ExceptionResource.SuperMustBeInExtendedClass);
        }

        /// <summary>
        /// Specifies whether this script object is equal to another <see cref="IScriptObject"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IScriptObject"/>.</param>
        /// <returns><see langword="true"/> if this script object is equal to the other <see cref="IScriptObject"/>; otherwise, <see langword="false"/>.</returns>
        public virtual bool Equals(IScriptObject other)
        {
            return base.Equals(other);
        }

        /// <summary>
        /// Returns the string representation of the script object.
        /// </summary>
        /// <returns>The string representation of the script object.</returns>
        public override string ToString()
        {
            try
            {
                ClassMember member; ClassMethod mtd;
                var cls = Class;
                while (cls != null)
                    if (cls.InstanceMembers.TryGetValue("toString", out member) &&
                       (mtd = member as ClassMethod) != null)
                    {
                        return mtd.Invoke(this, new List<IScriptObject>()).ToString();
                    }
                    else
                        cls = cls.Super;
            }
            finally
            {
                
            }
            return string.Format("instance of {0}", Class.Name);
        }
    }
}
