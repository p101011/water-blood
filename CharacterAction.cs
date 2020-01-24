using System;
using System.Collections.Generic;
using System.Text;

namespace water_blood
{
    public abstract class CharacterAction
    {
        // TODO: add conspirators/recruitment of lackeys
        public string ActionName { get; set; }
        public Person Executor { get; }
        public List<Person> Targets { get; }

        public List<Priorities> BenefitsPriorities { get; set; }

        protected CharacterAction(Person executor, List<Person> targets)
        {
            this.Executor = executor;
            this.Targets = targets;
        }

        public abstract void Execute();
        public abstract void PrettyPrint();

        public static CharacterAction GetCharacterAction(string name, Person executor, List<Person> targets)
        {
            return new SeekLowbirthPartnerAction(executor, targets);
        }

        public override string ToString()
        {
            return ActionName;
        }
    }

    public class SeekLowbirthPartnerAction:CharacterAction
    {
        public SeekLowbirthPartnerAction(Person executor, List<Person> targets) : base(executor, targets)
        {
            ActionName = "SeekLowbirthPartnerAction";
            BenefitsPriorities = new List<Priorities>
            {

            };
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override void PrettyPrint()
        {
            throw new NotImplementedException();
        }
    }
}
