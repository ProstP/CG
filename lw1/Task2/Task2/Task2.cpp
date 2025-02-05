#include <windows.h>
#include <tchar.h>
#include <windowsx.h>
#include <math.h>
#include <string>
#include "Store/FigureStore.h"

TCHAR const CLASS_NAME[] = _T("MainWndClass");
TCHAR const WINDOW_TITLE[] = _T("Task2");

void OnDestroy(HWND hWnd)
{
	PostQuitMessage(0);
}

int g_selected = -1;
// Избавится от глобальных переменных

int g_lastMouseX = 0;
int g_lastMouseY = 0;

void OnPaint(HWND hwnd)
{
	PAINTSTRUCT ps;
	HDC dc = BeginPaint(hwnd, &ps);
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int centerX = rcClient.right / 2;
	int centerY = rcClient.bottom / 2;

	for (int i = 0; i < FigureStore::FIGURE_COUNT; i++)
	{
		HPEN pen = CreatePen(PS_SOLID, 3, RGB(g_figures[i].borderColor.R, g_figures[i].borderColor.G, g_figures[i].borderColor.B));

		LOGBRUSH brushInfo;
		brushInfo.lbStyle = BS_SOLID;
		brushInfo.lbColor = RGB(g_figures[i].fillColor.R, g_figures[i].fillColor.G, g_figures[i].fillColor.B);
		brushInfo.lbHatch = 0;
		HBRUSH brush = CreateBrushIndirect(&brushInfo);

		HPEN oldPen = SelectPen(dc, pen);
		HBRUSH oldBrush = SelectBrush(dc, brush);

		switch (g_figures[i].type)
		{
		case FigureType::Rectangle:
			Rectangle(dc, centerX + g_figures[i].left, centerY + g_figures[i].top,
				centerX + g_figures[i].left + g_figures[i].width, centerY + g_figures[i].top + g_figures[i].height);
			break;
		case FigureType::Triangle:
		{
			POINT triangle[3];
			triangle[0] = { centerX + g_figures[i].left + g_figures[i].width / 2, centerY + g_figures[i].top };
			triangle[1] = { centerX + g_figures[i].left, centerY + g_figures[i].top + g_figures[i].height };
			triangle[2] = { centerX + g_figures[i].left + g_figures[i].width, centerY + g_figures[i].top + g_figures[i].height };

			Polygon(dc, triangle, 3);
		}
		break;
		case FigureType::Ellipse:
			Ellipse(dc, centerX + g_figures[i].left, centerY + g_figures[i].top,
				centerX + g_figures[i].left + g_figures[i].width, centerY + g_figures[i].top + g_figures[i].height);
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

void OnLButtonDown(HWND hwnd, BOOL fDoubleClick, int x, int y, UINT keyFlags)
{
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int mouseX = x - rcClient.right / 2;
	int mouseY = y - rcClient.bottom / 2;

	for (int i = FIGURE_COUNT - 1; i >= 0; i--)
	{
		if (g_figures[i].left <= mouseX
			&& mouseX <= g_figures[i].left + g_figures[i].width
			&& g_figures[i].top <= mouseY
			&& mouseY <= g_figures[i].top + g_figures[i].height)
		{
			g_selected = i;

			g_lastMouseX = mouseX;
			g_lastMouseY = mouseY;

			SetCapture(hwnd);
			return;
		}
	}
}

void OnMouseMove(HWND hwnd, int x, int y, UINT keyFlags)
{
	if (g_selected == -1 || g_selected >= FigureStore::FIGURE_COUNT)
	{
		return;
	}

	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int mouseX = x - rcClient.right / 2;
	int mouseY = y - rcClient.bottom / 2;

	int dx = mouseX - g_lastMouseX;
	int dy = mouseY - g_lastMouseY;

	g_lastMouseX = mouseX;
	g_lastMouseY = mouseY;

	g_figures[g_selected].left += dx;
	g_figures[g_selected].top += dy;

	InvalidateRect(hwnd, NULL, TRUE);
	UpdateWindow(hwnd);
}

void OnLButtonUp(HWND hwnd, int x, int y, UINT keyFlags)
{
	if (g_selected != -1)
	{
		g_selected = -1;
	}

	ReleaseCapture();
}

LRESULT CALLBACK WindowProc(
	HWND hwnd,
	UINT uMsg,
	WPARAM wParam,
	LPARAM lParam)
{
	switch (uMsg)
	{
		HANDLE_MSG(hwnd, WM_DESTROY, OnDestroy);
		HANDLE_MSG(hwnd, WM_PAINT, OnPaint);
		HANDLE_MSG(hwnd, WM_LBUTTONDOWN, OnLButtonDown);
		HANDLE_MSG(hwnd, WM_MOUSEMOVE, OnMouseMove);
		HANDLE_MSG(hwnd, WM_LBUTTONUP, OnLButtonUp);
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

HWND CreateMainWindow(HINSTANCE hInstance)
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
		NULL);                          // доп. параметры окна

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
			// произошла ошибка - нужно обработать ее и, вероятно,
			// завершить работу приложения
		}
		else
		{
			// Если это сообщение о нажатии виртуальной клавиши,
			// то добавляем в очередь сообщений сообщения, несущие информацию о
			// коде вводимого пользователем символа
			TranslateMessage(&msg);
			// передаем сообщение в соответствующую оконную процедуру
			DispatchMessage(&msg);
		}
	}
	// сюда мы попадем только в том случае извлечения сообщения WM_QUIT
	// msg.wParam содержит код возврата, помещенный при помощи функции
	// PostQuitMessage()
	return static_cast<int>(msg.wParam);
}

int WINAPI WinMain(
	HINSTANCE hInstance,
	HINSTANCE /*hPrevInstance*/,
	LPSTR /*lpCmdLine*/,
	int nCmdShow)
{
	// Регистрируем класс главного окна
	if (!RegisterWndClass(hInstance))
	{
		return 1;
	}

	// Создаем главное окно приложения
	HWND hMainWindow = CreateMainWindow(hInstance);
	if (hMainWindow == NULL)
	{
		return 1;
	}

	// Показываем главное окно приложения
	ShowWindow(hMainWindow, nCmdShow);
	UpdateWindow(hMainWindow);

	// Запускаем цикл выборки сообщений, пока не получим
	// сигнал о завершении приложения
	return MainLoop();
}