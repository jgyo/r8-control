using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace R8LocoCtrl.Tools
{
    public class BoolsMultiBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bools = new bool[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                bools[i] = values[i] is bool ? (bool)values[i] : false;
            }

            return bools;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
