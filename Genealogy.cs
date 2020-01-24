using System;
using System.Collections.Generic;

namespace water_blood
{
	public class Genealogy
	{

        public List<Person> Figures { get; set; }
        public List<CharacterAction> History { get; set; }

        public Genealogy(List<Person> seeds)
        {
            Figures = seeds;
            History = new List<CharacterAction>();
        }

		public void Tick(int monthOffset)
		{
            List<CharacterAction> tickActions = new List<CharacterAction>();
            foreach (Person p in Figures)
            {
                CharacterAction a = p.Tick(monthOffset);
                if (a != null) tickActions.Add(a);
            }

            foreach (CharacterAction a in tickActions)
            {
                a.Execute();
                History.Add(a);
            }
		}

        public void PrintHistory()
        {
			foreach (CharacterAction action in History)
            {
                action.PrettyPrint();
            }
        }

        public void PrintPeople()
        {
            foreach (Person p in Figures)
            {
                Console.WriteLine(p);
            }
        }
    }
}
