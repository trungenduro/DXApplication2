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

                }
            
        }
    }

	private async void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
	{
		if (e.Item is not ExcelSheet item)
			return;
		if (DatabaseViewModel.CurrentOrder is null)
			return;
		if (e.DataChangeType == DevExpress.Maui.Core.DataChangeType.Add)
		{

			var c = DatabaseViewModel.CurrentOrder.ExcelSheetsCount;
			item.SheetNo = c + 1;
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
	}
}