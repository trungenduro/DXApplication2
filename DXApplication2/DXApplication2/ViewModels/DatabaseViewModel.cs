using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core;
using DevExpress.XtraRichEdit.Forms;
using DXApplication2.Converters;
using DXApplication2.Domain.Data;
using DXApplication2.Domain.Services;
using DXApplication2.Infrastructure.Data;
using LiningCheckRecord;

namespace DXApplication2.ViewModels;

public partial class DatabaseViewModel : ObservableObject {
    readonly ICacheService cacheService;

    [ObservableProperty]
    ObservableCollection<DHFOrder>? orders;    
    [ObservableProperty]
    ObservableCollection<CheckerTable>? peoples;

    [ObservableProperty]
    ObservableCollection<ExcelSheet>? sheets;

	[ObservableProperty]
	DHFOrder? currentOrder;	
    
    [ObservableProperty]
	ExcelSheet? currentSheet;

    [ObservableProperty]
	LiningSpool? currentSpool;



	[ObservableProperty]
	ObservableCollection<LiningSpool>? spools;

	[ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
    bool isInitialized;

    public DatabaseViewModel(ICacheService cacheService) {
        this.cacheService = cacheService;
    }

    [RelayCommand(CanExecute = nameof(CanInitialize))]
    async Task InitializeAsync() {
        var data = await GetItems();
        Orders = new ObservableCollection<DHFOrder>(data);


        IsInitialized = true;
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
	public List<string> Sizes => new List<string> {"15A", "20A", "25A", "50A", "65A", "80A", "100A","125A","150A","200A","250A","300A","350A","400A","450A","500A" };
   
    public async void SaveDatabase()
	{
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
		await unitOfWork.SaveAsync();
        GetItems();
	}
	internal async Task UpdateOrderAsync()
    {
        if(CurrentOrder == null)
			return;
		Action? pendingAction = null;
		using var unitOfWork = new SQLiteUnitOfWork(cacheService);
				
			unitOfWork.CustomersRepository.Update(CurrentOrder);
         var sheets= unitOfWork.SheetRepository.GetAsync();

        

        // var order= unitOfWork.CustomersRepository.GetAsync();


        var k= Orders.ToList().FindIndex(x => x.Id == CurrentOrder.Id);
		pendingAction = () =>
            Orders[k] = CurrentOrder;
        
		await unitOfWork.SaveAsync();
        pendingAction?.Invoke();

        Orders = new ObservableCollection<DHFOrder>(Orders);
       await InitializeAsync();
    }


	public IEnumerable<SpoolType> SpoolTypes => Enum.GetValues(typeof(SpoolType)).Cast<SpoolType>();

	

	public async void AddPeople(string newpeople)
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


	public async Task Validate(ValidateItemEventArgs args)
	{
		args.AutoUpdateItemsSource = true;
        using var unitOfWork = new SQLiteUnitOfWork(cacheService);
        Action? pendingAction = null;
        if (args.Item is DHFOrder order)
        {
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


                unitOfWork.CustomersRepository.Update(order);               
            }
        }
            if (args.Item is ExcelSheet item)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(Orders);
              
                
                if (args.DataChangeType == DataChangeType.Add)
                {                    
                   // unitOfWork.SheetRepository.Add(item);
                    //pendingAction = () => Sheets.Add(item);
                }
                if (args.DataChangeType == DataChangeType.Edit)
                {                   

                    unitOfWork.SheetRepository.Update(item);
                    //pendingAction = () => Sheets[args.SourceIndex] = item;
                }
                if (args.DataChangeType == DataChangeType.Delete)
                {
                    unitOfWork.SheetRepository.Delete(item);
                    //pendingAction = () => Sheets.Remove(item);
                }
                await unitOfWork.SaveAsync();

              //  pendingAction?.Invoke();
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
                    unitOfWork.SpoolRepository.Update(sp);
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

			await unitOfWork.SaveAsync();


		 await UpdateOrderAsync();
        return;
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
                pendingAction = () => Orders.Add(item);
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
