using ECommon.Components;
using ENode.Commanding;
using Person.Commands;

namespace Person.CommandHandlers
{
    [Component]
    public class PersonCommandHandler : ICommandHandler<CreatePerson>
    {
        public void Handle(ICommandContext context, CreatePerson command)
        {
            var person = new Domain.Person(command.AggregateRootId, command.Age, command.Name, command.Remark, command.Sex);
            context.Add(person);
        }
    }
}