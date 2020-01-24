using System;
using System.Collections.Generic;
using System.Text;

namespace water_blood
{
    public interface IActionDef
    {
        public string ActionName { get; }

        public List<Priorities> BenefitsPriorities { get; }

        public List<Person> FindTargets(Person executor);
    }
}
