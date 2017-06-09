using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;
using Person.Commands;

namespace Person.Tests.Providers
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public CommandTopicProvider()
        {
            RegisterTopic(
                Topics.PersonCommandTopic,
                typeof(CreatePerson)
                );
        }
    }
}