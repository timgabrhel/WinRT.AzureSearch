//Copyright 2014 Microsoft

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Globalization;
using System.Net.Http;
using AdventureWorksPortable.Models;
using CatalogCommon;

namespace AdventureWorksWeb
{
    public class CatalogSearch
    {
        private readonly Uri _serviceUri;
        private HttpClient _httpClient;
        public string errorMessage;

        public CatalogSearch(string searchServiceName, string searchServiceApiKey)
        {
            try
            {
                _serviceUri = new Uri("https://" + searchServiceName + ".search.windows.net");
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("api-key", searchServiceApiKey);
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }

        public SearchResult Search(string searchText, string sort, string color, string category, double? priceFrom, double? priceTo)
        {
            string search = "&search=" + Uri.EscapeDataString(searchText);
            string facets = "&facet=color&facet=categoryName&facet=listPrice,values:10|25|100|500|1000|2500";
            string paging = "&$top=10";
            string filter = BuildFilter(color, category, priceFrom, priceTo);
            string orderby = BuildSort(sort);

            Uri uri = new Uri(_serviceUri, "/indexes/catalog/docs?$count=true" + search + facets + paging + filter + orderby);
            HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(_httpClient, HttpMethod.Get, uri);
            AzureSearchHelper.EnsureSuccessfulSearchResponse(response);

            return AzureSearchHelper.DeserializeJson<SearchResult>(response.Content.ReadAsStringAsync().Result);
        }
        
        public dynamic Suggest(string searchText)
        {
            // we still need a default filter to exclude discontinued products from the suggestions
            Uri uri = new Uri(_serviceUri, "/indexes/catalog/docs/suggest?$filter=discontinuedDate eq null&$select=productNumber&search=" + Uri.EscapeDataString(searchText));
            HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(_httpClient, HttpMethod.Get, uri);
            AzureSearchHelper.EnsureSuccessfulSearchResponse(response);

            return AzureSearchHelper.DeserializeJson<dynamic>(response.Content.ReadAsStringAsync().Result);
        }

        private string BuildSort(string sort)
        {
            if (string.IsNullOrWhiteSpace(sort))
            {
                return string.Empty;
            }

            // could also add asc/desc if we want to allow both sorting directions
            if (sort == "listPrice" || sort == "color")
            {
                return "&$orderby=" + sort;
            }

            throw new Exception("Invalid sort order");
        }

        private string BuildFilter(string color, string category, double? priceFrom, double? priceTo)
        {
            // carefully escape and combine input for filters, injection attacks that are typical in SQL
            // also apply here. No "DROP TABLE" risk, but a well injected "or" can cause unwanted disclosure

            string filter = "&$filter=discontinuedDate eq null";

            if (!string.IsNullOrWhiteSpace(color))
            {
                filter += " and color eq '" + EscapeODataString(color) + "'";
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                filter += " and categoryName eq '" + EscapeODataString(category) + "'";
            }

            if (priceFrom.HasValue)
            {
                filter += " and listPrice ge " + priceFrom.Value.ToString(CultureInfo.InvariantCulture);
            }

            if (priceTo.HasValue && priceTo > 0)
            {
                filter += " and listPrice le " + priceTo.Value.ToString(CultureInfo.InvariantCulture);
            }

            return filter;
        }

        private string EscapeODataString(string s)
        {
            return Uri.EscapeDataString(s).Replace("\'", "\'\'");
        }
    }
}
