using System;
using System.Collections.Generic;
using MyHikingAPI.Models;

namespace My_Hiking_API.Services;

public interface IMountainService
{
    List<Mountain> GetAllMountains();

}
