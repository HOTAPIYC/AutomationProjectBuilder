using AutomationProjectBuilder.Misc;
using System;
using System.Windows.Data;

namespace AutomationProjectBuilder.Converter
{
	public class EnumConverterToString : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return EnumHelper.Description((Enum)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
