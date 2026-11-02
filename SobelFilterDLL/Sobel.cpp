#include "pch.h"
#include "Sobel.h"

void applySobelPart(const BYTE* input, BYTE* output, int width, int height, int stride, int start, int end)
{
    start = std::max(start, 1);
    end = std::min(end, height - 1);

    for (int y = start; y < end; y++) {
        const BYTE* prev = input + (y - 1) * stride;
        const BYTE* row = input + y * stride;
        const BYTE* next = input + (y + 1) * stride;
        BYTE* rowOut = output + y * stride;

        for (int x = 1; x < width - 1; x++) {
            const int gx = (prev[x + 1] + 2 * row[x + 1] + next[x + 1])
                         - (prev[x - 1] + 2 * row[x - 1] + next[x - 1]);
            const int gy = (prev[x - 1] + 2 * prev[x] + prev[x + 1])
                         - (next[x - 1] + 2 * next[x] + next[x + 1]);

            const int magnitude = std::abs(gx) + std::abs(gy);
            rowOut[x] = static_cast<BYTE>(magnitude > 255 ? 255 : magnitude);
        }
    }
}

void applySobelParallel(const BYTE* input, BYTE* output, int width, int height, int stride, int threadCount)
{
    runParallelRows(height, threadCount, [=](int start, int end) {
        applySobelPart(input, output, width, height, stride, start, end);
    });
}
