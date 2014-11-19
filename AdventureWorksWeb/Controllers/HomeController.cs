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

using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace AdventureWorksWeb.Controllers
{
    public class HomeController : Controller
    {
        private CatalogSearch _catalogSearch = new CatalogSearch(ConfigurationManager.AppSettings["SearchServiceName"],
                ConfigurationManager.AppSettings["SearchServiceApiKey"]);

        [HttpGet]
        public ActionResult Index()
        {
            //Check that the user entered in a valid service url and api-key
            if (_catalogSearch.errorMessage != null)
                ViewBag.errorMessage = "Please ensure that you have added your SearchServiceName and SearchServiceApiKey to the Web.config. Error: " + _catalogSearch.errorMessage;
            return View();
        }

        [HttpGet]
        public ActionResult Search(string q = "", string color = null, string category = null, double? priceFrom = null, double? priceTo = null, string sort = null)
        {
            dynamic result = null;

            // If blank search, assume they want to search everything
            if (string.IsNullOrWhiteSpace(q))
                q = "*";

            result = _catalogSearch.Search(q, sort, color, category, priceFrom, priceTo);
            ViewBag.searchString = q;
            ViewBag.color = color;
            ViewBag.category = category;
            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
            ViewBag.sort = sort;

            return View("Index", result);
        }

        [HttpGet]
        public ActionResult Suggest(string term)
        {
            var result = _catalogSearch.Suggest(term);

            var options = new List<string>();
            foreach (var option in result.value)
            {
                options.Add((string)option["@search.text"] + " (" + (string)option["productNumber"] + ")");
            }

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = options
            };
        }
    }
}
