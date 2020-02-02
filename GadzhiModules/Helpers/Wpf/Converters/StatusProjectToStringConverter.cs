using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GadzhiModules.Helpers.Wpf.Converters
{
    [ValueConversion(typeof(StatusProcessingProject), typeof(string))]
    public class StatusProjectToStringConverter : IValueConverter
    {  
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertedValue = String.Empty;
            if (value is StatusProcessingProject statusProcessingProject)
            {
                convertedValue = StatusProcessingProjectConverter.
                                 ConvertStatusProcessingProjectToString(statusProcessingProject);
            }
            return convertedValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
