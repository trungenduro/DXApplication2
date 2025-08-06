using DevExpress.CodeParser;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataGrid;
using DXApplication2.ViewModels;
using LiningCheckRecord;

namespace DXApplication2.Views;

public partial class NewSpoolPage : ContentPage
{
	public NewSpoolPage()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;
	public NewSpoolPage(DatabaseViewModel model)
	{

		DatabaseViewModel = model;
		this.BindingContext = model;
		InitializeComponent();
	}

    ExcelSheet Sheet;
    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
        if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
        {
            //form.DataControlContext.DataControlContext
            var sp = form.Item as LiningSpool;
            if (form.DataControlContext is DetailEditFormViewModel sheetform)
            {
                if( sheetform.Item is ExcelSheet sh)
                {
                    Sheet = sh;
                }
                if(sheetform.DataControlContext is DetailEditFormViewModel form1)
                {
                    if (form1.DataControlContext is DatabaseViewModel viewmodel)
                        DatabaseViewModel = viewmodel;
                }

            }
        }
    }

	private void DXButton_Clicked(object sender, EventArgs e)
	{

			//Navigation.PushAsync(new OCRPage());
		
	}
}