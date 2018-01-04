﻿namespace Jane.ENode
{
    public class ENodeConfiguration : IENodeConfiguration
    {
        public int BrokerAdminPort { get; set; } = 10003;

        public int BrokerConsumerPort { get; set; } = 10002;

        public int BrokerProducerPort { get; set; } = 10001;

        public string BrokerStorePath { get; set; }

        public string EventStoreConnectionString { get; set; }

        public int NameServerPort { get; set; } = 10000;
    }
}