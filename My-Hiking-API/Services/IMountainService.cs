using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyHikingAPI.Models;

namespace MyHikingAPI.Services;

public interface IMountainService
{
    List<Mountain> GetAllMountains();
    Task<List<Mountain>> GetAllMountainsFromDbAsync();
    Task InsertMountainsDataToDbAsync(List<Mountain> mountains);

}
