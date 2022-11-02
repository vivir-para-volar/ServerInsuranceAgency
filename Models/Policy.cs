using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Policy
    {
        public int ID { get; set; }
        public string InsuranceType { get; set; }
        public int InsurancePremium { get; set; }
        public int InsuranceAmount { get; set; }
        public DateTime DateOfConclusion { get; set; }
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("Policyholder")]
        public int PolicyholderID { get; set; }

        [ForeignKey("PolicyholderID")]
        [JsonIgnore]
        public Policyholder Policyholder { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }

        [ForeignKey("CarID")]
        [JsonIgnore]
        public Car Car { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        [JsonIgnore]
        public Employee Employee { get; set; }

        public ICollection<PersonAllowedToDrive> PersonsAllowedToDrive { get; set; }

        [JsonIgnore]
        public ICollection<InsuranceEvent> InsuranceEvents { get; set;}

        public Policy()
        {
            PersonsAllowedToDrive = new List<PersonAllowedToDrive>();
            InsuranceEvents = new List<InsuranceEvent>();
        }
    }
}