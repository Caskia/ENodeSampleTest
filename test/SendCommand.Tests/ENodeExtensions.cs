using System.IO;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using EQueue.Broker;
using EQueue.Configurations;
using EQueue.NameServer;
using System.Net;
using System.Collections.Generic;
using ECommon.Extensions;
using EQueue.Clients.Producers;

namespace SendCommand.Tests
{
    public static class ENodeExtensions
    {
        private static BrokerController _broker;
        private static CommandService _commandService;
        private static NameServerController _nameServerController;

        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Shutdown();
            _broker.Shutdown();
            _nameServerController.Shutdown();
            return enodeConfiguration;
        }

        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _nameServerController.Start();
            _broker.Start();
            _commandService.Start();
            return enodeConfiguration;
        }

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            var brokerStorePath = @"c:\equeue-store";

            if (Directory.Exists(brokerStorePath))
            {
                Directory.Delete(brokerStorePath, true);
            }

            configuration.RegisterEQueueComponents();
            var nameServerEndpoint = new IPEndPoint(IPAddress.Loopback, 10000);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };
            var nameServerSetting = new NameServerSetting()
            {
                BindingAddress = nameServerEndpoint
            };
            _nameServerController = new NameServerController(nameServerSetting);

            var brokerSetting = new BrokerSetting(false, brokerStorePath);
            brokerSetting.NameServerList = nameServerEndpoints;
            brokerSetting.BrokerInfo.ProducerAddress = new IPEndPoint(IPAddress.Loopback, 10001).ToAddress();
            brokerSetting.BrokerInfo.AdminAddress = new IPEndPoint(IPAddress.Loopback, 10003).ToAddress();
            _broker = BrokerController.Create(brokerSetting);

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