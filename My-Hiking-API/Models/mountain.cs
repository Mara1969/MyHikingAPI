using System;

namespace MyHikingAPI.Models
{
    public class Mountain 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Height { get; set; } 
        //public string Location { get; set; }
    }
}
