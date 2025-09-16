using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using LiningCheckRecord;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace DXApplication2.ReportLibrary
{
    public partial class XtraReportLiningSpool : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReportLiningSpool()
        {
            InitializeComponent();
			 resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraReportLiningSpool));
			if (this.DataSource is List<LiningSpool> spools)
			{
				Spools = spools;
			}
		}
		System.ComponentModel.ComponentResourceManager resources;
		List<LiningSpool> Spools=new();

		private void xrLabel55_BeforePrint(object sender, CancelEventArgs e)
		{
            
		}
		int i = -1;
		private void xrPictureBox1_BeforePrint(object sender, CancelEventArgs e)
		{
			i++;
			var n = this.CurrentRowIndex;
			if (this.DataSource is not List<LiningSpool> spools) return;
			if (i >= spools.Count) return;
			
			if (sender is XRPictureBox xRPictureBox)
			{
				if (spools[i].SpoolNo == "")
				{
					xrPictureBox1.ImageSource = null;
					return;
				}
				//xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("type1"));
				this.xrPictureBox1.ImageUrl = $"type{spools[i].SpoolType+1}.png";
				if (spools[i].SpoolType > 2 && File.Exists(spools[i].ImagePath))
				{

					FileInfo fileInfo = new FileInfo(spools[i].ImagePath);

					long fileSizeInBytes = fileInfo.Length;
					double fileSizeInKB = fileSizeInBytes / 1024.0;

					Debug.WriteLine($"File size: {fileSizeInKB:F2} KB");

					xrPictureBox1.ImageSource = ImageSource.FromFile(spools[i].ImagePath);
				}
			}
		}

		private void XtraReportLiningSpool_DataSourceRowChanged(object sender, DataSourceRowEventArgs e)
		{
			var n= this.CurrentRowIndex;
		}

		private void XtraReportLiningSpool_BeforePrint(object sender, CancelEventArgs e)
		{
			if (this.DataSource is List<LiningSpool> spools)
			{
				Spools = spools;

				if (spools.Count > 0)
				{
					if (spools[0].Sheet != null)
					{
						this.Op1.Text = spools[0].Sheet.Option1 == 0 ? "（検査電圧　10，000V）" : "（検査電圧　12，000V）";
						this.Op2.Text = spools[0].Sheet.Option2 == 0 ? "電磁式膜厚計" : "渦電流式膜厚計";

					}
				}
			}
		}

		private void xrLabel35_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
