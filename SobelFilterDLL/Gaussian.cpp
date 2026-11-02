#include "pch.h"
#include "Gaussian.h"

void applyGaussianPart(const BYTE* input, BYTE* output, int width, int height, int stride, int start, int end)
{
    for (int y = start; y < end; y++) {
        const BYTE* row = input + y * stride;
        BYTE* rowOut = output + y * stride;

        if (y == 0 || y == height - 1) {
            std::copy(row, row + width, rowOut);
            continue;
        }

        const BYTE* prev = row - stride;
        const BYTE* next = row + stride;
        rowOut[0] = row[0];
        rowOut[width - 1] = row[width - 1];

        for (int x = 1; x < width - 1; x++) {
            const int sum = prev[x - 1] + 2 * prev[x] + prev[x + 1]
                          + 2 * row[x - 1] + 4 * row[x] + 2 * row[x + 1]
                          + next[x - 1] + 2 * next[x] + next[x + 1];
            rowOut[x] = static_cast<BYTE>(sum / 16);
        }
    }
}

void applyGaussianParallel(const BYTE* input, BYTE* output, int width, int height, int stride, int threadCount)
{
    runParallelRows(height, threadCount, [=](int start, int end) {
        applyGaussianPart(input, output, width, height, stride, start, end);
    });
}
