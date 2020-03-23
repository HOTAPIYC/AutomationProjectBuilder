using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace AutomationProjectBuilder.Misc
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

	public class EnumConverterToList : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Enum.GetValues(value.GetType()).Cast<Enum>().Select(e => new EnumValueDescription() { Value = e, Description = EnumHelper.Description(e) });
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public static class EnumHelper
	{
		public static string Description(this Enum e)
		{
			return (e.GetType()
					 .GetField(e.ToString())
					 .GetCustomAttributes(typeof(DescriptionAttribute), false)
					 .FirstOrDefault() as DescriptionAttribute)?.Description ?? e.ToString();
		}
	}
}
