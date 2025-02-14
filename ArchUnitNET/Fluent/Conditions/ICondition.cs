﻿using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public interface ICondition<in TRuleType> : IHasDescription
        where TRuleType : ICanBeAnalyzed
    {
        IEnumerable<ConditionResult> Check(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        );
        bool CheckEmpty();
    }
}
