using ENode.Domain;
using System;

namespace Person.Domain
{
    [Serializable]
    public class Person : AggregateRoot<long>
    {
        private string _age;
        private string _name;
        private string _remark;
        private string _sex;

        public Person(long id, string age, string name, string remark, string sex) : base(id)
        {
            ApplyEvent(new PersonCreatedEvent(age, name, remark, sex));
        }

        private void Handle(PersonCreatedEvent evnt)
        {
            _age = evnt.Age;
            _name = evnt.Name;
            _remark = evnt.Remark;
            _sex = evnt.Sex;
        }
    }
}