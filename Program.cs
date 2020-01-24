using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace water_blood
{
    internal class Program
    {
        private static Random _random;

        static void Main(string[] args)
        {
            _random = new Random(1);
            string familiesFile = ".\\families.json";
            string seedFile = ".\\seeds.json";
            if (!int.TryParse(args[0], out int timespan))
            {
                Console.WriteLine($"Invalid parameter {args[3]} - must be int");
                return;
            }
            List<Person> seeds = GetSeedsFromFile(seedFile);
            Genealogy genealogy = new Genealogy(seeds);
            int month = 0;
            while (month < timespan * 12)
            {
                genealogy.Tick(month);
                month++;
            }

            genealogy.PrintHistory();
            genealogy.PrintPeople();
        }

        private static List<Person> GetSeedsFromFile(string filepath)
        {
            string fileContents = File.ReadAllText(filepath);
            List<Person> people = JsonConvert.DeserializeObject<List<Person>>(fileContents);
            foreach (Person p in people)
            {
                p.Init(new List<Person>()); // seeds don't have parents
            }
            return people;
        }

        public string GetDateStringFromMonth(int month)
        {
            const int yearOffset = 302;
            int years = yearOffset + month / 12;
            month %= 12;
            string monthName = "";

            switch (month)
            {
                case 0:
                {
                    monthName = "January";
                    break;
                }
                case 1: {
                    monthName = "February";
                    break;
                }
                case 2: {
                    monthName = "March";
                    break;
                }
                case 3: {
                    monthName = "April";
                    break;
                }
                case 4: {
                    monthName = "May";
                    break;
                }
                case 5: {
                    monthName = "June";
                    break;
                }
                case 6: {
                    monthName = "July";
                    break;
                }
                case 7: {
                    monthName = "August";
                    break;
                }
                case 8: {
                    monthName = "September";
                    break;
                }
                case 9: {
                    monthName = "October";
                    break;
                }
                case 10: {
                    monthName = "November";
                    break;
                }
                case 11: {
                    monthName = "December";
                    break;
                }
            }

            return $"{monthName} {years}";
        }

        public static float GetRandFloat(float min = 0f, float max = 1f)
        {
            float next = (float) _random.NextDouble();
            return min + (next * (max - min));
        }

        public static IEnumerable<T> ShuffleEnumerable<T>(IEnumerable<T> inputList)
        {
            return inputList.OrderBy(a => Guid.NewGuid());
        }
    }
}
