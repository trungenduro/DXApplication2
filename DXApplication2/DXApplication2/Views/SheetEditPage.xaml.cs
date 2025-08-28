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
                    mess += "管番号未入力\n";
                }
				else if (order != null)
				{
					if(e.DataChangeType== DataChangeType.Add && order.SpoolNames.Where(x=>x.Equals(sp.SpoolNo)).Any())
					{
                        mess += "管番号重複\n";
                    }
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

				if(e.DataChangeType== DataChangeType.Add)
				{
					if(sp.SpoolType == 3)
					{
						var ocrpage = new CameraView(sp);

						Navigation.PushAsync(ocrpage);

					}
				}
			}

            //form.SaveCommand.execute
           // await form.SaveAsync();
        }

    }
	DevExpress.Maui.Core.DetailEditFormViewModel editform;

	DHFOrder order;

    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
		if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{
			editform = form;

            if (form.DataControlContext is DetailEditFormViewModel form1)
			{
				order = form1.Item as DHFOrder;				
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
           // spoolGrid.DeleteRow(e.RowHandle);
          //  DatabaseViewModel.DeleteSpoolAsync(sp).Wait();
        }
    }

	private void spoolGrid_Tap(object sender, DevExpress.Maui.CollectionView.CollectionViewGestureEventArgs e)
	{
		popup.IsOpen = true;
		
	}

	private void DismissPopup(object sender, EventArgs e)
	{
		popup.IsOpen = false;
	}

	private void DeleteClick(object sender, EventArgs e)
	{

	}

	private void EditClick(object sender, EventArgs e)
	{
		//popup.IsOpen = true;
		//if(sender is  Microsoft.Maui.Controls.View view)
		//  popup.PlacementTarget = view;
	}
}