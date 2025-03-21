namespace Task1.Figure;

public class Face
{
    List<Point3F> _vertexes;
    Color _color { get; set; }

    public Color Color => _color;
    public IReadOnlyList<Point3F> Vertexes => _vertexes;

    public Face(Color color, List<Point3F> vertexes)
    {
        _color = color;
        _vertexes = vertexes;
    }
}
