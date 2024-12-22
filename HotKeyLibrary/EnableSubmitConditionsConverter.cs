using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HotKeyLibrary
{
    public class EnableSubmitConditionsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // In order to enable the Submit button,
            // HasChanges must be true, and HasErrors
            // must be false

            bool hasChanges = (bool)values[0];
            bool hasErrors = (bool)values[1];

            return hasChanges && !hasErrors;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
