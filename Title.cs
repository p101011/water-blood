using System;
using System.Collections.Generic;
using System.Text;

namespace water_blood
{
    public class Title
    {
        public string Name { get; set; } // name of the title
        public int Prominence { get; set; } // natural modifier to holder's prominence
        public int Salary { get; set; } // income provided by holdings
        public int Power { get; set; } // military power attached to title
        public List<CharacterAction> Actions { get; set; } // abilities provided to holder
        public override string ToString() {
            return Name;
        }
    }
}
