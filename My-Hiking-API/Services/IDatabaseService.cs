using System;
using System.Collections.Generic;
using System.Data.Common;
using MyHikingAPI.Models;

namespace MyHikingAPI.Services
{
    public interface IDatabaseService
    {
        List<Mountain> GetAllMountains();
        void InsertMountains(List<Mountain> mountains);
    }

}
