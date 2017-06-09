using ENode.Eventing;
using System;

namespace Person.Domain
{
    [Serializable]
    public class PersonCreatedEvent : DomainEvent<long>
    {
        public PersonCreatedEvent()
        {
        }

        public PersonCreatedEvent(string age, string name, string remark, string sex)
        {
            Age = age;
            Name = name;
            Remark = remark;
            Sex = sex;
        }

        public string Age { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public string Sex { get; set; }
    }
}