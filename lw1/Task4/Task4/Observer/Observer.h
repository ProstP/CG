#pragma once
#include <set>

namespace Observer
{

	template <typename T>
	class IObserver
	{
	public:
		virtual void Update(T data) = 0;
		virtual ~IObserver() = default;
	};

	template <typename T>
	class IObservable
	{
	public:
		virtual void RegisterObserver(IObserver<T>& observer) = 0;
		virtual void NotifyObservers() = 0;
		virtual void RemoveObserver(IObserver<T>& observer) = 0;

		virtual ~IObservable() = default;
	};

	template <typename T>
	class Observable : public IObservable<T>
	{
	public:
		void RegisterObserver(IObserver<T>& observer) override
		{
			m_observers.insert(&observer);
		}

		void NotifyObservers() override
		{
			T data = GetChangedData();
			auto observers = m_observers;
			for (auto* observer : observers)
			{
				observer->Update(data);
			}
		}

		void RemoveObserver(IObserver<T>& observer) override
		{
			m_observers.erase(&observer);
		}

	protected:
		virtual T GetChangedData() const = 0;

	private:
		std::set<IObserver<T>*> m_observers;
	};

}