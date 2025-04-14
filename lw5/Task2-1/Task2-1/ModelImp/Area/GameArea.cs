using Task2_1.Model;
using static Task2_1.Model.IModel;

namespace Task2_1.ModelImp.Area;

public class GameArea
{
    public event OpenCellEventHandler OnOpenCell;
    public event CellEventHandler OnCloseCell;
    public event DoubleCellEventHandler OnClearCell;
    public event DoubleCellEventHandler OnDoubleCloseCell;
    public event WithoutArgsEventHandler OnAreaEmpty;

    List<string> _values;
    Random _random = new();

    Dictionary<Cell, string> _cells = [];
    List<Cell> _openedCells = [];
    Cell? _currentCell = null;

    public GameArea(string fileName = "Values/values.txt")
    {
        _values = ReadValuesFromFile(fileName);
    }

    // Init area
    public void Init(int width, int height)
    {
        int count = width * height;
        if (count % 2 != 0)
        {
            throw new ArgumentException("Cell count must be even");
        }

        _cells.Clear();
        _openedCells.Clear();
        _currentCell = null;
        FillCells(width, height);
    }
    private void FillCells(int width, int height)
    {
        List<Cell> cells = GenerateCells(width, height);

        while (cells.Count > 0)
        {
            Cell first = GetRandomAndRemoveCell(cells);
            Cell second = GetRandomAndRemoveCell(cells);
            string value = GetRandomValue();

            _cells[first] = value;
            _cells[second] = value;
        }
    }
    private List<Cell> GenerateCells(int width, int height)
    {
        List<Cell> cells = [];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells.Add(new(x, y));
            }
        }

        return cells;
    }
    private Cell GetRandomAndRemoveCell(List<Cell> cells)
    {
        int index = _random.Next(cells.Count);

        Cell cell = cells[index];
        cells.RemoveAt(index);

        return cell;
    }
    private string GetRandomValue()
    {
        int index = _random.Next(_values.Count);

        return _values[index];
    }

    // Click to cell
    public void ClickToCell(Cell cell)
    {
        if (_currentCell is Cell current)
        {
            if (current == cell)
            {
                CloseCurrent(current);
            }
            else if (_cells[current] == _cells[cell])
            {
                ClearEqualCells(cell, current);
            }
            else
            {
                CloseNotEmptyCells(cell, current);
            }
        }
        else if (_currentCell == null)
        {
            OpenNew(cell);
        }
    }
    private void CloseNotEmptyCells(Cell cell, Cell current)
    {
        _currentCell = null;

        OnOpenCell.Invoke(cell, _cells[cell]);

        OnDoubleCloseCell.Invoke(cell, current);
    }
    private void ClearEqualCells(Cell cell, Cell current)
    {
        _currentCell = null;

        OnOpenCell.Invoke(cell, _cells[cell]);

        _openedCells.Add(cell);
        _openedCells.Add(current);

        OnClearCell.Invoke(cell, current);

        if (_openedCells.Count == _cells.Count)
        {
            OnAreaEmpty.Invoke();
        }
    }
    private void CloseCurrent(Cell cell)
    {
        _currentCell = null;
        OnCloseCell.Invoke(cell);
    }
    private void OpenNew(Cell cell)
    {
        _currentCell = cell;
        OnOpenCell.Invoke(cell, _cells[cell]);
    }

    // Get current state
    public Cell? GetOpened()
    {
        return _currentCell;
    }
    public IReadOnlyList<Cell> GetClosedCells()
    {
        return _cells.Where(c => !_openedCells.Contains(c.Key))
                     .Select(c => c.Key)
                     .ToList();
    }

    // Read values from file
    private List<string> ReadValuesFromFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new ArgumentException("File not exist");
        }

        List<string> values = [];
        using (StreamReader reader = new(fileName))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                values.Add(line);
            }
        }

        return values;
    }
}
