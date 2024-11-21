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
    public class DTMFCommandParametersConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(values[0] is string) || !(values[1] is bool))
                return null!;
            return new DTMFCommandParameter() { Tone = (DTMFToneValues)Enum.Parse(typeof(DTMFToneValues), (string)values[0]), IsButtonPressed = (bool)values[1] };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
