using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core;
using DevExpress.XtraRichEdit.Forms;
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

    public List<string> Sizes = new List<string> { "20A", "25A", "50A", "65A", "80A", "100A" };
   internal async Task UpdateOrderAsync()
    {
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



    public async Task Validate(ValidateItemEventArgs args)
	{
		args.AutoUpdateItemsSource = true;
        using var unitOfWork = new SQLiteUnitOfWork(cacheService);
        Action? pendingAction = null;
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
                    //unitOfWork.SheetRepository.Add(item);
                    //pendingAction = () => Sheets.Add(item);
                }
                if (args.DataChangeType == DataChangeType.Edit)
                {
                    unitOfWork.SpoolRepository.Update(sp);
                    //pendingAction = () => Sheets[args.SourceIndex] = item;
                }
                if (args.DataChangeType == DataChangeType.Delete)
                {
                    unitOfWork.SpoolRepository.Delete(sp);
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

        await unitOfWork.SaveAsync();
        pendingAction?.Invoke();

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

            unitOfWork.SaveAsync().Wait();
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

        foreach (var sh in sheets)
        {
            if (sh.Order == null)
                unitOfWork.SheetRepository.Delete(sh);
            else
            {
                if (sh.Order.ExcelSheetsCount > 0)
                {
                    //if (sh.Order.ExcelSheets.Where(x => x.ID == sh.ID).Count() == 0)

                       // unitOfWork.SheetRepository.Delete(sh);
                }
            }
        }

        for (int i1 = 0; i1 < spools.Count(); i1++)
        {

            var sp = spools.ToList()[i1]; 
            
            if (sp.Sheet == null)
                unitOfWork.SpoolRepository.Delete(sp);
            else
            {
                if (sp.Sheet.Spools.Count > 0)
                {
                    var t = sp.Sheet.Spools.Where(x => x.ID == sp.ID).Count();

                      //  unitOfWork.SpoolRepository.Delete(sp);
                }
            }
        }
        await unitOfWork.SaveAsync();
         data = await unitOfWork.CustomersRepository.GetAsync();
         sheets = await unitOfWork.SheetRepository.GetAsync();
        spools = await unitOfWork.SpoolRepository.GetAsync();
        Sheets = new ObservableCollection<ExcelSheet>(sheets?? Enumerable.Empty<ExcelSheet>());

        return data ?? Enumerable.Empty<DHFOrder>();
    }
}
