namespace Task2_1.Model;

public struct Cell
{
    public int X { get; private init; }
    public int Y { get; private init; }

    public Cell(
        int x,
        int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Cell other)
        {
            return other.X == X && other.Y == Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return (X, Y).GetHashCode();
    }

    public static bool operator ==(Cell first, Cell second)
    {
        return first.X == second.X && first.Y == second.Y;
    }
    public static bool operator !=(Cell first, Cell second)
    {
        return first.X != second.X || first.Y != second.Y;
    }
}
