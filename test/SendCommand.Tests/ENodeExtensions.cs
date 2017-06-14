using ECommon.Socketing;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using EQueue.Clients.Producers;
using EQueue.Configurations;
using EQueue.NameServer;
using System.Collections.Generic;
using System.Net;

namespace SendCommand.Tests
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;

        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Shutdown();
            return enodeConfiguration;
        }

        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Start();
            return enodeConfiguration;
        }

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.RegisterEQueueComponents();
            var nameServerEndpoint = new IPEndPoint(SocketUtils.GetLocalIPV4(), 10000);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };
            var nameServerSetting = new NameServerSetting()
            {
                BindingAddress = nameServerEndpoint
            };

            var producerSetting = new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            };
            _commandService = new CommandService(null, producerSetting);
            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            return enodeConfiguration;
        }
    }
}