using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRelationSystem
{
    public class PersonContainer:MaskContainer
    {
        protected Dictionary<string, Person> people = new Dictionary<string, Person>();
        protected List<string> peopleNames = new List<string>();

        public void CreateNewPerson(string personName, Person person)
        {
            if ((person.GetLinks(typeMask.selfPerception))[0] != null)
            {
                people.Add(personName, person);
                peopleNames.Add(personName);
            }
            else
            {
                Console.WriteLine("Error: person '" + personName + "' to be inserted has no selfPernality Link. Aborting insert.");
                return;
            }
        }

        public Person GetPerson(string name)
        {
            return people[name];
        }
    }
}