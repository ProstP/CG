#pragma once

enum class FigureType
{
	Rectangle = 0,
	Triangle,
	Ellipse
};

struct RGBColor
{
	unsigned char R;
	unsigned char G;
	unsigned char B;
};

struct Figure
{
	FigureType type;
	int left;
	int top;
	int width;
	int height;
	RGBColor fillColor;
	RGBColor borderColor;
};

class FigureStore
{
public:
	const static int FIGURE_COUNT = 13;

	Figure GetFigure(int index);
	void SetOffset(int x, int y);
	int GetOffsetX();
	int GetOffestY();

private:
	Figure m_figures[FIGURE_COUNT]
	{
		{FigureType::Rectangle, -500, 150, 1000, 200, {13, 158, 0}, {29, 209, 13}},
		{FigureType::Rectangle, -300, -50, 300, 200, {197, 212, 32}, {157, 171, 0}},
		{FigureType::Rectangle, -100, -170, 50, 100, {66, 66, 66}, {54, 54, 54}},
		{FigureType::Triangle, -350, -150, 400, 100, {227, 91, 50}, {143, 46, 4}},
		{FigureType::Ellipse, -250, 10, 80, 80, {98, 113, 245}, {31, 47, 191}},
		{FigureType::Rectangle, -115, 10, 80, 140, {179, 60, 9}, {143, 46, 4}},
		{FigureType::Ellipse, -50, 70, 10, 10, {64, 64, 64}, {77, 77, 77}},
		{FigureType::Rectangle, 0, 70, 250, 15, {140, 77, 6}, {99, 58, 10}},
		{FigureType::Rectangle, 0, 100, 250, 15, {140, 77, 6}, {99, 58, 10}},
		{FigureType::Rectangle, 40, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
		{FigureType::Rectangle, 100, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
		{FigureType::Rectangle, 160, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
		{FigureType::Rectangle, 220, 50, 15, 100, {140, 77, 6}, {99, 58, 10}},
	};

	int m_offsetX = 0;
	int m_offsetY = 0;
};

