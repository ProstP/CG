#include <windows.h>
#include "Task3-2.h"
#include <windowsx.h>
#include <tchar.h>
#include "Painter/EllipsePainter.h"

TCHAR const CLASS_NAME[] = _T("MainWndClass");
TCHAR const WINDOW_TITLE[] = _T("Task2");

void OnDestroy(HWND hWnd)
{
	PostQuitMessage(0);
}

static LRESULT CALLBACK WindowProc(
	HWND hwnd,
	UINT uMsg,
	WPARAM wParam,
	LPARAM lParam)
{
	CirclePainter* painter;
	if (uMsg == WM_NCCREATE)
	{
		CREATESTRUCT* pCreate = reinterpret_cast<CREATESTRUCT*>(lParam);
		painter = reinterpret_cast<CirclePainter*>(pCreate->lpCreateParams);
		SetWindowLongPtr(hwnd, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(painter));
	}
	else
	{
		painter = reinterpret_cast<CirclePainter*>(GetWindowLongPtr(hwnd, GWLP_USERDATA));
	}

	switch (uMsg)
	{
		case WM_PAINT:
		{
			PAINTSTRUCT ps;
			HDC hdc = BeginPaint(hwnd, &ps);
			RECT rcClient;
			GetClientRect(hwnd, &rcClient);

			painter->PaintCircle(hdc, rcClient.right, rcClient.bottom);

			EndPaint(hwnd, &ps);
		}
		break;
		case WM_DESTROY:
			OnDestroy(hwnd);
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

HWND CreateMainWindow(HINSTANCE hInstance, CirclePainter& painter)
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
		&painter);                          // доп. параметры окна

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

	CirclePainter painter(500, 250, 100, { 100, 200, 255 });
	HWND hMainWindow = CreateMainWindow(hInstance, painter);
	if (hMainWindow == NULL)
	{
		return 1;
	}

	ShowWindow(hMainWindow, nCmdShow);
	UpdateWindow(hMainWindow);

	return MainLoop();
}