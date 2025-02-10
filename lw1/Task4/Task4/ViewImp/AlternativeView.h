#pragma once
#include "../View/IView.h"

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

	void RegisterObserver(Observer::IObserver<char>& observer) override;

	void NotifyObservers() override;

	void RemoveObserver(Observer::IObserver<char>& observer) override;

private:
	std::string m_question;
	char m_lastOpenedLetter;
	std::vector<std::optional<char>> m_word;
	int m_mistakeCount = 0;
	std::vector<std::pair<char, Model::WordStates>> m_alphabet;
	bool m_needPaint = true;
	std::set<Observer::IObserver<char>*> m_observers;

	void PaintQuestion(HDC hdc, int x, int y);
	void PaintWord(HDC hdc, int x, int y);
	void PaintMistakes(HDC hdc, int x, int y);
	void PaintAlphabet(HDC hdc, int x, int y);
	void PaintUsedLetters(HDC hdc, int x, int y);
	void OpenLetter(int x, int y);
	int GetIndexOfLetterInAlphabet(char letter);
	static std::wstring StringToWstring(const std::string& str);
	static wchar_t CharToWChar(char ch);
};

