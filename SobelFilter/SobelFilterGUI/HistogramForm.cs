using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SobelFilterGUI
{
    public partial class HistogramForm : Form
    {
        public HistogramForm()
        {
            InitializeComponent();
        }
        public HistogramForm(Bitmap image)
        {
            InitializeComponent();
             
            this.Text = "Intensity Histogram";
            UpdateHist(image);
        }

        public void UpdateHist(Bitmap image)
        {
            int[] histData = CalculateHist(image);
            DisplayHist(histData);
        }

        private int[] CalculateHist(Bitmap bmp)
        {
            int[] hist = new int[256];
            int width = bmp.Width;
            int height = bmp.Height;
 
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            
            int stride = bmpData.Stride;
            int byteCount = stride * height;
            byte[] pixelValues = new byte[byteCount];
  
            Marshal.Copy(bmpData.Scan0, pixelValues, 0, byteCount);

            for(int y=0; y<height; y++)
            {
                int rowStart = y * stride;
                for(int x=0; x<width; x++)
                {
                    byte val = pixelValues[rowStart + x];
                    hist[val]++;
                }
            }
            bmp.UnlockBits(bmpData);

            return hist;
        }

        private void DisplayHist(int[] data)
        {
            histChart.Series.Clear();
            Series series = new Series("Intensity");

            series.ChartType = SeriesChartType.Column;
            series.Color = Color.FromArgb(100, 18, 160, 165);
            series.BorderColor = Color.Black;
            series.BorderWidth = 0;

            histChart.Legends[0].Enabled = false;

            for(int i = 0; i < 256; i++)
            {
                series.Points.AddXY(i, data[i]);
            }

            histChart.Series.Add(series);

            var area = histChart.ChartAreas[0];
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = 255;
            area.AxisX.IntervalOffset = 50;
            area.AxisX.Title = "Intensity";
            area.AxisY.Title = "Pixel count";           
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;
        }
    }
}
