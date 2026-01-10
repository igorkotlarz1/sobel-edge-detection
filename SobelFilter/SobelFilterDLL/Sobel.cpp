#include "pch.h"
#include "Sobel.h"

void applySobelPart(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int start, int end)
{
    for (int y = start; y < end; y++) {
        BYTE* rowOut = outputImage + (y * stride);

        if (y == 0 || y == height - 1) 
            continue;
        for (int x = 1; x < width-1; x++) { //int x = 0; x < width; x++
            //GX KERNEL 
            //-1 0 1
            //-2 0 2
            //-1 0 1
            
            //GY KERNEL
            // 1  2  1
            // 0  0  0
            //-1 -2 -1 

            int gx = 0;
            int gy = 0;

            BYTE* prevRow = inputImage + (y - 1) * stride;
            BYTE* currRow = inputImage + (y * stride);
            BYTE* nextRow = inputImage + (y + 1) * stride;

            gx += prevRow[x - 1] * (-1);
            gx += prevRow[x + 1];

            gx += currRow[x - 1] * (-2);
            gx += currRow[x + 1] * 2;

            gx += nextRow[x - 1] * (-1);
            gx += nextRow[x + 1];

            gy += prevRow[x - 1];
            gy += prevRow[x] * 2;
            gy += prevRow[x + 1];

            gy += nextRow[x - 1] * (-1);
            gy += nextRow[x] * (-2);
            gy += nextRow[x + 1] * (-1);

            int magnitude = abs(gx) + abs(gy);

            if (magnitude > 255) magnitude = 255;
            rowOut[x] = (BYTE)magnitude;
        }
    }
}

void applySobelParallel(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int threadCount)
{
    int chunkSize = height / threadCount;
    std::vector<std::thread> threads;

    for (int i = 0; i < threadCount; i++)
    {
        int start = i * chunkSize;
        int end;

        if (i == threadCount - 1)
            end = height;
        else
            end = start + chunkSize;

        threads.emplace_back(applySobelPart, inputImage, outputImage, width, height, stride, start, end);
    }

    for (auto& thread : threads)
        thread.join();
}

