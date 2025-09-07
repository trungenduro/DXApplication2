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

    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
        if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
        {
            //form.DataControlContext.DataControlContext
            if( form.Item is ExcelSheet sh)            
                Sheet = sh;

            if (form.DataControlContext is DetailEditFormViewModel sheetform)
            {
               
                if (sheetform.DataControlContext is DetailEditFormViewModel form1)
                {
                    if (form1.DataControlContext is DatabaseViewModel viewmodel)
                        Viewmodel = viewmodel;
                    Viewmodel.CurrentSheet = Sheet;

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
			{
				if(checkerList1.SelectedValues is ObservableCollection<object> list)
				{
					if (list.Count == 0)
					{

					}
					else
					{
						var oblist = list.Where(x => x is CheckerTable).Select(x => x as CheckerTable).Select(x => x.Name as object).ToList();
						sheet.Checkers = oblist;
					}
				}
				if (sheet.Checked == null) sheet.Checked = "-";
				
				//	form.CloseOnSave = false;
				await form.SaveAsync();
				
			//	this.spoolGrid.Commands.ShowDetailNewItemForm.Execute(null);
			}

		}
	}

    private void collectionView_ValidateAndSave(object sender, ValidateItemEventArgs e)
    {

    }

    private void spoolGrid_Tap(object sender, DevExpress.Maui.CollectionView.CollectionViewGestureEventArgs e)
    {

    }
}