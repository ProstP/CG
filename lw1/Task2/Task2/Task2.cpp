#include <windows.h>
#include <tchar.h>
#include <windowsx.h>
#include <math.h>
#include <string>

TCHAR const CLASS_NAME[] = _T("MainWndClass");
TCHAR const WINDOW_TITLE[] = _T("Task2");

void OnDestroy(HWND hWnd)
{
	PostQuitMessage(0);
}

enum class FigureType
{
	Rectangle = 0,
	Triangle,
	Ellipse
};

struct RGBColor
{
	unsigned char R;
	unsigned char G;
	unsigned char B;
};
struct Figure
{
	FigureType type;
	int left;
	int top;
	int width;
	int height;
	RGBColor fillColor;
	RGBColor borderColor;
};

const int FIGURE_COUNT = 13;
Figure g_figures[FIGURE_COUNT]
{
	{FigureType::Rectangle, -500, 150, 1000, 200, {13, 158, 0}, {29, 209, 13}},
	{FigureType::Rectangle, -300, -50, 300, 200, {197, 212, 32}, {157, 171, 0}},
	{FigureType::Rectangle, -100, -170, 50, 100, {66, 66, 66}, {54, 54, 54}},
	{FigureType::Triangle, -350, -150, 400, 100, {227, 91, 50}, {143, 46, 4}},
	{FigureType::Ellipse, -250, 10, 80, 80, {98, 113, 245}, {31, 47, 191}},
	{FigureType::Rectangle, -115, 10, 80, 140, {179, 60, 9}, {143, 46, 4}},
	{FigureType::Ellipse, -50, 70, 10, 10, {64, 64, 64}, {77, 77, 77}},
	{FigureType::Rectangle, 0, 70, 250, 15, {140, 77, 6}, {99, 58, 10}},
	{FigureType::Rectangle, 0, 100, 250, 15, {140, 77, 6}, {99, 58, 10}},
	{FigureType::Rectangle, 40, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
	{FigureType::Rectangle, 100, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
	{FigureType::Rectangle, 160, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
	{FigureType::Rectangle, 220, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
};

void OnPaint(HWND hwnd)
{
	PAINTSTRUCT ps;
	HDC dc = BeginPaint(hwnd, &ps);
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int centerX = rcClient.right / 2;
	int centerY = rcClient.bottom / 2;
	POINT triangle[3];

	for (int i = 0; i < FIGURE_COUNT; i++)
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
			triangle[0] = { centerX + g_figures[i].left + g_figures[i].width / 2, centerY + g_figures[i].top };
			triangle[1] = { centerX + g_figures[i].left, centerY + g_figures[i].top + g_figures[i].height };
			triangle[2] = { centerX + g_figures[i].left + g_figures[i].width, centerY + g_figures[i].top + g_figures[i].height };

			Polygon(dc, triangle, 3);
			break;
		case FigureType::Ellipse:
			Ellipse(dc, centerX + g_figures[i].left, centerY + g_figures[i].top,
				centerX + g_figures[i].left + g_figures[i].width, centerY + g_figures[i].top + g_figures[i].height);
			break;
		default:
			break;
		}

		SelectPen(dc, oldPen);
		SelectBrush(dc, oldBrush);

		DeletePen(pen);
		DeleteBrush(brush);
	}

	EndPaint(hwnd, &ps);
	//PAINTSTRUCT ps;
	//HDC dc = BeginPaint(hwnd, &ps);

	//// Создаем перо и выбираем его в контексте устройства
	//HPEN pen = CreatePen(PS_SOLID, 2, RGB(255, 0, 0));
	//HPEN oldPen = SelectPen(dc, pen);

	//// Получаем размеры клиентской области окна и находим его центр
	//RECT rcClient;
	//GetClientRect(hwnd, &rcClient);
	//int centerX = rcClient.right / 2;
	//int centerY = rcClient.bottom / 2;
	//// вычисляем угол вращения стрелки в радианах
	//double angleInRadians = g_rotationAngle * M_PI / 180.0;

	//// рисуем стрелку
	//MoveToEx(dc, centerX, centerY, NULL);
	//LineTo(
	//	dc,
	//	int(centerX + 100 * cos(angleInRadians)),
	//	int(centerY + 100 * sin(angleInRadians))
	//);

	//// Восстанавливаем ранее выбранное перо и удаляем наше
	//SelectPen(dc, oldPen);
	//DeletePen(pen);

	//EndPaint(hwnd, &ps);
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