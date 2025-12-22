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

        // Generic method to retrieve data entries from the database based on the provided SQL query
        public async Task<List<T>> GetDataEntries<T>(string sql)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var result = await connection.QueryAsync<T>(sql);
                return result.AsList();
            }
        }

        // Method to insert values into the mountain table in the database
         public async Task InsertDataEntries<T>(string sql, IEnumerable<T> data)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                    
                await connection.ExecuteAsync(sql, data);
                
            }
        }

    }
}
