#include "Presentation.h"

IView* Presentation::GetCurrentView()
{
	return m_currentView;
}

void Presentation::OpenLetter(char letter)
{
	m_gameHandler.OpenLetter(letter);
}

void Presentation::ToggleView()
{
	if (m_isMainView)
	{
		m_currentView = &m_alternativeView;

		m_isMainView = false;
	}
	else
	{
		m_currentView = &m_mainView;

		m_isMainView = true;
	}

	UpdateInfoForNewView();
}

void Presentation::NewGame()
{
	m_gameHandler.NewGame();

	m_currentView->InitWord(m_gameHandler.GetWord().size());
	m_currentView->SetQuestion(m_gameHandler.GetQuestion());
	m_currentView->SetMistakeCount(0);
	m_currentView->InitAlphabet();
}

bool Presentation::IsWin() const
{
	return m_gameHandler.IsWin();
}

bool Presentation::IsLost() const
{
	return m_gameHandler.IsLost();
}

void Presentation::Update(GameChangedData data)
{
	m_currentView->SetMistakeCount(data.mistakeCount);

	for (const auto& letterInWord : data.openedSymbolsInWord)
	{
		m_currentView->OpenLetter(letterInWord.first, letterInWord.second);
	}
	for (const auto& letterInAlphabet : data.openedSymbolsInAlphabet)
	{
		m_currentView->SetLetterStateInAlphabet(letterInAlphabet.first, letterInAlphabet.second);
	}
}

void Presentation::Update(ViewData data)
{
	if (data.needChangeView)
	{
		ToggleView();
	}
	else
	{
		OpenLetter(data.lastOpenedChar);
	}
}

void Presentation::UpdateInfoForNewView()
{
	m_currentView->SetMistakeCount(m_gameHandler.GetMistake());

	auto word = m_gameHandler.GetWord();
	m_currentView->InitAlphabet();
	m_currentView->InitWord(word.size());
	m_currentView->SetQuestion(m_gameHandler.GetQuestion());

	for (int i = 0; i < word.size(); i++)
	{
		if (word[i] == std::nullopt)
		{
			continue;
		}

		m_currentView->OpenLetter(i, word[i].value());
	}

	for (const auto& letters : m_gameHandler.GetAlphabet())
	{
		m_currentView->SetLetterStateInAlphabet(letters.first, letters.second);
	}
}
