#pragma once
#include "Utils.h"

// 3x3 Gaussian blur, kernel [1 2 1; 2 4 2; 1 2 1] / 16. Border pixels are copied.
void applyGaussianPart(const BYTE* input, BYTE* output, int width, int height, int stride, int start, int end);
void applyGaussianParallel(const BYTE* input, BYTE* output, int width, int height, int stride, int threadCount);
