using ENode.Commanding;
using System;

namespace Person.Commands
{
    [Serializable]
    public class CreatePerson : Command<long>
    {
        public CreatePerson()
        {
        }

        public CreatePerson(long id) : base(id)
        {
        }

        public string Age { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public string Sex { get; set; }
    }
}