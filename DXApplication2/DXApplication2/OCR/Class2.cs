using Microsoft.Maui.Graphics.Platform;
using SampleApp.DI;
using Xamarin.Google.Android.Odml.Image;
using Xamarin.Google.MLKit.Vision.Text;
using Xamarin.Google.MLKit.Vision.Text.Japanese;
using IImage = Microsoft.Maui.Graphics.IImage;

// 名前空間はプロジェクトに合わせてください。
namespace SampleApp.Platforms.Android.DI;

public class OcrServiceDI : IOcrServiceDI
{
	/// <inheritdoc />
	public async Task<List<Text.TextBlock>> GetTextAsync(IImage image)
	{
		TaskCompletionSource<List<Text.TextBlock>>? tcs = new();

		ITextRecognizer recognizer = TextRecognition.GetClient(new JapaneseTextRecognizerOptions.Builder().Build());
		MlImage mlImage = new BitmapMlImageBuilder(image.AsBitmap()).Build();
		
		recognizer.Process(mlImage).AddOnSuccessListener(new OnSuccessListener<Text>(
			text =>
			{
				tcs.TrySetResult(text.TextBlocks.ToList());
			}));

		return await tcs.Task;
	}
}