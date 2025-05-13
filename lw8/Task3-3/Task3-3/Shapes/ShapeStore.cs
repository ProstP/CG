using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task3_3.Shaders;

namespace Task3_3.Shapes;

public class ShapeStore
{
    List<TorusData> _toruses =
    [
        new TorusData(new Vector3(0, -0.8f, 0), 0.9f, 0.3f),
        new TorusData(new Vector3(0, -0.3f, 0), 0.7f, 0.25f),
        new TorusData(new Vector3(0, 0.1f, 0), 0.5f, 0.2f),
        new TorusData(new Vector3(0, 0.4f, 0), 0.3f, 0.15f),
    ];
    List<BufferData> _torusBuffers = [];

    Vector3[] _colors =
    [
        new(1, 0, 0),
        new(0, 1, 0),
        new(0, 0, 1),
        new(1, 1, 0),
    ];

    public IReadOnlyList<TorusData> Toruses => _toruses;

    // Paint
    public void Paint(Shader shader)
    {
        for (int i = 0; i < _toruses.Count; i++)
        {
            shader.SetMatrix4("model", _toruses[i].ModelMatrix);

            GL.BindVertexArray(_torusBuffers[i].VAO);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, _torusBuffers[i].VertexCount);
        }

        GL.BindVertexArray(0);
    }

    // Create points
    public void CreatePoints()
    {
        int slices = 50;
        int stacks = 30;

        for (int i = 0; i < _toruses.Count; i++)
        {
            float[] points = CreateTorusPoints(_toruses[i], _colors[i], slices, stacks);
            _torusBuffers.Add(CreateBufferData(points));
        }
    }
    private static float[] CreateTorusPoints(TorusData torus, Vector3 color, int slices, int stacks)
    {
        List<float> data = [];

        for (int i = 0; i < slices; i++)
        {
            float u0 = (float)i / slices * 2 * MathF.PI;
            float u1 = (float)(i + 1) / slices * 2 * MathF.PI;

            for (int j = 0; j <= stacks; j++)
            {
                float v = (float)j / stacks * 2 * MathF.PI;

                AddTorusVertex(data, torus, u0, v, color);
                AddTorusVertex(data, torus, u1, v, color);
            }
        }

        return data.ToArray();
    }

    private static void AddTorusVertex(List<float> data, TorusData torus, float u, float v, Vector3 color)
    {
        float R = torus.MajorRadius;
        float r = torus.MinorRadius;

        float cosU = MathF.Cos(u);
        float sinU = MathF.Sin(u);
        float cosV = MathF.Cos(v);
        float sinV = MathF.Sin(v);

        float x = (R + r * cosV) * cosU;
        float y = (R + r * cosV) * sinU;
        float z = r * sinV;

        Vector3 pos = new(x, y, z);

        Vector3 normal = new(cosV * cosU, cosV * sinU, sinV);
        normal = Vector3.Normalize(normal);

        data.AddRange(
        [
            pos.X, pos.Z, pos.Y,
            normal.X, normal.Z, normal.Y,
            color.X, color.Y, color.Z
        ]);
    }


    // Buffer data
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

