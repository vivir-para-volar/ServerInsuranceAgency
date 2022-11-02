using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class PersonAllowedToDrive
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string DrivingLicence { get; set; }

        [JsonIgnore]
        public ICollection<Policy> Policies { get; set; }

        public PersonAllowedToDrive()
        {
            Policies = new List<Policy>();
        }
    }
}
