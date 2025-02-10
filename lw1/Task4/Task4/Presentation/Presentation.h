#pragma once
#include "../View/IView.h"
#include "../Model/Model.h"
#include "../Observer/Observer.h"
#include "../ViewImp/MainView.h"
#include "../ViewImp/AlternativeView.h"

using namespace View;
using namespace Model;

class Presentation : public Observer::IObserver<GameChangedData>, public Observer::IObserver<char>
{
public:
	Presentation(
		GameHandler& game
	)
		: m_gameHandler(game)
		, m_isMainView(true)
	{
		m_gameHandler.RegisterObserver(*this);
		m_mainView.RegisterObserver(*this);
		m_alternativeView.RegisterObserver(*this);

		m_currentView = &m_mainView;
	}

	IView* GetCurrentView();

	void OpenLetter(char letter);
	void ToggleView();

	void NewGame();
	bool IsWin() const;
	bool IsLost() const;

	void Update(GameChangedData data) override;
	void Update(char data) override;


	~Presentation()
	{
		m_gameHandler.RemoveObserver(*this);
		m_mainView.RemoveObserver(*this);
		m_alternativeView.RemoveObserver(*this);
	}

private:
	bool m_isMainView;
	MainView m_mainView;
	AlternativeView m_alternativeView;
	IView* m_currentView;
	GameHandler& m_gameHandler;

	void UpdateInfoForNewView();

};

