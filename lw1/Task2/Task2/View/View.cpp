#include "View.h"
#include "../Finalizer/Finalizer.h"

void View::MouseDown(int x, int y)
{
	m_mouseX = x;
	m_mouseY = y;

	m_isDraging = true;
}

void View::MouseUp()
{
	m_mouseX = 0;
	m_mouseY = 0;

	m_isDraging = false;
}

void View::SetNewMousePos(int x, int y)
{
	if (!m_isDraging)
	{
		return;
	}

	int dx = x - m_mouseX;
	int dy = y - m_mouseY;

	m_mouseX = x;
	m_mouseY = y;

	m_store.AddOffset(dx, dy);
}

bool View::IsDraging()
{
	return m_isDraging;
}

FigureStore& View::GetStore()
{
	return m_store;
}

void View::PaintPicture(HDC dc, int centerX, int centerY)
{
	for (int i = 0; i < FigureStore::FIGURE_COUNT; i++)
	{
		Figure figure = m_store.GetFigure(i);

		HPEN pen = CreatePen(PS_SOLID, 3, RGB(figure.borderColor.R,
											  figure.borderColor.G,
											  figure.borderColor.B));

		LOGBRUSH brushInfo;
		brushInfo.lbStyle = BS_SOLID;
		brushInfo.lbColor = RGB(figure.fillColor.R,
								figure.fillColor.G,
								figure.fillColor.B);
		brushInfo.lbHatch = 0;
		HBRUSH brush = CreateBrushIndirect(&brushInfo);

		auto restoreOldPen = Finally([dc, oldPen = SelectObject(dc, pen), pen]
									 {
										 SelectObject(dc, oldPen);
										 DeleteObject(pen);
									 });
		auto restoreOldBrush = Finally([dc, oldBrush = SelectObject(dc, brush), brush]
									   {
										   SelectObject(dc, oldBrush);
										   DeleteObject(brush);
									   });

		PaintFigure(dc, centerX, centerY, figure, m_store.GetOffsetX(), m_store.GetOffsetY());
	}
}

void View::PaintFigure(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY)
{
	switch (figure.type)
	{
		case FigureType::Rectangle:
			PaintRectangle(dc, centerX, centerY, figure, offsetX, offsetY);
			break;
		case FigureType::Triangle:
			PaintTriangle(dc, centerX, centerY, figure, offsetX, offsetY);
			break;
		case FigureType::Ellipse:
			PaintEllipse(dc, centerX, centerY, figure, offsetX, offsetY);
			break;
		default:
			break;
	}
}

void View::PaintRectangle(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY)
{
	Rectangle(dc, centerX + figure.left + offsetX, centerY + figure.top + offsetY,
			  centerX + figure.left + figure.width + offsetX, centerY + figure.top + figure.height + offsetY);
}

void View::PaintTriangle(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY)
{
	POINT triangle[3];
	triangle[0] = { centerX + figure.left + figure.width / 2 + offsetX,
		centerY + figure.top + offsetY };
	triangle[1] = { centerX + figure.left + offsetX,
		centerY + figure.top + figure.height + offsetY };
	triangle[2] = { centerX + figure.left + figure.width + offsetX,
		centerY + figure.top + figure.height + offsetY };

	Polygon(dc, triangle, 3);
}

void View::PaintEllipse(HDC dc, int centerX, int centerY, const Figure& figure, int offsetX, int offsetY)
{
	Ellipse(dc, centerX + figure.left + offsetX, centerY + figure.top + offsetY,
			centerX + figure.left + figure.width + offsetX, centerY + figure.top + figure.height + offsetY);
}