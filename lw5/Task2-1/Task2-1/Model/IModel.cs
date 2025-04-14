namespace Task2_1.Model;

public interface IModel
{
    delegate void OpenCellEventHandler(Cell cell, string value);
    delegate void CellEventHandler(Cell cell);
    delegate void DoubleCellEventHandler(Cell first, Cell second);
    delegate void WithoutArgsEventHandler();
    delegate void DoubleIntEventHandler(int first, int second);

    event OpenCellEventHandler OnOpenCell;
    event CellEventHandler OnCloseCell;
    event DoubleCellEventHandler OnClearCell;
    event DoubleCellEventHandler OnDoubleCloseCell;
    event WithoutArgsEventHandler OnWin;
    event DoubleIntEventHandler OnInit;

    // Setup and start game
    void NewGame(int difficulty);

    // Game functions
    void ClickToCell(Cell cell);

    // Get current state of game
    IReadOnlyList<Cell> GetClosedCells();
    Cell? GetOpenedCell();
    bool IsWin { get; }
    bool IsPlaying { get; }
}
