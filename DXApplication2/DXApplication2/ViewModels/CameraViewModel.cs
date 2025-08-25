using CommunityToolkit.Mvvm.ComponentModel;
using SampleApp.DI;
using Xamarin.Google.MLKit.Vision.Text;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace DXApplication2.ViewModels;

public class CameraViewModel : ObservableObject
{
    private IOcrServiceDI _ocrServiceDI;
    public CameraViewModel(IOcrServiceDI ocrServiceDI)
	{
        _ocrServiceDI= ocrServiceDI;

    }
    public CameraViewModel()
	{
       // _ocrServiceDI= ocrServiceDI;

    }



    internal Task<List<Text.TextBlock>> GetTextAsync(IImage image)
    {

        return _ocrServiceDI.GetTextAsync(image);
    }
}