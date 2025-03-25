using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task3_2.Colors;
using Task3_2.Primitives;

namespace Task3_2.Map;

public class GameMap
{
    List<List<int>> _map = new();

    public const float CUBE_SIZE = 1.0f;

    ColorRGB _floorColor = new(0.6f, 0.4f, 0.7f);
    ColorRGB _ceilingColor = new(0.4f, 0.7f, 0.7f);
    ColorRGB _wallColor = new(0.4f, 0.5f, 0.4f);

    public IReadOnlyList<IReadOnlyList<int>> Map => _map;

    public GameMap(List<List<int>> map)
    {
        _map = map;
    }

    public void PaintMap()
    {
        PaintFloor();
        PaintCeiling();
        PaintBlocks();
    }

    public bool IsPositionFree(float x, float z)
    {
        float start = -CUBE_SIZE * 8;

        int cellX = (int)((x - start) / CUBE_SIZE);
        int cellZ = (int)((z - start) / CUBE_SIZE);

        return _map[cellZ][cellX] != 1;
    }

    public void FindCameraPosition(out int x, out int z)
    {
        List<int> row = _map.FirstOrDefault(row => row.Contains(2));

        if (row == null)
        {
            throw new ArgumentException("Position of camera not found");
        }
        z = _map.IndexOf(row);
        x = row.IndexOf(2);
    }

    // Floor drawing
    private void PaintFloor()
    {
        DrawFace(new FlatFace(
            [
                Point3F.Create(-CUBE_SIZE * 8, 0, -CUBE_SIZE * 8),
                Point3F.Create(-CUBE_SIZE * 8, 0, CUBE_SIZE * 8),
                Point3F.Create(CUBE_SIZE * 8, 0, CUBE_SIZE * 8),
                Point3F.Create(CUBE_SIZE * 8, 0, -CUBE_SIZE * 8),
            ], _floorColor));
    }

    // Ceiling drawing
    private void PaintCeiling()
    {
        DrawFace(new FlatFace(
            [
                Point3F.Create(-CUBE_SIZE * 8, CUBE_SIZE, -CUBE_SIZE * 8),
                Point3F.Create(CUBE_SIZE * 8, CUBE_SIZE, -CUBE_SIZE * 8),
                Point3F.Create(CUBE_SIZE * 8, CUBE_SIZE, CUBE_SIZE * 8),
                Point3F.Create(-CUBE_SIZE * 8, CUBE_SIZE, CUBE_SIZE * 8),
            ], _ceilingColor));
    }

    // Block drawing
    private void PaintBlocks()
    {
        float startX = -CUBE_SIZE * 8;
        float startZ = -CUBE_SIZE * 8;

        for (int z = 0; z < _map.Count; z++)
        {
            for (int x = 0; x < _map[z].Count; x++)
            {
                if (_map[z][x] == 0 || _map[z][x] == 2)
                {
                    continue;
                }

                float offsetX = startX + x * CUBE_SIZE + CUBE_SIZE / 2;
                float offsetZ = startZ + z * CUBE_SIZE + CUBE_SIZE / 2;
                DrawCube(new Vector3(offsetX, CUBE_SIZE / 2, offsetZ), CUBE_SIZE / 2);
            }
        }
    }

    // Cube drawing
    private void DrawCube(Vector3 center, float side)
    {
        DrawFace(new FlatFace(
            [
                Point3F.Create(center.X + side, center.Y + side, center.Z + side),
                Point3F.Create(center.X + side, center.Y - side, center.Z + side),
                Point3F.Create(center.X + side, center.Y - side, center.Z - side),
                Point3F.Create(center.X + side, center.Y + side, center.Z - side),
            ],
            _wallColor));
        DrawFace(new FlatFace(
            [
                Point3F.Create(center.X - side, center.Y + side, center.Z + side),
                Point3F.Create(center.X - side, center.Y + side, center.Z - side),
                Point3F.Create(center.X - side, center.Y - side, center.Z - side),
                Point3F.Create(center.X - side, center.Y - side, center.Z + side),
            ],
            _wallColor));
        DrawFace(new FlatFace(
            [
                Point3F.Create(center.X + side, center.Y + side, center.Z + side),
                Point3F.Create(center.X - side, center.Y + side, center.Z + side),
                Point3F.Create(center.X - side, center.Y - side, center.Z + side),
                Point3F.Create(center.X + side, center.Y - side, center.Z + side),
            ],
            _wallColor));
        DrawFace(new FlatFace(
            [
                Point3F.Create(center.X + side, center.Y + side, center.Z - side),
                Point3F.Create(center.X + side, center.Y - side, center.Z - side),
                Point3F.Create(center.X - side, center.Y - side, center.Z - side),
                Point3F.Create(center.X - side, center.Y + side, center.Z - side),
            ],
            _wallColor));
        DrawFace(new FlatFace(
            [
                Point3F.Create(center.X + side, center.Y + side, center.Z - side),
                Point3F.Create(center.X - side, center.Y + side, center.Z - side),
                Point3F.Create(center.X - side, center.Y + side, center.Z + side),
                Point3F.Create(center.X + side, center.Y + side, center.Z + side),
            ],
            _wallColor));
        DrawFace(new FlatFace(
            [
                Point3F.Create(center.X + side, center.Y - side, center.Z - side),
                Point3F.Create(center.X + side, center.Y - side, center.Z + side),
                Point3F.Create(center.X - side, center.Y - side, center.Z + side),
                Point3F.Create(center.X - side, center.Y - side, center.Z - side),
            ],
            _wallColor));
    }

    // Drawing points
    private void DrawFace(FlatFace flatFace)
    {
        List<float> pointsXYZRGB = [];
        List<float> border = [];
        Vector3 normal = flatFace.Normal;

        for (int i = 0; i < flatFace.Vertexes.Count; i++)
        {
            Point3F vertex = flatFace.Vertexes[i];

            pointsXYZRGB.AddRange([vertex.X, vertex.Y, vertex.Z,
                normal.X, normal.Y, normal.Z,
                flatFace.Color.R, flatFace.Color.G, flatFace.Color.B]);

            border.AddRange([vertex.X, vertex.Y, vertex.Z,
                normal.X, normal.Y, normal.Z,
                0.2f, 0.2f, 0.2f]);
        }

        GL.BufferData(BufferTarget.ArrayBuffer, pointsXYZRGB.Count * sizeof(float), pointsXYZRGB.ToArray(), BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, pointsXYZRGB.Count / 9);

        DrawBorder(border.ToArray());
    }
    private void DrawBorder(float[] points)
    {
        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.LineLoop, 0, points.Length / 9);
    }
}
