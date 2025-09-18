using DevExpress.Maui.Core;
using DevExpress.Maui.Editors;
using DevExpress.Spreadsheet;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DXApplication2.Views;

public partial class NewSheetView : ContentPage
{
	public NewSheetView()
	{
		InitializeComponent();


	}

	ExcelSheet Sheet;
    DatabaseViewModel Viewmodel;

	void SetToken()
	{
        if (Sheet == null) return ;
        if (Sheet.Checkers == null) return ;
      
        var items = new ObservableCollection<object>();
        foreach (var item in Sheet.Checkers)
        {
            items.Add(new CheckerTable { Name = item });
        }
      // this.checkerList1.SelectedValues=   items;
    }
    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
        if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
        {
			
            //form.DataControlContext.DataControlContext
            if( form.Item is ExcelSheet sh)            
                Sheet = sh;
			//SetToken();
            if (form.DataControlContext is DetailEditFormViewModel sheetform)
            {
				

				if (sheetform.DataControlContext is DatabaseViewModel viewmodel)
				{
					Viewmodel = viewmodel;
					Viewmodel.CurrentSheet = Sheet;
					Viewmodel.SheetChanged();

				}
                

            }
        }

    }

	private void inputChipGroup_Completed(object sender, DevExpress.Maui.Editors.CompletedEventArgs e)
	{
		var chipGroup = sender as InputChipGroup;
		if (chipGroup.EditorText == null) return;
			if (chipGroup.EditorText.Length <= 1)
		{
			e.ClearEditorText = false;
		}
		else
		{
			if(chipGroup.ItemsSource is List<object> list){

				list.Add(chipGroup.EditorText);
			}
			if (this.BindingContext is DetailEditFormViewModel form)
			{

				if (form.DataControlContext is DetailEditFormViewModel form1)
				{
					//form1.DeleteCommand
					if (form1.DataControlContext is DatabaseViewModel vm) vm.AddPeople(chipGroup.EditorText);
				}
			}
			// AddPeople
		//	IList<CheckerTable> list = chipGroup.ItemsSource as BindingList<CheckerTable>;
		//	list.Add(new CheckerTable() { Name = chipGroup.EditorText });
		}
	}

	private void TokenEdit_SelectionChanged(object sender, EventArgs e)
	{

	}

	private async void DXButton_Clicked(object sender, EventArgs e)
	{
		if (this.BindingContext is DetailEditFormViewModel form)
		{
			if(form.Item is ExcelSheet sheet)
            {//-		checkerList1.SelectedValues	Count = 2	DevExpress.Maui.Core.Internal.ObservableCollectionCore<object>
				var test = sheet.Checkers;

                if (Viewmodel.SelectedPeoples != null)
				{
					
						var oblist = Viewmodel.SelectedPeoples.Select(x => x.Name).ToList();
						sheet.Checkers = oblist;
                       sheet.Checker = string.Join("-", oblist);
				}
			

				if (sheet.Checked == null) sheet.Checked = "-";

				if (sheet.Checkers == null || sheet.Checkers.Count == 0)
				{
					await Shell.Current.DisplayAlert("確認", "検査者を指定してください", "OK");
					return;
				}
				if (sheet.SheetNo == 0)
				{
					await Shell.Current.DisplayAlert("確認", "シートNoを入力してください", "OK");
					return;
				}
					if (string.IsNullOrEmpty( sheet.Kiki1 ) || string.IsNullOrEmpty(sheet.Kiki2) )
				{
					await Shell.Current.DisplayAlert("確認", "機器名を入力してください", "OK");
					return;
				}

					if ( sheet.CheckDate2 ==null  )
				{
					await Shell.Current.DisplayAlert("確認", "管入荷日を入力してください", "OK");
					return;
				}

				//form.CloseOnSave = false;
				//await form.SaveAsync();
               // form.CloseOnSave = true;

                this.slideView.Commands.ShowNext.Execute(null);
				//this.slideView...Execute(null);
				

				//	form.CloseOnSave = false;
				//await form.SaveAsync();
				
			//	this.spoolGrid.Commands.ShowDetailNewItemForm.Execute(null);
			}

		}
	}

    private async void collectionView_ValidateAndSave(object sender, ValidateItemEventArgs e)
    {
		if (e.Item is not LiningSpool sp) return;	
		if (Sheet!=null)
		{		
			
			Viewmodel.CheckSpool(Sheet, e);
		}
		

	}

    private void spoolGrid_Tap(object sender, DevExpress.Maui.CollectionView.CollectionViewGestureEventArgs e)
    {

    }

	private void TextEdit_Completed(object sender, EventArgs e)
	{
		TextEdit textEdit = sender as TextEdit;
		if (textEdit.Text.Trim() == "") {
            textEdit.HasError = false;
            return; }
		if (Sheet == null) return;

            if (textEdit.Text.StartsWith("."))
		{
			if (Sheet != null)
			{
				textEdit.Text = $"{Sheet.ThickNess}{textEdit.Text}";
			}
		}
		double d = 0;
		if(!Double.TryParse(textEdit.Text,out d))
		{
			textEdit.HasError = true;
        }else
		{
			
            textEdit.HasError = d< Sheet.ThickNess;
        }
        var grid = textEdit.Parent as Grid;

		var ind = grid.Children.ToList().FindIndex(x => x == textEdit);

		if(grid.Children.Count > ind+1)
			grid.Children[ind + 1].Focus();
	}

    private void DXButton_Clicked_1(object sender, EventArgs e)
    {

    }

    private async void Next_Clicked(object sender, EventArgs e)
    {

		if (Sheet.Spools.Count == 0) {
            await Shell.Current.DisplayAlert("確認", "管を追加してください", "OK");
            return;
        }
        this.slideView.Commands.ShowNext.Execute(null);
    }

    private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
    {

    }
}