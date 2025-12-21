#ifndef GAUSSIAN_H
#define GAUSSIAN_H
#endif 

#include "Utils.h"

std::vector<BYTE> applyGaussian(const std::vector<BYTE> &image, int width, int height);
std::vector<BYTE> applyGaussianParallel(const std::vector<BYTE>& image, int width, int height, int threadCount);
void applyGaussianPart(const std::vector<BYTE>& image, std::vector<BYTE>& resultImage, int width, int height, 
	int start, int end);

void applyGaussianPartCS(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int start, int end);
void applyGaussianParallelCS(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int threadCount);