namespace Task3_1.Shapes;
public struct ParaboloidBufferData
{
    public int VAO;
    public int VertexCount;

    public int CapVAO;
    public int CapVertexCount;

    public ParaboloidBufferData(int vao, int vertexCount, int capVAO, int capVertexCount)
    {
        VAO = vao;
        VertexCount = vertexCount;
        CapVAO = capVAO;
        CapVertexCount = capVertexCount;
    }
}
