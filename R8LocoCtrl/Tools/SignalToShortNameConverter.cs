//-----------------------------------------------------------------------
// <copyright file="SignalToShortNameConverter.cs" company="Xcoder Software">
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
    [ValueConversion(typeof(SignalInstructions), typeof(String))]
    public class SignalToShortNameConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.GetType().Equals(typeof(SignalInstructions)))
                return string.Empty;

            var signalInstructions = (SignalInstructions)value;
            switch (signalInstructions)
            {
                case SignalInstructions.Clear:
                    return "Clear";

                case SignalInstructions.MediumClear:
                    return "Medium Clear";

                case SignalInstructions.LimitedClear:
                    return "Limited Clear";

                case SignalInstructions.SlowClear:
                    return "Slow Clear";

                case SignalInstructions.Approach:
                    return "Approach";

                case SignalInstructions.ApproachLimited:
                    return "Approach Limited";

                case SignalInstructions.ApproachMedium:
                    return "Approach Medium";

                case SignalInstructions.ApproachMediumDoubleYellow:
                    return "Approach Medium...";

                case SignalInstructions.ApproachSlow:
                    return "Approach Slow";

                case SignalInstructions.ApproachRestricted:
                    return "Approach Restricted";

                case SignalInstructions.ApproachThirty:
                    return "Approach Thirty";

                case SignalInstructions.ApproachDiverging:
                    return "Approach Diverging";

                case SignalInstructions.LimitedApproach:
                    return "Limited Approach";

                case SignalInstructions.MediumApproach:
                    return "Medium Approach";

                case SignalInstructions.SlowApproach:
                    return "Slow Approach";

                case SignalInstructions.MediumApproachMedium:
                    return "Medium Approach Med";

                case SignalInstructions.MediumApproachSlow:
                    return "Medium Approach Slow";

                case SignalInstructions.AdvanceApproach:
                    return "Advance Approach";

                case SignalInstructions.MediumAdvanceApproach:
                    return "Medium Adv Approach";

                case SignalInstructions.SlowApproachSlow:
                    return "Slow Approach Slow";

                case SignalInstructions.Restricting:
                    return "Restricting";

                case SignalInstructions.Stop:
                    return "Stop";

                case SignalInstructions.StopAndProceed:
                    return "Stop and Proceed";

                case SignalInstructions.DivergingClear:
                    return "Diverging Clear";

                case SignalInstructions.DivergingClear3rdAhead:
                    return "Diverging Clear 3rd Ahead";

                case SignalInstructions.DivergingApproachDiverging:
                    return "Diverging App Diverging";

                case SignalInstructions.DivergingApproachMedium:
                    return "Diverging App Medium";

                case SignalInstructions.DivergingApproach:
                    return "Diverging Approach";

                case SignalInstructions.DivergingApproach3rdAhead:
                    return "Diverging Approach 3rd Ahead";

                case SignalInstructions.DivergingRestricting:
                    return "Diverging Restricting";

                case SignalInstructions.DivergingApproachRestricting:
                    return "Diverging App Restricting";

                case SignalInstructions.DraggingEquipment:
                    return "Dragging Equipment";

                case SignalInstructions.Unknown:
                    return "Unknown";

                default:
                    return string.Empty;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
