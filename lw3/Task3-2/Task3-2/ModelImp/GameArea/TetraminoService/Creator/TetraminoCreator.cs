using Task3_2.Model.Tetraminoes;
using Task3_2.Model.Tetraminoes.Cells;

namespace Task3_2.ModelImp.GameArea.TetraminoService.Creator;
public static class TetraminoCreator
{
    public static List<Cell> Create(TetraminoType type, int x, int y)
    {
        return type switch
        {
            TetraminoType.I => [new(x, y), new(x, y + 1), new(x, y + 2), new(x, y + 3)],
            TetraminoType.J => [new(x, y), new(x, y + 1), new(x, y + 2), new(x - 1, y + 2)],
            TetraminoType.L => [new(x, y), new(x, y + 1), new(x, y + 2), new(x + 1, y + 2)],
            TetraminoType.O => [new(x, y), new(x + 1, y), new(x, y + 1), new(x + 1, y + 1)],
            TetraminoType.S => [new(x - 1, y + 1), new(x, y + 1), new(x, y), new(x + 1, y)],
            TetraminoType.T => [new(x - 1, y), new(x, y), new(x + 1, y), new(x, y + 1)],
            TetraminoType.Z => [new(x - 1, y), new(x, y), new(x, y + 1), new(x + 1, y + 1)],
            _ => throw new ArgumentException("Unknown type"),
        };
    }
}
