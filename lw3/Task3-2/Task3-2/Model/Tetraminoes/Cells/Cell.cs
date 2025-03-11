namespace Task3_2.Model.Tetraminoes.Cells;

public class Cell
{
    public int X;
    public int Y;

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Cell(Cell cell)
    {
        X = cell.X;
        Y = cell.Y;
    }

    public override int GetHashCode()
    {
        return (X, Y).GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Cell cell && X == cell.X && Y == cell.Y;
    }
}
