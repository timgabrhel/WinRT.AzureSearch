using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AdventureWorksPortable;
using AdventureWorksPortable.Models;
using AdventureWorksWeb;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace AdventureWorksSearch.ViewModels
{
    public class MainViewModel : BindableBase
    {
        readonly CatalogSearch _catalogSearch = new CatalogSearch("timsadvworks", "2B007A37C1063B37B5BA073323C9C916");

        private string _searchString;
        private SearchResult _searchResult;
        private SearchFacets.Color _colorFilter;
        private string _sortProperty;
        private SearchFacets.Category _categoryFilter;
        private SearchFacets.ListPrice _listPriceFilter;
        private bool _isDataLoaded;

        public string SearchString
        {
            get { return _searchString; }
            set { SetProperty(ref _searchString, value); }
        }

        public SearchFacets.Color ColorFilter
        {
            get { return _colorFilter; }
            set { SetProperty(ref _colorFilter, value); }
        }

        public SearchFacets.Category CategoryFilter
        {
            get { return _categoryFilter; }
            set { SetProperty(ref _categoryFilter, value); }
        }

        public SearchFacets.ListPrice ListPriceFilter
        {
            get { return _listPriceFilter; }
            set { SetProperty(ref _listPriceFilter, value); }
        }

        public string SortProperty
        {
            get { return _sortProperty; }
            set { SetProperty(ref _sortProperty, value); }
        }

        public SearchResult SearchResult
        {
            get { return _searchResult; }
            set { SetProperty(ref _searchResult, value); }
        }

        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { SetProperty(ref _isDataLoaded, value); }
        }


        public ICommand SearchCommand { get; private set; }

        public ICommand SortCommand { get; private set; }

        public ICommand FilterCommand { get; private set; }

        public ICommand ClearFiltersCommand { get; private set; }

        public MainViewModel()
        {
            SearchCommand = new RelayCommand(Search);
            SortCommand = new RelayCommand(Sort);
            FilterCommand = new RelayCommand(Filter);
            ClearFiltersCommand = new RelayCommand(ExecuteObject);

            // initialize a default result. 
            SearchResult = new SearchResult() {Count = "0"};
            IsDataLoaded = true;
        }

        private void ExecuteObject()
        {
            ColorFilter = null;
            CategoryFilter = null;
            ListPriceFilter = null;
            Search();
        }

        private void Filter(object o)
        {
            // if there was no input just return. This shouldn't happen as the parameter should be the textbox.Text property
            if (o == null) return;

            // cast the object to a string
            var filterType = (string)o;

            // if this is empty, just return
            if (string.IsNullOrWhiteSpace(filterType)) return;

            // make sure we have products loaded
            if (SearchResult == null || SearchResult.Products == null) return;

            switch (filterType)
            {
                case "colors":
                    break;
                case "categories":
                    break;
                case "prices":
                    break;
            }

            Search();
        }

        private void Sort(object o)
        {
            // if there was no input just return. This shouldn't happen as the parameter should be the textbox.Text property
            if (o == null) return;

            // cast the object to a string
            var sortType = (string)o;

            // if this is empty, just return
            if (string.IsNullOrWhiteSpace(sortType)) return;

            SortProperty = o.ToString();

            Search();
        }
        
        private async void Search()
        {   
            // if the string is empty, set it to the search wildcard, *
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                SearchString = "*";
            }

            IsDataLoaded = false;

            // run the search request inside an async task for a responsive ui
            await Task.Run(delegate
            {
                var result = _catalogSearch.Search(SearchString,
                                                    SortProperty,
                                                    ColorFilter == null ? "" : ColorFilter.Value,
                                                    CategoryFilter == null ? "" : CategoryFilter.Value,
                                                    ListPriceFilter == null ? null : ListPriceFilter.From,
                                                    ListPriceFilter == null ? null : ListPriceFilter.To);

                // when we get the result, update our view model property on the ui thread so property changed handlers can fire
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                delegate
                {
                    SearchResult = result;
                });
            });

            IsDataLoaded = true;
        }
    }
}
