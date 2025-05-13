using OpenTK.Graphics.OpenGL;

namespace Task2_8.Figure;

public abstract class Figure
{
    List<BufferData> _buffers = [];

    public abstract void CreatePoints();

    public void Paint()
    {
        foreach (BufferData buffer in _buffers)
        {
            GL.BindVertexArray(buffer.Vao);
            GL.DrawArrays(buffer.PrimitiveType, 0, buffer.VertexCount);
        }
    }

    protected void CreateAndSetUpBuffer(float[] points, PrimitiveType primitiveType)
    {
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        ConfigurateShaders();

        GL.BindVertexArray(0);

        _buffers.Add(new BufferData(vao, primitiveType, points.Length / 3));
    }

    // Congigurate shaders
    private void ConfigurateShaders()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }
}
