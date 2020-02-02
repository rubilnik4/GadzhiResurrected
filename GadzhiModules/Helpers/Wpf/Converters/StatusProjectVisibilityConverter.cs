using GadzhiCommon.Enums.FilesConvert;
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
    [ValueConversion(typeof(StatusProcessingProject), typeof(Visibility))]
    public class StatusProjectVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public StatusProjectVisibilityConverter()
        {           
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is StatusProcessingProject statusProcessingProject)
            {
                if (statusProcessingProject != StatusProcessingProject.NeedToLoadFiles &&
                    statusProcessingProject != StatusProcessingProject.Error &&
                    statusProcessingProject != StatusProcessingProject.End)
                {
                    flag = true;
                }
            }
            return (flag ? TrueValue : FalseValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
