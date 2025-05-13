using OpenTK.Mathematics;

namespace Task3_3.Shapes;

public struct TorusData
{
    public Vector3 Position;
    public float MajorRadius;  // R
    public float MinorRadius;  // r
    public Matrix4 ModelMatrix;

    public TorusData(Vector3 position, float majorRadius, float minorRadius)
    {
        Position = position;
        MajorRadius = majorRadius;
        MinorRadius = minorRadius;
        ModelMatrix = Matrix4.CreateTranslation(position);
    }
}

