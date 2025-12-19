using System.Data.Common;
using Microsoft.Extensions.Options;
using MyHikingAPI.Services;
using MyHikingAPI.Models.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using MyHikingAPI.Models;
using Dapper;

namespace MyHikingAPI.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SqlDatabaseOptions _settings;

        public DatabaseService(IOptions<SqlDatabaseOptions> options)
        {
            _settings = options.Value; // contains the bound ConnectionString 
        }

        // Method to insert values into the mountain table in the database
         public void InsertMountains(List<Mountain> mountains)
        {
            // using Dapper for simplified data insertion
            const string sql = @"
                INSERT INTO Mountains (Id, Name, Height)
                VALUES (@Id, @Name, @Height);"; // act as plaeholders for the actual values

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                    
                connection.Execute(sql, mountains);
                
            }
        }

        // Method to read from the database
        public List<Mountain> GetAllMountains()
        {
            // using Dapper to simplify data retrieval
            const string sql = @"
                SELECT Id, Name, Height
                FROM Mountains;
            ";

            // Connect to the database 

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                return connection.Query<Mountain>(sql).AsList();
            }
         
        }
    }
}
