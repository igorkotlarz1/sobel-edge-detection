#include "pch.h"
#include "Histogram.h"
#include <fstream>

std::map<int, int> getIntensityHist(const std::vector<BYTE>& image, int width, int height)
{
	int size = height * width;
	std::map<int, int> intensityDict;

	for (int i = 0; i < size; i++) {
		int brightness = (int)image[i];
		intensityDict[brightness]++;
	}

	return intensityDict;
}

void toCSV(std::map<int, int>& intensityDict, const std::string& filePath)
{
	std::ofstream file(filePath);
	int range = 255;

	for (int i = 0; i <= 255; i++)
		file << i << "," << intensityDict[i] << "\n";

	file.close();
	//std::cout << "Saved hist data to " << filePath << std::endl;
	
}
