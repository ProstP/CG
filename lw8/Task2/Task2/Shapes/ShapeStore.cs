using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Task2.Shapes;

public class ShapeStore
{
    List<BufferData> _datas = [];
    List<Vector3> _centres =
    [
        new(-0.5f, 0, 0),
        new(0.5f, -0.5f, 0),
    ];
    List<float> _sizes =
    [
        0.2f,
        0.3f,
    ];
    List<Vector3> _colors =
    [
        new(0, 0, 1),
        new(0, 1, 0),
    ];

    public void CreatePoints()
    {
        _datas.AddRange(CreateCubes());
        _datas.AddRange(CreateFloor());
    }

    public Vector3[] GetCentres()
    {
        return _centres.ToArray();
    }
    public float[] GetSizes()
    {
        return _sizes.ToArray();
    }
    public int GetCubeCount()
    {
        return _centres.Count;
    }
    public void Paint()
    {
        foreach (BufferData bufferData in _datas)
        {
            GL.BindVertexArray(bufferData.VAO);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, bufferData.VertexCount);
        }
    }

    // Figures
    private List<BufferData> CreateCubes()
    {
        List<BufferData> datas = [];

        for (int i = 0; i < _centres.Count; i++)
        {
            datas.AddRange(CreateCube(_centres[i], _sizes[i], _colors[i]));
        }

        return datas;
    }
    private static List<BufferData> CreateCube(Vector3 center, float size, Vector3 color)
    {
        List<BufferData> datas = [];

        float halfSize = size / 2f;
        datas.Add(CreateCubeFaceByX(center.X - halfSize, center, halfSize, color));
        datas.Add(CreateCubeFaceByX(center.X + halfSize, center, halfSize, color));
        datas.Add(CreateCubeFaceByY(center.Y - halfSize, center, halfSize, color));
        datas.Add(CreateCubeFaceByY(center.Y + halfSize, center, halfSize, color));
        datas.Add(CreateCubeFaceByZ(center.Z - halfSize, center, halfSize, color));
        datas.Add(CreateCubeFaceByZ(center.Z + halfSize, center, halfSize, color));

        return datas;
    }
    private static BufferData CreateCubeFaceByX(float x, Vector3 center, float halfSize, Vector3 color)
    {
        Vector3 normal = new(x - center.X, 0, 0);
        normal.Normalize();

        float[] points =
        [
            x, center.Y - halfSize, center.Z - halfSize,
            x, center.Y - halfSize, center.Z + halfSize,
            x, center.Y + halfSize, center.Z + halfSize,
            x, center.Y + halfSize, center.Z - halfSize,
        ];

        return CreateBufferData(AddNormalAndColorToPoints(points, normal, color));
    }
    private static BufferData CreateCubeFaceByY(float y, Vector3 center, float halfSize, Vector3 color)
    {
        Vector3 normal = new(0, y - center.Y, 0);
        normal.Normalize();

        float[] points =
        [
            center.X - halfSize, y, center.Z - halfSize,
            center.X - halfSize, y, center.Z + halfSize,
            center.X + halfSize, y, center.Z + halfSize,
            center.X + halfSize, y, center.Z - halfSize,
        ];

        return CreateBufferData(AddNormalAndColorToPoints(points, normal, color));
    }
    private static BufferData CreateCubeFaceByZ(float z, Vector3 center, float halfSize, Vector3 color)
    {
        Vector3 normal = new(0, 0, z - center.Z);
        normal.Normalize();

        float[] points =
        [
            center.X - halfSize, center.Y - halfSize, z,
            center.X - halfSize, center.Y + halfSize, z,
            center.X + halfSize, center.Y + halfSize, z,
            center.X + halfSize, center.Y - halfSize, z,
        ];

        return CreateBufferData(AddNormalAndColorToPoints(points, normal, color));
    }
    private static BufferData[] CreateFloor()
    {
        Vector3 color = new(1, 0, 0);

        Vector3 normalUp = new(0, 1, 0);
        Vector3 normalDown = new(0, -1, 0);

        float[] pointsUp =
        [
            -1f, -1f, -1f,
            1f, -1f, -1f,
            1f, -1f, 1f,
            -1f, -1f, 1f,
        ];
        float[] pointsDown =
        [
            -1f, -1.001f, -1f,
            1f, -1.001f, -1f,
            1f, -1.001f, 1f,
            -1f, -1.001f, 1f,
        ];

        return
        [
            CreateBufferData(AddNormalAndColorToPoints(pointsUp, normalUp, color)),
            CreateBufferData(AddNormalAndColorToPoints(pointsDown, normalDown, color)),
        ];
    }
    private static float[] AddNormalAndColorToPoints(float[] points, Vector3 normal, Vector3 color)
    {
        List<float> data = [];

        for (int i = 0; i <= points.Length - 3; i += 3)
        {
            float x = points[i];
            float y = points[i + 1];
            float z = points[i + 2];

            data.AddRange([x, y, z, normal.X, normal.Y, normal.Z, color.X, color.Y, color.Z]);
        }

        return data.ToArray();
    }
    private static BufferData CreateBufferData(float[] points)
    {
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();

        GL.BindVertexArray(0);

        return new BufferData(vao, points.Length / 9);
    }

    // Shader layout
    private static void ConfigurateShaderLayout()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);
    }
}
