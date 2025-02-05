#include <windows.h>
#include <tchar.h>
#include <windowsx.h>
#include <math.h>
#include <string>
#include "Store/FigureStore.h"
#include "View/View.h"

TCHAR const CLASS_NAME[] = _T("MainWndClass");
TCHAR const WINDOW_TITLE[] = _T("Task2");

void OnDestroy(HWND hWnd)
{
	PostQuitMessage(0);
}

void OnPaint(HWND hwnd, View& view)
{
	PAINTSTRUCT ps;
	HDC dc = BeginPaint(hwnd, &ps);
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int centerX = rcClient.right / 2;
	int centerY = rcClient.bottom / 2;

	for (int i = 0; i < FigureStore::FIGURE_COUNT; i++)
	{
		Figure figure = view.GetStore().GetFigure(i);

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

		HPEN oldPen = SelectPen(dc, pen);
		HBRUSH oldBrush = SelectBrush(dc, brush);

		switch (figure.type)
		{
			case FigureType::Rectangle:
				Rectangle(dc, centerX + figure.left + view.GetStore().GetOffsetX(), centerY + figure.top + view.GetStore().GetOffsetY(),
					centerX + figure.left + figure.width + view.GetStore().GetOffsetX(), centerY + figure.top + figure.height + view.GetStore().GetOffsetY());
				break;
			case FigureType::Triangle:
			{
				POINT triangle[3];
				triangle[0] = { centerX + figure.left + figure.width / 2 + view.GetStore().GetOffsetX(),
					centerY + figure.top + view.GetStore().GetOffsetY() };
				triangle[1] = { centerX + figure.left + view.GetStore().GetOffsetX(),
					centerY + figure.top + figure.height + view.GetStore().GetOffsetY() };
				triangle[2] = { centerX + figure.left + figure.width + view.GetStore().GetOffsetX(),
					centerY + figure.top + figure.height + view.GetStore().GetOffsetY() };

				Polygon(dc, triangle, 3);
			}
			break;
			case FigureType::Ellipse:
				Ellipse(dc, centerX + figure.left + view.GetStore().GetOffsetX(), centerY + figure.top + view.GetStore().GetOffsetY(),
					centerX + figure.left + figure.width + view.GetStore().GetOffsetX(), centerY + figure.top + figure.height + view.GetStore().GetOffsetY());
				break;
			default:
				break;
		}

		SelectPen(dc, oldPen);
		SelectBrush(dc, oldBrush); // Использовать обёртку

		DeletePen(pen);
		DeleteBrush(brush);
	}

	EndPaint(hwnd, &ps);
}

void OnLButtonDown(HWND hwnd, int x, int y, View& view)
{
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int mouseX = x - rcClient.right / 2;
	int mouseY = y - rcClient.bottom / 2;

	// Не известно нужна ли вообще эта проверка
	if (view.GetStore().IsClickToFigures(mouseX, mouseY))
	{
		view.MouseDown(mouseX, mouseY);

		SetCapture(hwnd);
	}
}

void OnMouseMove(HWND hwnd, int x, int y, View& view)
{
	if (!view.IsDraging())
	{
		return;
	}

	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int mouseX = x - rcClient.right / 2;
	int mouseY = y - rcClient.bottom / 2;

	view.SetNewMousePos(mouseX, mouseY);

	InvalidateRect(hwnd, NULL, TRUE);
	UpdateWindow(hwnd);
}


void OnLButtonUp(HWND hwnd, View& view)
{
	if (view.IsDraging())
	{
		view.MouseUp();
	}

	ReleaseCapture();
}

static LRESULT CALLBACK WindowProc(
	HWND hwnd,
	UINT uMsg,
	WPARAM wParam,
	LPARAM lParam)
{
	View* view;
	if (uMsg == WM_NCCREATE)
	{
		CREATESTRUCT* pCreate = reinterpret_cast<CREATESTRUCT*>(lParam);
		view = reinterpret_cast<View*>(pCreate->lpCreateParams);
		SetWindowLongPtr(hwnd, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(view));
	}
	else
	{
		view = reinterpret_cast<View*>(GetWindowLongPtr(hwnd, GWLP_USERDATA));
	}

	switch (uMsg)
	{
		HANDLE_MSG(hwnd, WM_DESTROY, OnDestroy);

		case  WM_PAINT:
			OnPaint(hwnd, *view);
			break;

		case WM_LBUTTONDOWN:
		{
			int x = LOWORD(lParam);
			int y = HIWORD(lParam);

			OnLButtonDown(hwnd, x, y, *view);
		}
		break;
		case WM_MOUSEMOVE:
		{
			int x = LOWORD(lParam);
			int y = HIWORD(lParam);

			OnMouseMove(hwnd, x, y, *view);
		}
		break;

		case WM_LBUTTONUP:
			OnLButtonUp(hwnd, *view);
			break;
	}

	return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

bool RegisterWndClass(HINSTANCE hInstance)
{
	WNDCLASSEX wndClass =
	{
		sizeof(wndClass), //UINT cbSize;
		CS_HREDRAW | CS_VREDRAW, //UINT style;
		&WindowProc, //WNDPROC lpfnWndProc;
		0, //int cbClsExtra;
		0, //int cbWndExtra;
		hInstance, //HINSTANCE hInstance;
		NULL, //HICON hIcon;
		LoadCursor(NULL, IDC_ARROW), //HCURSOR hCursor;
		(HBRUSH)(COLOR_BTNFACE + 1), //HBRUSH hbrBackground;
		NULL, //LPCTSTR lpszMenuName;
		CLASS_NAME, //LPCTSTR lpszClassName;
		NULL, //HICON hIconSm;
	};
	return RegisterClassEx(&wndClass) != FALSE;
}

HWND CreateMainWindow(HINSTANCE hInstance, View& view)
{
	HWND hMainWindow = CreateWindowEx(
		0,                                  // расширенные стили окна
		CLASS_NAME,
		WINDOW_TITLE,
		WS_OVERLAPPEDWINDOW,            // стили окна
		CW_USEDEFAULT, CW_USEDEFAULT,   // координаты по-умолчанию
		CW_USEDEFAULT, CW_USEDEFAULT,   // размер по умолчанию
		NULL,                           // дескриптор родительского окна
		NULL,                           // дескриптор меню
		hInstance,
		&view);                          // доп. параметры окна

	// Тут в доп параметры опрокинуть ссылку на изменеяемый объект

	return hMainWindow;
}

int MainLoop()
{
	MSG msg;
	BOOL res;
	while ((res = GetMessage(&msg, NULL, 0, 0)) != 0)
	{
		if (res == -1)
		{
			return 1;
		}
		else
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return static_cast<int>(msg.wParam);
}

int WINAPI WinMain(
	HINSTANCE hInstance,
	HINSTANCE /*hPrevInstance*/,
	LPSTR /*lpCmdLine*/,
	int nCmdShow)
{
	if (!RegisterWndClass(hInstance))
	{
		return 1;
	}

	FigureStore store;
	View view(store);

	HWND hMainWindow = CreateMainWindow(hInstance, view);
	if (hMainWindow == NULL)
	{
		return 1;
	}

	ShowWindow(hMainWindow, nCmdShow);
	UpdateWindow(hMainWindow);

	return MainLoop();
}