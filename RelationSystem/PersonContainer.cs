using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class PersCont:MaskCont
    {
        protected Dictionary<string, Person> people = new Dictionary<string, Person>();
        protected List<string> peopleNames = new List<string>();

        public void CreateNewPerson(string personName, Person person)
        {
            if ((person.GetLinks(TypeMask.selfPerc))[0] != null)
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