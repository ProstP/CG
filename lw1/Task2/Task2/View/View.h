#pragma once
#include "../Store/FigureStore.h"
#include <windows.h>

class View
{
public:
	View(FigureStore& store)
		: m_store(store)
	{
	}

	void MouseDown(int x, int y);
	void MouseUp();
	void SetNewMousePos(int x, int y);
	bool IsDraging();
	FigureStore& GetStore();

	void PaintPicture(HDC dc, int centerX, int centerY);

private:
	FigureStore& m_store;
	bool m_isDraging = false;
	int m_mouseX = 0;
	int m_mouseY = 0;

	static void PaintFigure(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY);
	static void PaintRectangle(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY);
	static void PaintTriangle(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY);
	static void PaintEllipse(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY);
};

