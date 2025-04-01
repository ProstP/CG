using OpenTK.Mathematics;

namespace Task1_6.Primitives;

public class FlatFace
{
    List<Point3F> _vertexes;
    Color _color { get; set; }

    public Color Color => _color;
    public IReadOnlyList<Point3F> Vertexes => _vertexes;
    public Point3F this[int index] => _vertexes[index];
    public Vector3 Normal => GetNormal();

    public FlatFace(List<Point3F> vertexes, Color color)
    {
        _color = color;
        _vertexes = vertexes;
    }
    public Vector3 GetNormal()
    {
        Vector3 first = CreateVectorByVertexes(0);
        Vector3 second = CreateVectorByVertexes(1);

        Vector3 normal = Vector3.Cross(first, second);
        normal.Normalize();

        return normal;
    }
    private Vector3 CreateVectorByVertexes(int start)
    {
        if (start >= _vertexes.Count - 1)
        {
            throw new ArgumentOutOfRangeException("Point position for vector more than point count");
        }

        Point3F firstP = _vertexes[start];
        Point3F secondP = _vertexes[start + 1];

        return new Vector3(secondP.X - firstP.X, secondP.Y - firstP.Y, secondP.Z - firstP.Z);
    }
}
