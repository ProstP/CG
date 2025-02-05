#pragma once

#include "../Store/FigureStore.h"

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

private:
	FigureStore& m_store;
	bool m_isDraging = false;
	int m_mouseX = 0;
	int m_mouseY = 0;
};

