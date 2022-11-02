using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Car
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string RegistrationPlate { get; set; }
        public string VehiclePassport { get; set; }

        [JsonIgnore]
        public ICollection<Policy> Policies { get; set; }

        public Car()
        {
            Policies = new List<Policy>();
        }
    }
}
