//-----------------------------------------------------------------------
// <copyright file="ThrottleNotchConverter.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using R8LocoCtrl.Interface;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace R8LocoCtrl.Tools
{
    public class ThrottleNotchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return string.Empty;

            int? notch = values[0] as int?;
            BrakeStatusBits? brakeStat = values[1] as BrakeStatusBits?;

            if (notch == null || brakeStat == null) return string.Empty;


            if ((brakeStat & BrakeStatusBits.DynamicBrakeMode) == BrakeStatusBits.DynamicBrakeMode)
                return $"B{notch}";

            if (notch == 0)
                return "Idle";

            return $"N{notch}";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
