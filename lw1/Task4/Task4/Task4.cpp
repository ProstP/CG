#include <windows.h>
#include <tchar.h>
#include <windowsx.h>
#include <math.h>
#include <string>
#include "Task4.h"
#include "Model/Model.h"
#include "Presentation/Presentation.h"
#include "ViewImp/MainView.h"

TCHAR const CLASS_NAME[] = _T("MainWndClass");
TCHAR const WINDOW_TITLE[] = _T("Task4");

static LRESULT CALLBACK WindowProc(
	HWND hwnd,
	UINT uMsg,
	WPARAM wParam,
	LPARAM lParam)
{
	Presentation* presentation;
	if (uMsg == WM_NCCREATE)
	{
		CREATESTRUCT* pCreate = reinterpret_cast<CREATESTRUCT*>(lParam);
		presentation = reinterpret_cast<Presentation*>(pCreate->lpCreateParams);
		SetWindowLongPtr(hwnd, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(presentation));
	}
	else
	{
		presentation = reinterpret_cast<Presentation*>(GetWindowLongPtr(hwnd, GWLP_USERDATA));
	}

	switch (uMsg)
	{
		case WM_DESTROY:
			presentation->GetCurrentView()->OnDestroy(hwnd);
			break;
		case WM_PAINT:
			presentation->GetCurrentView()->OnPaint(hwnd);
			break;

		case WM_LBUTTONDOWN:
		{
			int x = LOWORD(lParam);
			int y = HIWORD(lParam);

			presentation->GetCurrentView()->OnLButtonDown(hwnd, x, y);

			if (presentation->GetCurrentView()->NeedPaint())
			{
				InvalidateRect(hwnd, NULL, TRUE);
				UpdateWindow(hwnd);
			}
		}
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

HWND CreateMainWindow(HINSTANCE hInstance, Presentation& presentation)
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
		&presentation);                          // доп. параметры окна

	// Тут в доп параметры опрокинуть ссылку на изменеяемый объект

	return hMainWindow;
}

int MainLoop(HWND mainWindow, Presentation& presentation)
{
	MSG msg;
	BOOL res;
	while ((res = GetMessage(&msg, NULL, 0, 0)) != 0)
	{
		if (res == -1)
		{
			return 1;
		}
		else if (presentation.IsLost())
		{
			MessageBox(mainWindow, _T("Я сожалею, но вы превысили количество даных вам попыток и проиграли, постарайтесь в следующий раз."), _T("Вы проиграли"), MB_OK);
			return 0;
		}
		else if (presentation.IsWin())
		{
			if (MessageBox(mainWindow, _T("Поздравляю, вы выиграли, не хотители разгадать ещё одно слово?"), _T("Вы выиграли!"), MB_YESNO) == IDYES)
			{
				presentation.NewGame();
				InvalidateRect(mainWindow, NULL, TRUE);
				UpdateWindow(mainWindow);
			}
			else
			{
				return 0;
			}
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

	GameHandler game("Words.txt");
	Presentation presentation(game);

	presentation.NewGame();

	HWND hMainWindow = CreateMainWindow(hInstance, presentation);
	if (hMainWindow == NULL)
	{
		return 1;
	}

	ShowWindow(hMainWindow, nCmdShow);
	UpdateWindow(hMainWindow);

	return MainLoop(hMainWindow, presentation);
}