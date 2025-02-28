﻿using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldConjunction
        : ObjectsShouldConjunction<ClassesShould, ClassesShouldConjunctionWithDescription, Class>
    {
        public ClassesShouldConjunction(IArchRuleCreator<Class> ruleCreator)
            : base(ruleCreator) { }
    }
}
