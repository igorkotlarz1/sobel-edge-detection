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
        public Form1()
        {
            InitializeComponent();
            label1.Text = $"(Logical CPU threads: {Environment.ProcessorCount})";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
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
            
            if (comboBoxThreads.SelectedItem == null) {
                MessageBox.Show("You need to select the number of threads!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!(radioButtonCPP.Checked || radioButtonASM.Checked)) {
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
            bool impCpp = radioButtonCPP.Checked;
            string impStr = impCpp ? "C++" : "ASSEMBLY";

            Bitmap resultBmp;
            Bitmap originalBmp = (Bitmap) pictureBoxOriginal.Image;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Bitmap grayImg = SobelEngine.toGrayscale(originalBmp, threads);
            Bitmap gaussImg = SobelEngine.applyGaussian(grayImg, threads);

            if (impCpp)
            {
                resultBmp = SobelEngine.applySobel(gaussImg, threads);
            }
            else
            {
                resultBmp = SobelEngine.applySobel(gaussImg, threads);
            }

            stopwatch.Stop();
            
            /*
            string message = $"Running program with {threads} threads | implemenation: {implementation}";
            MessageBox.Show(message, "Processing image",
                MessageBoxButtons.OK, MessageBoxIcon.Information);*/

            pictureBoxProcessed.Image = resultBmp;
            labelTime.Text = $"{stopwatch.ElapsedMilliseconds} ms";
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
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

            bool impCpp = radioButtonCPP.Checked;
            string impStr = impCpp ? "C++" : "ASM";
            string path = @"C:\Users\igor\Desktop\Sobel-edge-detection\Dokumenty\probing.csv";

            Bitmap originalBmp = (Bitmap)pictureBoxOriginal.Image;

            foreach (int threadNum in threads)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Bitmap grayImg = SobelEngine.toGrayscale(originalBmp, threadNum);
                Bitmap gaussImg = SobelEngine.applyGaussian(grayImg, threadNum);
                Bitmap resultBmp = SobelEngine.applySobel(gaussImg, threadNum);

                stopwatch.Stop();
                long time = stopwatch.ElapsedMilliseconds;
                string line = $"{impStr},{threadNum},{time}";

                File.AppendAllText(path, line);
            }
            MessageBox.Show($"Probed runtimes for different thread counts!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
