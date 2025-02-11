#include "CirclePainter.h"
#include "../Finalizer/Finalizer.h"

void CirclePainter::PaintCircle(HDC hdc, int screenWidth, int screenHeight) const
{
	if (m_radius <= 0)
	{
		return;
	}

	int x = 0;
	int y = m_radius;
	int delta = 1 - 2 * m_radius;
	int error = 0;

	while (y >= x)
	{
		SetPoint(hdc, m_centerX - x, m_centerY + y, screenWidth, screenHeight);
		SetPoint(hdc, m_centerX - x, m_centerY - y, screenWidth, screenHeight);
		SetPoint(hdc, m_centerX + x, m_centerY + y, screenWidth, screenHeight);
		SetPoint(hdc, m_centerX + x, m_centerY - y, screenWidth, screenHeight);
		SetPoint(hdc, m_centerX - y, m_centerY + x, screenWidth, screenHeight);
		SetPoint(hdc, m_centerX - y, m_centerY - x, screenWidth, screenHeight);
		SetPoint(hdc, m_centerX + y, m_centerY + x, screenWidth, screenHeight);
		SetPoint(hdc, m_centerX + y, m_centerY - x, screenWidth, screenHeight);

		error = 2 * (delta + y) - 1;

		if (delta < 0 && error <= 0)
		{
			delta += (2 * ++x + 1);
		}
		else if (delta > 0 && error > 0)
		{
			delta -= (2 * --y + 1);
		}
		else
		{
			delta += (2 * (++x - --y));
		}
	}
}

void CirclePainter::SetPoint(HDC hdc, int x, int y, int screenWidth, int screenHeight) const
{
	if (x < 0 || y < 0 || x > screenWidth || y > screenHeight)
	{
		return;
	}

	COLORREF color = RGB(m_rgb.r, m_rgb.g, m_rgb.b);

	SetPixel(hdc, x, y, color);
}
