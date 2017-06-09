using ECommon.Components;
using ECommon.Utilities;
using Person.Commands;
using Person.ReadModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Person.Tests.Person_Tests
{
    public class Person_Test : PersonTestBase
    {
        private PersonRepository _personRepository;

        public Person_Test()
        {
            _personRepository = ObjectContainer.Resolve<PersonRepository>();
        }

        [Fact(DisplayName = "Should_Create_User_Concurrent_Test")]
        public async Task Should_Create_User_Concurrent_Test()
        {
            Parallel.For(0, 100000, async (i) =>
            {
                var personCommand = new CreatePerson(i)
                {
                    Age = ObjectId.GenerateNewStringId(),
                    Name = ObjectId.GenerateNewStringId(),
                    Sex = ObjectId.GenerateNewStringId(),
                    Remark = ObjectId.GenerateNewStringId()
                };

                await _commandService.SendAsync(personCommand);
            });

            Thread.Sleep(50000);

            var persons = _personRepository.GetAll();
        }
    }
}