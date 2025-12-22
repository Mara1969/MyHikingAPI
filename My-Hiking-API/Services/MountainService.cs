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
        public async Task<List<Mountain>> GetAllMountainsFromDb()
        {
            const string sql = @"
            SELECT Id, Name, Height 
            FROM Mountains;";

            return await _databaseService.GetDataEntries<Mountain>(sql);
        }  
        public async Task InsertMountainsDataToDb(List<Mountain> mountains)
        {
            const string sql = @"
            INSERT INTO Mountains (Id, Name, Height) 
            VALUES (@Id, @Name, @Height);";

            await _databaseService.InsertDataEntries<Mountain>(sql, mountains);
        }       
    }
}
