using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Commanding;
using ENode.Configurations;
using System;
using System.Reflection;
using ECommonConfiguration = ECommon.Configurations.Configuration;

namespace Person.Tests
{
    public abstract class PersonTestBase
    {
        protected static ICommandService _commandService;

        protected static ILogger _logger;

        private static ECommonConfiguration _ecommonConfiguration;

        private static ENodeConfiguration _enodeConfiguration;

        public PersonTestBase()
        {
            Initialize();
        }

        protected static void Initialize()
        {
            if (_enodeConfiguration == null)
            {
                InitializeECommon();
                try
                {
                    InitializeENode();
                }
                catch (Exception ex)
                {
                    _logger.Error("Initialize ENode failed.", ex);
                    throw;
                }
                _commandService = ObjectContainer.Resolve<ICommandService>();
            }
        }

        protected CommandResult ExecuteCommand(ICommand command)
        {
            return _commandService.Execute(command, CommandReturnType.EventHandled, 10000);
        }

        private static void InitializeECommon()
        {
            _ecommonConfiguration = ECommonConfiguration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(PersonTestBase).Name);
            _logger.Info("ECommon initialized.");
        }

        private static void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Person.CommandHandlers"),
                Assembly.Load("Person.Commands"),
                Assembly.Load("Person.Domain"),
                Assembly.Load("Person.ReadModel"),
                Assembly.GetExecutingAssembly()
            };

            var setting = new ConfigurationSetting(ConfigSettings.EventStoreConnectionString);

            _enodeConfiguration = _ecommonConfiguration
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue();
        }
    }
}