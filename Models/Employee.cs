using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }

        [JsonIgnore]
        public ICollection<Policy> Policies { get; set; }

        public Employee()
        {
            Policies = new List<Policy>();
        }
    }
}
