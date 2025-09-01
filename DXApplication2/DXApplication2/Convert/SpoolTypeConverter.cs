using LiningCheckRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXApplication2.Converters
{
	public enum SpoolType
	{
		直管 = 0,
		直管枝付= 1,
		曲げ = 2,
		手書き =3 ,
		カメラ = 4,
	};

	public class IntEnumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return 0;
			var myEnum = (SpoolType)Enum.Parse(typeof(SpoolType), value.ToString());
			return (int)myEnum;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}


	public class ImageEnumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;
			var myEnum = (SpoolType)Enum.Parse(typeof(SpoolType), value.ToString());
			switch (myEnum)
			{
				case SpoolType.直管:
					//return "/Image/Type1.PNG";
					return new Uri("type1.png", UriKind.RelativeOrAbsolute);
				case SpoolType.直管枝付:
					return new Uri("type2.png", UriKind.RelativeOrAbsolute);
				case SpoolType.曲げ:
					return new Uri("type3.png", UriKind.RelativeOrAbsolute);
				default:
					return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class IntToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;
			if (value is not int myEnum)
				return null;
			
			switch (myEnum)
			{
				case 0:
					//return "/Image/Type1.PNG";
					return new Uri("type1.svg", UriKind.RelativeOrAbsolute);
				case 1:
					return new Uri("type2.svg", UriKind.RelativeOrAbsolute);
				case 2:
					return new Uri("type3.svg", UriKind.RelativeOrAbsolute);
				default:
					return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

		public class SpoolToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;
			if (value is not  LiningSpool spool)
				return null;
			
			switch (spool.SpoolType)
			{
				case 0:
					//return "/Image/Type1.PNG";
					return new Uri("type1.svg", UriKind.RelativeOrAbsolute);
				case 1:
					return new Uri("type2.svg", UriKind.RelativeOrAbsolute);
				case 2:
					return new Uri("type3.svg", UriKind.RelativeOrAbsolute);	
				case 3:
					return new Uri( spool.ImagePath, UriKind.Absolute);
				case 4:
					return new Uri( spool.ImagePath, UriKind.Absolute);
				default:
					return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class SpoolToChartDataConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;
			if (value is not  LiningSpool spool)
				return null;

            List<ChartData> list = new List<ChartData>();
            double val = 0;
            if (Double.TryParse(spool.A1, out val)) list.Add(new ChartData { Name = "A", Value = val });
            if (Double.TryParse(spool.A2, out val)) list.Add(new ChartData { Name = "A", Value = val });
            if (Double.TryParse(spool.A3, out val)) list.Add(new ChartData { Name = "A", Value = val });
            if (Double.TryParse(spool.A4, out val)) list.Add(new ChartData { Name = "A", Value = val });
            if (Double.TryParse(spool.B1, out val)) list.Add(new ChartData { Name = "B", Value = val });
            if (Double.TryParse(spool.B2, out val)) list.Add(new ChartData { Name = "B", Value = val });
            if (Double.TryParse(spool.B3, out val)) list.Add(new ChartData { Name = "B", Value = val });
            if (Double.TryParse(spool.B4, out val)) list.Add(new ChartData { Name = "B", Value = val });
            if (Double.TryParse(spool.C1, out val)) list.Add(new ChartData { Name = "C", Value = val });
            if (Double.TryParse(spool.C2, out val)) list.Add(new ChartData { Name = "C", Value = val });
            if (Double.TryParse(spool.C3, out val)) list.Add(new ChartData { Name = "C", Value = val });
            if (Double.TryParse(spool.C4, out val)) list.Add(new ChartData { Name = "C", Value = val });
            if (Double.TryParse(spool.D1, out val)) list.Add(new ChartData { Name = "D", Value = val });
            if (Double.TryParse(spool.D2, out val)) list.Add(new ChartData { Name = "D", Value = val });
            if (Double.TryParse(spool.D3, out val)) list.Add(new ChartData { Name = "D", Value = val });
            if (Double.TryParse(spool.D4, out val)) list.Add(new ChartData { Name = "D", Value = val });
            if (Double.TryParse(spool.E1, out val)) list.Add(new ChartData { Name = "E", Value = val });
            if (Double.TryParse(spool.E2, out val)) list.Add(new ChartData { Name = "E", Value = val });
            if (Double.TryParse(spool.E3, out val)) list.Add(new ChartData { Name = "E", Value = val });
            if (Double.TryParse(spool.E4, out val)) list.Add(new ChartData { Name = "E", Value = val });
            if (Double.TryParse(spool.F1, out val)) list.Add(new ChartData { Name = "F", Value = val });
            if (Double.TryParse(spool.F2, out val)) list.Add(new ChartData { Name = "F", Value = val });
            if (Double.TryParse(spool.F3, out val)) list.Add(new ChartData { Name = "F", Value = val });
            if (Double.TryParse(spool.F4, out val)) list.Add(new ChartData { Name = "F", Value = val });

			if (list.Count == 0) return list;
            var minvalue= list.GroupBy(x=>x.Name).Select(x=>new ChartData { Name = x.Key, Value = x.Min(x1 => x1.Value) }).ToList();

            return minvalue;
        }

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}



}
