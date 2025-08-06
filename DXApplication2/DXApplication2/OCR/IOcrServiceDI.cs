using Xamarin.Google.MLKit.Vision.Text;
using IImage = Microsoft.Maui.Graphics.IImage;

// 名前空間はプロジェクトに合わせてください。
namespace SampleApp.DI;

/// <summary>
/// OCRに関することを定義します。
/// </summary>
public interface IOcrServiceDI
{
	/// <summary>
	/// 文字列を取得します。
	/// </summary>
	/// <param name="image"></param>
	/// <returns></returns>
	 Task<List<Text.TextBlock>> GetTextAsync(IImage image);
}