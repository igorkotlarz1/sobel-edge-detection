#ifndef GRAYSCALE_H
#define GRAYSCALE_H
#endif 

#include "Utils.h"

void toGrayScalePart(BYTE* inputImage, BYTE* outputImage, int width, int stride, int strideOut, int start, int end);
void toGrayScaleParallel(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int strideOut, int threadCount);