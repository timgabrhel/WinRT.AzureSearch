using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdventureWorksPortable.Models
{
    [JsonObject]
    public class Product : BindableBase
    {
        [JsonProperty("@search.score")]
        public float SearchScore { get; set; }

        [JsonProperty("productID")]
        public string ProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("productNumber")]
        public string ProductNumber { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("standardCost")]
        public float StandardCost { get; set; }

        [JsonProperty("listPrice")]
        public float ListPrice { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("weight")]
        public float? Weight { get; set; }

        [JsonProperty("DiscontinuedDate")]
        public DateTime? DiscontinuedDate { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
