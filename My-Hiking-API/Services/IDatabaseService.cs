using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MyHikingAPI.Models;

namespace MyHikingAPI.Services
{
    public interface IDatabaseService
    {
        Task<List<T>> GetDataEntries<T>(string sql);
        Task InsertDataEntries<T>(string sql, IEnumerable<T> data);
    }

}
