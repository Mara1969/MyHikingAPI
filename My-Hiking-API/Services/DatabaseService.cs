using System.Data.Common;
using Microsoft.Extensions.Options;
using MyHikingAPI.Services;
using MyHikingAPI.Models.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using MyHikingAPI.Models;

namespace MyHikingAPI.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SqlDatabaseOptions _settings;

        public DatabaseService(IOptions<SqlDatabaseOptions> options)
        {
            _settings = options.Value;
        }
        
        public List<Mountain> GetMountainNames()
        {
            var mountainNames = new List<Mountain>();

            // Connect to the database 

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                // Create queries to retrieve mountain data from the database 
            }

            return mountainNames;
         
        }
    }
}
