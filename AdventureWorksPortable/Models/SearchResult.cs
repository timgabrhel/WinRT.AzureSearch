using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdventureWorksPortable.Models
{
    [JsonObject]
    public class SearchResult : BindableBase
    {
        private List<Product> _products;

        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("@odata.count")]
        public string Count { get; set; }

        [JsonProperty("@search.facets")]
        public SearchFacets Facets { get; set; }

        [JsonProperty("value")]
        public List<Product> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }
    }
}
