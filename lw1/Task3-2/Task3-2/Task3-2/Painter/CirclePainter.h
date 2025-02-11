#pragma once
#include <math.h>
#include <windows.h>

struct RGBColor
{
	unsigned char r;
	unsigned char g;
	unsigned char b;
};

class CirclePainter
{
public:
	CirclePainter(int centerX, int centerY, int radius, RGBColor rgb)
		: m_centerX(centerX)
		, m_centerY(centerY)
		, m_radius(abs(radius))
		, m_rgb(rgb)
	{
	};

	void PaintCircle(HDC hdc, int screenWidth, int screenHeight) const;

private:
	int m_centerX;
	int m_centerY;
	int m_radius;
	RGBColor m_rgb;

	void SetPoint(HDC hdc, int x, int y, int screenWidth, int screenHeight) const;
};

