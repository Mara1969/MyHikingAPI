using System.Collections.Generic;
using MyHikingAPI.Services;
using MyHikingAPI.Models;
using System.Threading.Tasks;


namespace MyHikingAPI.Services
{
    public class MountainService :  IMountainService
    {
        private readonly IDatabaseService _databaseService; // Dependency on database service

        // Constructor with dependency injection. The IDatabaseService is passed into the constructor by the DI container 
        public MountainService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public List<Mountain> GetAllMountains()
        {
            return JsonReader.GetData<Mountain>("Data/mountains.json");
        }      

        // Asynchronous method to retrieve mountains data from the database
        public async Task<List<Mountain>> GetAllMountainsFromDbAsync()
        {
            const string sql = @"
            SELECT Id, Name, Height 
            FROM Mountains;";

            return await _databaseService.GetDataEntriesAsync<Mountain>(sql); // await the asynchronous task to complete and return the result (this ensures that the result is returned rather than the Task object itself)
        }  

        // Asynchronous method to insert mountains data into the database
        public async Task InsertMountainsDataToDbAsync(List<Mountain> mountains)
        {
            const string sql = @"
            INSERT INTO Mountains (Id, Name, Height) 
            VALUES (@Id, @Name, @Height);";

            await _databaseService.InsertDataEntriesAsync<Mountain>(sql, mountains);
        }       
    }
}
