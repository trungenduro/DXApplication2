using AndroidX.Lifecycle;

using DevExpress.Maui.Charts;
using DevExpress.Maui.Core;

using DevExpress.Maui.Editors;

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

	//	DataGridView dataGridView = new DataGridView();
        //dataGridView.Commands.ShowDetailEditForm

    }

	private void Save_Clicked(object sender, EventArgs e)
	{
        DatabaseViewModel.UpdateOrderAsync();
    }

	private void collectionView_CreateDetailFormViewModel(object sender, DevExpress.Maui.Core.CreateDetailFormViewModelEventArgs e)
	{

	}



	//add edit spool
    private async void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
    {
		if( this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{

			

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

	private void TextEdit_Completed(object sender, EventArgs e)
	{
		TextEdit textEdit = sender as TextEdit;
		if(textEdit == null) return;
        if (textEdit.Text.StartsWith("."))
		{
			if (Sheet != null)
			{
				textEdit.Text = $"{Sheet.ThickNess}{textEdit.Text}";
			}
		}
		var grid = textEdit.Parent as Grid;

		if (grid == null) return;
		var ind = grid.Children.ToList().FindIndex(x => x == textEdit);

		grid.Children[ind + 1].Focus();


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
