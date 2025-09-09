using CommunityToolkit.Maui.Views;
using DevExpress.Maui.CollectionView;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataGrid;
using DevExpress.Spreadsheet;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using Microsoft.Maui.Controls;
using static Bumptech.Glide.DiskLruCache.DiskLruCache;

namespace DXApplication2.Views;

public partial class SheetsPage1 : ContentPage
{
	public SheetsPage1()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;

	DHFOrder? dhfOrder;



	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		DXCollectionView collectionView = new DXCollectionView();
		//collectionView.ShowDetailEditForm();
		//new DetailEditFormViewModel()
		//collectionView.Commands.de.de
	}


    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
        if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
        {		
			
			form.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(DevExpress.Maui.Core.DetailEditFormViewModel.Item))
				{
					
				}
			};
			var item= form.Item;

                if (form.DataControlContext is DatabaseViewModel viewmodel)
                {
                    DatabaseViewModel = viewmodel;
				if (form.Item is DHFOrder order)
					DatabaseViewModel.CurrentOrder = order;
                }
            
        }
    }

	private async void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
	{
		

		if (e.Item is not ExcelSheet item)
			return;
		if (DatabaseViewModel.CurrentOrder is null)
			return;
		if (item.Checked == null) item.Checked = "-";
		if (e.DataChangeType == DevExpress.Maui.Core.DataChangeType.Add)
		{

			var c = DatabaseViewModel.CurrentOrder.ExcelSheetsCount;
			item.SheetNo = c + 1;

			//sheetGrid1.ShowDetailEditForm(sheetGrid1..)
		}
		if (e.DataChangeType == DevExpress.Maui.Core.DataChangeType.Edit)
		{
			var v = this.BindingContext;

		}
		if (e.DataChangeType == DevExpress.Maui.Core.DataChangeType.Delete)
		{
			var v = this.BindingContext;

			bool confirm = await Application.Current.MainPage.DisplayAlert(
					"確認", $"シート No.{item.SheetNo} を削除しますか？", "はい", "キャンセル");
			if (!confirm) e.IsValid = false;
			else
			{
				if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
				{

					if (form.Item is DHFOrder order)
					{

					}

				}
			}

		}
		await DatabaseViewModel.Validate(e);
		if (e.DataChangeType == DevExpress.Maui.Core.DataChangeType.Add)
		{
			sheetGrid1.SelectedItem = item;
			var in1=	sheetGrid1.FindItemHandle(e.Item);
			
			//this.sheetGrid1.ShowDetailEditForm(in1);
			//	DetailEditFormViewModel form = new DetailEditFormViewModel(DatabaseViewModel)

		}

	}

	private void ToolbarButton_Clicked(object sender, EventArgs e)
	{
		


	}

    private void sheetGrid1_Tap(object sender, CollectionViewGestureEventArgs e)
    {

    }
    ExcelSheet ActiveSheet;
    int ActiveHandle = -1;

    private void spoolGrid_Tap(object sender, DevExpress.Maui.CollectionView.CollectionViewGestureEventArgs e)
    {
        this.popup.IsOpen = true;
        if (e.Item is ExcelSheet sp)
        {
            ActiveSheet = sp;
            ActiveHandle = e.ItemHandle;
            DatabaseViewModel.CurrentSheet = sp;
        }

	//	this.sheetGrid1.DeleteItem(ActiveHandle);
	}

    private void DismissPopup(object sender, EventArgs e)
    {
        popup.IsOpen = false;
    }

    private async void DeleteClick(object sender, EventArgs e)
    {
		if (this.sheetGrid1.GetItem(ActiveHandle) is ExcelSheet sheet)
		{
			if (sheet.Spools.Count > 0)
			{
				await Shell.Current.DisplayAlert("確認", $"シート {sheet.SheetNo} に管が存在します。先に管を削除してください。", "OK");
				popup.IsOpen = false;
				return;
			}

			//	await Shell.Current.DisplayAlert("確認", mess, "OK");
			bool confirm = await Shell.Current.DisplayAlert(
						"確認", $"シート {ActiveSheet.SheetNo} を削除しますか？", "はい", "キャンセル");
			if (!confirm) return;
			this.sheetGrid1.DeleteItem(ActiveHandle);

			await DatabaseViewModel.DeleteSheetAsync(sheet);
		}
		popup.IsOpen = false;
    }

    private void EditClick(object sender, EventArgs e)
    {
        popup.IsOpen = false;
        //if(sender is  Microsoft.Maui.Controls.View view)
        //  popup.PlacementTarget = view;
        if (ActiveSheet != null) this.sheetGrid1.ShowDetailEditForm(ActiveHandle);
    }
}