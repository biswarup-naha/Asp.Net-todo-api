using System;

namespace TodoApi.Models
{
    public class TodoDatabaseSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string TodoCollectionName { get; set; }
    }
}
