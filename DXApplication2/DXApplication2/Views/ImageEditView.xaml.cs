using System;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using DevExpress.Maui.Editors;
using LiningCheckRecord;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.ImageEditor;
using static Bumptech.Glide.DiskLruCache.DiskLruCache;
using ImageFormat = DevExpress.Maui.Editors.ImageFormat;

namespace DemoCenter.Maui.Views;

public partial class ImageEditView : ContentPage {
    private TaskCompletionSource<string> pageResultCompletionSource;

    public ImageEditView(ImageSource imageSource) {
        InitializeComponent();
        pageResultCompletionSource = new TaskCompletionSource<string>();
        editor.Source = imageSource;
    }
    string FileName= "";
	public ImageEditView(string filename) {
        InitializeComponent();
        pageResultCompletionSource = new TaskCompletionSource<string>();
		FileName = filename;

		editor.Source = filename;
    }

	LiningSpool Spool;
	public ImageEditView(LiningSpool sp) {
        InitializeComponent();
        pageResultCompletionSource = new TaskCompletionSource<string>();
		if(sp.ImagePath == null) return;
		Spool = sp;
		if (sp.ImagePath == "")
		{
			InitCache();
			if(sp.SpoolType<3) 
				editor.Source = $"type{sp.SpoolType+1}.png";
			else 
				editor.Source = "sample.png";
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
        pageResultCompletionSource.SetResult(null);
        await Navigation.PopAsync();
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
			editor.Save(Syncfusion.Maui.ImageEditor.ImageFileType.Png, filePath);
			var files = Directory.GetFiles(Path.Combine(FileSystem.Current.AppDataDirectory, "Photo"), "*.*").ToList();

			pageResultCompletionSource.SetResult(filePath);
			await Navigation.PopAsync();

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
	public void SaveStreamToFile(string fileFullPath, Stream stream)
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
	}
}
