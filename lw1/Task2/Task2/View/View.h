#pragma once

#include "../Store/FigureStore.h"

class View
{
public:
	View(FigureStore& store)
		: m_store(store)
		, m_isDraging(false)
	{
	}

	void MouseDown();
	void MouseUp();
	void SetNewMousePos(int x, int y);
	bool IsDraging();
	FigureStore& GetStore();

private:
	FigureStore& m_store;
	bool m_isDraging;
};

