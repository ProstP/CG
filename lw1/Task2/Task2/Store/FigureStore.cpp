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
	x -= m_offsetX;
	y -= m_offsetY;

	for (const Figure& figure : m_figures)
	{
		switch (figure.type)
		{
			case FigureType::Rectangle:
				if (IsClickToRectangle(figure.left, figure.top, figure.width, figure.height, x, y))
				{
					return true;
				}
				break;
			case FigureType::Triangle:
				if (IsClickToTriangle(figure.left, figure.top, figure.width, figure.height, x, y))
				{
					return true;
				}
				break;
			case FigureType::Ellipse:
				if (IsClickToEllipse(figure.left, figure.top, figure.width, figure.height, x, y))
				{
					return true;
				}
				break;
			default:
				return false;
		}
	}

	return false;
}

bool FigureStore::IsClickToRectangle(int left, int top, int width, int height, int mouseX, int mouseY)
{
	return left <= mouseX && mouseX <= left + width && top <= mouseY && mouseY <= top + height;
}

bool FigureStore::IsClickToTriangle(int left, int top, int width, int height, int mouseX, int mouseY)
{
	int x1 = left + (width / 2);
	int y1 = top;
	int x2 = left + width;
	int y2 = top + height;
	int x3 = left;
	int y3 = top + height;

	auto area = [](int x1, int y1, int x2, int y2, int x3, int y3) {
		return std::abs((x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)) / 2.0);
		};

	double A = area(x1, y1, x2, y2, x3, y3);

	double A1 = area(mouseX, mouseY, x2, y2, x3, y3);
	double A2 = area(x1, y1, mouseX, mouseY, x3, y3);
	double A3 = area(x1, y1, x2, y2, mouseX, mouseY);

	return (A == A1 + A2 + A3);
}

bool FigureStore::IsClickToEllipse(int left, int top, int width, int height, int mouseX, int mouseY)
{
	double cx = left + width / 2;
	double cy = top + height / 2;
	double a = width / 2.0;
	double b = height / 2.0;

	return ((mouseX - cx) * (mouseX - cx)) / (a * a) + ((mouseY - cy) * (mouseY - cy)) / (b * b) <= 1;
}
