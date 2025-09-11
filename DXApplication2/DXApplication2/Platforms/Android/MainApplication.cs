using Android.App;
using Android.Runtime;
using DevExpress.Drawing;

namespace DXApplication2;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}
		
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
       
       
        using (StreamReader rd = new StreamReader(Assets.Open("NotoSansJP-Regular.ttf")))
        {
            using (var ms = new MemoryStream())
            {
                rd.BaseStream.CopyTo(ms);

                DXFontRepository.Instance.AddFont(ms);
            }
        }

      
        base.OnCreate();
    }
}