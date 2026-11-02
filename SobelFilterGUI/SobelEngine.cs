using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SobelFilterGUI
{
    /// <summary>
    /// Thin wrapper around SobelFilterDLL. Each step locks the bitmaps, calls the
    /// native routine and reports how long the native call alone took, so the
    /// timings exclude bitmap allocation and locking.
    /// </summary>
    public static class SobelEngine
    {
        private const string DllName = "SobelFilterDLL.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ProcessGrayscale(IntPtr input, IntPtr output, int width, int height, int stride, int strideOut, int threads);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ProcessGaussian(IntPtr input, IntPtr output, int width, int height, int stride, int threads);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ProcessSobel_CPP(IntPtr input, IntPtr output, int width, int height, int stride, int threads);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ProcessSobel_ASM(IntPtr input, IntPtr output, int width, int height, int stride, int threads);

        private delegate void NativeStep(IntPtr input, IntPtr output, int inputStride, int outputStride);

        private static Bitmap NewGrayBitmap(int width, int height)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bmp.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            bmp.Palette = palette;
            return bmp;
        }

        private static Bitmap Run(Bitmap source, PixelFormat inputFormat, NativeStep step, out double milliseconds)
        {
            int width = source.Width, height = source.Height;
            var rect = new Rectangle(0, 0, width, height);
            Bitmap result = NewGrayBitmap(width, height);

            BitmapData input = source.LockBits(rect, ImageLockMode.ReadOnly, inputFormat);
            BitmapData output = result.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            try
            {
                var sw = Stopwatch.StartNew();
                step(input.Scan0, output.Scan0, input.Stride, output.Stride);
                sw.Stop();
                milliseconds = sw.Elapsed.TotalMilliseconds;
            }
            finally
            {
                source.UnlockBits(input);
                result.UnlockBits(output);
            }
            return result;
        }

        public static Bitmap ToGrayscale(Bitmap source, int threads, out double ms)
        {
            int w = source.Width, h = source.Height;
            return Run(source, PixelFormat.Format24bppRgb,
                (i, o, s, so) => ProcessGrayscale(i, o, w, h, s, so, threads), out ms);
        }

        public static Bitmap ApplyGaussian(Bitmap gray, int threads, out double ms)
        {
            int w = gray.Width, h = gray.Height;
            return Run(gray, PixelFormat.Format8bppIndexed,
                (i, o, s, so) => ProcessGaussian(i, o, w, h, s, threads), out ms);
        }

        public static Bitmap ApplySobel(Bitmap gray, bool useAsm, int threads, out double ms)
        {
            int w = gray.Width, h = gray.Height;
            if (useAsm)
                return Run(gray, PixelFormat.Format8bppIndexed,
                    (i, o, s, so) => ProcessSobel_ASM(i, o, w, h, s, threads), out ms);
            return Run(gray, PixelFormat.Format8bppIndexed,
                (i, o, s, so) => ProcessSobel_CPP(i, o, w, h, s, threads), out ms);
        }
    }
}
