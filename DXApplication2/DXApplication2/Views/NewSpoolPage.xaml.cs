using DemoCenter.Maui.Views;
using DevExpress.CodeParser;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataGrid;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DXApplication2.Views;

public partial class NewSpoolPage : ContentPage
{
	public NewSpoolPage()
	{
		InitializeComponent();
		finished = true;
	}

	DatabaseViewModel DatabaseViewModel;
	public NewSpoolPage(DatabaseViewModel model)
	{

		DatabaseViewModel = model;
		this.BindingContext = model;
		InitializeComponent();

		finished = true;
	}
	bool finished = false;
    LiningSpool sp;

    ExcelSheet Sheet;
    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
        if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
        {
            //form.DataControlContext.DataControlContext
             sp = form.Item as LiningSpool;
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

	private async void TakePhotoClicked(object sender, EventArgs args)
	{
		if (!MediaPicker.Default.IsCaptureSupported)
			return;

		try
		{
			var photo = await MediaPicker.Default.CapturePhotoAsync();
			try
			{
				await ProcessResult(photo);
			}
			catch (Exception e1)
			{

				
			}
			
		}
		catch (PermissionException ex)
		{
			await DisplayAlert("Permission Denied", ex.Message, "OK");
		}
	}

	private async Task ProcessResult(FileResult result)
	{
		if (result == null)
			return;

		
	
		var editorPage = new ImageEditView(result.FullPath);
		await Navigation.PushAsync(editorPage);
		var cropResult = await editorPage.WaitForResultAsync();
		editorPage.Handler.DisconnectHandler();
		ImageSource imageSource = null;
		
		sp.SpoolType = 4;
		type.SelectedIndex = 4;
		sp.ImagePath = result.FullPath;

		if (cropResult != null)
		{
			sp.ImagePath = cropResult;
			
			//imageSource = ImageSource.FromFile(cropResult);
		}else
		{
			if (System.IO.Path.IsPathRooted(result.FullPath))
				imageSource = ImageSource.FromFile(result.FullPath);
			else
			{
				imageSource = ImageSource.FromStream(() => result.OpenReadAsync().Result);
			}
		}

	//	preview.Source = imageSource;

	
	//	preview.IsVisible = true;
	
		
	
	}

	private  async Task Capture()
	{
        var ocrpage = new CameraView(sp);


		ocrpage.OcrCompleted += (s, e) =>
		{
			// Handle result			
			Navigation.PopAsync(); // Go back
		//	this.image.Source = ImageSource.FromFile(Path.Combine(FileSystem.AppDataDirectory, "Photo", e.Result));
		//	this.UpdateChildrenLayout();
		};


		await Navigation.PushAsync(ocrpage);


        var str = "";
		//image.Source = ImageSource.FromFile(() => stream);

	}

	private void type_SelectionChanged(object sender, EventArgs e)
	{
		if (!finished) return;
		var x = sender;
       
	}

	private void DXButton_Clicked(object sender, EventArgs e)
	{
        Capture();
	}

    private void DXButton_Clicked_1(object sender, EventArgs e)
    {

    }

    private async void FreeHandClicked(object sender, EventArgs e)
    {
	
        var editorPage = new ImageEditView(sp);
        await Navigation.PushAsync(editorPage);
        var cropResult = await editorPage.WaitForResultAsync();
        editorPage.Handler.DisconnectHandler();    

        sp.SpoolType = 5;
        type.SelectedIndex = 5;
    }
}