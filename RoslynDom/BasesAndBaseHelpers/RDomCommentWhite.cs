﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;

namespace RoslynDom
{
    public abstract class RDomCommentWhite : RDomBase, ICommentWhite 
    {
    
        public MemberKind MemberKind
        { get { return MemberKind.Whitespace; } }

        public override object OriginalRawItem
        { get { return null; } }

        public override string OuterName
        { get { return null; } }

        public override object RawItem
        { get { return null; } }

        public StemMemberKind StemMemberKind
        { get { return StemMemberKind.Whitespace; } }

        public override ISymbol Symbol
        { get { return null; } }

        public override object RequestValue(string propertyName)
        { return null; }
      
    }

}
