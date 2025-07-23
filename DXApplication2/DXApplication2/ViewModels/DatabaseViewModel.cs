using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core;
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
        return data ?? Enumerable.Empty<DHFOrder>();
    }
}
