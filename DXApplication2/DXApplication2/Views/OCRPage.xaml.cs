using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Layouts;
using SampleApp.DI;
using System.IO;
using IImage = Microsoft.Maui.Graphics.IImage;
namespace DXApplication2.Views;

public partial class OCRPage : ContentPage
{
	private IOcrServiceDI _ocrServiceDI;

	public OCRPage(IOcrServiceDI ocrServiceDI)
	{
		InitializeComponent();
		_ocrServiceDI = ocrServiceDI;
		//imagePath = Path.Combine(fileSystem.CacheDirectory, "camera-view-image.jpg");
	//	cameraView.in += loaded;

		
	}

	private void loaded(object? sender, EventArgs e)
	{
		int width = (int)cameraView.ImageCaptureResolution.Width;
		int height = (int)cameraView.ImageCaptureResolution.Height;

		double aspectRatio =16.0/9.0;

		// Apply aspect ratio to scale height based on current width
		double displayWidth = cameraView.Width;
		double scaledHeight = displayWidth * aspectRatio;

		cameraView.WidthRequest = cameraView.Height * aspectRatio;
		overlayContainer.WidthRequest = cameraView.Height * aspectRatio;
	}

	private async void OnCaptureClicked(object sender, EventArgs e)
{
	//	cameraView.StartCameraPreview();
		var photo = await cameraView.CaptureImage(CancellationToken.None);
    if (photo is null) return;

		//cameraView.pa();


		// ストリームを複製
		using var memoryStream = new MemoryStream();
		await photo.CopyToAsync(memoryStream);
		//memoryStream.Position = 0;

		// 画像表示用にコピー
	//	img.Source = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));

		// OCR用にコピー
		using var ocrStream = new MemoryStream(memoryStream.ToArray());
		var image = PlatformImage.FromStream(ocrStream);


		var imageWidth = image.Width;
		var imageHeight = image.Height;

		var displayWidth = cameraView.Width;
		var displayHeight = cameraView.Height;

		var scaleX = displayWidth / imageWidth;
		var scaleY = displayHeight / imageHeight;



		var textBlocks =  await _ocrServiceDI.GetTextAsync(image);



		overlayContainer.Children.Clear();

		foreach (var block in textBlocks)
		{
			var rect = block.BoundingBox;

			// Get image and display dimensions
		

			// Scale bounding box coordinates
		

			var scaledLeft = rect.Left * scaleX;
			var scaledTop = rect.Top * scaleY;
			var scaledWidth = rect.Width() * scaleX + 10;
			var scaledHeight = rect.Height() * scaleY + 10 ;

			// Determine text color based on confidence
		

			var label = new Label
			{
				Text = block.Text,
				TextColor = block.Lines[0].Confidence > 0.6 ? Colors.Blue : Colors.Red,
				BackgroundColor = block.Lines[0].Confidence > 0.6 ? Colors.Yellow :  Colors.White,
				FontSize = 12,
				LineBreakMode = LineBreakMode.WordWrap,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start
			};

			
			//label.re = scaledWidth;

			AbsoluteLayout.SetLayoutBounds(label, new Rect(scaledLeft, scaledTop, scaledWidth, scaledHeight));
			AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.None);

			overlayContainer.Children.Add(label);
			for (int i = 0; i < 10; i++)
			{

				//if (label.Width < scaledWidth) label.FontSize++;
			}
		}

		var startCameraPreviewTCS = new CancellationTokenSource(TimeSpan.FromSeconds(1));

		// Use the Camera field defined above in XAML (`<toolkit:CameraView x:Name="Camera" />`)
		//await cameraView.StartCameraPreview(startCameraPreviewTCS.Token);
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
}