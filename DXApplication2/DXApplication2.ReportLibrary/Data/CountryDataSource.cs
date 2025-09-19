using LiningCheckRecord;

namespace DXApplication2.ReportLibrary.Data
{
    public class CountryDataSource : List<LiningSpool>
    {
        public CountryDataSource()
        {
			DHFOrder order = new DHFOrder { OrderNo = "DHF2023001",  ãqêÊñº = "JFE",  Total=10, àƒåèñº = "ç≤ê¢ï€" };
			ExcelSheet sheet = new ExcelSheet { SheetNo = 1, Kiki1 = "22", Kiki2 = "33", CheckResult = "çáäi", ThickNess = 2, CheckDate1 = DateTime.Now, CheckDate2=DateTime.Now, Checkers = new List<string> {"DHF" }, Checked="OK" ,Order=order, Option1=1, Option2 = 1 };

			List<LiningSpool> sales= Enumerable.Range(1, 5).Select(i =>
                new LiningSpool { ID = i, SpoolNo = $"Spool{i}", Size1 = $"{i * 25}A", Sheet= sheet
                ,A1="1.2",A2= "1.3",
					A3 = "1.4",
					A4 = "1.5",
				B1= "2.1",
					B2 = "2.2",
					B3 = "2.3",
					B4 = "2.4",
					C1 = "3.1",
					C2 = "3.2",
					C3 = "3.3",
					C4 = "3.4",
					D1 = "4.1",
					D2 = "4.2",
					D3 = "4.3",
					D4 = "4.4",
					E1 = "5.1",
					E2 = "5.2",
					E3 = "5.3",
					E4 = "5.4",
					F1 = "6.1",
					F2 = "6.2",
					F3 = "6.3",
					F4 = "6.4",
				  
				}
                ).ToList();
			int i1 = (sales.Count + 9) / 10;
		

				for (int i = 0; i < 10 && sales.Count < 10 * i1; i++)
				{
					sales.Add(new LiningSpool { SpoolNo = "", Size1 = "" });
				}

			sheet.Spools = sales;
			order.ExcelSheets = new List<ExcelSheet> { sheet };	

			this.AddRange(sales);
        }
    }

    
}