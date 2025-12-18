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
        using (var connection = new SqlConnection(_settings.ConnectionString)) 
        {    
        // Create a query that retrieves all books with an author name of "John Smith"    
         
        }
    }
}
