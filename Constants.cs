using System;
using System.Collections.Generic;
using System.Text;

namespace water_blood
{
    public static class Constants
    {
        public static float CPM_GET_GOAL = 0.08f; // chance per month for a satisfied person to get a new goal
        public static int MALE_AGE_PANIC = 25; // age when men start desiring heirs
        public static int MALE_HEIR_DESIRE = 30; // the lower the value, the more the person wants an heir
        public static int NURTURING_DESIRE = 5; // the amount a nurturing person wants a child
        public static int NURTURING_TARGET = 3; // a nurturing person wants at least this many children
        public static int ATTR_SAT_THRESHOLD = 50; // when all attributes are above this level, a person is satisfied
        public static int FAMILY_ETA = 1; // how much the family's fortunes can affect a person per month
        public static float SALARY_SPOUSE_SHARE = 0.3f;
        public static float SALARY_LOVER_SHARE = 0.1f;
        public static int AVERAGE_MOURNING = 24;
    }
}
