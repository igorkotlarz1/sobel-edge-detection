#ifndef GAUSSIAN_H
#define GAUSSIAN_H
#endif 

#include "Utils.h"

void applyGaussianPart(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int start, int end);
void applyGaussianParallel(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int threadCount);