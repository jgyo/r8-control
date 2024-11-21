using R8LocoCtrl.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace R8LocoCtrl.Tools
{
    [ValueConversion(typeof(ReverserPositions), typeof(String))]
    public class ReverserEnumToInitialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ReverserPositions)
            {
                var pos = (ReverserPositions)value;
                if(pos < ReverserPositions.Neutral || pos > ReverserPositions.Forward)
                    return string.Empty;

                return pos.ToString().Substring(0, 1);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
