#include "pch.h"
#include "Grayscale.h"

void toGrayscalePart(const BYTE* input, BYTE* output, int width, int stride, int strideOut, int start, int end)
{
    for (int y = start; y < end; y++) {
        const BYTE* rowIn = input + y * stride;
        BYTE* rowOut = output + y * strideOut;

        for (int x = 0; x < width; x++) {
            const BYTE* px = rowIn + 3 * x;   // B, G, R
            rowOut[x] = static_cast<BYTE>(0.299 * px[2] + 0.587 * px[1] + 0.114 * px[0]);
        }
    }
}

void toGrayscaleParallel(const BYTE* input, BYTE* output, int width, int height, int stride, int strideOut, int threadCount)
{
    runParallelRows(height, threadCount, [=](int start, int end) {
        toGrayscalePart(input, output, width, stride, strideOut, start, end);
    });
}
