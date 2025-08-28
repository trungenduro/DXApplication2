using Android.Media;
using CommunityToolkit.Maui.Views;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Layouts;
using SampleApp.DI;
using System.IO;
using IImage = Microsoft.Maui.Graphics.IImage;
namespace DXApplication2.Views;


public class CameraResultEventArgs : EventArgs
{
	public string Result { get; set; }
}

public partial class CameraView : ContentPage
{
	private IOcrServiceDI _ocrServiceDI;

	public CameraView()
	{
		InitializeComponent();
    }
	LiningSpool LiningSpool;
    public CameraView(LiningSpool spool)
	{
		InitializeComponent();
        LiningSpool = spool;
		 
		Directory.CreateDirectory(Path.Combine(FileSystem.AppDataDirectory, "Photo"));
		
	}

	string img = $"photo_{DateTime.Now:yyyyMMdd_HHmmss}.png";



	public event EventHandler<CameraResultEventArgs> OcrCompleted;

	protected void OnOcrCompleted(string result)
	{
		OcrCompleted?.Invoke(this, new CameraResultEventArgs { Result = result });
	}


	MemoryStream stream;

	private async void OnCaptureClicked(object sender, EventArgs e)
{

	

		//	cameraView.StartCameraPreview();
		var photo = await cameraView.CaptureImage(CancellationToken.None);
		cameraView.IsEnabled = false;

		if (photo is null) return;


		stream = new MemoryStream();
		await photo.CopyToAsync(stream);

		this.CapturedImage.Source = ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));

		
	
		var filePath = Path.Combine(FileSystem.AppDataDirectory,"Photo", img );


		using var fileStream = File.OpenWrite(filePath);
		await stream.CopyToAsync(fileStream);
		LiningSpool.ImagePath = img;
		LiningSpool.SpoolType = 4;
		//var picname = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LiningSpool.SpoolNo + ".png");

		cameraView.IsEnabled = true;
	}
	readonly string imagePath;
	async void HandleStartCameraPreviewButtonTapped()
	{
		try
		{
			var startCameraPreviewTCS = new CancellationTokenSource(TimeSpan.FromSeconds(3));

			// Use the Camera field defined above in XAML (`<toolkit:CameraView x:Name="Camera" />`)
			await cameraView.StartCameraPreview(startCameraPreviewTCS.Token);
		}
		catch (Exception e)
		{
			// Handle Exception
			//Trace.WriteLine(e);
		}
	}

	private void ContentPage_Appearing(object sender, EventArgs e)
	{
		var displayWidth = cameraView.WidthRequest;
		var displayHeight = cameraView.Height;
	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		if(stream != null)
		{


			OnOcrCompleted(LiningSpool.ImagePath);
		}
		

	}
}