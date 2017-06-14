using ECommon.Components;
using ECommon.Configurations;
using ECommon.Extensions;
using ECommon.Logging;
using ECommon.Socketing;
using EQueue.Broker;
using EQueue.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using ECommonConfiguration = ECommon.Configurations.Configuration;

namespace Broker.Tests
{
    public class Bootstrap
    {
        private static BrokerController _broker;
        private static ECommonConfiguration _configuration;
        private static ILogger _logger;

        public Bootstrap()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitializeECommon();
            try
            {
                InitializeEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error("Initialize EQueue failed.", ex);
                throw;
            }
        }

        public void Start()
        {
            try
            {
                _broker.Start();
            }
            catch (Exception ex)
            {
                _logger.Error("Broker start failed.", ex);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                if (_broker != null)
                {
                    _broker.Shutdown();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Broker stop failed.", ex);
                throw;
            }
        }

        private void InitializeECommon()
        {
            _configuration = ECommonConfiguration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName);
            _logger.Info("ECommon initialized.");
        }

        private void InitializeEQueue()
        {
            _configuration.RegisterEQueueComponents();
            var storePath = ConfigurationManager.AppSettings["equeueStorePath"];
            var setting = new BrokerSetting(false, storePath);
            setting.NameServerList = new List<IPEndPoint> { new IPEndPoint(SocketUtils.GetLocalIPV4(), 10000) };
            setting.BrokerInfo.BrokerName = "GloodBroker";
            setting.BrokerInfo.GroupName = "GloodGroupName";
            setting.BrokerInfo.ProducerAddress = new IPEndPoint(SocketUtils.GetLocalIPV4(), 10001).ToAddress();
            setting.BrokerInfo.AdminAddress = new IPEndPoint(SocketUtils.GetLocalIPV4(), 10003).ToAddress();
            _broker = BrokerController.Create(setting);
            _logger.Info("EQueue initialized.");
        }
    }
}