using System;
using System.Data.Common;

namespace MyHikingAPI.Services
{
    public interface IDatabaseService
    {
        DbConnection CreateConnection();
    }

}
