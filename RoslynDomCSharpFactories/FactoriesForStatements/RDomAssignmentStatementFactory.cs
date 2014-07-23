﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDom.Common;

namespace RoslynDom.CSharp
{
    public class RDomAssignmentStatementFactory
         : RDomStatementFactory<RDomAssignmentStatement, ExpressionStatementSyntax>
    {
        protected  override IStatementCommentWhite CreateItemFrom(SyntaxNode syntaxNode, IDom parent, SemanticModel model)
        {
            var syntax = syntaxNode as ExpressionStatementSyntax;
            var newItem = new RDomAssignmentStatement(syntaxNode, parent,model);

            var binary = syntax.Expression as BinaryExpressionSyntax;
            if (binary == null) throw new InvalidOperationException();
            // TODO: handle all the other kinds of assigments here (like +=)
            if (binary.CSharpKind() != SyntaxKind.SimpleAssignmentExpression) { throw new NotImplementedException(); }
            var left = binary.Left;
            var identifier = left as IdentifierNameSyntax;
            if (identifier == null) throw new InvalidOperationException();
            var right = binary.Right;
            var expression = right as ExpressionSyntax;
            if (expression == null) throw new InvalidOperationException();
            newItem.Name = identifier.ToString();
            newItem.Expression = RDomFactoryHelper.GetHelperForExpression().MakeItems(expression, newItem, model).FirstOrDefault();

            return newItem ;
        }
        public override IEnumerable<SyntaxNode> BuildSyntax(IStatementCommentWhite item)
        {
            var itemAsT = item as IAssignmentStatement;
            var nameSyntax = SyntaxFactory.IdentifierName(itemAsT.Name);
            var expressionSyntax = RDomCSharpFactory.Factory.BuildSyntax(itemAsT.Expression );

            var assignmentSyntax = SyntaxFactory.BinaryExpression(SyntaxKind.SimpleAssignmentExpression, nameSyntax, (ExpressionSyntax)expressionSyntax);
            var node = SyntaxFactory.ExpressionStatement(assignmentSyntax );
            return new SyntaxNode[] { RoslynUtilities.Format(node) };

        }
        public override bool CanCreateFrom(SyntaxNode syntaxNode)
        {
            var statement = syntaxNode as ExpressionStatementSyntax;
            if (statement == null) { return false; }
            return statement.Expression.CSharpKind() == SyntaxKind.SimpleAssignmentExpression;
        }
    }
}
