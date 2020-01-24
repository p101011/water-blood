using System;
using System.Collections.Generic;
using System.Text;

namespace water_blood
{
    public class Family
    {
        public string Name { get; set; } // the name of the family
        public int Wealth { get; set; } // the total wealth of the family
        public int Popularity { get; set; } // the overall popularity of the family
        public int Prominence { get; set; } // the overall prominence of the family
        public int Power { get; set; } // the total military power of the family
        public List<Person> Members { get; set; } // living and dead members of the family
        public Person Head { get; set; } // the person at the head of the family
        public override string ToString()
        {
            return Name;
        }
    }
}
