#include "Grayscale.h"

std::vector<BYTE> toGrayscale(BYTE* image, int width, int height)
{
	int size = width * height * 3;
	int j = 0;
	std::vector<BYTE> resultImage(width * height);

	for (int i = 0; i < size; i += 3)
	{
		BYTE red = image[i];
		BYTE green = image[i + 1];
		BYTE blue = image[i + 2];

		BYTE gray = (BYTE)(0.299 * red + 0.587 * green + 0.114 * blue);

		resultImage[j] = gray;
		j++;
	}
	//std::cout << "Converted image to grayscale." << std::endl;

	return resultImage;
}

std::vector<BYTE> toGrayscaleParallel(BYTE* image, int width, int height, int threadCount)
{
	int size = width * height * 3;
	int chunkSize = size / threadCount;

	std::vector<BYTE> resultImage(size);
	std::vector<std::thread> threads;

	for (int i = 0; i < threadCount; i++)
	{
		int start = i * chunkSize;
		int end;

		if (i == threadCount - 1)
			end = size;
		else
			end = start + chunkSize;

		threads.emplace_back(toGrayScalePart, image, std::ref(resultImage), start, end);
	}

	for (auto& thread : threads)
		thread.join();

	return resultImage;
}

void toGrayScalePart(BYTE* image, std::vector<BYTE>& resultImage, int start, int end)
{
	int j = start/3;

	for (int i = start; i < end; i += 3)
	{
		BYTE red = image[i];
		BYTE green = image[i + 1];
		BYTE blue = image[i + 2];

		BYTE gray = (BYTE)(0.299 * red + 0.587 * green + 0.114 * blue);

		resultImage[j] = gray;
		j++;
	}
}

void toGrayScalePartCS(BYTE* inputImage, BYTE* outputImage, int width, int stride, int strideOut, int start, int end)
{
	for (int y = start; y < end; y++) {

		BYTE* rowIn = inputImage + y * stride;
		BYTE* rowOut = outputImage + y * strideOut;

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

void toGrayScaleParallelCS(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int strideOut, int threadCount)
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

		threads.emplace_back(toGrayScalePartCS, inputImage, outputImage, width, stride, strideOut, start, end);
	}

	for (auto& thread : threads)
		thread.join();
}
