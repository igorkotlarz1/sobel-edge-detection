// C ABI exported to the C# GUI (see SobelEngine.cs).
#include "pch.h"
#include "Gaussian.h"
#include "Grayscale.h"
#include "Sobel.h"

#ifdef _WIN32
#define DLL_EXPORT extern "C" __declspec(dllexport)
#else
#define DLL_EXPORT extern "C"
#endif

// Must match the offsets used in SobelASM.asm.
struct SobelParams {
    const BYTE* input;   // +0
    BYTE* output;        // +8
    int width;           // +16
    int height;          // +20
    int stride;          // +24
};

extern "C" void SobelASM(SobelParams* params);

DLL_EXPORT void ProcessGrayscale(const BYTE* input, BYTE* output, int width, int height, int stride, int strideOut, int threads)
{
    toGrayscaleParallel(input, output, width, height, stride, strideOut, threads);
}

DLL_EXPORT void ProcessGaussian(const BYTE* input, BYTE* output, int width, int height, int stride, int threads)
{
    applyGaussianParallel(input, output, width, height, stride, threads);
}

DLL_EXPORT void ProcessSobel_CPP(const BYTE* input, BYTE* output, int width, int height, int stride, int threads)
{
    applySobelParallel(input, output, width, height, stride, threads);
}

// The ASM routine processes rows 1..height-2 of whatever window it is given.
// Each thread therefore gets a window that starts one row above its band and
// ends one row below it, so the bands meet without gaps or overlaps.
DLL_EXPORT void ProcessSobel_ASM(const BYTE* input, BYTE* output, int width, int height, int stride, int threads)
{
    runParallelRows(height, threads, [=](int start, int end) {
        start = std::max(start, 1);
        end = std::min(end, height - 1);
        if (start >= end)
            return;

        SobelParams params;
        params.input = input + (start - 1) * stride;
        params.output = output + (start - 1) * stride;
        params.width = width;
        params.height = (end - start) + 2;
        params.stride = stride;
        SobelASM(&params);
    });
}
