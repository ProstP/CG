using OpenTK.Mathematics;

namespace Task3_1.Shapes;
public struct ParaboloidData
{
    public float Size;
    public float MaxHeight;

    public Vector3 Position;

    public ParaboloidData(
        float size,
        float maxHeight,
        Vector3 position)
    {
        MaxHeight = maxHeight;
        Position = position;
        Size = size;
    }
}
