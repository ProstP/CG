using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Task3_2.Model.Tetraminoes;
using Task3_2.Model.Tetraminoes.Cells;
using Task3_2.ViewImp.MyColor;

namespace Task3_2.ViewImp;

public class GameAreaView
{
    float _cellWidth;
    float _cellHeight;
    Dictionary<Cell, ColorRGB> _cells = new();
    List<Cell> _tetraminoCells = new();
    ColorRGB _tetraminoColor = ColorGenerator.GenerateRand();

    public GameAreaView()
    {
        _cellHeight = 2f / 20;
        _cellWidth = _cellHeight;
    }

    public void Clear()
    {
        _cells.Clear();
        _tetraminoCells.Clear();
        _tetraminoColor = ColorGenerator.GenerateRand();
    }

    // Update tetramino
    public void UpdateTetramino(Tetramino tetramino)
    {
        _tetraminoCells = tetramino.Cells.Select(cell => new Cell(cell)).ToList();
    }
    public void BlockTetramino()
    {
        _tetraminoCells.ForEach(cell => _cells[cell] = _tetraminoColor);
        _tetraminoCells.Clear();
        _tetraminoColor = ColorGenerator.GenerateRand();
    }

    // Clear rows
    public void ClearRows(List<int> rows)
    {
        RemoveCells(rows);
        ShiftCells(rows);
    }
    private void RemoveCells(List<int> rows)
    {
        List<Cell> cellsToRemove = _cells.Keys.Where(cell => rows.Contains(cell.Y)).ToList();

        foreach (Cell cell in cellsToRemove)
        {
            _cells.Remove(cell);
        }
    }
    private void ShiftCells(List<int> deletedRows)
    {
        Dictionary<Cell, ColorRGB> newCells = new();

        foreach (KeyValuePair<Cell, ColorRGB> item in _cells)
        {
            int addValue = 0;

            foreach (int row in deletedRows)
            {
                if (item.Key.Y < row)
                {
                    addValue++;
                }
            }

            Cell newCell = new(item.Key.X, item.Key.Y + addValue);

            newCells[newCell] = item.Value;
        }

        _cells = newCells;
    }

    // View game area
    public void Display()
    {
        DrawBackround();

        if (_tetraminoCells != null)
        {
            List<Cell> tetramino = new(_tetraminoCells);
            tetramino.ForEach(cell => DrawSquare(cell.X, cell.Y, _tetraminoColor));
        }

        Dictionary<Cell, ColorRGB> cells = new(_cells);
        foreach (KeyValuePair<Cell, ColorRGB> cell in cells)
        {
            DrawSquare(cell.Key.X, cell.Key.Y, cell.Value);
        }
    }

    private void DrawBackround()
    {
        float color = 1f;
        DrawQuads(-1.0f, -1.0f, 1.0f, 2.0f, new(color, color, color));
    }

    // Draw square
    private void DrawSquare(int left, int top, ColorRGB color)
    {
        (float left, float top) leftAndTop = TransformPointsToGeneralCoordinates(left * _cellWidth, -top * _cellHeight);
        DrawQuads(leftAndTop.left, leftAndTop.top, _cellWidth, -_cellHeight, color);
    }

    // Draw quads using begin-end
    private void DrawQuads(float left, float top, float width, float height, ColorRGB color)
    {
#pragma warning disable 0618
        GL.Begin(BeginMode.Quads);

        GL.Color3(color.R, color.G, color.B);
        GL.Vertex2(left, top);
        GL.Vertex2(left + width, top);
        GL.Vertex2(left + width, top + height);
        GL.Vertex2(left, top + height);

        GL.End();
#pragma warning restore 0618
    }
    private (float, float) TransformPointsToGeneralCoordinates(float left, float top)
    {
        left = -1f + left;
        top = 1f + top;

        return (left, top);
    }
}
