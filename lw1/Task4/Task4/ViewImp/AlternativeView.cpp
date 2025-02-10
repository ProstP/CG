#include "AlternativeView.h"
#include "../Finalizer/Finalizer.h"
#include "../Model/Model.h"

using namespace Utils;

void AlternativeView::OnPaint(HWND hwnd)
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
	PaintUsedLetters(hdc, centerX, centerY);

	EndPaint(hwnd, &ps);
	m_needPaint = false;
}

void AlternativeView::OnLButtonDown(HWND hwnd, int x, int y)
{
	RECT rcClient;
	GetClientRect(hwnd, &rcClient);
	int mouseX = x - rcClient.right / 2;
	int mouseY = y - rcClient.bottom / 2;

	OpenLetter(mouseX, mouseY);
}

void AlternativeView::OnDestroy(HWND hwnd)
{
	PostQuitMessage(0);
}

void AlternativeView::InitWord(int length)
{
	m_word = std::vector<std::optional<char>>(length, std::nullopt);
	m_needPaint = true;
}

void AlternativeView::SetQuestion(const std::string& question)
{
	if (question.empty() || m_question == question)
	{
		return;
	}

	m_question = question;
}

void AlternativeView::InitAlphabet()
{
	m_alphabet.clear();
}

void AlternativeView::OpenLetter(int index, char letter)
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

void AlternativeView::SetLetterStateInAlphabet(char letter, Model::WordStates state)
{
	if (state == Model::WordStates::None)
	{
		return;
	}

	int index = GetIndexOfLetterInAlphabet(letter);
	if (index == -1)
	{
		m_alphabet.push_back(std::pair<char, Model::WordStates>(letter, state));
	}
	else
	{
		m_alphabet[index].second = state;
	}

	m_needPaint = true;
}

void AlternativeView::SetMistakeCount(int count)
{
	m_mistakeCount = count;
}

bool AlternativeView::NeedPaint() const
{
	return m_needPaint;
}

void AlternativeView::RegisterObserver(Observer::IObserver<char>& observer)
{
	m_observers.insert(&observer);
}

void AlternativeView::NotifyObservers()
{
	auto observers = m_observers;
	for (auto* observer : observers)
	{
		observer->Update(m_lastOpenedLetter);
	}
}

void AlternativeView::RemoveObserver(Observer::IObserver<char>& observer)
{
	m_observers.erase(&observer);
}

void AlternativeView::PaintQuestion(HDC hdc, int x, int y)
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

	std::wstring wquestion = StringToWstring(m_question);

	TextOut(hdc, x - 475, y - 200, wquestion.c_str(), wquestion.length());
}

void AlternativeView::PaintWord(HDC hdc, int x, int y)
{
	HFONT hFont = CreateFont(
		20,
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
		int offsetX = i * 28 - 450;

		Rectangle(hdc, offsetX + x, y - 70, offsetX + x + 15, y - 75);

		if (m_word[i] == std::nullopt)
		{
			continue;
		}

		std::string str;
		str += m_word[i].value();
		TextOut(hdc, offsetX + x + 3, y - 100, StringToWstring(str).c_str(), 1);
	}
}

void AlternativeView::PaintMistakes(HDC hdc, int x, int y)
{
	HFONT hFont = CreateFont(
		20,
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

	int attemptsCount = Model::GameHandler::MAX_MISTAKES - m_mistakeCount;

	std::string text = "Количество попыток: " + std::to_string(attemptsCount);
	std::wstring wText = StringToWstring(text);

	TextOut(hdc, x - 475, y, wText.c_str(), wText.length());
}

void AlternativeView::PaintAlphabet(HDC hdc, int x, int y)
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
	for (wchar_t ch = L'А'; ch <= L'Я'; ch++)
	{
		COLORREF textColor;

		int letterIndex = GetIndexOfLetterInAlphabet('А' + count);

		if (letterIndex != -1)
		{
			switch (m_alphabet[letterIndex].second)
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
		TextOut(hdc, offsetX + x, y + 200, wstring.c_str(), 1);

		count++;
	}
}

void AlternativeView::PaintUsedLetters(HDC hdc, int x, int y)
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

	std::wstring wText = L"Использованные буквы:";
	TextOut(hdc, x - 475, y + 50, wText.c_str(), wText.length());

	int	count = 0;
	for (int i = 0; i < m_alphabet.size(); i++)
	{
		if (m_alphabet[i].second == Model::WordStates::None)
		{
			continue;
		}

		COLORREF textColor;

		switch (m_alphabet[i].second)
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

		auto restoreOldColor = Finally([hdc, textColor, oldColor = SetTextColor(hdc, textColor)]
									   {
										   SetTextColor(hdc, oldColor);
									   });

		int offsetX = count * 20 - 475;

		std::wstring wstring;
		wstring += CharToWChar(m_alphabet[i].first);
		TextOut(hdc, offsetX + x, y + 75, wstring.c_str(), 1);

		count++;
	}
}

void AlternativeView::OpenLetter(int x, int y)
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

	m_lastOpenedLetter = 'А' + letterNum;

	NotifyObservers();
}

int AlternativeView::GetIndexOfLetterInAlphabet(char letter)
{
	for (int i = 0; i < m_alphabet.size(); i++)
	{
		if (m_alphabet[i].first == letter)
		{
			return i;
		}
	}

	return -1;
}

std::wstring AlternativeView::StringToWstring(const std::string& str)
{
	std::wstring wStr;

	for (int i = 0; i < str.length(); i++)
	{
		wStr += CharToWChar(str[i]);
	}

	return wStr;
}

wchar_t AlternativeView::CharToWChar(char ch)
{
	if ('А' <= ch && ch <= 'Я')
	{
		int offset = ch - 'А';

		wchar_t wch = L'А';

		wch += offset;

		return wch;
	}
	else if ('а' <= ch && ch <= 'я')
	{
		int offset = ch - 'а';

		wchar_t wch = L'а';

		wch += offset;

		return wch;
	}

	return static_cast<wchar_t>(ch);
}
