using Task2_1.Model;

namespace Task2_1.View;

public interface IView
{
    delegate void ClickToCellEventHandler(Cell cell);
    delegate void IntEventHandler(int value);

    event ClickToCellEventHandler OnClickToCell;
    event IntEventHandler OnStartNewGame;

    // Init
    void Init(int width, int height);

    // Update
    void OpenCell(Cell cell, string value);
    void CloseCell(Cell cell);
    void CloseCell(Cell first, Cell second);
    void ClearCell(Cell first, Cell second);

    // End game
    void Win();
}
