#include "View.h"

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
