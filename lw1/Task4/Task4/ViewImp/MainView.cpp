#include "MainView.h"
#include "../Finalizer/Finalizer.h"
#include <Windows.h>
#include <iostream>
#include <tchar.h>

using namespace Utils;

void MainView::OnPaint(HWND hwnd)
{
	PAINTSTRUCT ps;
	HDC hdc = BeginPaint(hwnd, &ps);
	SetBkMode(hdc, TRANSPARENT);
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int centerX = rcClient.right / 2;
	int centerY = rcClient.bottom / 2;

	PaintQuestion(hdc, centerX, centerY);
	PaintAlphabet(hdc, centerX, centerY);
	PaintMistakes(hdc, centerX, centerY);
	PaintWord(hdc, centerX, centerY);

	EndPaint(hwnd, &ps);
	m_needPaint = false;
}

void MainView::OnLButtonDown(HWND hwnd, int x, int y)
{
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int mouseX = x - rcClient.right / 2;
	int mouseY = y - rcClient.bottom / 2;

	OpenLetter(mouseX, mouseY);
}

void MainView::OnDestroy(HWND hwnd)
{
	PostQuitMessage(0);
}

void MainView::InitWord(int length)
{
	m_word = std::vector<std::optional<char>>(length, std::nullopt);
	m_needPaint = true;
}

void MainView::SetQuestion(const std::string& question)
{
	if (m_question == question || question.empty())
	{
		return;
	}

	m_question = question;
}

void MainView::InitAlphabet()
{
	m_alphabet.clear();
}

void MainView::OpenLetter(int index, char letter)
{
	if (index < 0 || index >= m_word.size())
	{
		return;
	}
	if (m_word[index] != std::nullopt)
	{
		return;
	}

	m_word[index] = letter;

	m_needPaint = true;
}

void MainView::SetLetterStateInAlphabet(char letter, Model::WordStates state)
{
	if (state == Model::WordStates::None)
	{
		return;
	}

	m_alphabet[letter] = state;

	m_needPaint = true;
}

void MainView::SetMistakeCount(int count)
{
	if (m_mistakeCount == count)
	{
		return;
	}

	m_mistakeCount = count;
	m_needPaint = true;
}

bool MainView::NeedPaint() const
{
	return m_needPaint;
}

void MainView::RegisterObserver(Observer::IObserver<char>& observer)
{
	m_observers.insert(&observer);
}

void MainView::PaintQuestion(HDC hdc, int x, int y)
{
	HFONT hFont = CreateFont(
		36,
		0,
		0,
		0,
		FW_NORMAL,
		FALSE,
		FALSE,
		FALSE,
		ANSI_CHARSET,
		OUT_DEFAULT_PRECIS,
		CLIP_DEFAULT_PRECIS,
		DEFAULT_QUALITY,
		DEFAULT_PITCH | FF_SWISS,
		L"Arial"
	);

	auto restoreOldFont = Finally([hdc, hFont, oldFont = SelectObject(hdc, hFont)]
								  {
									  SelectObject(hdc, oldFont);
									  DeleteObject(hFont);
								  });

	std::wstring wquestion = StringToWstring(m_question);

	TextOut(hdc, x - 200, y - 250, wquestion.c_str(), wquestion.length());
}

void MainView::PaintWord(HDC hdc, int x, int y)
{
	HFONT hFont = CreateFont(
		24,
		0,
		0,
		0,
		FW_NORMAL,
		FALSE,
		FALSE,
		FALSE,
		ANSI_CHARSET,
		OUT_DEFAULT_PRECIS,
		CLIP_DEFAULT_PRECIS,
		DEFAULT_QUALITY,
		DEFAULT_PITCH | FF_SWISS,
		L"Arial"
	);


	auto restoreOldFont = Finally([hdc, hFont, oldFont = SelectObject(hdc, hFont)]
								  {
									  SelectObject(hdc, oldFont);
									  DeleteObject(hFont);
								  });

	LOGBRUSH brushInfo;
	brushInfo.lbStyle = BS_SOLID;
	brushInfo.lbColor = RGB(0, 0, 255);
	brushInfo.lbHatch = 0;
	HBRUSH brush = CreateBrushIndirect(&brushInfo);

	auto restoreOldBrush = Finally([hdc, brush, oldBrush = SelectObject(hdc, brush)]
								   {
									   SelectObject(hdc, oldBrush);
									   DeleteObject(brush);
								   });

	for (int i = 0; i < m_word.size(); i++)
	{
		int offsetX = i * 40 - 80;

		Rectangle(hdc, offsetX + x, 20 + y, offsetX + x + 20, 15 + y);

		if (m_word[i] == std::nullopt)
		{
			continue;
		}

		std::string str;
		str += m_word[i].value();
		TextOut(hdc, offsetX + x + 3, y - 10, StringToWstring(str).c_str(), 1);
	}
}

void MainView::PaintMistakes(HDC hdc, int x, int y)
{
	HPEN pen = CreatePen(PS_SOLID, 2, RGB(100, 100, 100));

	auto restoreOldPen = Finally([hdc, pen, oldPen = SelectObject(hdc, pen)]
								 {
									 SelectObject(hdc, oldPen);
									 DeleteObject(pen);
								 });

	LOGBRUSH brushInfo;
	brushInfo.lbStyle = BS_SOLID;
	brushInfo.lbColor = RGB(90, 90, 90);
	brushInfo.lbHatch = 0;
	HBRUSH brush = CreateBrushIndirect(&brushInfo);

	auto restoreOldBrush = Finally([hdc, brush, oldBrush = SelectObject(hdc, brush)]
								   {
									   SelectObject(hdc, oldBrush);
									   DeleteObject(brush);
								   });

	Rectangle(hdc, x - 500, y - 150, x - 490, y + 100);
	Rectangle(hdc, x - 500, y - 150, x - 350, y - 140);

	HPEN penForHuman = CreatePen(PS_SOLID, 4, RGB(217, 153, 26));
	auto restoreOldPenForHuman = Finally([hdc, penForHuman, oldPen = SelectObject(hdc, penForHuman)]
										 {
											 SelectObject(hdc, oldPen);
											 DeleteObject(penForHuman);
										 });
	LOGBRUSH brushForHumanInfo;
	brushForHumanInfo.lbStyle = BS_SOLID;
	brushForHumanInfo.lbColor = RGB(217, 153, 26);
	brushForHumanInfo.lbHatch = 0;
	HBRUSH brushForHuman = CreateBrushIndirect(&brushForHumanInfo);

	auto restoreOldBrushForHuman = Finally([hdc, brushForHuman, oldBrush = SelectObject(hdc, brushForHuman)]
										   {
											   SelectObject(hdc, oldBrush);
											   DeleteObject(brushForHuman);
										   });

	if (m_mistakeCount >= 1)
	{
		MoveToEx(hdc, x - 352, y - 140, NULL);
		LineTo(hdc, x - 352, y - 75);
	}

	if (m_mistakeCount >= 2)
	{
		Ellipse(hdc, x - 372, y - 95, x - 332, y - 55);
	}

	if (m_mistakeCount >= 3)
	{
		MoveToEx(hdc, x - 352, y - 55, NULL);
		LineTo(hdc, x - 352, y + 10);
	}

	if (m_mistakeCount >= 4)
	{
		MoveToEx(hdc, x - 352, y + 10, NULL);
		LineTo(hdc, x - 369, y + 70);
	}

	if (m_mistakeCount >= 5)
	{
		MoveToEx(hdc, x - 352, y + 10, NULL);
		LineTo(hdc, x - 335, y + 70);
	}

	if (m_mistakeCount >= 6)
	{
		MoveToEx(hdc, x - 352, y - 45, NULL);
		LineTo(hdc, x - 369, y);
	}

	if (m_mistakeCount >= 7)
	{
		MoveToEx(hdc, x - 352, y - 45, NULL);
		LineTo(hdc, x - 335, y);
	}
}

void MainView::PaintAlphabet(HDC hdc, int x, int y)
{
	HFONT hFont = CreateFont(
		18,
		0,
		0,
		0,
		FW_NORMAL,
		FALSE,
		FALSE,
		FALSE,
		ANSI_CHARSET,
		OUT_DEFAULT_PRECIS,
		CLIP_DEFAULT_PRECIS,
		DEFAULT_QUALITY,
		DEFAULT_PITCH | FF_SWISS,
		L"Arial"
	);


	auto restoreOldFont = Finally([hdc, hFont, oldFont = SelectObject(hdc, hFont)]
								  {
									  SelectObject(hdc, oldFont);
									  DeleteObject(hFont);
								  });

	int count = 0;
	for (wchar_t ch = L'À'; ch <= L'ß'; ch++)
	{
		COLORREF textColor;

		if (m_alphabet.contains('À' + count))
		{
			switch (m_alphabet.at('À' + count))
			{
				case Model::WordStates::Error:
					textColor = RGB(255, 100, 100);
					break;
				case Model::WordStates::Right:
					textColor = RGB(100, 255, 100);
					break;
				default:
					textColor = RGB(100, 100, 100);
					break;
			}
		}
		else
		{
			textColor = RGB(100, 100, 100);
		}

		auto restoreOldColor = Finally([hdc, textColor, oldColor = SetTextColor(hdc, textColor)]
									   {
										   SetTextColor(hdc, oldColor);
									   });

		int offsetX = count * 20 - 300;

		std::wstring wstring;
		wstring += ch;
		TextOut(hdc, offsetX + x, y + 200, wstring.c_str(), 2);

		count++;
	}
}

void MainView::OpenLetter(int x, int y)
{
	if (y < 200 || y > 215)
	{
		return;
	}
	if (x < -305 || x > 340)
	{
		return;
	}

	double letterSize = 645 / 32;

	x += 305;

	int letterNum = x / letterSize;

	m_lastOpenedLetter = 'À' + letterNum;

	NotifyObservers();
}

void MainView::NotifyObservers()
{
	auto observers = m_observers;
	for (auto* observer : observers)
	{
		observer->Update(m_lastOpenedLetter);
	}
}

void MainView::RemoveObserver(Observer::IObserver<char>& observer)
{
	m_observers.erase(&observer);
}

std::wstring MainView::StringToWstring(const std::string& str)
{
	std::wstring wStr;

	for (int i = 0; i < str.length(); i++)
	{
		wStr += CharToWChar(str[i]);
	}

	return wStr;
}

wchar_t MainView::CharToWChar(char ch)
{
	if ('À' <= ch && ch <= 'ß')
	{
		int offset = ch - 'À';

		wchar_t wch = L'À';

		wch += offset;

		return wch;
	}
	else if ('à' <= ch && ch <= 'ÿ')
	{
		int offset = ch - 'à';

		wchar_t wch = L'à';

		wch += offset;

		return wch;
	}

	return static_cast<wchar_t>(ch);
}
