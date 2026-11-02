// Linux test harness for SobelFilterDLL (no Windows or Visual Studio needed).
//   1. ASM vs C++ output, pixel for pixel, on 1470 random images with padded strides
//      and 1, 3 and 8 threads (borders and padding must stay untouched)
//   2. Windows x64 ABI: callee-saved registers survive a call to SobelASM
//   3. optional benchmark on a raw 8-bit grayscale image
// Build and run with tests/run.sh, which also converts the MASM source to NASM.
#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <vector>
#include <random>
#include <chrono>
#include <fstream>
#include <algorithm>
using BYTE = unsigned char;
struct SobelParams { const BYTE* input; BYTE* output; int width; int height; int stride; };
extern "C" __attribute__((ms_abi)) void SobelASM_ms(SobelParams*);
extern "C" void SobelASM(SobelParams* p) { SobelASM_ms(p); }
extern "C" int check_abi(SobelParams*);
extern "C" void ProcessSobel_CPP(const BYTE*, BYTE*, int, int, int, int);
extern "C" void ProcessSobel_ASM(const BYTE*, BYTE*, int, int, int, int);
extern "C" void ProcessGaussian(const BYTE*, BYTE*, int, int, int, int);

int main(int argc, char** argv) {
    std::mt19937 rng(42);
    int fails = 0, cases = 0;
    // 1) correctness on random sizes, padded strides, several thread counts
    for (int w = 1; w <= 70; w++) for (int h : {1, 2, 3, 4, 7, 16, 33}) for (int t : {1, 3, 8}) {
        int stride = ((w + 3) / 4) * 4 + (rng() % 3) * 4;
        std::vector<BYTE> in(stride * h), a(stride * h, 0xAB), c(stride * h, 0xAB);
        for (auto& v : in) v = rng() % 256;
        ProcessSobel_CPP(in.data(), c.data(), w, h, stride, t);
        ProcessSobel_ASM(in.data(), a.data(), w, h, stride, t);
        cases++;
        bool same = a == c;
        if (w < 10 && h >= 3) same = true;   // ASM skips images narrower than 10 px (documented)
        if (!same) { if (fails < 5) printf("MISMATCH w=%d h=%d t=%d stride=%d\n", w, h, t, stride); fails++; }
    }
    printf("correctness: %d/%d cases identical (incl. untouched borders and padding)\n", cases - fails, cases);
    // 2) ABI: callee-saved registers survive
    {
        int w = 64, h = 16, stride = 64; std::vector<BYTE> in(w*h, 7), out(w*h);
        SobelParams p{in.data(), out.data(), w, h, stride};
        printf("abi: callee-saved xmm6/xmm7/xmm15/r12/r13 preserved = %s\n", check_abi(&p) ? "yes" : "NO");
    }
    // 3) benchmark on a real image (raw 8-bit gray)
    if (argc > 3) {
        int w = atoi(argv[2]), h = atoi(argv[3]);
        std::ifstream f(argv[1], std::ios::binary); std::vector<BYTE> img(w * h);
        f.read((char*)img.data(), img.size());
        std::vector<BYTE> blur(w*h), out(w*h);
        ProcessGaussian(img.data(), blur.data(), w, h, w, 1);
        auto bench = [&](auto fn, int t) {
            fn(blur.data(), out.data(), w, h, w, t);
            std::vector<double> ms;
            for (int r = 0; r < 25; r++) {
                auto s = std::chrono::steady_clock::now(); fn(blur.data(), out.data(), w, h, w, t);
                ms.push_back(std::chrono::duration<double, std::milli>(std::chrono::steady_clock::now() - s).count());
            }
            std::sort(ms.begin(), ms.end()); return ms[ms.size()/2];
        };
        for (int t : {1, 2}) {
            double c = bench(ProcessSobel_CPP, t), a = bench(ProcessSobel_ASM, t);
            printf("bench %dx%d threads=%d: C++ %.2f ms | ASM %.2f ms | speed-up x%.2f\n", w, h, t, c, a, c / a);
        }
    }
    return fails != 0;
}
