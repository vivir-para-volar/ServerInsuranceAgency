using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class InsuranceEvent
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int InsurancePayment { get; set; }
        public string Description { get; set; }

        [ForeignKey("Policy")]
        public int PolicyID { get; set; }

        [ForeignKey("PolicyID")]
        [JsonIgnore]
        public Policy Policy { get; set; }
    }
}
