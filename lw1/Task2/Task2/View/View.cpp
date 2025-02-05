#include "View.h"

void View::MouseDown()
{
	m_isDraging = true;
}

void View::MouseUp()
{
	m_isDraging = false;
}

void View::SetNewMousePos(int x, int y)
{
	if (!m_isDraging)
	{
		return;
	}

	m_store.SetOffset(x, y);
}

bool View::IsDraging()
{
	return m_isDraging;
}

FigureStore& View::GetStore()
{
	return m_store;
}
