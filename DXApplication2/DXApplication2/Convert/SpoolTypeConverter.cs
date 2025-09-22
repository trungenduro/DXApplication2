using LiningCheckRecord;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
		カメラ = 3,
		手書き =4 ,
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
					return new Uri("type1a.png", UriKind.RelativeOrAbsolute);
				case SpoolType.直管枝付:
					return new Uri("type2a.png", UriKind.RelativeOrAbsolute);
				case SpoolType.曲げ:
					return new Uri("type3a.png", UriKind.RelativeOrAbsolute);
				case SpoolType.カメラ:
					return new Uri("camera.svg", UriKind.RelativeOrAbsolute);
				case SpoolType.手書き:
					return new Uri("freehand.svg", UriKind.RelativeOrAbsolute);
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
					return new Uri("type1a.svg", UriKind.RelativeOrAbsolute);
				case 1:
					return new Uri("type2a.svg", UriKind.RelativeOrAbsolute);
				case 2:
					return new Uri("type3a.svg", UriKind.RelativeOrAbsolute);
				case 3:
					return new Uri("camera.svg", UriKind.RelativeOrAbsolute);
				case 4:
					return new Uri("photo.svg", UriKind.RelativeOrAbsolute);

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
				case 3 or 4 :
					return new Uri(spool.ImagePath, UriKind.RelativeOrAbsolute);
				
				default:
					return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class SheetToIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;
			if (value is not  ExcelSheet sheet)
				return null;
			
			switch (sheet.CheckResult)
			{
				case "合格":
				
					return new Uri("ok.svg", UriKind.RelativeOrAbsolute);
			
				default:
					return new Uri("error.svg", UriKind.RelativeOrAbsolute);				
			
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class SpoolToIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;
			if (value is not LiningSpool spool)
				return null;

			var data = spool.GetData();
			if(data.Count==0) return new Uri("error.svg", UriKind.RelativeOrAbsolute);
			var minVal= data.Select(x => x.Value).Min();
			if (spool.Sheet == null) return new Uri("error.svg", UriKind.RelativeOrAbsolute);
			if(minVal<spool.Sheet.ThickNess)
				return new Uri("error.svg", UriKind.RelativeOrAbsolute);
			if(string.IsNullOrEmpty( spool.CheckShape))
				return new Uri("error.svg", UriKind.RelativeOrAbsolute);

			return new Uri("ok.svg", UriKind.RelativeOrAbsolute);
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

          
            return spool.GetData();
        }

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	

	public class SheetNumberConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (values == null) return null;

			return $"{values[0]}/{values[1]}";
			          
        }

		

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class ListIndexConverter : IMultiValueConverter
	{
		public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;

			if (value[0] is not List<string> list) return null;
			if ( list.Count <1) return null;
			if (value[1] is not int i) return list[0];

			return list[i];


		
        }

		public object ConvertBack(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}



	public static class GetChartData
	{
		public static List<ChartData> GetData(this LiningSpool spool)
		{
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
			var minvalue = list.GroupBy(x => x.Name).Select(x => new ChartData { Name = x.Key, Value = x.Min(x1 => x1.Value) }).ToList();

			return minvalue;
		}
	}


	public class ListToTokenItemsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return null;
			if (value is not string list)
				return null;
	
				//items.Add(new CheckerTable { Name = item });
			
			return new CheckerTable { Name = list };
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			//return null;
			if (value == null) return null;
			if (value is not CheckerTable list) return null;
					

            return list.Name;
        }
	}

        public class BoolToColorConverter : IValueConverter
    {


		public Color FalseSource { get; set; }
        public Color TrueSource { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return null;
            }
            return (bool)value ? TrueSource : FalseSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
