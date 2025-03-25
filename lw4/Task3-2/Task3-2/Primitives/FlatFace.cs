using OpenTK.Mathematics;
using Task3_2.Colors;

namespace Task3_2.Primitives;

public class FlatFace
{
    List<Point3F> _vertexes;
    ColorRGB _color { get; set; }

    public ColorRGB Color => _color;
    public IReadOnlyList<Point3F> Vertexes => _vertexes;
    public Vector3 Normal => GetNormal();

    public FlatFace(List<Point3F> vertexes, ColorRGB color)
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

/*


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
*/
