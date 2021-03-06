﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetDescriptorContext : RuleSetBase
    {
        public RuleGroup<IRuleGetValues<Type>> DescriptionSourceRules { get; } 
                = new RuleGroup<IRuleGetValues<Type>>();

        public override void ReplaceAbstractRules(DescriptorMakerSpecificSourceBase tools)
        {
            DescriptionSourceRules.ReplaceAbstractRules(tools);
        }

        public override string Report(int tabsCount)
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<DetailReportStructure> GetRulesReportStructure()
        {
            return DescriptionSourceRules.Select(x => new DetailReportStructure("Description source", x.RuleDescription<IRuleGetCandidates>(), x.GetType()));
        }

   
    }
}
