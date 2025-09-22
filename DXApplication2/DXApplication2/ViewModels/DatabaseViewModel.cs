using System.Collections.ObjectModel;
using System.IO;
using Android.Content;
using AndroidX.Lifecycle;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core;




using DXApplication2.Converters;
using DXApplication2.Domain.Data;
using DXApplication2.Domain.Services;
using DXApplication2.Infrastructure.Data;
//using Java.IO;
using LiningCheckRecord;
//using File = Java.IO.File;

namespace DXApplication2.ViewModels;

public partial class DatabaseViewModel : ObservableObject {
    readonly ICacheService cacheService;

    [ObservableProperty]
    ObservableCollection<DHFOrder>? orders;
    
    [ObservableProperty]
    ObservableCollection<string>? custormers;



    [ObservableProperty]
    ObservableCollection<DHFOrder>? ordersMarked;
   

        [ObservableProperty]
    ObservableCollection<CheckerTable>? peoples;
    [ObservableProperty]
    ObservableCollection<CheckerTable>? selectedPeoples; 
    
    [ObservableProperty]
    CheckerTable? selectedPeople;


       [ObservableProperty]
    ObservableCollection<string>? orderFilter;



    [ObservableProperty]
    ObservableCollection<ExcelSheet>? sheets;

	[ObservableProperty]
	DHFOrder? currentOrder;	
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SheetChangedCommand))]
    ExcelSheet? currentSheet;


  
    [RelayCommand()]  

    public  void SheetChanged()
    {
        var selected = new ObservableCollection<CheckerTable>();

        foreach (var item in currentSheet.Checkers)
        {
            selected.Add(Peoples.Where(x => x.Name == item).FirstOrDefault());
        }
        this.SelectedPeoples = selected;
        this.SelectedPeople= Peoples.Where(x => x.Name == currentSheet.Checked).FirstOrDefault();
       // return Task.CompletedTask;
    }

    [ObservableProperty]
	LiningSpool? currentSpool;



	[ObservableProperty]
	ObservableCollection<LiningSpool>? spools;

	[ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
    bool isInitialized;
    [ObservableProperty]  
    bool isRunning=false;



    public DatabaseViewModel(ICacheService cacheService) {
        this.cacheService = cacheService;
    }

    [RelayCommand(CanExecute = nameof(CanInitialize))]
    async Task InitializeAsync() {
        var data = await GetItems();
        Orders = new ObservableCollection<DHFOrder>(data);

        OrderFilter = new ObservableCollection<string> { "未完成" };

        if (Orders != null)
        {
            var cus1 = Orders.Select(x => x.客先名).ToList();
            var cus2 = Orders.Select(x => x.案件名).ToList();
            cus1.AddRange(cus2);

            FilterList = cus1;
        }
            IsInitialized = true;
    }

	[ObservableProperty]
	public List<string> filterList;
  
    public List<string> PeoplesName=> Peoples?.Select(x => x.Name).ToList() ?? new List<string>();
    public void AddFilter(string str)
    {
        if (OrderFilter == null) return;
		{
			
		}
		List<string> filters = OrderFilter.ToList();

        if(filters.Contains(str))
		{
			filters.Remove(str);
		}
		else
		{
			filters.Add(str);
		}

		OrderFilter = new ObservableCollection<string>(filters);        
	}

  
   // [ObservableProperty]
    //PdfDocumentSource? documentSource;

    [RelayCommand()]
    public async Task InitializePDFAsync() {
        IsInitialized = false;
            List<LiningSpool> liningSpools = new List<LiningSpool>();
        if (CurrentSheet == null)
        {
            if (Orders != null)
                if (Orders.Count > 0)
                    if (Orders[0].ExcelSheets.Count > 0)
                        liningSpools = new List<LiningSpool>(Orders[0].ExcelSheets[0].Spools);
        }
        else
        { liningSpools = new List<LiningSpool>(CurrentSheet.Spools); }

        int i1;
		for ( i1=1 ; i1 < 10 && liningSpools.Count > 10 * i1; i1++)
        
		for (int i = 0; i < 10 && liningSpools.Count < 10 * i1; i++)
		{
			liningSpools.Add(new LiningSpool { SpoolNo = "", Size1 = "" });
		}
		if (liningSpools.Count >0)
        {            
            var report = new DXApplication2.ReportLibrary.XtraReportLiningSpool() { Name="test",DataSource= liningSpools} ;
            report.CreateDocument();
            string resultFile = Path.Combine(FileSystem.Current.AppDataDirectory, report.Name + ".pdf");
			MemoryStream stream = new MemoryStream();
            await report.ExportToPdfAsync(stream);
				
			//DocumentSource = stream;
            //LinningReport 
            // LinningReport
        }
       
        IsInitialized = true;
    }

     public async Task GeneratePDFOrder() {
        IsRunning = true;
            List<LiningSpool> liningSpools = new List<LiningSpool>();
        if (CurrentOrder == null)
            return;
       
        { liningSpools = new List<LiningSpool>(); }

        foreach (var sheet in CurrentOrder.ExcelSheets.OrderBy(x=>x.SheetNo))
        {
            var spools = new List<LiningSpool>(sheet.Spools);

            for (int i = 0; i < 10 && spools.Count < 10 * ((spools.Count + 9) / 10); i++)
            {
				spools.Add(new LiningSpool { SpoolNo = "", Size1 = "" });
            }

			liningSpools.AddRange(spools);  
		}


		if (liningSpools.Count >0)
        {            
            var report = new DXApplication2.ReportLibrary.XtraReportLiningSpool() { Name="test",DataSource= liningSpools} ;
            report.CreateDocument();
            var downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            // Fix for CS8602 and MVVMTK0034: Use the generated property `CurrentOrder` instead of directly referencing the field `currentOrder`.
            // Additionally, ensure `CurrentOrder` is not null before accessing its properties to avoid null reference exceptions.

            string resultFile =  $"{CurrentOrder?.OrderNo ?? "Order"}_成績表.pdf";
            MemoryStream stream = new MemoryStream();
            await report.ExportToPdfAsync(stream);
            stream.Position = 0;
            try
            {
                SaveFileToAndroid(CurrentOrder?.OrderNo ?? "Order", resultFile, stream);
            }
            catch (Exception)
            {

            }
        }

        IsRunning = false;
	}



     public async Task GeneratePDFSheet(ExcelSheet sheet) {
        IsRunning = true;
        List<LiningSpool> liningSpools = new List<LiningSpool>( sheet.Spools);
	
       

		for (int i = 0; i < 10 && liningSpools.Count < 10 * ((liningSpools.Count + 9) / 10); i++)
		{
			liningSpools.Add(new LiningSpool { SpoolNo = "", Size1 = "" });
		}

		if (liningSpools.Count >0)
        {            
            var report = new DXApplication2.ReportLibrary.XtraReportLiningSpool() { Name="test",DataSource= liningSpools} ;
            report.CreateDocument();
            var downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
         //  var filePath = Path.Combine(downloadsPath, fileName);


            string resultFile =  $"{sheet?.Order?.OrderNo ?? "Order"}_シート{sheet?.SheetNo ?? 0}.pdf";
			
            if( File.Exists(resultFile))
            {
               //  resultFile = Path.Combine(downloadsPath, $"{currentOrder.OrderNo}_シート{sheet.SheetNo}_{}.pdf");

               //await Shell.Current.DisplayAlert("Error", "File.Exists", "OK");
                  //  IsRunning = false;
                 //   return;
                
            }
            MemoryStream stream = new MemoryStream();
            await report.ExportToPdfAsync(stream);
            stream.Position = 0;

            try
            {
                SaveFileToAndroid(sheet.Order?.OrderNo??"", resultFile, stream);
            }
            catch (Exception e1 )
            {
                await Shell.Current.DisplayAlert("Error", e1.Message, "OK");
            }
            //DocumentSource = stream;         
        }

        IsRunning = false;
	}

    public void SaveFileToAndroid(string folder, string resultFile, MemoryStream stream)
    {
        var documentPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
        var dir = Path.Combine(documentPath, "ライニング検査","成績表");
        if(dir != null && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var dir1 = Path.Combine(documentPath, "ライニング検査", "成績表",folder);
        if (dir1 != null && !Directory.Exists(dir1))
        {
            Directory.CreateDirectory(dir1);
        }
        string fullpath= Path.Combine(dir1, resultFile);

        using (var fileStream = File.Create(fullpath))
        {
            stream.CopyTo(fileStream);
        }

        var context = Android.App.Application.Context;
        var uri = Android.Net.Uri.FromFile(new Java.IO.File(fullpath));
        context.SendBroadcast(new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile, uri));

       
         OpenFile(fullpath, "application/pdf");


    }

    public void OpenFile(string filePath, string mimeType)
    {
        try
        {
            var context = Android.App.Application.Context;
            // Replace the following line:  
            // var file = new File(filePath);  

            // With this corrected line:  
            var file = new Java.IO.File(filePath);
            var uri = FileProvider.GetUriForFile(context, context.PackageName + ".fileprovider", file);

            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, mimeType);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);


           // Shell.Current.DisplayAlert("データ保存", "ファイルを開く際にエラーが発生しました。", "OK").Wait();

            context.StartActivity(intent);
        }
        catch (Exception ex)
        {
            // Log the error or display an alert to the user  
           System.Console.WriteLine($"Error opening file: {ex.Message}");
            Shell.Current.DisplayAlert("Error", "ファイルを開く際にエラーが発生しました。", "OK").Wait();
        }
    }




    [RelayCommand]
    async Task Refresh()
    {
       await InitializeAsync();
    }

    [RelayCommand]
    async Task DeleteItemAsync(DHFOrder item) {
        using var unitOfWork = new SQLiteUnitOfWork(cacheService);
        unitOfWork.CustomersRepository.Delete(item);
        try {
            await unitOfWork.SaveAsync();
        } catch (Exception e) {
            await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            return;
        }
		Orders?.Remove(item);
    }
	public List<string> Options1 => new List<string> { "10,000V", "12,000V" };
	public List<string> LinningType => new List<string> { "内面", "外面" };
	public List<string> CheckResults => new List<string> { "","合格", "不合格" };
	public List<string> Options2 => new List<string> { "電磁式膜厚計", "渦電流式膜厚計" };
	public List<string> Sizes => new List<string> {"1/2B","1B", "2B", "3B", "4B", "6B", "10B", "12BA","14B","16B","18B","20B","24B"};
   
    public async void SaveDatabase()
	{
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
		await unitOfWork.SaveAsync();
        await GetItems();
	}
	
	public ObservableCollection<DHFOrder> Favorites
    {
        get
        {
            if (Orders == null) return null;
            return Orders?? new ObservableCollection<DHFOrder>(Orders.Where(x => x.IsFavorite));
        }
    }
	internal async void AddToFavorites(DHFOrder house)
    {
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
		house.IsFavorite = !house.IsFavorite;
		Action? pendingAction = null;
			
				var k = Orders.ToList().FindIndex(x => x.Id == house.Id);
				if (k != -1)
				{
					pendingAction = () =>
						Orders[k] = house;

					unitOfWork.CustomersRepository.Update(house);
				}
		await unitOfWork.SaveAsync();
		pendingAction?.Invoke();

	}
    internal async Task UpdateOrderAsync()
    {
        if(CurrentOrder == null)
			return;
		Action? pendingAction = null;
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
				
			unitOfWork.CustomersRepository.Update(CurrentOrder);
        // var sheets= unitOfWork.SheetRepository.GetAsync();

        

        // var order= unitOfWork.CustomersRepository.GetAsync();


        var k= Orders.ToList().FindIndex(x => x.Id == CurrentOrder.Id);
		pendingAction = () =>
            Orders[k] = CurrentOrder;
        
		await unitOfWork.SaveAsync();
        pendingAction?.Invoke();

        Orders = new ObservableCollection<DHFOrder>(Orders);
       await InitializeAsync();
    }

    public static List<LiningSpool> liningSpools;
    //public List<LiningSpool> Spools
    //{
    //    get
    //    {
    //        if (CurrentSheet == null) return new List<LiningSpool>();
    //        return CurrentSheet.Spools;
    //    }
    //}
            

	public IEnumerable<SpoolType> SpoolTypes => Enum.GetValues(typeof(SpoolType)).Cast<SpoolType>();

	

	public async void AddPeople(string newpeople)
    {
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
		var p = new CheckerTable() { Name = newpeople };
		try
		{
			unitOfWork.PeoplesRepository.Add(p);
		    await unitOfWork.SaveAsync();
            List<CheckerTable> checkerTables = Peoples.ToList();
			checkerTables.Add(p);
			Peoples = new ObservableCollection<CheckerTable>(checkerTables);
		}
		catch (Exception e)
		{
			await Shell.Current.DisplayAlert("Error", e.Message, "OK");
			return;
		}
	}


	public async Task Validate(ValidateItemEventArgs args)
	{
		args.AutoUpdateItemsSource = true;
		string mess = "";
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
        Action? pendingAction = null;
        if (args.Item is DHFOrder order)
        {
            if (string.IsNullOrEmpty(order.OrderNo))
                mess += "OrderNoを入力してください\n";
            if (string.IsNullOrEmpty(order.客先名))
                mess += "客先名を入力してください\n";
              if (string.IsNullOrEmpty(order.案件名))
                mess += "案件名を入力してください\n";
            if (order.Total <= 0)
                mess += "管数を入力してください\n";
            if(args.DataChangeType == DataChangeType.Add)
            if (Orders.Where(x => x.OrderNo == order.OrderNo && x.案件名 == order.案件名 && x.客先名 == order.客先名).Any())
                mess += "オーダーが 重複してます\n";
            if (mess != "")
            {
                args.IsValid = false;
                await Shell.Current.DisplayAlert("確認", mess, "OK");
                return;
            }

            if (args.DataChangeType == DataChangeType.Edit)
            {

                var sheets = await unitOfWork.SheetRepository.GetAsync();
                var orderS= sheets.Where(x=>x.Order!=null).Where(x => x.Order.Id == order.Id).ToList();
                foreach (var sh in orderS)
                {
                  if(!order.ExcelSheets.Where(x=>x.ID==  sh.ID).Any())
                    {
                        //unitOfWork.SheetRepository.Delete(sh);
                    }
                  
                }
				var k = Orders.ToList().FindIndex(x => x.Id == CurrentOrder.Id);
                if (k != -1)
                {
                    pendingAction = () =>
                        Orders[k] = CurrentOrder;

                    unitOfWork.CustomersRepository.Update(order);
                }
			
			}
            if (args.DataChangeType == DataChangeType.Add)
            {               



                unitOfWork.CustomersRepository.Add(order);
				pendingAction = () => Orders.Add(order);
			}
            if (args.DataChangeType == DataChangeType.Delete)
            {
				bool confirm = await Shell.Current.DisplayAlert(
						"確認", $"シート {order.OrderNo} を削除しますか？", "はい", "キャンセル");
                if (!confirm)
                {
                    args.IsValid = false;
                    return;
                }
				unitOfWork.CustomersRepository.Delete(order);
				pendingAction = () => Orders.Remove(order);
			}

        }
            if (args.Item is ExcelSheet item)
        {
            try
            {
                item.Checked = SelectedPeople?.Name ?? "-";
                if (SelectedPeoples != null)
                {

                    var oblist = SelectedPeoples.Select(x => x.Name).ToList();
                    item.Checkers = oblist;
                    item.Checker = string.Join("-", oblist);
                }
              
                ArgumentNullException.ThrowIfNull(Orders);
                if (item.Checked == null) item.Checked = "-";
                if (item.Kiki1 == "" || item.Kiki2=="")
                    mess = "機器名を入力してください";
               
                if (item.ThickNess <= 0)
                    mess = "指定膜厚を入力してください";
                if (item.Checker == "")
                    mess = "検査員を入力してください";

                if(item.CheckResult.Equals("合格"))
                {
                    if (item.Spools.Where(x => x.CheckShape != "良").Any())
                    {
                        mess = "合格できい管番号があります";

                        args.IsValid = false;
                        await Shell.Current.DisplayAlert("確認", "合格できません", "OK");
                        return;
                    }
                }

                if (mess != "")
                {

                    args.IsValid = false;
                    await Shell.Current.DisplayAlert("確認", mess, "OK");
                    return;
                }

                if (args.DataChangeType == DataChangeType.Add)
                {
                    // unitOfWork.SheetRepository.Add(item);
                    //pendingAction = () => Sheets.Add(item);
                    
                

                }
                if (args.DataChangeType == DataChangeType.Edit)
                {                   

                    if(item.ID!=0) unitOfWork.SheetRepository.Update(item);
                   // pendingAction = () => Sheets[args.SourceIndex] = item;
                }
                if (args.DataChangeType == DataChangeType.Delete)
                {
                    unitOfWork.SheetRepository.Delete(item);
                    //pendingAction = () => Sheets.Remove(item);
                }
          
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
           
        }
         if (args.Item is LiningSpool sp)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(Orders);
               

                
                if (args.DataChangeType == DataChangeType.Add)
                {
					
					//CurrentOrder.ExcelSheets.Add(item);
					//unitOfWork.SpoolRepository.Add(sp);
					//pendingAction = () => Sheets.Add(item);
				}
				if (args.DataChangeType == DataChangeType.Edit)
                {
                   // unitOfWork.SpoolRepository.Update(sp);
                    //pendingAction = () => Sheets[args.SourceIndex] = item;
                }
                if (args.DataChangeType == DataChangeType.Delete)
                {
                   // unitOfWork.SpoolRepository.Delete(sp);
                    //pendingAction = () => Sheets.Remove(item);
                }

               
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
           
        }

		if (args.Item is CheckerTable people)
        {
            if (args.DataChangeType == DataChangeType.Delete)
            {
                unitOfWork.PeoplesRepository.Delete(people);
            }
            else
            {
                if (Peoples.Where(x => x.Name == people.Name).Any() || people.Name.Trim() == "")
                {
                    args.IsValid = false;
                    await Shell.Current.DisplayAlert("Error", "名前が重複しています", "OK");
                    return;
                }
            }
			if (args.DataChangeType == DataChangeType.Add)
			{
				unitOfWork.PeoplesRepository.Add(people);
				
				//List<CheckerTable> checkerTables = Peoples.ToList();
				//checkerTables.Add(people);
				//Peoples = new ObservableCollection<CheckerTable>(checkerTables);

			}
			if (args.DataChangeType == DataChangeType.Edit)
			{
				unitOfWork.PeoplesRepository.Update(people);
				
			}
		}


		if (mess != "")
		{

			args.IsValid = false;
			await Shell.Current.DisplayAlert("確認", mess, "OK");
			return;
		}
		await unitOfWork.SaveAsync();
        pendingAction?.Invoke();

      //  await UpdateOrderAsync();
        return;
    }

    public async void CheckSpool( ExcelSheet sheet, DevExpress.Maui.Core.ValidateItemEventArgs e)
    {
        if (CurrentOrder == null) return;
		
					
		if (e.Item is not LiningSpool sp) return;
		string mess = "";

		if (sp.SpoolNo == null || sp.SpoolNo == "")
		{
			mess += "管番号未入力\n";
		}
		if (sp.Size == null || sp.Size == "")
		{
			mess += "管サイズ未入力\n";
		}
		if (mess != "")
		{
			e.IsValid = false;
			await Shell.Current.DisplayAlert("確認", mess, "OK");
			return;
		}
		
            var spools = CurrentOrder.Spools.Select(x=>x.SpoolNo).ToList();
        foreach (var item in sheet.Spools)
        {
            if (!spools.Contains(item.SpoolNo)) spools.Add(item.SpoolNo);
        }
		

			if (e.DataChangeType == DataChangeType.Edit)
			{
				if (spools.Where(x => x.Trim().Equals(sp.SpoolNo)).Count() > 1)
				{
					mess += "管番号重複\n";
				}
			}
			if (e.DataChangeType == DataChangeType.Add)
			{
				if (spools.Where(x => x.Equals(sp.SpoolNo)).Any())
				{
					mess += "管番号重複\n";
				}
			}
			if (!string.IsNullOrEmpty(mess))
			{
				e.IsValid = false;
				await Shell.Current.DisplayAlert("確認", mess, "OK");
				return;
			}
		
	}


    internal async Task DeleteSheetAsync(ExcelSheet sheet)
    {

        if (sheet == null)
            return;
        using var unitOfWork = new SQLiteUnitOfWork(cacheService);

       // var sheets = await unitOfWork.SheetRepository.GetAsync();
        try
        {
            unitOfWork.SheetRepository.Delete(sheet);
            unitOfWork.SaveAsync().Wait();
        }
        catch (Exception e)
        {
            Shell.Current.DisplayAlert("Error", e.Message, "OK").Wait();
            return;
        }
       // CurrentOrder?.ExcelSheets.Remove(CurrentSheet);
    }
    internal async Task DeleteSpoolAsync(LiningSpool spool)
    {

        if (spool == null)
            return;
        using var unitOfWork = new SQLiteUnitOfWork(cacheService);

        //var sheets = await unitOfWork.SheetRepository.GetAsync();
        try
        {
            unitOfWork.SpoolRepository.Delete(spool);
            await  unitOfWork.SaveAsync();
        }
        catch (Exception e)
        {
            Shell.Current.DisplayAlert("Error", e.Message, "OK").Wait();
            return;
        }
       // CurrentOrder?.ExcelSheets.Remove(CurrentSheet);
    }



        [RelayCommand]
    async Task ValidateAndSaveAsync(ValidateItemEventArgs args) {
        args.AutoUpdateItemsSource = false;
        if (args.Item is not DHFOrder item)
            return;

        try {
            ArgumentNullException.ThrowIfNull(Orders);
            Action? pendingAction = null;

            using var unitOfWork = new SQLiteUnitOfWork(cacheService);
            if (args.DataChangeType == DataChangeType.Add) {
                unitOfWork.CustomersRepository.Add(item);
                //pendingAction = () => Orders.Add(item);
            }
            if (args.DataChangeType == DataChangeType.Edit) {
                unitOfWork.CustomersRepository.Update(item);
                pendingAction = () => Orders[args.SourceIndex] = item;
            }
            if (args.DataChangeType == DataChangeType.Delete) {

                unitOfWork.CustomersRepository.Delete(item);
                pendingAction = () => Orders.Remove(item);
            }

            await unitOfWork.SaveAsync();
            pendingAction?.Invoke();
        } catch (Exception ex) {
            args.IsValid = false;
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            return;
        }
    }
    [RelayCommand]
    void CreateDetailFormViewModel(CreateDetailFormViewModelEventArgs args) {
        if (args.DetailFormType != DetailFormType.Edit)
            return;
        var item = new Customer();
        //Customer.Copy((Customer)args.Item!, item);
       // args.Result = new DetailEditFormViewModel(item, isNew: false);
    }

    bool CanInitialize() => !IsInitialized;
   
    async Task<IEnumerable<DHFOrder>> GetItems() {
        using var unitOfWork = new SQLiteUnitOfWork(cacheService);
        var data = await unitOfWork.CustomersRepository.GetAsync();
        var sheets = await unitOfWork.SheetRepository.GetAsync();
       var spools = await unitOfWork.SpoolRepository.GetAsync();
        var ps = await unitOfWork.PeoplesRepository.GetAsync();
         var sheets1=  unitOfWork.context.ExcelSheet.ToList();
        var spools1=  unitOfWork.context.Spools.ToList();
        //var spoolsa = unitOfWork.context.Spools.ToList().ToList();
        //var orderS = spools.Where(x => x.Sheet != null).Where(x=>!x.Sheet.Spools.Where(x1=>x1.ID == x.ID).Any()).ToList();

        if(data!=null)
            Custormers = new ObservableCollection<string>(data.GroupBy(x => x.客先名).Select(x => x.Key));

		Peoples = new ObservableCollection<CheckerTable>(ps ?? Enumerable.Empty<CheckerTable>());
        return data ?? Enumerable.Empty<DHFOrder>();
    }
	public async void RemovePeople(string newpeople)
	{
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
		var p = new CheckerTable() { Name = newpeople };
		try
		{
			unitOfWork.PeoplesRepository.Add(p);
			//await unitOfWork.SaveAsync();
			List<CheckerTable> checkerTables = Peoples.ToList();
			checkerTables.Add(p);
			Peoples = new ObservableCollection<CheckerTable>(checkerTables);
		}
		catch (Exception e)
		{
			await Shell.Current.DisplayAlert("Error", e.Message, "OK");
			return;
		}
	}
	internal async void RemovePeople(CheckerTable peo)
	{
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
		unitOfWork.PeoplesRepository.Delete(peo);
		await unitOfWork.SaveAsync();
        List<CheckerTable> checkerTables = Peoples.ToList();
		checkerTables?.Remove(peo);
		Peoples = new ObservableCollection<CheckerTable>( checkerTables);

	}
}
