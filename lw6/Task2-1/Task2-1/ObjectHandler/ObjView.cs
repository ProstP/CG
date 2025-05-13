using ObjLoader.Loader.Data;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using OpenTK.Graphics.OpenGL;

namespace Task2_1.ObjectHandler;

public class ObjView
{
    LoadResult _obj;

    int _vao;
    int _vbo;
    int _vertexCount;

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float Scale { get; private init; }

    public ObjView(
        string fileName,
        float x,
        float y,
        float z,
        float scale)
    {
        X = x;
        Y = y;
        Z = z;
        Scale = scale;
        LoadObjectFromFile(fileName);
        SetUpBuffers();
    }

    private void LoadObjectFromFile(string fileName)
    {
        ObjLoaderFactory factory = new();
        IObjLoader objLoader = factory.Create();

        _obj = objLoader.Load(File.OpenRead(fileName));
    }
    private void SetUpBuffers()
    {
        List<float> points = [];
        foreach (Group group in _obj.Groups)
        {
            Material material = group.Material;
            Vec3 color = material != null ? material.DiffuseColor : new(1, 1, 1);

            foreach (Face face in group.Faces)
            {
                for (int i = 0; i < face.Count; i++)
                {
                    Vertex vertex = _obj.Vertices[face[i].VertexIndex - 1];
                    Normal normal = _obj.Normals[face[i].NormalIndex - 1];

                    points.AddRange([
                        X + vertex.X * Scale, Y + vertex.Y * Scale, Z + vertex.Z * Scale,
                        normal.X, normal.Y, normal.Z,
                        color.X, color.Y, color.Z,
                    ]);
                }
            }
        }

        _vertexCount = points.Count / 9;

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, points.Count * sizeof(float), points.ToArray(), BufferUsageHint.StaticDraw);
        ConfigurateShadersForPoints();

        GL.BindVertexArray(0);
    }

    // Paint
    public void Paint()
    {
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
        GL.BindVertexArray(0);
    }

    // Configurating Shaders for points drawing
    private void ConfigurateShadersForPoints()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);
    }
}
