#include "FigureStore.h"
#include <iostream>

Figure FigureStore::GetFigure(int index)
{
	if (index < 0 || index >= FIGURE_COUNT)
	{
		throw std::out_of_range("Index out of range");
	}

	return m_figures[index];
}

void FigureStore::AddOffset(int x, int y)
{
	m_offsetX += x;
	m_offsetY += y;
}

int FigureStore::GetOffsetX()
{
	return m_offsetX;
}

int FigureStore::GetOffsetY()
{
	return m_offsetY;
}

bool FigureStore::IsClickToFigures(int x, int y)
{
	return true;
}
