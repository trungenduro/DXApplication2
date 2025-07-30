using DXApplication2.ViewModels;
using LiningCheckRecord;

namespace DXApplication2.Views;

public partial class SheetEditPage : ContentPage
{
	public SheetEditPage()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;
	public SheetEditPage(DatabaseViewModel model)
	{

		DatabaseViewModel = model;
		this.BindingContext = model;
		InitializeComponent();
	}

	private void Save_Clicked(object sender, EventArgs e)
	{

	}

	private void collectionView_CreateDetailFormViewModel(object sender, DevExpress.Maui.Core.CreateDetailFormViewModelEventArgs e)
	{

	}

	private void Swipe_Open(object sender, DevExpress.Maui.DataGrid.SwipeItemTapEventArgs e)
	{
		DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
		if (e.Item is LiningSpool sp)
		{
			vm.CurrentSpool = sp;
			Navigation.PushAsync(new NewSpoolPage(vm));
		}
	}
}