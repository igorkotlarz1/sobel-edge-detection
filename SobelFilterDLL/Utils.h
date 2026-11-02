#pragma once

#include <algorithm>
#include <thread>
#include <vector>

using BYTE = unsigned char;

// Splits rows [0, height) into `threadCount` contiguous bands and runs
// `work(start, end)` for each band on its own thread.
template <typename Work>
void runParallelRows(int height, int threadCount, Work work)
{
    threadCount = std::max(1, std::min(threadCount, height));
    const int chunk = height / threadCount;

    std::vector<std::thread> threads;
    threads.reserve(threadCount);
    for (int i = 0; i < threadCount; i++) {
        const int start = i * chunk;
        const int end = (i == threadCount - 1) ? height : start + chunk;
        threads.emplace_back(work, start, end);
    }
    for (auto& t : threads)
        t.join();
}
