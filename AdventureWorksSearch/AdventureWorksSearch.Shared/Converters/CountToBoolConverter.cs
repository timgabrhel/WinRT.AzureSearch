using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace AdventureWorksSearch.Converters
{
    public class CountToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return false;

            var val = int.Parse(value.ToString());

            if (val > 0) return true;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
