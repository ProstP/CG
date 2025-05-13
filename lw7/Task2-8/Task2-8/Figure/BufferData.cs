using OpenTK.Graphics.OpenGL;

namespace Task2_8.Figure;

public struct BufferData
{
    public int Vao;
    public PrimitiveType PrimitiveType;
    public int VertexCount;

    public BufferData(int vao, PrimitiveType primitiveType, int vertexCount)
    {
        Vao = vao;
        PrimitiveType = primitiveType;
        VertexCount = vertexCount;
    }
}
