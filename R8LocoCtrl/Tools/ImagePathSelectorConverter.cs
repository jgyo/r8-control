using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace R8LocoCtrl.Tools
{
    public class ImagePathSelectorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(parameter is string))
            {
                return string.Empty;
            }

            var paths = ((string)parameter).Split('/');
            var file = paths[0];

            // If the value is a bool, the first parameter is the false condition to return.
            if(value is bool && (bool)value)
            {
                // The second value is the true condition to return
                file = paths[1];
            }
            else if (value is Enum)
            {
                // If the value is an enum, the first parameter is a test value
                // to compare to it.
                var val = (int)value;
                var test = int.Parse(file);

                // if test value is -1, take the path specified by value
                if(test == -1)
                {
                    file = paths[val+1];
                }
                // val is a flag enum if it contains test, send image 2, otherwise send image 1
                else if((val & test) == 0)
                {
                    file = paths[1];
                }
                else
                {
                    file= paths[2];
                }
            }

            var uri = new Uri($"pack://siteoforigin:,,,/icons/{file}.png");
            return new BitmapImage(uri);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
