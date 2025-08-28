using DevExpress.Maui.DataGrid;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using Microsoft.Maui.Controls;
using static Bumptech.Glide.DiskLruCache.DiskLruCache;

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

	private async void Save_Clicked(object sender, EventArgs e)
	{
		DataGridView collectionView = new DataGridView();
       // collectionView.Commands.va();
        //this.co
       // collectionView.Commands.ValidateAndSave();


        await DatabaseViewModel.UpdateOrderAsync();


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
		DataGridView dataGridView = (DataGridView)sender;
       // dataGridView.Commands.va
    }

	// sheet add or edit
	private async void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
	{
        //	DatabaseViewModel.ValidateSheets(e);

        //collectionView.com
        if (e.Item is not ExcelSheet item)
            return;
		if(DatabaseViewModel.CurrentOrder is null)
            return;
        if (e.DataChangeType== DevExpress.Maui.Core.DataChangeType.Add)
		{

			var c= DatabaseViewModel.CurrentOrder.ExcelSheetsCount;
			item.SheetNo = c+1;
        }
		if (e.DataChangeType== DevExpress.Maui.Core.DataChangeType.Edit)
		{
			var v=  this.BindingContext;


        }


       await DatabaseViewModel.Validate(e);

    }

    private void collectionView_ChildAdded(object sender, ElementEventArgs e)
    {

    }



	private async void Swipe_Delete(object sender, DevExpress.Maui.DataGrid.SwipeItemTapEventArgs e)
	{
		
    }

    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
        if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
        {
			var item= form.Item;

                if (form.DataControlContext is DatabaseViewModel viewmodel)
                {
                    DatabaseViewModel = viewmodel;

                }
            
        }
    }
}