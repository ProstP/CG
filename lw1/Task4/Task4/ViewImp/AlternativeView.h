#pragma once
#include "../View/IView.h"

using namespace View;

class AlternativeView : public View::IView
{
public:
	void OnPaint(HWND hwnd) override;
	void OnLButtonDown(HWND hwnd, int x, int y) override;
	void OnDestroy(HWND hwnd) override;

	void InitWord(int length) override;
	void SetQuestion(const std::string& question) override;
	void InitAlphabet() override;
	void OpenLetter(int index, char letter) override;
	void SetLetterStateInAlphabet(char letter, Model::WordStates state) override;
	void SetMistakeCount(int count) override;

	bool NeedPaint() const override;

	void RegisterObserver(Observer::IObserver<ViewData>& observer) override;

	void NotifyObservers() override;

	void RemoveObserver(Observer::IObserver<ViewData>& observer) override;

private:
	std::string m_question;
	char m_lastOpenedLetter;
	std::vector<std::optional<char>> m_word;
	int m_mistakeCount = 0;
	std::vector<std::pair<char, Model::WordStates>> m_alphabet;
	bool m_needPaint = true;
	std::set<Observer::IObserver<ViewData>*> m_observers;
	bool m_needChangeMenu = false;

	void PaintQuestion(HDC hdc, int x, int y);
	void PaintWord(HDC hdc, int x, int y);
	void PaintMistakes(HDC hdc, int x, int y);
	void PaintAlphabet(HDC hdc, int x, int y);
	void PaintUsedLetters(HDC hdc, int x, int y);
	void PaintButtonToChangeView(HDC hdc, int x, int y);
	void CheckAndClickToBtn(int x, int y);
	void CheckAndOpenLetter(int x, int y);
	int GetIndexOfLetterInAlphabet(char letter);
	static std::wstring StringToWstring(const std::string& str);
	static wchar_t CharToWChar(char ch);
};

