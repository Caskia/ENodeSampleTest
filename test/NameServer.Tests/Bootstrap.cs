using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ECommon.Socketing;
using EQueue.Configurations;
using EQueue.NameServer;
using System;
using System.Net;
using ECommonConfiguration = ECommon.Configurations.Configuration;

namespace NameServer.Tests
{
    public class Bootstrap
    {
        private ECommonConfiguration _configuration;
        private ILogger _logger;
        private NameServerController _nameServer;

        public Bootstrap()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitializeECommon();
            try
            {
                InitializeNameServer();
            }
            catch (Exception ex)
            {
                _logger.Error("Initialize NameServer failed.", ex);
                throw;
            }
        }

        public void Start()
        {
            try
            {
                _nameServer.Start();
            }
            catch (Exception ex)
            {
                _logger.Error("NameServer start failed.", ex);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                if (_nameServer != null)
                {
                    _nameServer.Shutdown();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("NameServer stop failed.", ex);
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

        private void InitializeNameServer()
        {
            _configuration.RegisterEQueueComponents();
            var setting = new NameServerSetting()
            {
                BindingAddress = new IPEndPoint(SocketUtils.GetLocalIPV4(), 10000)
            };
            _nameServer = new NameServerController(setting);
            _logger.Info("NameServer initialized.");
        }
    }
}