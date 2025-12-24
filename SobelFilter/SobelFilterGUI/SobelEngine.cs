using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SobelFilterGUI
{
    public static class SobelEngine
    {
        private const string DLLName = "SobelFilterDLL.dll";

        [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ProcessGrayscale(IntPtr inputImage, IntPtr outputImage, int width, int height, int stride, int strideOut, int threads);

        [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ProcessGaussian(IntPtr inputImage, IntPtr outputImage, int width, int height, int stride, int threads);

        [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ProcessSobel(IntPtr inputImage, IntPtr outputImage, int width, int height, int stride, int threads);

        private static void adjustPalette(Bitmap bmp)
        {
            ColorPalette palette = bmp.Palette;
            for(int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            bmp.Palette = palette;

        }

        public static Bitmap toGrayscale(Bitmap sourceBmp, int threads)
        {
            int width = sourceBmp.Width;
            int height = sourceBmp.Height;

            Bitmap resultBmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            adjustPalette(resultBmp);

            BitmapData inputData = sourceBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,PixelFormat.Format24bppRgb);
            BitmapData outputData = resultBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            ProcessGrayscale(inputData.Scan0, outputData.Scan0, width, height, inputData.Stride, outputData.Stride, threads);

            sourceBmp.UnlockBits(inputData);
            resultBmp.UnlockBits(outputData);

            return resultBmp;
        }

        public static Bitmap applyGaussian(Bitmap sourceBmp, int threads)
        {
            int width = sourceBmp.Width;
            int height = sourceBmp.Height;

            Bitmap resultBmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            adjustPalette(resultBmp);

            BitmapData inputData = sourceBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            BitmapData outputData = resultBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            ProcessGaussian(inputData.Scan0, outputData.Scan0, width, height, inputData.Stride, threads);

            sourceBmp.UnlockBits(inputData);
            resultBmp.UnlockBits(outputData);

            return resultBmp;
        }

        public static Bitmap applySobel(Bitmap sourceBmp, int threads)
        {
            int width = sourceBmp.Width;
            int height = sourceBmp.Height;

            Bitmap resultBmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            adjustPalette(resultBmp);

            BitmapData inputData = sourceBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            BitmapData outputData = resultBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            ProcessSobel(inputData.Scan0, outputData.Scan0, width, height, inputData.Stride, threads);

            sourceBmp.UnlockBits(inputData);
            resultBmp.UnlockBits(outputData);

            return resultBmp;
        }

    }
}
