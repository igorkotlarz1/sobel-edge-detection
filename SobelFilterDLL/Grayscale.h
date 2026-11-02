#pragma once
#include "Utils.h"

// 24-bit BGR (GDI+ layout) to 8-bit luma: Y = 0.299 R + 0.587 G + 0.114 B.
void toGrayscalePart(const BYTE* input, BYTE* output, int width, int stride, int strideOut, int start, int end);
void toGrayscaleParallel(const BYTE* input, BYTE* output, int width, int height, int stride, int strideOut, int threadCount);
