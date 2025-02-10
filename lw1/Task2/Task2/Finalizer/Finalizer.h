#pragma once
#include <utility>

template <typename Fn>
class Finalizer
{
public:
	explicit Finalizer(Fn&& fn) noexcept
		: m_fn(std::move(fn))
	{
	}
	explicit Finalizer(const Fn& fn) noexcept
		: m_fn(fn)
	{
	}

	Finalizer(const Finalizer&) = delete;
	Finalizer(Finalizer&& other) noexcept
		: m_fn(std::move(other.m_fn))
		, m_invoke(std::exchange(other.m_invoke, false))
	{
	}
	Finalizer& operator=(const Finalizer&) = delete;
	Finalizer& operator=(Finalizer&&) = delete;

	~Finalizer() noexcept
	{
		m_fn();
	}

private:
	Fn m_fn;
	bool m_invoke = true;
};

template <typename Fn>
[[nodiscard]] auto Finally(Fn&& fn)
{
	return Finalizer<std::decay_t<Fn>>(std::forward<Fn>(fn));
}
