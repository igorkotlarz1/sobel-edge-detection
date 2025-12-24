#include "pch.h"
#include "Gaussian.h"

void applyGaussianPart(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int start, int end)
{
	for (int y = start; y < end; y++) {
		BYTE* rowOut = outputImage + (y * stride);

		for (int x = 0; x < width; x++) {
			if (y == 0 || y == height - 1 || x == 0 || x == width - 1) {
				rowOut[x] = inputImage[y * stride + x];
				continue;
			}

			int sum = 0;

			BYTE* prevRow = inputImage + (y - 1) * stride;
			BYTE* currRow = inputImage + (y * stride);
			BYTE* nextRow = inputImage + (y + 1) * stride;

			sum += prevRow[x - 1];
			sum += prevRow[x] * 2;
			sum += prevRow[x + 1];

			sum += currRow[x - 1] * 2;
			sum += currRow[x] * 4;
			sum += currRow[x + 1] * 2;

			sum += nextRow[x - 1];
			sum += nextRow[x] * 2;
			sum += nextRow[x + 1];

			sum /= 16;
			rowOut[x] = (BYTE)sum;
		}
	}
}

void applyGaussianParallel(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int threadCount)
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

		threads.emplace_back(applyGaussianPart, inputImage, outputImage, width, height, stride, start, end);
	}

	for (auto& thread : threads)
		thread.join();
}
