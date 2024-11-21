//-----------------------------------------------------------------------
// <copyright file="LocoStatusToAlerterStatusConverter.cs" company="Xcoder Software">
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
    public class LocoStatusToAlerterStatusConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is LocoStatusBits))
                return AlerterStatus.OK;

            switch ((LocoStatusBits)value)
            {
                case LocoStatusBits.AlerterPreWarning:
                    return AlerterStatus.PreWarning;
                case LocoStatusBits.AlerterWarning:
                    return AlerterStatus.Warning;
                default:
                    return AlerterStatus.OK;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
