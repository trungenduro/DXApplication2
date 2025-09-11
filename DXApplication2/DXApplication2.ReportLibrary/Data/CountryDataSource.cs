using LiningCheckRecord;

namespace DXApplication2.ReportLibrary.Data
{
    public class CountryDataSource : List<LiningSpool>
    {
        public CountryDataSource()
        {
            List<LiningSpool> sales= Enumerable.Range(1, 10).Select(i =>
                new LiningSpool { ID = i, SpoolNo = $"Spool{i}", Size1 = $"{i * 25}A" }
                ).ToList();
           
            this.AddRange(sales);
        }
    }

    
}