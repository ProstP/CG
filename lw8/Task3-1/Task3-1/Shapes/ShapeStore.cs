using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Task3_1.Shapes;

public class ShapeStore
{
    List<ParaboloidBufferData> _data = [];
    List<ParaboloidData> _paraboloids =
    [
        new(1, 1, new(0, -1, 0)),
        new(0.7f, 0.5f, new(0, 0, 0)),
        new(0.5f, 0.4f, new(0, 0.5f, 0)),
    ];
    List<Vector3> _colors =
    [
        new(1, 0, 0),
        new(0, 0, 1),
        new(0, 1, 0),
    ];

    public IReadOnlyList<ParaboloidData> Paraboloids => _paraboloids;

    // Paint
    public void Paint()
    {
        foreach (ParaboloidBufferData data in _data)
        {
            GL.BindVertexArray(data.VAO);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, data.VertexCount);

            GL.BindVertexArray(data.CapVAO);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, data.CapVertexCount);

            GL.BindVertexArray(0);
        }
    }

    // Create points
    public void CreatePoints()
    {
        int slices = 50;
        int stacks = 50;

        for (int i = 0; i < _paraboloids.Count; i++)
        {
            _data.Add(CreateBufferData(
                CreateParaboloidPoints(_paraboloids[i], _colors[i], slices, stacks),
                CreateParaboloidCap(_paraboloids[i], _colors[i], slices, stacks))
            );
        }
    }
    private static float[] CreateParaboloidPoints(ParaboloidData data, Vector3 color, int slices, int stacks)
    {
        List<float> points = [];

        for (int i = 0; i < slices; i++)
        {
            float u0 = (float)i / slices * MathF.PI * 2;
            float u1 = (float)(i + 1) / slices * MathF.PI * 2;

            for (int j = 0; j <= stacks; j++)
            {
                float v = (float)j / stacks;

                AddVertex(points, data.Size, data.Position, data.MaxHeight, u0, v, color);

                AddVertex(points, data.Size, data.Position, data.MaxHeight, u1, v, color);
            }
        }

        return points.ToArray();
    }
    private static void AddVertex(
        List<float> data,
        float size,
        Vector3 position,
        float height,
        float u,
        float v,
        Vector3 color)
    {
        float x = size * v * MathF.Cos(u);
        float y = size * v * MathF.Sin(u);
        float z = height * v * v;

        Vector3 pos = new Vector3(x, z, y) + position;

        Vector3 normal = new Vector3(2 * x / (size * size), 2 * y / (size * size), -1);
        normal = Vector3.Normalize(normal);

        data.AddRange([
            pos.X, pos.Y, pos.Z,
            normal.X, normal.Z, normal.Y,
            color.X, color.Y, color.Z,
        ]);
    }
    private static float[] CreateParaboloidCap(ParaboloidData data, Vector3 color, int slices, int stacks)
    {
        List<float> points =
        [
            data.Position.X, data.Position.Y + data.MaxHeight, data.Position.Z
        ];

        for (int i = 0; i <= slices; i++)
        {
            float u = (float)i / slices * MathF.PI * 2;
            float x = data.Size * MathF.Cos(u);
            float y = data.Size * MathF.Sin(u);

            points.AddRange([
                x + data.Position.X,
                data.MaxHeight + data.Position.Y,
                y + data.Position.Z,
            ]);
        }

        return AddNormalAndColorToPoints(points.ToArray(), new(0, 1, 0), color);
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

    // Buffer data
    private static ParaboloidBufferData CreateBufferData(float[] paraboloidPoints, float[] capPoints)
    {
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, paraboloidPoints.Length * sizeof(float), paraboloidPoints, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();

        int capVao = GL.GenVertexArray();
        GL.BindVertexArray(capVao);

        int capVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, capVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, capPoints.Length * sizeof(float), capPoints, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();

        GL.BindVertexArray(0);

        return new ParaboloidBufferData(vao, paraboloidPoints.Length / 9, capVao, capPoints.Length / 9);
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
