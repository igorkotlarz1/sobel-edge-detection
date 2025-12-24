#include "pch.h"
#include "Grayscale.h"
#include "Gaussian.h"
#include "Sobel.h"

#define DLL_EXPORT extern "C" __declspec(dllexport)

DLL_EXPORT void ProcessGrayscale(unsigned char* inputImage, unsigned char* outputImage, int width, int height, int stride, int strideOut, int threads) 
{
	toGrayScaleParallel(inputImage, outputImage, width, height, stride, strideOut, threads);
}

DLL_EXPORT void ProcessGaussian(unsigned char* inputImage, unsigned char* outputImage, int width, int height, int stride, int threds)
{
	applyGaussianParallel(inputImage, outputImage, width, height, stride, threds);
}

DLL_EXPORT void ProcessSobel(unsigned char* inputImage, unsigned char* outputImage, int width, int height, int stride, int threds)
{
	applySobelParallel(inputImage, outputImage, width, height, stride, threds);
}