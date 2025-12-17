using System.Data.Common;
using Microsoft.Extensions.Options;
using MyHikingAPI.Services;
using MyHikingAPI.Models.Configuration;
using Microsoft.Data.SqlClient;

namespace MyHikingAPI.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SqlDatabaseOptions _settings;

        public DatabaseService(IOptions<SqlDatabaseOptions> options)
        {
            _settings = options.Value;
        }
        
        // Connect to the database 
        public DbConnection CreateConnection()
        {
            return new SqlConnection(_settings.ConnectionString); 
        }

    }
}
