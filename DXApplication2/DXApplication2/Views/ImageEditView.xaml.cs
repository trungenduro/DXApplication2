using System;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using Android.Text;
using DevExpress.Android.Editors;
using DevExpress.Maui.Editors;
//using DevExpress.XtraGauges.Core;
using LiningCheckRecord;
using Microsoft.Maui.Controls;
using SkiaSharp;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.ImageEditor;
using static Bumptech.Glide.DiskLruCache.DiskLruCache;
using Image = Microsoft.Maui.Controls.Image;
using ImageFormat = DevExpress.Maui.Editors.ImageFormat;
using Point = Microsoft.Maui.Graphics.Point;

namespace DemoCenter.Maui.Views;

public partial class ImageEditView : ContentPage {
    private TaskCompletionSource<string> pageResultCompletionSource;

    public ImageEditView(ImageSource imageSource) {
        InitializeComponent();
        InitToolbar();
        pageResultCompletionSource = new TaskCompletionSource<string>();
        editor.Source = imageSource;
    }
    string FileName= "";
	public ImageEditView(string filename) {
        InitializeComponent();
        InitToolbar();
        pageResultCompletionSource = new TaskCompletionSource<string>();
		FileName = filename;

		editor.Source = filename;
    }
    public Stream CreateBlankImageStream(int width, int height)
    {
        var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White); // or any background color

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return new MemoryStream(data.ToArray());
    }

    void InitToolbar()
	{
        Image browseImage = new Image()
        {
            Source = ImageSource.FromFile("save.png"),
            HeightRequest = 30,
            WidthRequest = 30
        };

        ImageEditorToolbar headerToolbar = this.editor.Toolbars[0];
        ImageEditorToolbarGroupItem headerToolbarGroup1 = (ImageEditorToolbarGroupItem)headerToolbar.ToolbarItems[1];
        ImageEditorToolbarItem saveItem = headerToolbarGroup1.Items.FirstOrDefault(i => i.Name == "Save");
        saveItem.View = new Image()
        {
            Source = ImageSource.FromFile("info.png"),
            HeightRequest = 30,
            WidthRequest = 30
        };
        saveItem.IsVisible = false;
        saveItem.IsEnabled = false;

		headerToolbarGroup1.Items.Add(new ImageEditorToolbarItem()
		{
			Name = "Saves",
			View = browseImage,
			IsVisible = true,

		});


		Image shareImage = new Image()
        {
            Source = ImageSource.FromFile("back.png"),
            HeightRequest = 30,
            WidthRequest = 30
        };

        ImageEditorToolbar headerToolbarItem = editor.Toolbars[0];
        ImageEditorToolbarGroupItem headerToolbarGroup0 = (ImageEditorToolbarGroupItem)headerToolbarItem.ToolbarItems[0];

        ImageEditorToolbarItem browseItem = headerToolbarGroup0.Items.FirstOrDefault(i => i.Name == "Browse");
        browseItem.IsVisible = false;


		headerToolbarGroup0.Items.Add(new ImageEditorToolbarItem()
		{
			Name = "Return",
			View = shareImage,
			IsVisible = true,

		});
        ImageEditorToolbar footerToolbarItem = this.editor.Toolbars[1];


		//var newItem = new ImageEditorToolbarItem()
		//{
		//	Name = "Custom",
		//	View = new Image()
		//	{
		//		Source = ImageSource.FromFile("spool.png"),
		//		HeightRequest = 30,
		//		WidthRequest = 30
		//	},
		//	IsVisible = true,
		//	IsEnabled = true
		//};
		//footerToolbarItem.ToolbarItems.Add(newItem);

		//newItem.SubToolbars = new List<ImageEditorToolbar>()
		//	{
		//		new ImageEditorToolbar()
		//		{
		//			ToolbarItems = new List<IImageEditorToolbarItem>()
		//			{
		//				new ImageEditorToolbarItem(){ Name = "insertspool",View = new Image()
		//	{
		//		Source = ImageSource.FromFile("spool.png"),
		//		HeightRequest = 30,
		//		WidthRequest = 30
		//	},},
		//			}
		//		}
  //              ,new ImageEditorToolbar()
		//		{
		//			ToolbarItems = new List<IImageEditorToolbarItem>()
		//			{
		//				new ImageEditorToolbarItem(){ Name = "inserteda"},
		//			}
		//		},


		//	};


		ImageEditorToolbarItem cropItem = (ImageEditorToolbarItem)footerToolbarItem.ToolbarItems.FirstOrDefault(i => i.Name == "Crop");
        cropItem.SubToolbarOverlay = false;
        cropItem.SubToolbars = new List<ImageEditorToolbar>()
            {
                new ImageEditorToolbar()
                {
                    ToolbarItems = new List<IImageEditorToolbarItem>()
                    {                        
                        new ImageEditorToolbarItem(){ Name = "square"},                        
                    }
                }
            };
       
    }
    LiningSpool Spool;
	public ImageEditView(LiningSpool sp) {
        InitializeComponent();
        InitCache();
        InitToolbar();
        pageResultCompletionSource = new TaskCompletionSource<string>();
		if(sp.ImagePath == null) return;
		Spool = sp;
		if (sp.ImagePath == "")
		{
            ImageSource blankImage = ImageSource.FromStream(() => CreateBlankImageStream(50, 50));
			editor.Source = blankImage;
        }
		else
		{
			editor.Source = sp.ImagePath;
			//img= Path.GetFileName( sp.ImagePath);
		}
    }
	string samplePath = Path.Combine(FileSystem.Current.AppDataDirectory, "sample.png");
	public void InitCache()
	{
		var c= File.Exists(samplePath);

        CopyFile("sample.png");
		CopyFile("type1.png");
		CopyFile("type2.png");
		CopyFile("type3.png");
		
	}	
	public void CopyFile(string filename)
	{
		string f = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
		if (!File.Exists(f))
		{

			var file = File.Create(f);
			var task = FileSystem.Current.OpenAppPackageFileAsync(filename);
			task.Wait();
			var fileStream = task.Result;
			fileStream.CopyTo(file);
			fileStream.Close();
			file.Close();
		}
	}

	public Task<string> WaitForResultAsync() {
        return pageResultCompletionSource.Task;
    }

    private async void BackPressed(object sender, EventArgs e) {
		try
		{
            pageResultCompletionSource.SetResult(null);
            await Navigation.PopAsync();
        }
		catch (Exception exx)
		{

			
		}
       
    }

    private async void CropPressed(object sender, EventArgs e) {
      //  pageResultCompletionSource.SetResult(editor.sa(ImageFormat.Jpeg));
        await Navigation.PopAsync();
    }
	string img = $"photo_{DateTime.Now:yyyyMMdd_HHmmss}.png";
	private async void ToolbarItem_Clicked(object sender, EventArgs e)
	{
        try
        {
			//string tempfile = Path.Combine(FileSystem.Current.AppDataDirectory, "tempReportOK.pdf");
			Directory.CreateDirectory(Path.Combine(FileSystem.Current.AppDataDirectory, "Photo"));
			var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Photo", img);
            var fname = Path.GetFileName(FileName);
			
			if (Spool != null)
			{
				if (Spool.ImagePath == "")
				{
					Spool.ImagePath = filePath;
					Spool.SpoolType = 4;
				}
				try
				{
					File.Delete(Spool.ImagePath);
					Spool.ImagePath = filePath;
				}
				catch (Exception exx)
				{

					
				}

			}
			editor.SaveEdits();
            var size=  editor.OriginalImageSize;

			int newWidth = 400;
			int newHeight = (int)(size.Height * (newWidth / size.Width));


			editor.Save(Syncfusion.Maui.ImageEditor.ImageFileType.Png, filePath,null, new Microsoft.Maui.Graphics.Size(newWidth, newHeight));
			var files = Directory.GetFiles(Path.Combine(FileSystem.Current.AppDataDirectory, "Photo"), "*.*").ToList();

                pageResultCompletionSource.SetResult(filePath);
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception exx1)
            {


            }

        }
		catch (Exception e2)
        {

           
        }
	

	}

	private void ImageEditor_ImageSaving(object sender, ImageSavingEventArgs args)
	{
		args.Cancel = true;
		var stream = args.ImageStream;
		var directory = Path.Combine(FileSystem.AppDataDirectory, "ImageEditorSavedImages");
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		
		var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Photo", img);
		SaveStreamToFile(filePath, stream);
	}
	public async void SaveStreamToFile(string fileFullPath, Stream stream)
	{
		if (stream.Length == 0)
		{
			return;
		}

		// Create a FileStream object to write a stream to a file
		using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
		{
			// Fill the bytes[] array with the stream data
			byte[] bytesInStream = new byte[stream.Length];
			stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

			// Use FileStream object to write to the specified file
			fileStream.Write(bytesInStream, 0, bytesInStream.Length);
		}

        pageResultCompletionSource.SetResult(fileFullPath);
		try
		{
            await Navigation.PopAsync();
        }
		catch (Exception)
		{

			
		}
        
    }

    private void editor_SavePickerOpening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Directory.CreateDirectory(Path.Combine(FileSystem.Current.AppDataDirectory, "Photo"));
        var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Photo", img);
        var fname = Path.GetFileName(FileName);

        if (Spool != null)
        {
            if (Spool.ImagePath == "")
            {
                Spool.ImagePath = filePath;
                Spool.SpoolType = 4;
            }
            try
            {
                File.Delete(Spool.ImagePath);
                Spool.ImagePath = filePath;
            }
            catch (Exception exx)
            {


            }

        }
        e.Cancel = true;
    }

	private void editor_ToolbarItemSelected(object sender, ToolbarItemSelectedEventArgs e)
	{
		if (e.ToolbarItem.Name == "Saves")
		{

			ToolbarItem_Clicked(sender, e);
		}
		else if (e.ToolbarItem.Name == "Return")
		{
			BackPressed(sender, e);
		}
	}

    private void editor_ImageSaved(object sender, ImageSavedEventArgs e)
    {

    }

    private void OnAddShapeClicked(object sender, EventArgs e)
    {

    }

    private void DXButton_Clicked(object sender, EventArgs e)
    {
        //this.editor.AddShape(AnnotationShape.Polyline,
        //   new ImageEditorShapeSettings()
        //   {
        //       Points = new PointCollection
        //       {
        //            new Point(10, 30),
        //            new Point(10, 60),
        //            new Point(10, 45),
        //            new Point(80, 45),                   
        //       },
        //       Color = Colors.Blue,
        //   });

        //this.editor.AddCustomAnnotationView(new Label { Text = "A", WidthRequest = 150, HeightRequest = 40 });

        Image image = new Image() { HeightRequest = 100, WidthRequest = 350, Aspect = Aspect.AspectFit };
        image.Source = ImageSource.FromFile("spool.png");

        this.editor.AddCustomAnnotationView(image,
             new Syncfusion.Maui.ImageEditor.ImageEditorAnnotationSettings
             {
               AllowDrag = true,
                 AllowResize = false,
                 IsRotatable=true
                 
             });

    }

    private void DXButton_Clicked_1(object sender, EventArgs e)
    {

    }

    private void Text_Clicked(object sender, EventArgs e)
    {

		if (currentChar > 'Z')
			currentChar = 'A'; // Reset to A after Z

		this.editor.AddText(currentChar.ToString(), new ImageEditorTextSettings
        {
            RotationAngle = 0,
            IsRotatable = true,
            IsEditable = false,
			Background = Colors.Transparent,
			TextAlignment = TextAlignment.Start,
            Bounds= new Rect(0,0,10,10),
            TextStyle = new ImageEditorTextStyle()
            {
                FontSize = 50,
                TextColor = Colors.Blue,
                FontFamily = "Arial",
                FontAttributes = FontAttributes.None
            }
        });
		currentChar++;
	}

    private void eda_Clicked(object sender, EventArgs e)
    {
        Image image = new Image() { HeightRequest = 60, WidthRequest = 60, Aspect = Aspect.AspectFit ,Rotation=90};
        image.Source = ImageSource.FromFile("eda.png");

        this.editor.AddCustomAnnotationView(image,
             new Syncfusion.Maui.ImageEditor.ImageEditorAnnotationSettings
             {
                 AllowDrag = true,
                 AllowResize = true,
                 IsRotatable = true
                 
             });
    }

    private void editor_AnnotationSelected(object sender, AnnotationSelectedEventArgs e)
    {
        this.deleteBTN.IsVisible = true;

		var ann= e.AnnotationSettings;
        if(ann is ImageEditorTextSettings ts)
		{
		//	ts.TextStyle.FontSize = 20;
			//ts.TextStyle.TextColor = Colors.Red;
		}
        if(ann is ImageEditorShapeSettings ss)
        {
			//ss.Color = Colors.Red;
			ss.StrokeThickness = 5;
		}

        if(Math.Abs( ann.RotationAngle ) < 5 )
            ann.RotationAngle = 0;
		if (Math.Abs(ann.RotationAngle - 90) < 5)
			ann.RotationAngle = 90;
	}

	private void L_Clicked(object sender, EventArgs e)
	{

		 this.editor.AddShape(AnnotationShape.Polyline,
		   new ImageEditorShapeSettings()
           {
               StrokeThickness=5,
               Points = new PointCollection
               {
                    new Point(10, 30),
                    new Point(10, 60),
                    new Point(10, 45),
                    new Point(80, 45),
               },
               Color = Colors.Blue
           });
    }
	private char currentChar = 'A';
	private void delete_Clicked(object sender, EventArgs e)
	{
		ImageEditorToolbar footerToolbarItem = this.editor.Toolbars[1];
		ImageEditorToolbarItem cropItem = (ImageEditorToolbarItem)footerToolbarItem.ToolbarItems.FirstOrDefault(i => i.Name == "Delete");
		this.editor.DeleteAnnotation();
		this.deleteBTN.IsVisible = false;
	}

	private void editor_AnnotationUnselected(object sender, AnnotationUnselectedEventArgs e)
	{
		this.deleteBTN.IsVisible = false;
	}

    private void redu_Clicked(object sender, EventArgs e)
    {
        Image image = new Image() { HeightRequest = 60, WidthRequest = 60, Aspect = Aspect.AspectFit };
        image.Source = ImageSource.FromFile("reducer.png");

        this.editor.AddCustomAnnotationView(image,
             new Syncfusion.Maui.ImageEditor.ImageEditorAnnotationSettings
             {
                 AllowDrag = true,
                 AllowResize = true,
                 IsRotatable = true
             });
    }
}
