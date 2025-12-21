#ifndef SOBEL_H
#define SOBEL_H
#endif 

#include "Utils.h"

std::vector<int> applySobelGX(const std::vector<BYTE>& image, int width, int height);
std::vector<int> applySobelGY(const std::vector<BYTE>& image, int width, int height);

std::vector<BYTE> calculatelMagnitude(const std::vector<BYTE>& image, int width, int height);

void detectEdges(std::vector<BYTE>& image, int width, int height, short threshold);

//------------------parallel-------------------------------
void sobelGXPart(const std::vector<BYTE>& image, std::vector<BYTE>& resultImage,int width, int height, int start, int end);
void sobelGYPart(const std::vector<BYTE>& image, std::vector<BYTE>& resultImage, int width, int height, int start, int end);

std::vector<int> sobelGXParallel(const std::vector<BYTE>& image, int width, int height, int threadCount);
std::vector<int> sobelGYParallel(const std::vector<BYTE>& image, int width, int height, int threadCount);

void calculateMagnitudePart(const std::vector<BYTE>& image, const std::vector<int>& GX, const std::vector<int>& GY, 
	std::vector<BYTE>& resultImage, int width, int height, int start, int end);
std::vector<BYTE> calculateMagnitudeParallel(const std::vector<BYTE>& image, int width, int height, int threadCount);

void applySobelPartCS(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int start, int end);
void applySobelParallelCS(BYTE* inputImage, BYTE* outputImage, int width, int height, int stride, int threadCount);