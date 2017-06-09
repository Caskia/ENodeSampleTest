using ECommon.Components;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Person.ReadModel
{
    [Component]
    public class PersonRepository
    {
        private ConcurrentDictionary<long, Person> persons;

        public PersonRepository()
        {
            persons = new ConcurrentDictionary<long, Person>();
        }

        public IList<Person> GetAll()
        {
            return persons.Select(p => p.Value).ToList();
        }

        public void Insert(Person person)
        {
            if (!persons.ContainsKey(person.Id))
            {
                persons.TryAdd(person.Id, person);
            }
        }
    }
}