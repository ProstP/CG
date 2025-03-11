using Task3_2.Model.Tetraminoes.Cells;
using Task3_2.Model.Tetraminoes;
using static Task3_2.Model.IModel;
using Task3_2.ModelImp.GameArea.TetraminoService.Rotater;
using Task3_2.ModelImp.GameArea.TetraminoService.Creator;

namespace Task3_2.ModelImp.GameArea;

public class GameArea
{
    public event TetraminoChangedEventHandler OnTetraminoMove;
    public event TetraminoChangedEventHandler OnTetraminoRotate;
    public event TetraminoChangedEventHandler OnTetraminoBlocked;
    public event TetraminoChangedEventHandler OnCreateNewTetramino;
    public event ClearRowEventHanlder OnClearRow;
    List<Cell> _usedCells = [];
    Tetramino _tetramino;

    public int WIDTH { get => 10; }
    public int HEIGHT { get => 20; }

    // Start
    public void Start(TetraminoType type)
    {
        _usedCells.Clear();
        CreateNewTetramino(type, out bool _);
    }

    // Tetramino moving
    public void MoveTetraminoByX(bool toRight)
    {
        if (!CanMoveTetraminoByX(toRight))
        {
            return;
        }

        _tetramino.Cells.ForEach(cell => cell.X += (toRight ? 1 : -1));

        OnTetraminoMove.Invoke(_tetramino);
    }
    private bool CanMoveTetraminoByX(bool toRight)
    {
        return _tetramino != null && !_tetramino.Cells.Any(cell => !CanCellMoveByX(cell, toRight));
    }
    private bool CanCellMoveByX(Cell cell, bool toRight)
    {
        Cell nextCell = new(cell.X + (toRight ? 1 : -1), cell.Y);

        return 0 <= nextCell.X && nextCell.X < WIDTH && !_usedCells.Contains(nextCell);
    }

    public void MoveTetraminoByY(TetraminoType nextType, out bool isLost)
    {
        if (!CanMoveTetraminoByY())
        {
            BlockTetraminoMoving();
            TryClearRows();
            CreateNewTetramino(nextType, out bool CanCreate);
            isLost = !CanCreate;
            return;
        }

        isLost = false;

        _tetramino.Cells.ForEach(cell => cell.Y++);

        OnTetraminoMove.Invoke(_tetramino);
    }

    private bool CanMoveTetraminoByY()
    {
        return _tetramino != null && !_tetramino.Cells.Any(cell => !CanCellMoveByY(cell));
    }
    private bool CanCellMoveByY(Cell cell)
    {
        Cell nextCell = new(cell.X, cell.Y + 1);

        return nextCell.Y < HEIGHT && !_usedCells.Contains(nextCell);
    }

    // Rotate tetramino
    public void RotateTetramino()
    {
        _tetramino.RotateTetramino(_usedCells, WIDTH, HEIGHT, out bool isSuccess);
        if (isSuccess)
        {
            OnTetraminoRotate.Invoke(_tetramino);
        }
    }

    // Clear rows
    private void TryClearRows()
    {
        List<int> rowsToClear = FindRowsToClear();
        if (rowsToClear == null || rowsToClear.Count == 0)
        {
            return;
        }

        ClearRows(rowsToClear);
        OnClearRow(rowsToClear);
    }
    private List<int> FindRowsToClear()
    {
        List<int> rowsToClear = new();
        Dictionary<int, int> rowsToCellCounts = new();

        foreach (Cell cell in _usedCells)
        {
            if (!rowsToCellCounts.ContainsKey(cell.Y))
            {
                rowsToCellCounts[cell.Y] = 1;
            }
            else
            {
                rowsToCellCounts[cell.Y]++;

                if (rowsToCellCounts[cell.Y] >= WIDTH)
                {
                    rowsToClear.Add(cell.Y);
                }
            }
        }

        return rowsToClear;
    }
    private void ClearRows(List<int> rows)
    {
        _usedCells.RemoveAll(cell => rows.Contains(cell.Y));

        foreach (Cell cell in _usedCells)
        {
            int addValue = 0;

            foreach (int removedRows in rows)
            {
                if (cell.Y < removedRows)
                {
                    addValue++;
                }
            }

            cell.Y += addValue;
        }
    }

    // Block moving of Tetramino
    private void BlockTetraminoMoving()
    {
        if (_tetramino == null)
        {
            return;
        }
        foreach (Cell cell in _tetramino.Cells)
        {
            if (_usedCells.Contains(cell))
            {
                throw new ArgumentException("Cell in tetramino already used");
            }

            _usedCells.Add(cell);
        }
        OnTetraminoBlocked.Invoke(_tetramino);
    }

    // Create Tetramino
    private void CreateNewTetramino(TetraminoType type, out bool canCreate)
    {
        List<Cell> cells = TetraminoCreator.Create(type, WIDTH / 2, 0);

        if (cells == null)
        {
            throw new ArgumentNullException("Cells list for tetramino is empty");
        }
        if (cells.Count != 4)
        {
            throw new ArgumentException("Count of cells in tetramino must be 4");
        }

        if (cells.Any(cell => _usedCells.Contains(cell)))
        {
            canCreate = false;
            return;
        }

        canCreate = true;

        if (_tetramino == null)
        {
            _tetramino = new();
        }

        _tetramino.Init(cells[0], cells[1], cells[2], cells[3], type);

        OnCreateNewTetramino.Invoke(_tetramino);
    }
}
