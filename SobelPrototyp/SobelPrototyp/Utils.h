#ifndef UTILS_H
#define UTILS_H
#endif // !UTILS_H

#include <iostream>
#include <vector>
#include <thread>

using BYTE = unsigned char;

int getIndexAt(int row, int col, int width);

inline int getOffset(int x, int y, int stride)
{
	return y * stride + x * 3;
}