using DevExpress.Maui.Core;
using DevExpress.Maui.DataGrid;
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

		DataGridView dataGridView = new DataGridView();
        //dataGridView.Commands.ShowDetailEditForm

    }

	private void Save_Clicked(object sender, EventArgs e)
	{
        DatabaseViewModel.UpdateOrderAsync();
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

	//add edit spool
    private async void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
    {
		if( this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{

			if(  form.DataControl is  DataGridView grid)
			{
				//grid.Commands.ShowDetailEditForm
				if(grid.BindingContext is DatabaseViewModel viewmodel)
				{


				}
			}

            if (e.Item is LiningSpool sp)
			{
				string mess = "";

                if (sp.SpoolNo == null || sp.SpoolNo == "")
                {
                    mess += "ä«î‘çÜñ¢ì¸óÕ\n";
                }
                if (sp.Size == null || sp.Size == "")
                {
                    mess += "ä«ÉTÉCÉYñ¢ì¸óÕ\n";
                }
                if (mess != "")
				{

					e.IsValid = false;
                    await Shell.Current.DisplayAlert("Error", mess, "OK");
                    return;
				}

			}

            //form.SaveCommand.execute
           // await form.SaveAsync();
        }

    }
	DevExpress.Maui.Core.DetailEditFormViewModel editform;

    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
		if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{
			editform = form;

            if (form.DataControlContext is DetailEditFormViewModel form1)
			{

				if (form1.DataControlContext is DatabaseViewModel viewmodel)
				{
                    DatabaseViewModel = viewmodel;

                }
			}
		}
	}

    private void GridSwipeItem_Tap(object sender, SwipeItemTapEventArgs e)
    {

    }

    private void delete_Tap(object sender, SwipeItemTapEventArgs e)
    {
        if (e.Item is LiningSpool sp)
        {
            spoolGrid.DeleteRow(e.RowHandle);
            DatabaseViewModel.DeleteSpoolAsync(sp).Wait();
        }
    }
}