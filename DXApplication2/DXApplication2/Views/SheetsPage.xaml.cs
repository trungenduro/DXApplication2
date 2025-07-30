using DXApplication2.ViewModels;
using LiningCheckRecord;

namespace DXApplication2.Views;

public partial class SheetsPage : ContentPage
{
	public SheetsPage()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;

	DHFOrder? dhfOrder;

	public SheetsPage(DatabaseViewModel viewModel, DHFOrder order)
	{
		DatabaseViewModel = viewModel;
		var c= viewModel.CurrentOrder;
		BindingContext = viewModel;
		dhfOrder = order;

		InitializeComponent();


	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{

	}

	private void Save_Clicked(object sender, EventArgs e)
	{
		DatabaseViewModel.UpdateOrderAsync();


	}

	private void Swipe_Open(object sender, DevExpress.Maui.DataGrid.SwipeItemTapEventArgs e)
	{
		DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
		if (e.Item is ExcelSheet sheet)
		{
			vm.CurrentSheet = sheet;
			Navigation.PushAsync(new SheetEditPage(vm));
		}


	}

	private void collectionView_CreateDetailFormViewModel(object sender, DevExpress.Maui.Core.CreateDetailFormViewModelEventArgs e)
	{

    }

	private void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
	{
		DatabaseViewModel.ValidateSheets(e);
	}
}