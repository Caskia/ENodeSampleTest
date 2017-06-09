using System;
using System.Configuration;

namespace Person.Tests
{
    public class ConfigSettings
    {
        public static int BrokerAdminPort { get; set; } = 10003;

        public static int BrokerConsumerPort { get; set; } = 10002;

        public static int BrokerProducerPort { get; set; } = 10001;

        public static string EventStoreConnectionString { get; set; }

        public static int NameServerPort { get; set; } = 10000;

        public static void Initialize()
        {
            if (ConfigurationManager.ConnectionStrings["EventStore"] != null)
            {
                EventStoreConnectionString = ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString;
            }

            if (ConfigurationManager.AppSettings["NameServerPort"] != null)
            {
                NameServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["NameServerPort"]);
            }

            if (ConfigurationManager.AppSettings["BrokerProducerPort"] != null)
            {
                BrokerProducerPort = Convert.ToInt32(ConfigurationManager.AppSettings["BrokerProducerPort"]);
            }

            if (ConfigurationManager.AppSettings["BrokerConsumerPort"] != null)
            {
                BrokerConsumerPort = Convert.ToInt32(ConfigurationManager.AppSettings["BrokerConsumerPort"]);
            }

            if (ConfigurationManager.AppSettings["BrokerAdminPort"] != null)
            {
                BrokerAdminPort = Convert.ToInt32(ConfigurationManager.AppSettings["BrokerAdminPort"]);
            }
        }
    }
}