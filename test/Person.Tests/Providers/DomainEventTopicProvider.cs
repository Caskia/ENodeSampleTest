using ECommon.Components;
using ENode.EQueue;
using ENode.Eventing;
using Person.Domain;

namespace Person.Tests.Providers
{
    [Component]
    public class DomainEventTopicProvider : AbstractTopicProvider<IDomainEvent>
    {
        public DomainEventTopicProvider()
        {
            RegisterTopic(
                Topics.PersonDomainEventTopic,
                typeof(PersonCreatedEvent)
                );
        }
    }
}