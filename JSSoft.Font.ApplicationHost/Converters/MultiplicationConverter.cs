using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JSSoft.Font.ApplicationHost.Converters
{
    class MultiplicationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 1)
            {
                try
                {
                    var value = (double)values[0];
                    for (var i = 1; i < values.Length; i++)
                    {
                        value *= (double)values[i];
                    }
                    return value;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
