
using DXApplication2.Domain.Data;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore.Diagnostics;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using System.ComponentModel.DataAnnotations.Schema;


namespace LiningCheckRecord
{
       
    public class LiningCheckContext : DbContext
    {
        public string DbPath { get; }
		
		public DbSet<DHFOrder>? DHFOrders { get; set; }
        public DbSet<ExcelSheet> ExcelSheet { get; set; }
        public DbSet<LiningSpool>? Spools { get; set; }
        public DbSet<CheckerTable>? CheckerTables { get; set; }
        public DbSet<Checker1Table>? Checker1Tables { get; set; }
        //public DbSet<DHFSettings> SettingTable { get; set; }
        public DbSet<GenKanCSV> GenKanData { get; set; }
             

		public LiningCheckContext()
        {
			 DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LinningCheck.db");
			//DbPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\LinningCheck.db";
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
		//	modelBuilder.Ignore<DHFOrder>();
			modelBuilder.Entity<LiningSpool>()
					.Property(x => x.Adata)
					.HasConversion(new ValueConverter<List<object>, string>(
						v => JsonConvert.SerializeObject(v), // Convert to string for persistence
						v => JsonConvert.DeserializeObject<List<object>>(v)));
			modelBuilder.Entity<LiningSpool>()
				.Property(x => x.Bdata)
				.HasConversion(new ValueConverter<List<object>, string>(
					v => JsonConvert.SerializeObject(v), // Convert to string for persistence
					v => JsonConvert.DeserializeObject<List<object>>(v)));
			modelBuilder.Entity<LiningSpool>()
				.Property(x => x.Cdata)
				.HasConversion(new ValueConverter<List<object>, string>(
					v => JsonConvert.SerializeObject(v), // Convert to string for persistence
					v => JsonConvert.DeserializeObject<List<object>>(v)));

			modelBuilder.Entity<LiningSpool>()
				.Property(x => x.Ddata)
				.HasConversion(new ValueConverter<List<object>, string>(
					v => JsonConvert.SerializeObject(v), // Convert to string for persistence
					v => JsonConvert.DeserializeObject<List<object>>(v)));
			modelBuilder.Entity<LiningSpool>()
				.Property(x => x.Edata)
				.HasConversion(new ValueConverter<List<object>, string>(
					v => JsonConvert.SerializeObject(v), // Convert to string for persistence
					v => JsonConvert.DeserializeObject<List<object>>(v)));

			modelBuilder.Entity<LiningSpool>()
			   .Property(x => x.Fdata)
			   .HasConversion(new ValueConverter<List<object>, string>(
				   v => JsonConvert.SerializeObject(v), // Convert to string for persistence
				   v => JsonConvert.DeserializeObject<List<object>>(v)));

			modelBuilder.Entity<ExcelSheet>()
				.Property(x => x.Checkers)
				.HasConversion(new ValueConverter<List<string>, string>(
					v => JsonConvert.SerializeObject(v), // Convert to string for persistence
					v => JsonConvert.DeserializeObject<List<string>>(v)));

			modelBuilder.Entity<DHFOrder>().HasMany(x => x.ExcelSheets).WithOne(y => y.Order);
			Seed(modelBuilder);
		}



		protected  void OnModelCreating1(ModelBuilder modelBuilder)
        {
					
			// Configure the value converter for the Animal
			modelBuilder.Entity<LiningSpool>()
                .Property(x => x.Adata)
                .HasConversion(new ValueConverter<List<object>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<object>>(v)));
            modelBuilder.Entity<LiningSpool>()
                .Property(x => x.Bdata)
                .HasConversion(new ValueConverter<List<object>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<object>>(v)));
            modelBuilder.Entity<LiningSpool>()
                .Property(x => x.Cdata)
                .HasConversion(new ValueConverter<List<object>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<object>>(v)));

            modelBuilder.Entity<LiningSpool>()
                .Property(x => x.Ddata)
                .HasConversion(new ValueConverter<List<object>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<object>>(v)));
            modelBuilder.Entity<LiningSpool>()
                .Property(x => x.Edata)
                .HasConversion(new ValueConverter<List<object>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<object>>(v)));

            modelBuilder.Entity<LiningSpool>()
               .Property(x => x.Fdata)
               .HasConversion(new ValueConverter<List<object>, string>(
                   v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                   v => JsonConvert.DeserializeObject<List<object>>(v)));

            modelBuilder.Entity<ExcelSheet>()
                .Property(x => x.Checkers)
                .HasConversion(new ValueConverter<List<object>, string>(
                    v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                    v => JsonConvert.DeserializeObject<List<object>>(v)));

            modelBuilder.Entity<DHFOrder>().HasMany(x => x.ExcelSheets).WithOne(y => y.Order);
            modelBuilder.Entity<ExcelSheet>().HasMany(x => x.Spools).WithOne(y => y.Sheet);
            //  modelBuilder.Entity<ExcelSheet>().HasMany(x => x.Spools);

        }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LinningCheck.db");
			optionsBuilder.UseSqlite($"Filename={dbPath}");
			
		//	optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore
       // Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.ac)			
		//	base.OnConfiguring(optionsBuilder);


		}
		private void Seed(ModelBuilder modelBuilder)
		{

			LiningSpool spool = new LiningSpool {ID=1, Adata = new List<object> { 1.0, 2.0, 3.0 }, Bdata = new List<object> { 4.0, 5.0, 6.0 }, Cdata = new List<object> { 7.0, 8.0, 9.0 }, Ddata = new List<object> { 10.0, 11.0, 12.0 }, Edata = new List<object> { 13.0, 14.0, 15.0 }, Fdata = new List<object> { 16.0, 17.0, 18.0 } ,SpoolNo="100-000" };
			//modelBuilder.Entity<LiningSpool>().HasData(new List<LiningSpool> { spool });

			ExcelSheet excelSheet = new ExcelSheet {ID=1, SheetNo=1, Total = 100, Kiki1 = "機器1", Kiki2 = "機器2" };

		//	modelBuilder.Entity<ExcelSheet>().HasData(new List<ExcelSheet> { excelSheet });
			var customers1 = new List<DHFOrder>();
		
				customers1.Add(new DHFOrder {Id=1, 客先名 = $"JFE", 案件名="佐世保", OrderNo = $"00001-0000", Total = 10,});
				customers1.Add(new DHFOrder {Id=2, 客先名 = $"JGC", 案件名="名古屋", OrderNo = $"00001-0001", Total = 5,});

            var checker = new List<CheckerTable>();
            checker.Add(new CheckerTable { ID = 1, Name = "太郎1" });
            checker.Add(new CheckerTable { ID = 2, Name = "太郎2" });


            modelBuilder.Entity<DHFOrder>().HasData(customers1);
            modelBuilder.Entity<CheckerTable>().HasData(checker);
		//	modelBuilder.Entity<ExcelSheet>().HasData(new List<ExcelSheet>() { excelSheet});

		
			//  var test= this.DHFOrders.ToList();



		}


	}


    public class GenKanCSV
    {
        [Key]
        public int ID { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public string KanbanNo { get; set; } = string.Empty;
        public string SizeA { get; set; } = string.Empty;
        public string SizeB { get; set; } = string.Empty;
        public string Custormer { get; set; } = string.Empty;
        public string ShapeType { get; set; } = string.Empty;
        public string SpoolType { get; set; } = string.Empty;
        public string ADim { get; set; } = string.Empty;
        public string BDim { get; set; } = string.Empty;
        public string CDim { get; set; } = string.Empty;
        public string DDim { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty;
        public string ShapeTypeNo { get; set; } = string.Empty;
        public string SizeNo { get; set; } = string.Empty;

        public bool IsUsed { get; set; } = false;
        public GenKanCSV()
        {

        }
        public GenKanCSV(List<string> list )
        {
            OrderNo = list[0];
            Custormer = list[1];
            KanbanNo = list[2];
            SizeA = list[5];
            SizeB = list[4];

            ShapeType = list[6];
            ADim = list[7];
            BDim = list[8];
            CDim = list[9];
            DDim = list[10];
            SizeNo = list[19];
            SpoolType= list[22];
		        //[0] "4PL-00713-01"  string
		        //[1] "帝人㈱"   string
		        //[2] "1-1-1" string
		        //[3] ""  string
		        //[4] "2" string
		        //[5] "50"    string
		        //[6] "TEE管"  string
		        //[7] "204"   string
		        //[8] "（102）" string
		        //[9] ""  string
		        //[10]    ""  string
		        //[11]    "7/23"  string
		        //[12]    ""  string
		        //[13]    "PA(内PEL+外塗装)"  string
		        //[14]    ""  string
		        //[15]    "7/24"  string
		        //[16]    ""  string
		        //[17]    ""  string
		        //[18]    ""  string
		        //[19]    "ｱｻﾏﾜｰｸ"    string
		        //[20]    ""  string
		        //[21]    "1" string
		        //[22]    "1" string
		        //[23]    "1" string
		} 
    }

    public class DHFSettings
    {
        [Key]
        public int ID { get; set; }

        public string Description { get; set; } = string.Empty;
        public double ValueDouble { get; set; } = 0;

    }

    public class DHFOrder 
    {
        [Key]
        public int Id { get; set; }
		public override int GetHashCode()
		{
			return Id;
		}
		[Required]
        [DataType(DataType.Text), Display(Name = "O/#", Description = "O/#")]
        public string OrderNo { get; set; } = string.Empty;
        public string 客先名 { get; set; } = string.Empty;
        [DataType(DataType.Text), Display(Name = "数量", Description = "合計数量")]
        public int Total { get; set; } = 0;
        public string 案件名
        {
            get;set;
        }
        public bool IsFavorite { get; set; }

        [DataType(DataType.Text), Display(Name = "客先名/案件名", Description = "合計数量")]
        public string CombineName { get => 客先名 + "/" + 案件名; }

        public List<ExcelSheet> ExcelSheets { get; set; } = new();

        public int ExcelSheetsCount { get => ExcelSheets.Count; }
        public int SpoolsCount
        {
            get
            {
                if (ExcelSheets == null)
                return 0;
               return ExcelSheets.Where(x => x.Spools != null).Select(x => x.Spools.Count).Sum();
            }
        }
          public List<string> SpoolNames
        {
            get
            {
                if (ExcelSheets == null)
                    return new();           
                List<string> names = new();
                ExcelSheets.Where(x => x.Spools != null).ToList().ForEach(x => names.AddRange( x.Spools.Select(x=>x.SpoolNo).ToList()));

                return names;
            }
        } 
        public List<LiningSpool> Spools
        {
            get
            {
                if (ExcelSheets == null)
                    return new();           
                List<LiningSpool> names = new();
                ExcelSheets.Where(x => x.Spools != null).ToList().ForEach(x => names.AddRange( x.Spools.ToList()));

                return names;
            }
        }
        

        public bool IsFinished
        {
            get
            {

                if (Total == 0) return false;
                return (SpoolsCount == Total);
            }
        }

        public double Completion { get
            {
                double d = 0;
                if (Total == 0) return d;
                var x = (SpoolsCount * 100) / Total;

                return x;
            }
        }

        [DisplayFormat(DataFormatString = "yy/MM/dd")]
        public DateTime? LastDay
        {
            get
            {
                if (ExcelSheets.Count == 0) 
                    return null;

                var days = ExcelSheets.Where(x => x.CheckDate1 != null);
               if (days == null) return null;
                return days.Select(x => x.CheckDate1).OrderByDescending(x => x).FirstOrDefault();
            }
        }
        public string LastMonth
        {
            get
            {
                string m = "-";
                if (LastDay == null) return m;

                DateTime d = LastDay ?? DateTime.MinValue;

                return d.ToString("yy年MM月");

            }
        }


     
     

	
        bool IsStringEmpty(string str)
        {
            return str == null || str.Trim().Length == 0;
        }
    
    }


    public class DHFLiningCheckRecord
    {
        public int ID { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public int Number { get; set; }
        [DataType(DataType.Text), Display(Name = "シート", Description = "")]
        public int Total { get; set; }

        [DataType(DataType.Text), Display(Name = "数量", Description = "合計数量")]
        public string 数量 { get => $"{Number}/{Total}"; }
        public DHFOrder? Order { get; set; }

        [DisplayFormat(DataFormatString = "yy/MM/dd")]
        [DataType(DataType.DateTime), Display(Name = "検査日")]
        public DateTime CheckDate { get; set; } = DateTime.Now;

    }


    public class ExcelSheet 
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "シート")]
        [Required]
        public int SheetNo { get; set; } = 1;

        public bool IsFavorite { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public string 客先名 { get {
                if (Order == null) return string.Empty;
                return Order.客先名;

            } }

        public string 案件名
        { get {
                if (Order == null) return string.Empty;
                return Order.案件名;

            } }


        [DataType(DataType.Text), Display(Name = "数量", Description = "合計数量")]
        public int Total { get; set; } = 0;


        [Display(Name = "シート")]
        // public string SpoolNo { get; set; } = 1;
        public string Kiki1 { get; set; } = string.Empty;
        public string Kiki2 { get; set; } = string.Empty;

        public List<string> Checkers { get; set; } = new();

        public string Checker
        {
            get; set;
            //get
            //{
            //    if (Checkers == null) return "-";

            //    if (Checkers.Count == 0) return "-";
            //    if (Checkers[0] == null) return "-";
            //    return string.Join("-", Checkers);
            //}
        } = "";
        public string Checked { get; set; } = "-";

        public List<LiningSpool> Spools { get; set; } = new List<LiningSpool>();

        [DisplayFormat(DataFormatString = "yy/MM/dd")]
        public DateTime? CheckDate1 { get; set; } = DateTime.Now;
        public string CheckDateString
        {
            get
            {
                if (CheckDate1 == null) return "-";
                DateTime dateTime = CheckDate1 ?? DateTime.MinValue;
                return dateTime.ToString("yy年MM月");
            }
        } 
        public string CheckDateStringFull
        {
            get
            {
                if (CheckDate1 == null) return "-";
                DateTime dateTime = CheckDate1 ?? DateTime.MinValue;
                return dateTime.ToString("yy年MM月dd");
            }
        }



        [DisplayFormat(DataFormatString = "yy/MM/dd")]
        public DateTime? CheckDate2 { get; set; }

        public string CheckResult { get; set; } = "";

        public bool Radio1 { get; set; } = false;
        public bool Radio2 { get; set; } = false;
        public bool Radio3 { get; set; } = false;
        public bool Radio4 { get; set; } = false;

        public int Option1 { get; set; }
        public int Option2 { get; set; }
        public int SpoolsCount { get => Spools.Count; }

        public DHFOrder? Order { get; set; }
        public double ThickNess { get; set; } = 1;
        public List<object> ThickNessList
        {
            get
            {
            var thicknesses = Enumerable.Range(0, 20).Select(x => 1 + ((double)x) * 0.1);
                if (ThickNess > 1)
                    thicknesses = Enumerable.Range(0, 10).Select(x => ThickNess + ((double)x) * 0.1);
                return thicknesses.Select(x => (object) x).ToList();
            }
        }
		public string LinningType { get; set; } = "内面";
		bool IsStringEmpty(string str)
        {
            return str == null || str.Trim().Length == 0;
        }
      
        bool IsArrayEmpty(List<object> str)
        {
            if (str == null) return true;
            if (str.Count == 0) return true;
            foreach (object item in str) {
                if (item == null) return true;

            }

            return false;
        }
    
    }

	public class LiningSpool
    {

        public LiningSpool()
        {
            SpoolType = 0;
			ShapeType = "Shape1";
            SpoolNo = "";
            ImagePath = "";

		}
        [Key]
        public int ID { get; set; }
        [Display(Name = "シート")]
        public int SpoolType
        {
            get; set;
        } = 0;

        [Display(Name = "形状")]
        public string ShapeType
        {

            get;set;
        }
        public bool IsFavorite { get; set; }
        [Display(Name = "O/#", Description = "d")]
        public DHFOrder? Order { get; set; }

        [Display(Name = "シート")]
        public string SpoolNo
        {
            get;set;
        } 
        [Display(Name = "サイズ")]
        public string Size
        {
            get
            {
                if (string.IsNullOrEmpty(Size2))
                    return Size1;
                return Size1 + "x" + Size2;
            }
        }
        public string OrderNo { get
            {
                if (Sheet == null) return "";
                if (Sheet.Order == null) return "";
                return Sheet.Order.OrderNo;
            } }
        public int SheetNo
        {
            get
            {
                if (Sheet == null) return 0;
                return Sheet.SheetNo;
            }
        }

        public List<double> Thicknesses { get
            {
                double tmin = 1;
                if (Sheet!=null)
                    if(Sheet.ThickNess>1) tmin = Sheet.ThickNess;
                return Enumerable.Range(0, 10).Select(x => Math.Round( tmin + ((double)x) * 0.1,1)).ToList();

            }
        }

        public DateTime? CheckDate1 { get
            {
                if (Sheet == null) return null;
                return Sheet.CheckDate1;
            }
        }
        public string CheckMonth
        {
            get
            {
                if (CheckDate1 == null) return "-";
                DateTime dateTime = CheckDate1 ?? DateTime.MinValue;
                return dateTime.ToString("yy年MM月");
            }
        }


        public string 客先名
        { get
            {
                if (Sheet == null) return "-";
                return Sheet.客先名;
            }
        }
        public string 案件名
        { get
            {
                if (Sheet == null) return "-";
                return Sheet.案件名;
            }
        }


        public string Checker { get
            {
                if (Sheet == null) return "-";
                if (Sheet.Checkers == null) return "-";
                if (Sheet.Checkers.Count == 0) return "-";
                if (Sheet.Checkers[0] == null) return "-";
                return string.Join("-", Sheet.Checkers);
            }
        }

        public ExcelSheet? Sheet { get; set; }

        public string Size1 { get; set; } = "50A";
        public string Size2 { get; set; } = string.Empty;
        public string ImagePath
        {
            get;set;
        } 

		[Display(Name = "外観")]
        public string CheckShape { get; set; } = string.Empty;

        //     public List<CheckData> CheckDatas { get;set; } = new List<CheckData>();

        public string A1 { get; set; }= "";
        public string A2 { get; set; }= "";
        public string A3 { get; set; }= "";
        public string A4 { get; set; }= "";   
                       
        public string B1 { get; set; }= "";
        public string B2 { get; set; }= "";
        public string B3 { get; set; }= "";
        public string B4 { get; set; }= "";
                        
        public string C1 { get; set; }= "";
        public string C2 { get; set; }= "";
        public string C3 { get; set; }= "";
        public string C4 { get; set; }= "";
                        
        public string D1 { get; set; }= "";
        public string D2 { get; set; }= "";
                        
        public string D3 { get; set; }= "";
                        
        public string D4 { get; set; }= "";
        public string E1 { get; set; }= "";
        public string E2 { get; set; }= "";
        public string E3 { get; set; }= "";                         
        public string E4 { get; set; }= "";
        public string F1 { get; set; }= "";
        public string F2 { get; set; }= "";
        public string F3 { get; set; }= "";
        public string F4 { get; set; } = "";

        public List<object> Adata { get; set; } = new();
        public List<object> Bdata { get; set; } = new();
        public List<object> Cdata { get; set; } = new();
        public List<object> Ddata { get; set; } = new();
        public List<object> Edata { get; set; } = new();
        public List<object> Fdata { get; set; } = new();

        public double Min
        {
            get
            {
                double min = 0;
                List<object> ABCD = new List<object>();
                if (Adata != null) if (Adata.Count > 0 ) ABCD.AddRange(Adata);
                if (Bdata != null) if (Bdata.Count > 0 ) ABCD.AddRange(Bdata);
                if (Cdata != null) if (Cdata.Count > 0  )ABCD.AddRange(Cdata);
                if (Edata != null) if (Edata.Count > 0 ) ABCD.AddRange(Edata);
                if (Ddata != null) if (Ddata.Count > 0  )ABCD.AddRange(Ddata);
                if (Fdata != null) if (Fdata.Count > 0  )ABCD.AddRange(Fdata);

              //  if (ABCD.Count > 0) min = ABCD.Select(x=>x.ParseDoubleValue()).Min();

                return min;
            }
        } 
        public double Max
        {
            get
            {
                double min = 0;
                List<object> ABCD = new List<object>();
                if (Adata != null) if (Adata.Count > 0 ) ABCD.AddRange(Adata);
                if (Bdata != null) if (Bdata.Count > 0 ) ABCD.AddRange(Bdata);
                if (Cdata != null) if (Cdata.Count > 0 ) ABCD.AddRange(Cdata);
                if (Ddata != null) if (Ddata.Count > 0 ) ABCD.AddRange(Ddata);
                if (Edata != null) if (Edata.Count > 0 ) ABCD.AddRange(Edata);
                if (Fdata != null) if (Fdata.Count > 0 ) ABCD.AddRange(Fdata);

               // if (ABCD.Count > 0) min = ABCD.Select(x => x.ParseDoubleValue()).Max();
                return min;
            }
        }

      
      

        [DisplayName("備考")]
        public string MeMo { get; set; } = string.Empty;
    }


    public class ChartData
    {
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; } = 0;
        public ChartData()
        {
          
        }
    }
    public class CheckData
    {
        [Display(Name = "シート")]
        public string SpoolNo { get; set; } = string.Empty;
        public decimal A { get; set; }
        public decimal B { get; set; }
        public decimal C { get; set; }
        public decimal D { get; set; }
        public decimal E { get; set; }
        public decimal F { get; set; }
        public LiningSpool Spool { get; set; } = new LiningSpool();

    }

    public class CheckerTable
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "名前")]
        [Required]
        public string Name { get; set; } = "";

        public override string ToString()
        {
            return Name;
        }
    } 
    public class Checker1Table
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "名前")]
        [Required]
        public string Name { get; set; } = "";
    }



    public class SpoolShape
    {
        public int Type { get; set; }
        public string Desc { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public SpoolShape(int i, string desc)
        {
            Type = i;
            Desc = desc;
            if (i == 1) Name = "直管";
            if (i == 2) Name = "直管(枝付)";
            if (i == 3) Name = "曲げ";
            if (i == 4) Name = "図面作成";
            if (i == 5) Name = "カメラ";
        }


    }


}
