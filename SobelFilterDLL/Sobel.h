#pragma once
#include "Utils.h"

// Sobel edge magnitude min(|Gx| + |Gy|, 255) for rows 1..height-2 and
// columns 1..width-2. The one-pixel border is left untouched.
//   Gx = [-1 0 1; -2 0 2; -1 0 1]    Gy = [1 2 1; 0 0 0; -1 -2 -1]
void applySobelPart(const BYTE* input, BYTE* output, int width, int height, int stride, int start, int end);
void applySobelParallel(const BYTE* input, BYTE* output, int width, int height, int stride, int threadCount);
