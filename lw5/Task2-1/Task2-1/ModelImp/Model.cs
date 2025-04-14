using Task2_1.Model;
using Task2_1.ModelImp.Area;
using static Task2_1.Model.IModel;

namespace Task2_1.ModelImp;

public class Model : IModel
{
    GameArea _area;

    Dictionary<int, (int width, int height)> _sizeByDifficulty = new()
    {
        { 1, (4, 4) },
        { 2, (6, 4) },
        { 3, (6, 6) },
        { 4, (8, 6) },
        { 5, (8, 8) },
    };

    public bool IsWin { get; private set; }

    public bool IsPlaying { get; private set; }

    // Events
    public event OpenCellEventHandler OnOpenCell
    {
        add => _area.OnOpenCell += value;

        remove => _area.OnOpenCell -= value;
    }

    public event CellEventHandler OnCloseCell
    {
        add => _area.OnCloseCell += value;

        remove => _area.OnCloseCell -= value;
    }

    public event DoubleCellEventHandler OnDoubleCloseCell
    {
        add => _area.OnDoubleCloseCell += value;

        remove => _area.OnDoubleCloseCell -= value;
    }

    public event DoubleCellEventHandler OnClearCell
    {
        add => _area.OnClearCell += value;

        remove => _area.OnClearCell -= value;
    }

    public event WithoutArgsEventHandler OnWin;
    public event DoubleIntEventHandler OnInit;

    public Model()
    {
        _area = new();

        _area.OnAreaEmpty += Win;
    }
    ~Model()
    {
        _area.OnAreaEmpty -= Win;
    }

    // Methods from IModel
    public void ClickToCell(Cell cell)
    {
        _area.ClickToCell(cell);
    }

    public IReadOnlyList<Cell> GetClosedCells()
    {
        return _area.GetClosedCells();
    }

    public Cell? GetOpenedCell()
    {
        return _area.GetOpened();
    }

    public void NewGame(int difficulty)
    {
        IsPlaying = true;
        IsWin = false;

        int width = _sizeByDifficulty[difficulty].width;
        int height = _sizeByDifficulty[difficulty].height;
        _area.Init(width, height);
        OnInit.Invoke(width, height);
    }

    // On win
    private void Win()
    {
        IsPlaying = false;
        IsWin = true;
        OnWin.Invoke();
    }
}
