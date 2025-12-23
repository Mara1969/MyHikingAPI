using System.Data.Common;
using Microsoft.Extensions.Options;
using MyHikingAPI.Services;
using MyHikingAPI.Models.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using MyHikingAPI.Models;
using Dapper;
using System.Threading.Tasks;

namespace MyHikingAPI.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SqlDatabaseOptions _settings;

        public DatabaseService(IOptions<SqlDatabaseOptions> options)
        {
            _settings = options.Value; // contains the bound ConnectionString 
        }

        // Generic asynchronous method to retrieve data entries from the database based on the provided SQL query
        public async Task<List<T>> GetDataEntriesAsync<T>(string sql)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString)) // Create and open a new SQL connection. Using statement ensures proper disposal
            {
                var result = await connection.QueryAsync<T>(sql); // Execute the query asynchronously and map results to a collection of type T. Await the task to complete
                return result.AsList();
            }
        }

        // Asynchronous method to insert values into the mountain table in the database with no return value (return type is Task)
         public async Task InsertDataEntriesAsync<T>(string sql, IEnumerable<T> data)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                    
                await connection.ExecuteAsync(sql, data); // Execute the insert command asynchronously for the provided data collection. Await the task to complete
                
            }
        }

    }
}
