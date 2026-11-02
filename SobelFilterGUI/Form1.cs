using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace SobelFilterGUI
{
    public partial class Form1 : Form
    {
        private static readonly int[] ProbeThreads = { 1, 2, 4, 8, 16, 32, 64 };
        private const int ProbeRepeats = 6;

        private HistogramForm histogram;

        public Form1()
        {
            InitializeComponent();
            label1.Text = $"(Logical CPU threads: {Environment.ProcessorCount})";
        }

        private bool UseAsm => radioButtonASM.Checked;

        private void buttonLoad_Click(object sender, EventArgs e) => LoadImage();
        private void buttonRun_Click(object sender, EventArgs e) => RunOnce();
        private void buttonProbe_Click(object sender, EventArgs e) => Probe();
        private void buttonSave_Click(object sender, EventArgs e) => SaveResult();

        private bool CheckInputs(bool needThreads)
        {
            string problem = null;
            if (needThreads && comboBoxThreads.SelectedItem == null) problem = "Select the number of threads.";
            else if (!radioButtonCPP.Checked && !radioButtonASM.Checked) problem = "Select an implementation.";
            else if (pictureBoxOriginal.Image == null) problem = "Load an image first.";

            if (problem != null)
                MessageBox.Show(problem, "Sobel Edge Detector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return problem == null;
        }

        /// <summary>Grayscale -> Gaussian blur -> Sobel; returns the native times in ms.</summary>
        private Bitmap Pipeline(Bitmap original, int threads, out Bitmap gray, out double preprocessMs, out double sobelMs)
        {
            gray = SobelEngine.ToGrayscale(original, threads, out double grayMs);
            using (Bitmap blurred = SobelEngine.ApplyGaussian(gray, threads, out double gaussMs))
            {
                preprocessMs = grayMs + gaussMs;
                return SobelEngine.ApplySobel(blurred, UseAsm, threads, out sobelMs);
            }
        }

        private void RunOnce()
        {
            if (!CheckInputs(needThreads: true))
                return;

            int threads = int.Parse(comboBoxThreads.SelectedItem.ToString());
            Bitmap result = Pipeline((Bitmap)pictureBoxOriginal.Image, threads, out Bitmap gray,
                                     out double preprocessMs, out double sobelMs);

            pictureBoxProcessed.Image?.Dispose();
            pictureBoxProcessed.Image = result;
            labelTime.Text = $"{preprocessMs + sobelMs:F2} ms";
            labelTimeSobel.Text = $"{sobelMs:F2} ms (Sobel)";

            if (histogram == null || histogram.IsDisposed)
            {
                histogram = new HistogramForm(gray);
                histogram.Show();
            }
            else
            {
                histogram.UpdateHist(gray);
            }
            gray.Dispose();
        }

        /// <summary>
        /// Benchmarks the selected implementation for every thread count and appends
        /// the averages to benchmark.csv next to the executable.
        /// </summary>
        private void Probe()
        {
            if (!CheckInputs(needThreads: false))
                return;

            var original = (Bitmap)pictureBoxOriginal.Image;
            string implementation = UseAsm ? "ASM" : "C++";
            string path = Path.Combine(Application.StartupPath, "benchmark.csv");
            if (!File.Exists(path))
                File.WriteAllText(path, "implementation;threads;width;height;total_ms;sobel_ms\n");

            double avgTotal = 0, avgSobel = 0;
            foreach (int threads in ProbeThreads)
            {
                // warm-up run: page in the DLL and the image, start the thread pool
                Pipeline(original, threads, out Bitmap warmGray, out _, out _).Dispose();
                warmGray.Dispose();

                double total = 0, sobel = 0;
                for (int i = 0; i < ProbeRepeats; i++)
                {
                    Pipeline(original, threads, out Bitmap gray, out double preMs, out double sobelMs).Dispose();
                    gray.Dispose();
                    total += preMs + sobelMs;
                    sobel += sobelMs;
                }
                avgTotal = total / ProbeRepeats;
                avgSobel = sobel / ProbeRepeats;

                File.AppendAllText(path, string.Format(CultureInfo.InvariantCulture,
                    "{0};{1};{2};{3};{4:F3};{5:F3}\n",
                    implementation, threads, original.Width, original.Height, avgTotal, avgSobel));
            }

            labelTime.Text = $"{avgTotal:F2} ms";
            labelTimeSobel.Text = $"{avgSobel:F2} ms (Sobel)";
            MessageBox.Show($"Averaged {ProbeRepeats} runs per thread count.\nResults appended to {path}",
                "Benchmark", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadImage()
        {
            string samples = FindSamplesFolder();
            if (samples != null)
                openFileDialog1.InitialDirectory = samples;
            openFileDialog1.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            pictureBoxOriginal.Image?.Dispose();
            pictureBoxProcessed.Image?.Dispose();
            pictureBoxOriginal.Image = new Bitmap(openFileDialog1.FileName);
            pictureBoxProcessed.Image = null;
        }

        /// <summary>Walks up from the executable to the repository's samples/ folder.</summary>
        private static string FindSamplesFolder()
        {
            var dir = new DirectoryInfo(Application.StartupPath);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "samples");
                if (Directory.Exists(candidate))
                    return candidate;
                dir = dir.Parent;
            }
            return null;
        }

        private void SaveResult()
        {
            if (pictureBoxProcessed.Image == null)
            {
                MessageBox.Show("Run the filter first.", "Save image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            saveFileDialog1.Filter = "PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg";
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.AddExtension = true;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            string file = saveFileDialog1.FileName;
            ImageFormat format = file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? ImageFormat.Png : ImageFormat.Jpeg;
            pictureBoxProcessed.Image.Save(file, format);
        }
    }
}
