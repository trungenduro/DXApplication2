using DemoCenter.Maui.Views;
using DevExpress.Maui.Core;
using DevExpress.Maui.Editors;
using DevExpress.Spreadsheet;
using DXApplication2.ViewModels;
using LiningCheckRecord;

namespace DXApplication2.Views;

public partial class EditSpoolPage : ContentPage
{
	public EditSpoolPage()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;
	public EditSpoolPage(DatabaseViewModel model)
	{
		DatabaseViewModel = model;
		this.BindingContext = model;
		InitializeComponent();
	}

	LiningSpool Spool;
	private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
		
		if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{
			//form.DataControlContext.DataControlContext
			Spool = form.Item as LiningSpool;			
		}
	}

	private void SfImageEditor_AnnotationSelected(object sender, Syncfusion.Maui.ImageEditor.AnnotationSelectedEventArgs e)
	{

    }

	private async void DXButton_Clicked(object sender, EventArgs e)
	{
		if(Spool==null) return;
		var editorPage = new ImageEditView(Spool);
		await Navigation.PushAsync(editorPage);
		var cropResult = await editorPage.WaitForResultAsync();
		editorPage.Handler.DisconnectHandler();
		if (cropResult != null)
		{
			image.Source = ImageSource.FromFile(Spool.ImagePath);
		}

	//	ImageSource imageSource = null;
			

	}

    private void TextEdit_Completed(object sender, EventArgs e)
    {
		

        TextEdit textEdit = sender as TextEdit;

		var grid = textEdit.Parent as Grid;

        var ind= grid.Children.ToList().FindIndex(x => x == textEdit);

		grid.Children[ind+1].Focus();
        // textEdit.fo = textEdit.Text.ToUpper();
    }
}