using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace R8LocoCtrl.Tools
{
    public class AddingMultiBindingConverter : IMultiValueConverter
    {
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0d;
            try
            {
                for (int i = 0; i < values.Length; i++)
                {
                    result += (double)values[i];
                }

                return result;
            }
            catch
            {

                return result;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
