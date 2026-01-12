using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;


namespace SobelFilterGUI
{
    public partial class Form1 : Form
    {
        private HistogramForm currentHistogram = null;
        private bool implementationCPP = true;
        private int prevSelectedIndex = 0;

        public Form1()
        {
            InitializeComponent();
            label1.Text = $"(Logical CPU threads: {Environment.ProcessorCount})";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void radioButtonASM_CheckedChanged(object sender, EventArgs e)
        {   
            if(radioButtonASM.Checked)
            {
                prevSelectedIndex = comboBoxThreads.SelectedIndex;
                //comboBoxThreads.SelectedIndex = 0;
                //comboBoxThreads.Enabled = false;
                implementationCPP = false;
            }
            else
            {
                //comboBoxThreads.SelectedIndex = prevSelectedIndex;
                //comboBoxThreads.Enabled = true;
                implementationCPP= true;
            }
        }

        private void radioButtonCPP_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            run();                  
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            load();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void labelTime_Click(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(pictureBoxProcessed.Image == null)
            {
                MessageBox.Show("Can't save blank image!", "Saving image",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            saveFileDialog1.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            saveFileDialog1.DefaultExt = "jpeg";
            saveFileDialog1.AddExtension = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;
                ImageFormat format = ImageFormat.Jpeg;

                pictureBoxProcessed.Image.Save(filePath, format);

                string message = $"Image saved successfully!";

                MessageBox.Show(message, "Saving image",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonProbe_Click(object sender, EventArgs e)
        {
            probe();
        }

        //button action handling methods:
        private void run()
        {
            if (comboBoxThreads.SelectedItem == null)
            {
                MessageBox.Show("You need to select the number of threads!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!(radioButtonCPP.Checked || radioButtonASM.Checked))
            {
                MessageBox.Show("You need to select an implementation!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (pictureBoxOriginal.Image == null)
            {
                MessageBox.Show("You need to load an image!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int threads = int.Parse(comboBoxThreads.SelectedItem.ToString());
            
            Bitmap originalBmp = (Bitmap)pictureBoxOriginal.Image;
            Bitmap resultBmp;

            Stopwatch swPreproc = new Stopwatch();
            Stopwatch swSobel = new Stopwatch();

            swPreproc.Start(); 
            Bitmap grayImg = SobelEngine.toGrayscale(originalBmp, threads);
            Bitmap gaussImg = SobelEngine.applyGaussian(grayImg, threads);
            swPreproc.Stop();

            if (implementationCPP)
            {
                swSobel.Start();
                resultBmp = SobelEngine.applySobel_CPP(gaussImg, threads);
                swSobel.Stop();
            }
            else
            {
                swSobel.Start();
                resultBmp = SobelEngine.applySobel_ASM(gaussImg);
                swSobel.Stop();
            }

            pictureBoxProcessed.Image = resultBmp;

            double timePreproc = (swPreproc.ElapsedTicks / (double)Stopwatch.Frequency) * 1000;
            double timeSobel = (swSobel.ElapsedTicks / (double)Stopwatch.Frequency) * 1000;
            double timeAll = timePreproc + timeSobel;

            labelTime.Text = $"{timeAll:F2} ms";
            labelTimeSobel.Text = $"{timeSobel:F2} ms (Sobel)";

            if (currentHistogram != null && !currentHistogram.IsDisposed)
            {
                currentHistogram.UpdateHist(grayImg);
                //currentHistogram.BringToFront();
            }
            else
            {
                currentHistogram = new HistogramForm(grayImg);
                currentHistogram.Show();
            }    
        }

        private void probe()
        {
            if (!(radioButtonCPP.Checked || radioButtonASM.Checked))
            {
                MessageBox.Show("You need to select an implementation!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (pictureBoxOriginal.Image == null)
            {
                MessageBox.Show("You need to load an image!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int[] threads = { 1, 2, 4, 8, 16, 32, 64 };

            string impStr = implementationCPP ? "C++" : "ASM";
            string path = @"C:\Users\igor\Desktop\Sobel-edge-detection\Dokumenty\probing.csv";

            Bitmap originalBmp = (Bitmap)pictureBoxOriginal.Image;

            const int cycles = 6;
            double timePreproc=0, timeSobel=0, avgTimePreproc=0, avgTimeSobel=0, avgTimeTotal=0;
            string imgSize = originalBmp.Width * originalBmp.Height > 4e6 ? "large" : "medium";         
            
            foreach (int threadNum in threads)
            {
                Stopwatch swPreproc = new Stopwatch();
                Stopwatch swSobel = new Stopwatch();
                timePreproc = 0;
                timeSobel = 0;

                for (int i = 0; i < cycles; i++)
                {
                    swPreproc.Restart();

                    Bitmap grayImg = SobelEngine.toGrayscale(originalBmp, threadNum);
                    Bitmap gaussImg = SobelEngine.applyGaussian(grayImg, threadNum);

                    swPreproc.Stop();

                    Bitmap resultBmp;

                    if (implementationCPP)
                    {
                        swSobel.Restart();
                        resultBmp = SobelEngine.applySobel_CPP(gaussImg, threadNum);
                        swSobel.Stop();
                    }
                    else
                    {
                        swSobel.Restart();
                        resultBmp = SobelEngine.applySobel_ASM(gaussImg);
                        swSobel.Stop();
                    }

                    timePreproc += (swPreproc.ElapsedTicks / (double)Stopwatch.Frequency) * 1000;
                    timeSobel += (swSobel.ElapsedTicks / (double)Stopwatch.Frequency) * 1000;

                    grayImg.Dispose();
                    gaussImg.Dispose();
                    resultBmp.Dispose();
                }
                avgTimePreproc = timePreproc / cycles;
                avgTimeSobel = timeSobel / cycles;

                avgTimeTotal = avgTimePreproc + avgTimeSobel;

                string line = $"{impStr};{threadNum};{avgTimeTotal:F2};{avgTimeSobel:F2};{imgSize}";
                line += "\n";

                File.AppendAllText(path, line);
            }

            labelTime.Text = $"{avgTimeTotal:F2} ms";
            labelTimeSobel.Text = $"{avgTimeSobel:F2} ms (Sobel)";


            MessageBox.Show($"Probed runtimes for different thread counts! (Cycling {cycles} times to find the average)", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void load()
        {
            string exePath = Application.StartupPath;
            string dataPath = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\..\Data"));
            openFileDialog1.InitialDirectory = dataPath;

            openFileDialog1.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                pictureBoxOriginal.Image = new Bitmap(filePath);
                pictureBoxProcessed.Image = null;
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
