using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace DXApplication2.Converters
{
	public class AspectRatioConverter : IValueConverter
	{
		// Set your desired aspect ratio here (e.g., 4:3 = 0.75, 16:9 = 0.5625)
		private const double AspectRatio = 0.75;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double width)
			{
				return width * AspectRatio;
			}
			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
