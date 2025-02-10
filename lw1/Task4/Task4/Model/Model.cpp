#include "Model.h"
#include <fstream>
#include <iostream>
#include <random>

Model::GameHandler::GameHandler(const std::string& fileName)
{
	std::ifstream inFile(fileName);

	if (!inFile.is_open())
	{
		throw std::invalid_argument("Failed to read input file");
	}

	std::string line;
	while (std::getline(inFile, line))
	{
		std::string question;
		std::string word;
		int i = 0;
		bool isWord = false;

		for (int i = 0; i < line.length(); i++)
		{
			if (line[i] == ':')
			{
				isWord = true;
			}
			else
			{
				if (isWord)
				{
					word += line[i];
				}
				else
				{
					question += line[i];
				}
			}
		}

		m_words.push_back(std::pair<std::string, std::string>(question, word));
		//m_questionToWords[question] = word;
	}
}

void Model::GameHandler::NewGame()
{
	SelectNewWord();
	ResetAlphabet();
	ResetOpenedLetters();
	m_mistake = 0;

	NotifyObservers();
}

std::vector<std::optional<char>> Model::GameHandler::GetWord() const
{
	if (m_currentPair == -1)
	{
		return std::vector<std::optional<char>>();
	}

	std::vector<std::optional<char>> word;

	for (int i = 0; i < m_openedLetters.size(); i++)
	{
		if (m_openedLetters[i])
		{
			word.push_back(m_words[m_currentPair].second[i]);
		}
		else
		{
			word.push_back(std::nullopt);
		}
	}

	return word;
}

std::map<char, Model::WordStates> Model::GameHandler::GetAlphabet() const
{
	std::map<char, WordStates> alphabet;

	for (char ch = 'À'; ch <= 'ß'; ch++)
	{
		if (m_alphabet.contains(ch))
		{
			alphabet[ch] = m_alphabet.at(ch);
		}
		else
		{
			alphabet[ch] = WordStates::None;
		}
	}

	return alphabet;
}

void Model::GameHandler::OpenLetter(char letter)
{
	m_changedData.mistakeCount = m_mistake;
	m_changedData.openedSymbolsInWord.clear();
	m_changedData.openedSymbolsInAlphabet.clear();

	if (IsLost() || IsWin())
	{
		return;
	}
	if (m_alphabet.contains(letter))
	{
		if (m_alphabet.at(letter) != WordStates::None)
		{
			return;
		}
	}

	std::string word = m_words[m_currentPair].second;
	std::vector<int> letterIndexes;

	for (int i = 0; i < word.length(); i++)
	{
		if (ChToLower(word[i]) == ChToLower(letter))
		{
			letterIndexes.push_back(i);
		}
	}

	if (letterIndexes.empty())
	{
		m_changedData.openedSymbolsInAlphabet[letter] = WordStates::Error;

		m_changedData.mistakeCount++;
	}
	else
	{
		m_changedData.openedSymbolsInAlphabet[letter] = WordStates::Right;

		for (int index : letterIndexes)
		{
			m_changedData.openedSymbolsInWord[index] = word[index];
		}
	}

	NotifyObservers();

	for (const auto& letter : m_changedData.openedSymbolsInAlphabet)
	{
		m_alphabet[letter.first] = letter.second;
	}
	m_mistake = m_changedData.mistakeCount;
	for (const auto& index : m_changedData.openedSymbolsInWord)
	{
		m_openedLetters[index.first] = true;
	}
}

bool Model::GameHandler::IsLost() const
{
	return m_mistake >= MAX_MISTAKES;
}

bool Model::GameHandler::IsWin() const
{
	for (int i = 0; i < m_openedLetters.size(); i++)
	{
		if (!m_openedLetters[i])
		{
			return false;
		}
	}

	return true;
}

int Model::GameHandler::GetMistake() const
{
	return m_mistake;
}

std::string Model::GameHandler::GetQuestion() const
{
	return m_words[m_currentPair].first;
}

Model::GameChangedData Model::GameHandler::GetChangedData() const
{
	return m_changedData;
}

void Model::GameHandler::SelectNewWord()
{
	if (m_words.size() == 0)
	{
		return;
	}
	if (m_words.size() == 1)
	{
		m_currentPair = 0;
		return;
	}

	int indexForNewPair;

	do
	{
		indexForNewPair = CalculateRandomValue(m_words.size());

	} while (indexForNewPair == m_currentPair);

	m_currentPair = indexForNewPair;
}

void Model::GameHandler::ResetAlphabet()
{
	m_alphabet.clear();
}

void Model::GameHandler::ResetOpenedLetters()
{
	m_openedLetters.clear();
	int wordLen = m_words[m_currentPair].second.length();
	m_openedLetters = std::vector<bool>(wordLen, false);
}

char Model::GameHandler::ChToLower(char ch)
{
	if (ch == '\n')
	{
		return ' ';
	}
	if ((int)ch <= -33 && (int)ch >= -64)
	{
		return (char)(ch + 32);
	}
	else
	{
		return std::tolower(ch);
	}
	return ch;
}

int Model::GameHandler::CalculateRandomValue(int maxValue)
{
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_int_distribution<> dis(0, maxValue - 1);
	return dis(gen);
}
