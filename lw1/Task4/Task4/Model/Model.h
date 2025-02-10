#pragma once
#include <string>
#include <vector>
#include <optional>
#include "WordStates/WordStates.h"
#include "../Observer/Observer.h"
#include <map>

namespace Model
{

	struct GameChangedData
	{
		int mistakeCount;
		std::map<int, char> openedSymbolsInWord;
		std::map<char, WordStates> openedSymbolsInAlphabet;
	};

	class GameHandler : public Observer::Observable<GameChangedData>
	{
	public:
		GameHandler(const std::string& fileName);

		void NewGame();
		std::vector<std::optional<char>> GetWord() const;
		std::map<char, WordStates>  GetAlphabet() const;
		void OpenLetter(char letter);
		bool IsLost() const;
		bool IsWin() const;
		int GetMistake() const;
		std::string GetQuestion() const;

		static const int MAX_MISTAKES = 7;

	protected:
		GameChangedData GetChangedData() const override;

	private:
		int m_mistake = 0;
		int m_currentPair = -1;
		std::vector<std::pair<std::string, std::string>> m_words;
		//std::string m_curentQuestion;
		//std::map<std::string, std::string> m_questionToWords;
		std::vector<bool> m_openedLetters;
		std::map<char, WordStates> m_alphabet;
		GameChangedData m_changedData;

		void SelectNewWord();
		void ResetAlphabet();
		void ResetOpenedLetters();
		static char ChToLower(char ch);
		static int CalculateRandomValue(int maxValue);
	};

} // namespace Model

