using ECommon.Components;
using ECommon.IO;
using ENode.Infrastructure;
using Person.Domain;
using System.Threading.Tasks;

namespace Person.ReadModel
{
    [Component]
    public class PersonDenormalizer : IMessageHandler<PersonCreatedEvent>
    {
        private PersonRepository _personRepository;

        public PersonDenormalizer(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Task<AsyncTaskResult> HandleAsync(PersonCreatedEvent message)
        {
            _personRepository.Insert(new Person()
            {
                Id = message.AggregateRootId,
                Age = message.Age,
                Name = message.Name,
                Remark = message.Remark,
                Sex = message.Sex
            });

            return Task.FromResult(AsyncTaskResult.Success);
        }
    }
}