#ifndef SOBEL_H
#define SOBEL_H
#endif 

#include "Utils.h"

void applySobelPart(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int start, int end);
void applySobelParallel(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int threadCount);