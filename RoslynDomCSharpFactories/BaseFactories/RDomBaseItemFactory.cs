﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDom.Common;

namespace RoslynDom.CSharp
{
    // Factories are specific to the type, FactoryHelpers are specific to the level (StemMember, TypeMember, Statement, Expression)
    public abstract class RDomBaseItemFactory<T, TSyntax, TKind> : IRDomFactory<TKind>
        where T : class, TKind  // This isn't stupid - TKind is the broader category, T is the specific 
        where TSyntax : SyntaxNode
        where TKind : class
    {
        public virtual FactoryPriority Priority
        {
            get
            {
                if (this.GetType().IsConstructedGenericType) { return FactoryPriority.Fallback; }
                return FactoryPriority.Normal;
            }
        }

        public abstract IEnumerable<SyntaxNode> BuildSyntax(TKind item);

        public virtual bool CanCreateFrom(SyntaxNode syntaxNode)
        {
            return (syntaxNode is TSyntax);
        }

        public virtual IEnumerable<TKind> CreateFrom(SyntaxNode syntaxNode, IDom parent, SemanticModel model)
        {
            var syntax = syntaxNode as TSyntax;
            var publicAnnotations = RDomFactoryHelper.GetPublicAnnotations(syntaxNode, parent);
            var newItem = Activator.CreateInstance(
                        typeof(T),
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                        new object[] { syntax, parent, model }, null);
            var itemAsT = newItem as T;
            InitializeItem(itemAsT, syntax);
            return new TKind[] { itemAsT };
        }

        public virtual void InitializeItem(T newItem, TSyntax syntax)
        { return; }
    }

    public class RDomRootContainerFactory<T, TSyntax> : RDomBaseItemFactory<T, TSyntax, IRoot>
             where T : class, IRoot
             where TSyntax : SyntaxNode
    {
        public override IEnumerable<SyntaxNode> BuildSyntax(IRoot item)
        {
            throw new NotImplementedException();
        }
    }

    public class RDomStemMemberFactory<T, TSyntax> : RDomBaseItemFactory<T, TSyntax, IStemMember>
            where T : class, IStemMember
            where TSyntax : SyntaxNode
    {

        public override IEnumerable<SyntaxNode> BuildSyntax(IStemMember item)
        { throw new NotImplementedException(); }
    }

    public class RDomTypeMemberFactory<T, TSyntax> : RDomBaseItemFactory<T, TSyntax, ITypeMember>
            where T : class, ITypeMember
            where TSyntax : SyntaxNode
    {
        public override IEnumerable<SyntaxNode> BuildSyntax(ITypeMember item)
        { throw new NotImplementedException(); }
    }

    public class RDomStatementFactory<T, TSyntax> : RDomBaseItemFactory<T, TSyntax, IStatement>
            where T : class, IStatement
            where TSyntax : SyntaxNode
    {
        public override IEnumerable<SyntaxNode> BuildSyntax(IStatement item)
        { throw new NotImplementedException(); }
    }

    public class RDomExpressionFactory<T, TSyntax> : RDomBaseItemFactory<T, TSyntax, IExpression>
             where T : class, IExpression
             where TSyntax : SyntaxNode
    {
        public override IEnumerable<SyntaxNode> BuildSyntax(IExpression item)
        { throw new NotImplementedException(); }
    }

    public class RDomMiscFactory<T, TSyntax> : RDomBaseItemFactory<T, TSyntax, IMisc>
             where T : class, IMisc
             where TSyntax : SyntaxNode
    {
        public override IEnumerable<SyntaxNode> BuildSyntax(IMisc item)
        { throw new NotImplementedException(); }
    }
}
