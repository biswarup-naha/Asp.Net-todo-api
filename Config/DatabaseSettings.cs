using System;

namespace TodoApi.Config
{
    public class DatabaseSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string TodoCollectionName { get; set; }
        public required string UserCollectionName { get; set; }
    }
}
