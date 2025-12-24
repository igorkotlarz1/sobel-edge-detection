#ifndef HISTOGRAM_H
#define HISTOGRAM_H
#endif 

#include "Utils.h"
#include <map>
#include <string>

std::map<int, int> getIntensityHist(const std::vector<BYTE>& image, int width, int height);
void toCSV(std::map<int, int>& intensityDict, const std::string& filePath);



