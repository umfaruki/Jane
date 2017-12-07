﻿using Jane.Configurations;
using MongoDB.Driver;

namespace Jane.MongoDb
{
    public class MongoDbProvider : IMongoDbProvider
    {
        private readonly IMongoDbConfiguration _configuration;
        private MongoClient _mongoClient;

        public MongoDbProvider(IMongoDbConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMongoClient GetClient()
        {
            if (_mongoClient == null)
            {
                _mongoClient = new MongoClient(_configuration.ConnectionString + "?maxPoolSize=500");
            }
            return _mongoClient;
        }

        public IMongoDatabase GetDatabase()
        {
            return GetClient().GetDatabase(_configuration.DatabaseName);
        }
    }
}