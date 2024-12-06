using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trading212ToMoneyhubInterface.Models
{
    public class Trading212
    {
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        public override string ToString()
        {
            return $"Total: {Total}";
        }
    }
}
