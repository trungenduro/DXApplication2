using AndroidX.Lifecycle;
using DevExpress.Maui.Charts;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataGrid;
using DevExpress.Spreadsheet;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using static Android.Icu.Text.IDNA;

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
				if (Sheet != null)
				{
					DatabaseViewModel.CheckSpool(Sheet, e);

				}
				
			}

            DatabaseViewModel.Validate(e);
            //form.SaveCommand.execute
            // await form.SaveAsync();
        }

    }
	DevExpress.Maui.Core.DetailEditFormViewModel editform;

	DHFOrder order;
	ExcelSheet Sheet;

	private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
		if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{
			editform = form;
			if(form.Item is ExcelSheet sh)
				Sheet = sh;
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

	LiningSpool ActiveSpool;
	int ActiveHandle = -1;

	private void spoolGrid_Tap(object sender, DevExpress.Maui.CollectionView.CollectionViewGestureEventArgs e)
	{
		popup.IsOpen = true;
		if (e.Item is LiningSpool sp)
		{
			ActiveSpool = sp;
            ActiveHandle= e.ItemHandle;
			DatabaseViewModel.CurrentSpool = sp;
        }
		
		
	}

	private void DismissPopup(object sender, EventArgs e)
	{
		popup.IsOpen = false;
	}

	private async void DeleteClick(object sender, EventArgs e)
	{
		
		popup.IsOpen = false;
		bool confirm = await  Shell.Current.DisplayAlert(
                    "確認", $"管 {ActiveSpool.SpoolNo} を削除しますか？", "はい", "キャンセル");
		if (!confirm) return;
	

			this.spoolGrid.DeleteItem(ActiveHandle);
	 await	DatabaseViewModel.DeleteSpoolAsync(ActiveSpool);

		
	}

	private void EditClick(object sender, EventArgs e)
	{
        popup.IsOpen = false;
        //if(sender is  Microsoft.Maui.Controls.View view)
        //  popup.PlacementTarget = view;
      if(ActiveSpool!=null)  this.spoolGrid.ShowDetailEditForm(ActiveHandle);
    }

    private void PhotoEditClick(object sender, EventArgs e)
    {

    }


   
    }

public class CustomColorizer : ICustomPointColorizer
{

    public ILegendItemProvider GetLegendItemProvider()
    {
        return null;
    }

    public Color GetColor(ColoredPointInfo info)
    {
		if (info.Value < 1)
			return Colors.Red;
        return Colors.Cyan;
    }
}
