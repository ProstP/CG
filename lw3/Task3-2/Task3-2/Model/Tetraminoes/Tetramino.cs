using Task3_2.Model.Tetraminoes.Cells;

namespace Task3_2.Model.Tetraminoes;
public class Tetramino
{
    public List<Cell> Cells { get; private set; }

    public TetraminoType Type { get; private set; }

    public Cell this[int index]
    {
        get => Cells[index];
    }

    public Tetramino()
    {
        Cells = new(4);
    }

    public void Init(
            Cell cell1,
            Cell cell2,
            Cell cell3,
            Cell cell4,
            TetraminoType type
        )
    {
        Cells = [cell1, cell2, cell3, cell4];
        Type = type;
    }
    public void SetNewCells(
            Cell cell1,
            Cell cell2,
            Cell cell3,
            Cell cell4
        )
    {
        Cells = [cell1, cell2, cell3, cell4];
    }
}
