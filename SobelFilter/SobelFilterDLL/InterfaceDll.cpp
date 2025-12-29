#include "pch.h"
#include "Grayscale.h"
#include "Gaussian.h"
#include "Sobel.h"

struct SobelParams {
	unsigned char* input;
	unsigned char* output;
	int width; 
	int height; 
	int stride;
};

#define DLL_EXPORT extern "C" __declspec(dllexport)

DLL_EXPORT void ProcessGrayscale(unsigned char* inputImage, unsigned char* outputImage, int width, int height, int stride, int strideOut, int threads) 
{
	toGrayScaleParallel(inputImage, outputImage, width, height, stride, strideOut, threads);
}

DLL_EXPORT void ProcessGaussian(unsigned char* inputImage, unsigned char* outputImage, int width, int height, int stride, int threads)
{
	applyGaussianParallel(inputImage, outputImage, width, height, stride, threads);
}

DLL_EXPORT void ProcessSobel_CPP(unsigned char* inputImage, unsigned char* outputImage, int width, int height, int stride, int threads)
{
	applySobelParallel(inputImage, outputImage, width, height, stride, threads);
}

extern "C" void SobelASM(SobelParams* params);

DLL_EXPORT void ProcessSobel_ASM(unsigned char* inputImage, unsigned char* outputImage, int width, int height, int stride) 
{
	SobelParams params;
	params.input = inputImage;
	params.output = outputImage;
	params.width= width;
	params.height = height;
	params.stride = std::abs(stride);

	//SobelASM(inputImage, outputImage, width, height, stride);
	SobelASM(&params);
}