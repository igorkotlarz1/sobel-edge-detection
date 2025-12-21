#include "Gaussian.h"

std::vector<BYTE> applyGaussian(const std::vector<BYTE>& image, int width, int height)
{
	std::vector<BYTE> gaussImage(width * height);

	for (int i = 0; i < height; i++) {
		for (int j = 0; j < width; j++) {

			if (i == 0 || i == height - 1 || j == 0 || j == width - 1) {
				gaussImage[getIndexAt(i, j, width)] = image[getIndexAt(i, j, width)];
				continue;
			}

			int sum = 0;
			sum += image[getIndexAt(i - 1, j - 1, width)];
			sum += image[getIndexAt(i - 1, j, width)] * 2;
			sum += image[getIndexAt(i - 1, j + 1, width)];

			sum += image[getIndexAt(i, j - 1, width)] * 2;
			sum += image[getIndexAt(i, j, width)] * 4;
			sum += image[getIndexAt(i, j + 1, width)] * 2;

			sum += image[getIndexAt(i + 1, j - 1, width)];
			sum += image[getIndexAt(i + 1, j, width)] * 2;
			sum += image[getIndexAt(i + 1, j + 1, width)];

			sum /= 16;
			gaussImage[getIndexAt(i, j, width)] = (BYTE) sum;
		}
	}

	return gaussImage;
}

std::vector<BYTE> applyGaussianParallel(const std::vector<BYTE>& image, int width, int height, int threadCount)
{
	int size = width * height;
	int chunkSize = size / threadCount;

	std::vector<BYTE> resultImage(size);
	std::vector<std::thread> threads;

	for (int i = 0; i < threadCount; i++) {

		int start = i * chunkSize;
		int end;

		if (i == threadCount - 1)
			end = size;
		else
			end = start + chunkSize;

		threads.emplace_back(applyGaussianPart, std::cref(image), std::ref(resultImage), width, height, start, end);
	}

	for (auto& thread : threads)
		thread.join();

	return resultImage;
}

void applyGaussianPart(const std::vector<BYTE>& image, std::vector<BYTE>& resultImage, int width, int height, int start, int end) 
{

	for (int i = start; i < end; i++) {
		for (int j = 0; j < width; j++) {

			if (i == 0 || i == height - 1 || j == 0 || j == width - 1) {
				resultImage[getIndexAt(i, j, width)] = image[getIndexAt(i, j, width)];
				continue;
			}

			int sum = 0;
			sum += image[getIndexAt(i - 1, j - 1, width)];
			sum += image[getIndexAt(i - 1, j, width)] * 2;
			sum += image[getIndexAt(i - 1, j + 1, width)];

			sum += image[getIndexAt(i, j - 1, width)] * 2;
			sum += image[getIndexAt(i, j, width)] * 4;
			sum += image[getIndexAt(i, j + 1, width)] * 2;

			sum += image[getIndexAt(i + 1, j - 1, width)];
			sum += image[getIndexAt(i + 1, j, width)] * 2;
			sum += image[getIndexAt(i + 1, j + 1, width)];

			sum /= 16;
			resultImage[getIndexAt(i, j, width)] = (BYTE)sum;
		}
	}
}

void applyGaussianPartCS(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int start, int end)
{
	for (int y = start; y < end; y++) {
		BYTE* rowOut = outputImage + y * stride;

		for (int x = 0; x < width; x++) {
			if (y == 0 || y == height - 1 || x == 0 || x == width - 1) {
				rowOut[x] = inputImage[y * stride + x];
				continue;
			}

			int sum = 0;

			BYTE* prevRow = inputImage + (y - 1) * stride;
			BYTE* currRow = inputImage + y * stride;
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
			outputImage[x] = (BYTE)sum;
		}
	}
}

void applyGaussianParallelCS(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int threadCount)
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

		threads.emplace_back(applyGaussianPartCS, inputImage, outputImage, width, height, stride, start, end);
	}

	for (auto& thread : threads)
		thread.join();
}
