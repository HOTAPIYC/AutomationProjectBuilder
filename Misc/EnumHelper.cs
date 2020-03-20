using System;
using System.ComponentModel;
using System.Linq;

namespace AutomationProjectBuilder.Misc
{
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
