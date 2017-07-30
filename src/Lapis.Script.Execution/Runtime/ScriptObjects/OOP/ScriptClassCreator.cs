/********************************************************************************
 * Module      : Lapis.Script.Execution
 * Class       : ScriptClassCreator
 * Description : Provides methods to build an instance of ScriptClass.
 * Created     : 2015/7/18
 * Note        :
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lapis.Script.Parser.Lexical;
using Lapis.Script.Parser.Parsing;
using Lapis.Script.Execution.Ast;
using Lapis.Script.Execution.Ast.Statements;
using Lapis.Script.Execution.Ast.Expressions;
using Lapis.Script.Execution.Ast.Members;
using Lapis.Script.Execution.Runtime.ScriptObjects;
using Lapis.Script.Execution.Runtime.ScriptObjects.NativeObjects;
using Lapis.Script.Execution.Runtime.ScriptObjects.OOP;
using Lapis.Script.Execution.Runtime.RuntimeContexts;

namespace Lapis.Script.Execution.Runtime.ScriptObjects.OOP
{
    internal class ScriptClassCreator
    {
        public FunctionContext Context { get; private set; }

        public ScriptClass Super { get; private set; }

        public ScriptClass Class { get; private set; }

        public ScriptClassCreator(RuntimeContext closure, string name, IClassObject super)
        {
            ScriptClass sup;
            if (super == null)
                sup = ObjectClass.Instance;
            else
            {
                sup = super as ScriptClass;
                if (sup == null)
                    throw new InvalidOperationException(ExceptionResource.NotInheritable);
            }
            Super = sup;
            Class = new ScriptClass(name, sup, null);
            Context = new FunctionContext(closure);
            Context.Memory.HoistingDecalreClass(Context, name);
            Context.Memory.DecalreClass(Context, name, Class);
        }

        public void DeclareMembers(MemberCollection members)
        {
            if (members != null)
                foreach (var member in members)
                    DeclareMember(member);
            if (Class.Constructor == null)
            {
                Class.Constructor = new ScriptConstructor(Class, null, null, Context);
            }
        }

        private void DeclareMember(Member member)
        {
            if (member == null)
                throw new ArgumentNullException();
            else if (member is Field)
                DeclareMember((Field)member);
            else if (member is Method)
                DeclareMember((Method)member);
            else if (member is Property)
                DeclareMember((Property)member);
            else if (member is Indexer)
                DeclareMember((Indexer)member);
            else if (member is Constructor)
                DeclareMember((Constructor)member);
            else
                throw new RuntimeException(member.LinePragma,
                    ExceptionResource.MemberExpected);
        }

        private void DeclareMember(Field member)
        {
            var name = member.Name;
            var modifier = member.Modifier;
            CheckName(name, member);
            name = RenameModifier(modifier, name);
            if (member.IsStatic)
            {
                IScriptObject value;
                if (member.InitialExpression != null)
                    value = Context.EvaluateExpression(member.InitialExpression);
                else
                    value = ScriptNull.Instance;
                Class.StaticFields.Add(name, value);
            }
            else
                Class.FieldInitializers.Add(name, member.InitialExpression);
        }

        private void DeclareMember(Method member)
        {
            var name = member.Name;
            CheckName(name, member);
            var modifier = member.Modifier;
            name = RenameModifier(modifier, name);
            var mtd = new ScriptMethod(Class, member.Parameters, member.Statements, Context);
            if (member.IsStatic)
                Class.StaticMembers.Add(name, mtd);
            else
                Class.InstanceMembers.Add(name, mtd);
        }

        private void DeclareMember(Property member)
        {
            var name = member.Name;
            CheckName(name, member);
            var modifier = member.Modifier;
            var g = member.Getter;
            var s = member.Setter;
            CheckModifier(modifier, g, s);
            ClassMethod gmtd = null, smtd = null;
            Modifier gmod = 0, smod = 0;
            if (g == null && s == null)
                throw new RuntimeException(member.LinePragma,
                    ExceptionResource.AccessorStatementsExpected);
            else if (g != null && s != null)
            {
                gmod = g.Modifier ?? member.Modifier;
                smod = s.Modifier ?? member.Modifier;
                if (g.Statements == null && s.Statements == null)
                {
                    var id = "#autoproperty_" + member.Name;
                    // CheckName(id, member);
                    if (member.IsStatic)
                        Class.StaticFields.Add(id, ScriptNull.Instance);
                    else
                        Class.FieldInitializers.Add(id, null);
                    gmtd = new ScriptMethod(Class,
                        new ParameterCollection(),
                        new StatementCollection(
                            new ReturnStatement(g.LinePragma,
                                new MemberReferenceExpression(g.LinePragma,
                                    member.IsStatic ?
                                    new VariableReferenceExpression(g.LinePragma, Class.Name) as Expression :
                                    new ThisReferenceExpression(g.LinePragma),
                                    id))),
                        Context);
                    smtd = new ScriptMethod(Class,
                        new ParameterCollection(new Parameter(s.LinePragma, "value")),
                        new StatementCollection(
                            new ExpressionStatement(s.LinePragma,
                                new AssignExpression(s.LinePragma,
                                    new MemberReferenceExpression(s.LinePragma,
                                        member.IsStatic ?
                                        new VariableReferenceExpression(s.LinePragma, Class.Name) as Expression :
                                        new ThisReferenceExpression(s.LinePragma),
                                        id),
                                    new VariableReferenceExpression(s.LinePragma, "value")))),
                        Context);
                }
                else if (g.Statements != null && s.Statements != null)
                {
                    gmtd = new ScriptMethod(Class,
                        new ParameterCollection(),
                        g.Statements, Context);
                    smtd = new ScriptMethod(Class,
                        new ParameterCollection(new Parameter(s.LinePragma, "value")),
                        s.Statements, Context);
                }
                else
                    throw new RuntimeException((g.Statements == null ? g : s).LinePragma,
                           ExceptionResource.AccessorStatementsExpected);
            }
            else if (g != null)
            {
                gmod = g.Modifier ?? member.Modifier;
                if (g.Statements == null)
                {
                    throw new RuntimeException(g.LinePragma,
                        ExceptionResource.AutoPropertyMustHasGetSet);
                }
                else
                {
                    gmtd = new ScriptMethod(Class,
                        new ParameterCollection(), g.Statements,
                        Context);
                }
            }
            else
            {
                smod = s.Modifier ?? member.Modifier;
                if (s.Statements == null)
                {
                    throw new RuntimeException(s.LinePragma,
                        ExceptionResource.AutoPropertyMustHasGetSet);
                }
                else
                {
                    smtd = new ScriptMethod(Class,
                        new ParameterCollection(new Parameter(s.LinePragma, "value")), s.Statements,
                        Context);
                }
            }
            if (gmtd != null)
            {
                var propget = ClassHelper.RenamePropertyGetter(name);
                propget = RenameModifier(gmod, propget);
                if (member.IsStatic)
                    Class.StaticMembers.Add(propget, new PropertyGetter(gmtd));
                else
                    Class.InstanceMembers.Add(propget, new PropertyGetter(gmtd));
            }
            if (smtd != null)
            {
                var propset = ClassHelper.RenamePropertySetter(name);
                propset = RenameModifier(smod, propset);
                if (member.IsStatic)
                    Class.StaticMembers.Add(propset, new PropertySetter(smtd));
                else
                    Class.InstanceMembers.Add(propset, new PropertySetter(smtd));
            }
        }

        private void DeclareMember(Indexer member)
        {
            CheckIndexer(member, member.IsStatic);
            var paras = member.Parameters;
            if (paras == null || paras.Count == 0)
                throw new RuntimeException(member.LinePragma,
                    ExceptionResource.ParemetersExpected);
            FunctionHelper.CheckParameters(paras);
            var modifier = member.Modifier;
            var g = member.Getter;
            var s = member.Setter;
            CheckModifier(modifier, g, s);
            ClassMethod gmtd = null, smtd = null;
            Modifier gmod = 0, smod = 0;
            if (g == null && s == null)
                throw new RuntimeException(member.LinePragma,
                    ExceptionResource.AccessorStatementsExpected);
            else if (g != null && s != null)
            {
                gmod = g.Modifier ?? member.Modifier;
                smod = s.Modifier ?? member.Modifier;
                if (g.Statements == null && s.Statements == null)
                {
                    throw new RuntimeException(member.LinePragma,
                        ExceptionResource.AccessorStatementsExpected);
                }
                else if (g.Statements != null && s.Statements != null)
                {
                    gmtd = new ScriptMethod(Class,
                        paras,
                        g.Statements, Context);
                    Parameter paraValue = paras.FirstOrDefault(p => p.Name == "value");
                    if (paraValue != null)
                        throw new RuntimeException(paraValue.LinePragma,
                            string.Format(ExceptionResource.AutoParameterNameExist, "value"));
                    var list = paras.ToList();
                    list.Insert(0, new Parameter(s.LinePragma, "value"));
                    smtd = new ScriptMethod(Class,
                        new ParameterCollection(list),
                        s.Statements, Context);
                }
                else
                    throw new RuntimeException((g.Statements == null ? g : s).LinePragma,
                        ExceptionResource.AccessorStatementsExpected);
            }
            else if (g != null)
            {
                gmod = g.Modifier ?? member.Modifier;
                if (g.Statements == null)
                {
                    throw new RuntimeException(g.LinePragma,
                        ExceptionResource.AccessorStatementsExpected);
                }
                else
                {
                    gmtd = new ScriptMethod(Class,
                        paras,
                        g.Statements, Context);
                }
            }
            else
            {
                smod = s.Modifier ?? member.Modifier;
                if (s.Statements == null)
                {
                    throw new RuntimeException(s.LinePragma,
                        ExceptionResource.AccessorStatementsExpected);
                }
                else
                {
                    Parameter paraValue = paras.FirstOrDefault(p => p.Name == "value");
                    if (paraValue != null)
                        throw new RuntimeException(paraValue.LinePragma,
                            string.Format(ExceptionResource.AutoParameterNameExist, "value"));
                    var list = paras.ToList();
                    list.Insert(0, new Parameter(s.LinePragma, "value"));
                    smtd = new ScriptMethod(Class,
                        new ParameterCollection(list),
                        s.Statements, Context);
                }
            }
            if (gmtd != null)
            {
                var idxget = ClassHelper.RenameIndexerGetter;
                idxget = RenameModifier(gmod, idxget);
                if (member.IsStatic)
                    Class.StaticMembers.Add(idxget, new IndexerGetter(gmtd));
                else
                    Class.InstanceMembers.Add(idxget, new IndexerGetter(gmtd));
            }
            if (smtd != null)
            {
                var idxset = ClassHelper.RenameIndexerSetter;
                idxset = RenameModifier(smod, idxset);
                if (member.IsStatic)
                    Class.StaticMembers.Add(idxset, new IndexerSetter(smtd));
                else
                    Class.InstanceMembers.Add(idxset, new IndexerSetter(smtd));
            }
        }

        private void DeclareMember(Constructor member)
        {
            if (member.Modifier != Modifier.Public)
                throw new RuntimeException(member.LinePragma,
                    ExceptionResource.ConstructorMustBePublic);
            if (member.IsStatic)
                throw new RuntimeException(member.LinePragma,
                    ExceptionResource.CannotBeStatic);
            var con = new ScriptConstructor(Class, member.Parameters, member.Statements, Context);
            Class.Constructor = con;
        }

        private HashSet<string> _names = new HashSet<string>();
        private void CheckName(string name, Member member)
        {
            if (_names.Contains(name))
                throw new RuntimeException(member.LinePragma,
                    string.Format(ExceptionResource.MemberAlreadyExists, name));
            else
                _names.Add(name);
        }

        private bool _hasIndexer = false;
        private bool _hasStaticIndexer = false;
        private void CheckIndexer(Indexer member, bool isStatic)
        {
            if (isStatic)
                if (_hasStaticIndexer)
                    goto fail;
                else
                    _hasStaticIndexer = true;
            else
                if (_hasIndexer)
                    goto fail;
                else
                    _hasIndexer = true;
            return;
        fail:
            throw new RuntimeException(member.LinePragma,
                   ExceptionResource.IndexerAlreadyExists);
        }

        private void CheckConstructor(Constructor member)
        {
            if (Class.Constructor != null)
            {
                throw new RuntimeException(member.LinePragma,
                    ExceptionResource.MultipleConstructors);
            }
        }

        private string RenameModifier(Modifier modifier, string name)
        {
            if (modifier == Modifier.Private)
                return ClassHelper.RenamePrivate(name, Class.Name);
            else if (modifier == Modifier.Protected)
                return ClassHelper.RenameProtected(name);
            else
                return name;
        }

        private void CheckModifier(Modifier main, Accessor g, Accessor s)
        {
            if (g != null && s != null)
            {
                if (g.Modifier.HasValue && s.Modifier.HasValue)
                {
                    throw new RuntimeException(g.LinePragma,
                            ExceptionResource.CannotUseBothGetSetModifier);
                }
                if (g.Modifier.HasValue)
                {
                    var mod = g.Modifier.Value;
                    if (!mod.IsMoreStrictThan(main))
                        throw new RuntimeException(g.LinePragma,
                             ExceptionResource.GetSetModifierMoreStrictThanProperty);
                }
                if (s.Modifier.HasValue)
                {
                    var mod = s.Modifier.Value;
                    if (!mod.IsMoreStrictThan(main))
                        throw new RuntimeException(s.LinePragma,
                             ExceptionResource.GetSetModifierMoreStrictThanProperty);
                }
            }
            else if (g != null && g.Modifier.HasValue)
            {
                throw new RuntimeException(g.LinePragma,
                     ExceptionResource.CannotUseGetSetModifier);
            }
            else if (s != null && s.Modifier.HasValue)
            {
                throw new RuntimeException(g.LinePragma,
                     ExceptionResource.CannotUseGetSetModifier);
            }
        }
    }
}
