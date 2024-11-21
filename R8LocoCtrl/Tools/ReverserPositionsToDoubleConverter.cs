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
    public class ReverserPositionsToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is ReverserPositions))
                return 1d;

            var position = (ReverserPositions)value;
            switch (position)
            {
                case ReverserPositions.Neutral:
                    return 1d;
                case ReverserPositions.Reverse:
                    return 0d;
                case ReverserPositions.Forward:
                    return 2d;
                default:
                    return 1d;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is double)) return ReverserPositions.Neutral;

            var position = (double)value;
            if(AlmostEqual (position, 0d))
                return ReverserPositions.Reverse;

            if(AlmostEqual (position, 2d))
                return ReverserPositions.Forward;

            return ReverserPositions.Neutral;
        }

        public static bool AlmostEqual(double a, double b, double tolerance = 0.01)
        {
            return Math.Abs(a - b) <= tolerance;
        }
    }
}
