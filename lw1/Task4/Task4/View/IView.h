#pragma once
#include <Windows.h>
#include <optional>
#include <vector>
#include <map>
#include "../Model/WordStates/WordStates.h"
#include "../Observer/Observer.h"
#include <string>

namespace View
{
	struct ViewData
	{
		char lastOpenedChar;
		bool needChangeView;
	};

	class IView : public Observer::IObservable<ViewData>
	{
	public:
		virtual void OnPaint(HWND hwnd) = 0;
		virtual void OnLButtonDown(HWND hwnd, int x, int y) = 0;
		virtual void OnDestroy(HWND hwnd) = 0;

		virtual void InitWord(int length) = 0;
		virtual void SetQuestion(const std::string& question) = 0;
		virtual void InitAlphabet() = 0;
		virtual void OpenLetter(int index, char letter) = 0;
		virtual void SetLetterStateInAlphabet(char letter, Model::WordStates state) = 0;
		virtual void SetMistakeCount(int count) = 0;

		virtual bool NeedPaint() const = 0;

		virtual ~IView() = default;
	};

}