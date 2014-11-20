using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdventureWorksPortable.Models
{
    [JsonObject]
    public class SearchFacets : BindableBase
    {
        [JsonProperty("color@odata.type")]
        public string ColorType { get; set; }
        
        [JsonProperty("color")]
        public Color[] Colors { get; set; }

        [JsonProperty("categoryName@odata.type")]
        public string CategoryType { get; set; }

        [JsonProperty("categoryName")]
        public Category[] Categories { get; set; }
        
        [JsonProperty("listPrice@odata.type")]
        public string ListPriceType { get; set; }
        
        [JsonProperty("listPrice")]
        public ListPrice[] ListPrices { get; set; }

        [JsonObject]
        public class Color
        {
            public int Count { get; set; }
            public string Value { get; set; }
        }

        [JsonObject]
        public class Category
        {
            public int Count { get; set; }
            public string Value { get; set; }
        }

        [JsonObject]
        public class ListPrice
        {
            public int Count { get; set; }
            public float? To { get; set; }
            public float? From { get; set; }
        }

    }
}
