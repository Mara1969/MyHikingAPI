using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MyHikingAPI.Models;

namespace MyHikingAPI.Services
{
    public interface IDatabaseService
    {
        Task<List<T>> GetDataEntriesAsync<T>(string sql);
        Task InsertDataEntriesAsync<T>(string sql, IEnumerable<T> data);
    }

}
