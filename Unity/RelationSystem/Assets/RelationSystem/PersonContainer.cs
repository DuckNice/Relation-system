using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class PersCont:MaskCont
    {
        public Dictionary<string, Person> people = new Dictionary<string, Person>();
        protected List<string> peopleNames = new List<string>();

        public void CreateNewPerson(string personName, Person person)
        {
            if ((person.GetLinks(TypeMask.selfPerc))[0] != null)
            {
                personName = personName.ToLower();
                people.Add(personName, person);
                peopleNames.Add(personName);
            }
            else
            {
                debug.Write("Error: person '" + personName + "' to be inserted has no selfPersonality Link. Aborting insert.");
                
				return;
            }
        }

        public Person GetPerson(string name)
        {
            name = name.ToLower();

            if (people.ContainsKey(name))
            {
                return people[name].Copy();
            }
            else
            {
                return null;
            }
        }
    }
}