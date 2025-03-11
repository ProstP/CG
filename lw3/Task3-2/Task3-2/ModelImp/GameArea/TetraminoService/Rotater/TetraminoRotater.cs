using Task3_2.Model.Tetraminoes;
using Task3_2.Model.Tetraminoes.Cells;

namespace Task3_2.ModelImp.GameArea.TetraminoService.Rotater;
public static class TetraminoRotater
{
    public static void RotateTetramino(this Tetramino tetramino,
        IReadOnlyList<Cell> usedCells, int maxX, int maxY, out bool isSucces)
    {
        if (tetramino.Cells.Count != 4)
        {
            throw new ArgumentException("Tetraimo must has 4 cells");
        }

        List<Cell> newCells = CalculateNewCoordinates(tetramino, maxX);

        if (newCells.Count != 4 || !CheckNewCellsForUsed(newCells, usedCells) || newCells.Any(cell => cell.Y >= maxY))
        {
            isSucces = false;
            return;
        }

        tetramino.SetNewCells(newCells[0], newCells[1], newCells[2], newCells[3]);
        isSucces = true;
    }

    // Check new cells for used and change x if need
    private static bool CheckNewCellsForUsed(List<Cell> newCells, IReadOnlyList<Cell> usedCells)
    {
        if (!CheckCellsAndChangeX(newCells, usedCells, 0))
        {
            return true;
        }
        if (!CheckCellsAndChangeX(newCells, usedCells, 1))
        {
            return true;
        }
        if (!CheckCellsAndChangeX(newCells, usedCells, -2))
        {
            return true;
        }

        return false;
    }
    private static bool CheckCellsAndChangeX(List<Cell> cells, IReadOnlyList<Cell> used, int deltaX)
    {
        bool isUsed = false;

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].X += deltaX;
            if (used.Contains(cells[i]))
            {
                isUsed = true;
            }
        }

        return isUsed;
    }

    // Calculate new coordinates by type
    private static List<Cell> CalculateNewCoordinates(Tetramino tetramino, int maxX)
    {
        switch (tetramino.Type)
        {
            case TetraminoType.I:
                return RotateI(tetramino, maxX);
            case TetraminoType.J:
                return RotateJ(tetramino, maxX);
            case TetraminoType.L:
                return RotateL(tetramino, maxX);
            case TetraminoType.O:
                return RotateO(tetramino, maxX);
            case TetraminoType.S:
                return RotateS(tetramino, maxX);
            case TetraminoType.T:
                return RotateT(tetramino, maxX);
            case TetraminoType.Z:
                return RotateZ(tetramino, maxX);
            default:
                throw new ArgumentException("Unknown type of tetramino");
        }
    }
    private static List<Cell> RotateI(Tetramino tetramino, int maxX)
    {
        bool isHorizontal = tetramino[0].Y == tetramino[1].Y;

        int x = tetramino[1].X;
        int y = tetramino[1].Y;

        if (isHorizontal)
        {
            return
            [
                new(x, y - 1),
                new(x, y),
                new(x, y + 1),
                new(x, y + 2),
            ];
        }
        else
        {
            int offsetXFromBorder = 0;
            if (x == 0)
            {
                offsetXFromBorder = 1;
            }
            else if (x + 2 >= maxX)
            {
                offsetXFromBorder = maxX - x - 3;
            }
            return
            [
                new(x - 1 + offsetXFromBorder, y),
                new(x + offsetXFromBorder, y),
                new(x + 1 + offsetXFromBorder, y),
                new(x + 2 + offsetXFromBorder, y),
            ];
        }
    }
    private static List<Cell> RotateJ(Tetramino tetramino, int maxX)
    {
        bool isHorizontal = tetramino[0].Y == tetramino[1].Y;

        int x = tetramino[1].X;
        int y = tetramino[1].Y;

        if (isHorizontal)
        {
            Cell cell4;
            if (tetramino[3].Y > y)
            {
                cell4 = new(x - 1, y + 1);
            }
            else
            {
                cell4 = new(x + 1, y - 1);
            }
            return
            [
                new(x, y - 1),
                new(x, y),
                new(x, y + 1),
                cell4,
            ];
        }
        else
        {
            int offsetXFromBorder = x == 0 ? 1 : x == maxX - 1 ? -1 : 0;
            Cell cell4;
            if (tetramino[3].X > x)
            {
                cell4 = new(x + 1 + offsetXFromBorder, y + 1);
            }
            else
            {
                cell4 = new(x - 1 + offsetXFromBorder, y - 1);
            }
            return
            [
                new(x - 1 + offsetXFromBorder, y),
                new(x + offsetXFromBorder, y),
                new(x + 1 + offsetXFromBorder, y),
                cell4,
            ];
        }
    }
    private static List<Cell> RotateL(Tetramino tetramino, int maxX)
    {
        bool isHorizontal = tetramino[0].Y == tetramino[1].Y;

        int x = tetramino[1].X;
        int y = tetramino[1].Y;

        if (isHorizontal)
        {
            Cell cell4;
            if (tetramino[3].Y > y)
            {
                cell4 = new(x - 1, y - 1);
            }
            else
            {
                cell4 = new(x + 1, y + 1);
            }
            return
            [
                new(x, y - 1),
                new(x, y),
                new(x, y + 1),
                cell4,
            ];
        }
        else
        {
            int offsetXFromBorder = x == 0 ? 1 : x == maxX - 1 ? -1 : 0;
            Cell cell4;
            if (tetramino[3].X > x)
            {
                cell4 = new(x - 1 + offsetXFromBorder, y + 1);
            }
            else
            {
                cell4 = new(x + 1 + offsetXFromBorder, y - 1);
            }
            return
            [
                new(x - 1 + offsetXFromBorder, y),
                new(x + offsetXFromBorder, y),
                new(x + 1 + offsetXFromBorder, y),
                cell4,
            ];
        }
    }
    private static List<Cell> RotateO(Tetramino tetramino, int maxX)
    {
        return tetramino.Cells;
    }
    private static List<Cell> RotateS(Tetramino tetramino, int maxX)
    {
        bool isHorizontal = tetramino[0].Y == tetramino[1].Y;

        int x = tetramino[2].X;
        int y = tetramino[2].Y;

        if (isHorizontal)
        {
            return
            [
                new(x - 1, y - 1),
                new(x - 1, y),
                new(x, y),
                new(x, y + 1),
            ];
        }
        else
        {
            int offsetXFromBorder = x == 0 ? 1 : x == maxX - 1 ? -1 : 0;
            return
            [
                new(x - 1 + offsetXFromBorder, y + 1),
                new(x + offsetXFromBorder, y + 1),
                new(x + offsetXFromBorder, y),
                new(x + 1 + offsetXFromBorder, y),
            ];
        }
    }
    private static List<Cell> RotateT(Tetramino tetramino, int maxX)
    {
        bool isHorizontal = tetramino[0].Y == tetramino[1].Y;

        int x = tetramino[1].X;
        int y = tetramino[1].Y;

        if (isHorizontal)
        {
            Cell cell4;
            if (tetramino[3].Y > y)
            {
                cell4 = new(x - 1, y);
            }
            else
            {
                cell4 = new(x + 1, y);
            }
            return
            [
                new(x, y - 1),
                new(x, y),
                new(x, y + 1),
                cell4,
            ];
        }
        else
        {
            int offsetXFromBorder = x == 0 ? 1 : x == maxX - 1 ? -1 : 0;
            Cell cell4;
            if (tetramino[3].X > x)
            {
                cell4 = new(x + offsetXFromBorder, y + 1);
            }
            else
            {
                cell4 = new(x + offsetXFromBorder, y - 1);
            }
            return
            [
                new(x - 1 + offsetXFromBorder, y),
                new(x + offsetXFromBorder, y),
                new(x + 1 + offsetXFromBorder, y),
                cell4,
            ];
        }
    }
    private static List<Cell> RotateZ(Tetramino tetramino, int maxX)
    {
        bool isHorizontal = tetramino[0].Y == tetramino[1].Y;

        int x = tetramino[1].X;
        int y = tetramino[1].Y;

        if (isHorizontal)
        {
            return
            [
                new(x, y - 1),
                new(x, y),
                new(x - 1, y),
                new(x - 1, y + 1),
            ];
        }
        else
        {
            int offsetXFromBorder = x == 0 ? 1 : x == maxX - 1 ? -1 : 0;
            return
            [
                new(x - 1 + offsetXFromBorder, y),
                new(x + offsetXFromBorder, y),
                new(x + offsetXFromBorder, y + 1),
                new(x + 1 + offsetXFromBorder, y + 1),
            ];
        }
    }
}
