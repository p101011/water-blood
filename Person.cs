using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace water_blood
{

    public enum Priorities
    {
        Wealth, Popularity, Prominence, Power, Heirs, Lovers, None
    }

    public enum Traits
    {
        Lustful, Greedy, Content, Nurturing
    }

    public class Person
    {
        public string Name { get; set; } // self explanatory
        public Family FamilyName { get; set; } // provides a person with connections
        public Title HeldTitle { get; set; } // provides means of attempting to boost stats
        public int Wealth { get; set; } // measure of how important person is to economy
        public int Popularity { get; set; } // how well-liked the person is
        public int Prominence { get; set; } // how (in)famous the person is - linked to popularity
        public int Power { get; set; } // how much military power the person can muster
        public int Age { get; set; } // duh
        public bool IsLiving { get; set; } // also duh
        public float Attractiveness { get; set; } // how attractive a person is: 0 - 1
        public float Susceptibility { get; set; } // how much a person lets themself be influenced: 0 - 1
        public float Fertility { get; set; } // odds that the person produces a child
        public bool IsHetero { get; set; } // whether the person actively wants fertile relationship - can hurt chances for heirs
        public bool IsMale { get; set; } // gets gender
        public Person Spouse { get; set; } // the people to whom this person has been married - 1 living max
        public bool IsMarried => Spouse != null; // whether the person has a living spouse
        public List<Person> Lovers { get; set; } // the people whom this person is currently seeing, including affairs and spouses
        public List<Person> Parents { get; set; } // person's parents
        public List<Person> Children { get; set; } // person's offspring
        public List<IActionDef> AvailableActions { get; set; } // universal actions and actions afforded by title
        public List<Traits> CharacterTraits { get; set; }
        public static List<IActionDef> UniversalActions { get; set; } // actions available to any person at all times

        private Priorities _priority = Priorities.None;
        private int _priorityTimer = 0;
        private bool _isSatisfied = false;
        private int _mourningTimer = -1;

        public override string ToString()
        {
            return IsLiving
                ? $"{Name} {FamilyName} is a {Age} year old {HeldTitle} with {Children.Count} children."
                : $"{Name} {FamilyName} was a {HeldTitle} who died at the age of {Age} with {Children.Count} children.";
        }

        public void Init(List<Person> parents)
        {
            Lovers = new List<Person>();
            Parents = parents;
            Children = new List<Person>();
        }

        // TODO: create Relationship class which tracks relationship dynamics
        // this is called when two people's relationships might be broken
        public void CheckRelationship(Person other)
        {
            bool changeFlag = false;
            // death case
            if (other.IsLiving) return;
            if (other == Spouse)
            {
                Spouse = null;
                _mourningTimer = (int) Program.GetRandFloat() * Constants.AVERAGE_MOURNING;
                changeFlag = true;
            }

            if (Lovers.Contains(other))
            {
                Lovers.Remove(other);
                changeFlag = true;
            }

            if (changeFlag)
            {
                UpdateAvailableActions();
            }
        }

        public CharacterAction Tick(int month)
        {
            // update character standing based on family, title, spouse, and lovers
            // bring character popularity and prominence towards family at rate bounded by FAMILY_ETA
            // also bring families towards character popularities
            int popularityGoal = (Popularity + FamilyName.Popularity) / 2;
            Popularity += Math.Max(Math.Min(Constants.FAMILY_ETA, popularityGoal - Popularity), -Constants.FAMILY_ETA);
            FamilyName.Popularity += Math.Max(Math.Min(Constants.FAMILY_ETA, popularityGoal - Popularity), -Constants.FAMILY_ETA);
            int prominenceGoal = (Popularity + FamilyName.Popularity) / 2;
            Prominence += Math.Max(Math.Min(Constants.FAMILY_ETA, prominenceGoal - Prominence), -Constants.FAMILY_ETA);
            FamilyName.Prominence += Math.Max(Math.Min(Constants.FAMILY_ETA, prominenceGoal - Prominence), -Constants.FAMILY_ETA);

            // split salary on spouses and lovers, if we have one
            if (HeldTitle != null)
            {
                int wages = HeldTitle.Salary;
                if (IsMarried) wages = (int) ((1f - Constants.SALARY_SPOUSE_SHARE) * wages);
                float loversCut = Constants.SALARY_LOVER_SHARE * Lovers.Count;
                wages -= (int) (HeldTitle.Salary * loversCut);
                Wealth += wages;
            }

            // collect salary from spouse
            if (Spouse != null && Spouse.HeldTitle != null)
            {
                int cut = (int) (Spouse.HeldTitle.Salary * Constants.SALARY_SPOUSE_SHARE);
                Wealth += cut;
            }

            // and from lovers
            foreach (Person l in Lovers)
            {
                if (l.HeldTitle != null) 
                {
                    int cut = (int)(Spouse.HeldTitle.Salary * Constants.SALARY_LOVER_SHARE);
                    Wealth += cut;
                }
            }

            if (_isSatisfied && Program.GetRandFloat() < Constants.CPM_GET_GOAL)
            {
                _isSatisfied = false;
                _priority = GetPriority();
                _priorityTimer = 0;
            }
            else
            {
                _priorityTimer++;
            }

            if (_isSatisfied) return null;

            // check to see if current priority is satisfied

            IEnumerable<IActionDef> options = GetActionsForPriority(_priority);
            foreach (IActionDef def in options)
            {
                List<Person> targets = def.FindTargets(this);
                if (!targets.Any()) continue;
                CharacterAction builtAction = CharacterAction.GetCharacterAction(def.ActionName, this, targets);
                if (builtAction == null) continue;
                return builtAction;
            }
            return null;
        }

        private Priorities GetPriority()
        {
            float min = float.PositiveInfinity;
            Priorities minVal = Priorities.None;
            if (CharacterTraits.Contains(Traits.Lustful) && Program.GetRandFloat() < 0.0125f) {
                return Priorities.Lovers;
            }
            if (IsMale && Age > Constants.MALE_AGE_PANIC && !Children.Any(c => c.IsMale))
            {
                min = Constants.MALE_HEIR_DESIRE;
                minVal = Priorities.Heirs;
            }
            if (CharacterTraits.Contains(Traits.Nurturing) && Children.Count < Constants.NURTURING_TARGET)
            {
                min = Constants.NURTURING_DESIRE;
                minVal = Priorities.Heirs;
            }
            if (Wealth < min)
            {
                min = Wealth;
                minVal = Priorities.Wealth;
            }
            if (Popularity < min) {
                min = Popularity;
                minVal = Priorities.Popularity;
            }
            if (Prominence < min) {
                min = Prominence;
                minVal = Priorities.Prominence;
            }
            if (Power < min) {
                min = Power;
                minVal = Priorities.Power;
            }

            return minVal;
        }

        private IEnumerable<IActionDef> GetActionsForPriority(Priorities priority)
        {
            IEnumerable<IActionDef> actions =
                AvailableActions.Where(action => action.BenefitsPriorities.Contains(priority));
            return Program.ShuffleEnumerable(actions);
        }

    }
}
