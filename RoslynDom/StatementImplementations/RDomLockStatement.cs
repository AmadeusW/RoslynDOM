using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;

namespace RoslynDom
{
   public class RDomLockStatement : RDomBase<ILockStatement, ISymbol>, ILockStatement
   {
      private RDomCollection<IStatementCommentWhite> _statements;

      public RDomLockStatement(IExpression expression, bool hasBlock)
      : this(null, null, null)
      {
         _expression = expression;
         _hasBlock = hasBlock;
      }

      public RDomLockStatement(SyntaxNode rawItem, IDom parent, SemanticModel model)
         : base(rawItem, parent, model)
      { Initialize(); }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1811:AvoidUncalledPrivateCode", Justification = "Called via Reflection")]
      internal RDomLockStatement(RDomLockStatement oldRDom)
          : base(oldRDom)
      {
         Initialize();
         var statements = RoslynDomUtilities.CopyMembers(oldRDom.Statements);
         StatementsAll.AddOrMoveRange(statements);
         _hasBlock = oldRDom.HasBlock;
         _expression = oldRDom.Expression.Copy();
      }

      private void Initialize()
      {
         _statements = new RDomCollection<IStatementCommentWhite>(this);
      }

      public override IEnumerable<IDom> Children
      {
         get
         {
            var list = base.Children.ToList();
            list.AddRange(Statements);
            return list;
         }
      }

      private IExpression _expression;
      public IExpression Expression
      {
         get { return _expression; }
         set { SetProperty(ref _expression, value); }
      }

      private bool _hasBlock;
      public bool HasBlock
      {
         get { return _hasBlock; }
         set { SetProperty(ref _hasBlock, value); }
      }

      public IEnumerable<IStatement> Statements
      { get { return _statements.OfType<IStatement>().ToList(); } }

      public RDomCollection<IStatementCommentWhite> StatementsAll
      { get { return _statements; } }
   }
}
