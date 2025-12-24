#include "pch.h"
#include "Grayscale.h"

void toGrayScalePart(BYTE* inputImage, BYTE* outputImage, int width, int stride, int strideOut, int start, int end)
{
	for (int y = start; y < end; y++) {

		BYTE* rowIn = inputImage + (y * stride);
		BYTE* rowOut = outputImage + (y * strideOut);

		for (int x = 0; x < width; x++) {

			int pixel = 3 * x;
			BYTE red = rowIn[pixel + 2];
			BYTE green = rowIn[pixel + 1];
			BYTE blue = rowIn[pixel];

			BYTE gray = (BYTE)(0.299 * red + 0.587 * green + 0.114 * blue);

			rowOut[x] = gray;
		}
	}
}

void toGrayScaleParallel(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int strideOut, int threadCount)
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

		threads.emplace_back(toGrayScalePart, inputImage, outputImage, width, stride, strideOut, start, end);
	}

	for (auto& thread : threads)
		thread.join();
}
