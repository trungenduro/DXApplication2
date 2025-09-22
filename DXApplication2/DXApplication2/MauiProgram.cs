using DevExpress.Maui;
using DevExpress.Maui.Core;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using SampleApp.DI;
using SampleApp.Platforms.Android.DI;
using CommunityToolkit.Maui.Core.Handlers;
using CommunityToolkit.Maui.Views;
using Syncfusion.Maui.Core.Hosting;



namespace DXApplication2;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        ThemeManager.ApplyThemeToSystemBars = true;

		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjGyl/Vkd+XU9FcVRDXXxIeUx0RWFcb1x6cVFMYFxBNQtUQF1hTH5ad01hWXxfcH1dRGJaWkd3");

        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
             .ConfigureSyncfusionCore()
               .ConfigureFonts(fonts =>
               {
                   fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                   fonts.AddFont("NotoSans-Regular.ttf", "NotosRegular");
                   fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                   fonts.AddFont("roboto-medium.ttf", "Roboto-Medium");
                   fonts.AddFont("roboto-regular.ttf", "Roboto");
               })
            .UseDevExpress(useLocalization: false)
            .UseDevExpressControls()
            // .UseDevExpressCharts()
            // .UseDevExpressTreeView()
            .UseDevExpressCollectionView()
            .UseDevExpressEditors()
          //  .UseDevExpressDataGrid()
            // .UseDevExpressScheduler()
            //.UseDevExpressGauges()
            .UseMauiCommunityToolkit()
            .RegisterAppServices()
            .RegisterViewModels()
            .UseMauiCommunityToolkitCamera()


            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CameraView, CameraViewHandler>();
            });

            
        return builder.Build();
    }

    static MauiAppBuilder RegisterViewModels(this MauiAppBuilder appBuilder) {
   
        appBuilder.Services.AddSingleton<ViewModels.DatabaseViewModel>();
       // appBuilder.Services.AddTransient<ViewModels.ReportingViewModel>();
		//appBuilder.Services.AddSingleton<IOcrServiceDI, OcrServiceDI>();
		return appBuilder;
    }
    static MauiAppBuilder RegisterAppServices(this MauiAppBuilder appBuilder) {
     
        appBuilder.Services.AddSingleton<Domain.Services.ICacheService, Infrastructure.Services.MemoryCacheService>();
		appBuilder.Services.AddSingleton<IOcrServiceDI, OcrServiceDI>();
		return appBuilder;
    }
}
