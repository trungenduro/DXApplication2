using CommunityToolkit.Maui.Media;
using DemoCenter.Maui.Views;
using DevExpress.Android.Editors;
using DevExpress.CodeParser;
using DevExpress.Data.Extensions;
using DevExpress.DataAccess.Native.Sql;
using DevExpress.Maui.Core;
using DevExpress.Maui.Editors;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using SampleApp.DI;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextEdit = DevExpress.Maui.Editors.TextEdit;

namespace DXApplication2.Views;

public partial class NewSpoolPage : ContentPage
{
	private ISpeechToText speechToText;
	public NewSpoolPage( )
	{
		InitializeComponent();
	}
	void OnHandlerChanged(object sender, EventArgs e)
	{
		if (Handler != null)
			speechToText = Handler.MauiContext.Services.GetService<ISpeechToText>();
	}

	private CancellationTokenSource tokenSource = new CancellationTokenSource();
	DatabaseViewModel DatabaseViewModel;
	public NewSpoolPage(DatabaseViewModel model)
	{

		DatabaseViewModel = model;
		this.BindingContext = model;
		InitializeComponent();

		//finished = true;
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
		//type.SelectedIndex = 4;
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

		preview.Source = imageSource;

	
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

	private async Task DXButton_Clicked(object sender, EventArgs e)
	{
       await Capture();
	}

    private async void DXButton_Clicked_1(object sender, EventArgs e)
    {
		if(this.edasize.SelectedValue == null) return;

		if (this.size1.Text.Contains("x"))
		{
			var s1 = this.size1.Text.Split('x').First();

			if (DatabaseViewModel != null && Sheet != null)
			{

				var i1 = DatabaseViewModel.Sizes.FindIndex(x => x == s1);

				if (i1 > -1)
				{
					if(i1 < edasize.SelectedIndex)
					{
						await DisplayAlert("Error", "Second size must be less than or equal to first size", "OK");
                        return;
					}
				}
			}
		}

            if (this.size1.Text=="") this.size1.Text += $"{this.edasize.SelectedValue}";
			else
        this.size1.Text += $"x{this.edasize.SelectedValue}";
		//this.tokenChip.item. .ite 
    }

    private async void FreeHandClicked(object sender, EventArgs e)
    {
	
        var editorPage = new ImageEditView(sp);
        await Navigation.PushAsync(editorPage);
        var cropResult = await editorPage.WaitForResultAsync();
        editorPage.Handler.DisconnectHandler();    

        
        if (cropResult != null)
        {
            sp.ImagePath = cropResult;
            sp.SpoolType = 4;
			preview.Source = ImageSource.FromFile(cropResult);
        } 
		
    }

	private void types_SelectionChanged(object sender, EventArgs e)
	{
		if (finished)
		{
			if(this.types.SelectedIndex < 3)
			{
				var file= $"type{this.types.SelectedIndex + 1}.png";
				preview.Source = ImageSource.FromFile(file);
			}
				
			if (this.types.SelectedIndex == 3)
				TakePhotoClicked(sender, e);	
			if (this.types.SelectedIndex == 4)
				FreeHandClicked(sender, e);
				
		}
	}

	private void ContentPage_Loaded(object sender, EventArgs e)
	{
		finished = true;
	}

	private void TextEdit_Completed(object sender, EventArgs e)
	{
		TextEdit textEdit = sender as TextEdit;
		if (textEdit.Text.Trim() == "")
		{
			textEdit.HasError = false;
			return;
		}
		if (Sheet == null) return;

		if (textEdit.Text.StartsWith("."))
		{
			if (Sheet != null)
			{
				textEdit.Text = $"{Sheet.ThickNess}{textEdit.Text}";
			}
		}
		double d = 0;
		if (!Double.TryParse(textEdit.Text, out d))
		{
			textEdit.HasError = true;
		}
		else
		{

			textEdit.HasError = d < Sheet.ThickNess;
		}
		var grid = textEdit.Parent as Grid;

		var ind = grid.Children.ToList().FindIndex(x => x == textEdit);

		if (grid.Children.Count > ind + 1)
			grid.Children[ind + 1].Focus();
	}

	private void DXButton_Clicked_2(object sender, EventArgs e)
	{

	}

	private void Micro_Clicked(object sender, EventArgs e)
	{
		if(sender is not DXButton btn) return;
		var grid = btn.Parent as DXStackLayout;
		var ind= grid.Children.FindIndex(x => x == btn);
		if (grid.Children.Count > ind + 1)
		{
			//grid.Children[ind + 1].Focus();
		}
		Listen();
	}

	private async void Listen()
	{
		string RecognitionText= "";
		var isAuthorized = await speechToText.RequestPermissions();
		if (isAuthorized)
		{
			try
			{
				RecognitionText = await speechToText.Listen(CultureInfo.GetCultureInfo("ja-JP"),
					new Progress<string>(partialText =>
					{
						if (DeviceInfo.Platform == DevicePlatform.Android)
						{
							RecognitionText = partialText;
						}						
						
					}), tokenSource.Token);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", ex.Message, "OK");
			}
		}
		else
		{
			await DisplayAlert("Permission Error", "No microphone access", "OK");
		}
		test.Text += RecognitionText;
	}
	string RecognitionText = "";




	async Task StartListening(CancellationToken cancellationToken)
	{
		var isGranted = await SpeechToText.Default.RequestPermissions(cancellationToken);
		if (!isGranted)
		{
			//await Toast.Make("Permission not granted").Show(CancellationToken.None);
			return;
		}

		var recognitionResult = await SpeechToText.Default.ListenAsync(
			CultureInfo.GetCultureInfo("ja-JP"),
			new Progress<string>(partialText =>
			{
				RecognitionText += partialText + " ";
			}),
			cancellationToken);

		if (recognitionResult.IsSuccessful)
		{
			RecognitionText = recognitionResult.Text;
			// You can now parse numbers from RecognitionText
		}
		else
		{
			//await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech").Show(CancellationToken.None);
		}
	}


}