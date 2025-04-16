using Assimp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Task1.Shaders;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;

namespace Task1.Picture;

public class Shape
{
    Scene _scene;
    Dictionary<Mesh, (int vbo, int ebo)> _meshBuffers = [];

    float _x;
    float _y;
    float _z;
    float _scale;

    bool _isInverse;

    public Shape(
        float x,
        float y,
        float z,
        float scale,
        bool isInverse = false)
    {
        _x = x;
        _y = y;
        _z = z;
        _scale = scale;
        _isInverse = isInverse;
    }

    public void LoadPicture(string path)
    {
        AssimpContext context = new AssimpContext();
        _scene = context.ImportFile(path,
            PostProcessSteps.Triangulate |
            PostProcessSteps.GenerateNormals |
            PostProcessSteps.CalculateTangentSpace
        );

        foreach (Mesh mesh in _scene.Meshes)
        {
            Vector3[] vertices = mesh.Vertices
                .Select(v => new Vector3(v.X, v.Y, v.Z))
                .ToArray();

            Vector3[] normals = mesh.Normals
                .Select(n => new Vector3(n.X, n.Y, n.Z))
                .ToArray();

            int[] indices = mesh.GetIndices();

            List<float> vertexData = new();

            for (int i = 0; i < indices.Length; i++)
            {
                int index = indices[i];

                Color4D color = _scene.Materials[mesh.MaterialIndex].ColorDiffuse;

                vertexData.AddRange([
                    _x + vertices[index].X * _scale, _y + vertices[index].Y * _scale, _z + vertices[index].Z * _scale,
                    normals[index].X, normals[index].Y, normals[index].Z,
                    1f, 1f, 1f
                ]);
            }

            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Count * sizeof(float), vertexData.ToArray(), BufferUsageHint.StaticDraw);

            int ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            _meshBuffers[mesh] = (vbo, ebo);
        }
    }

    public void Paint(Shader shader)
    {
        foreach (Mesh mesh in _scene.Meshes)
        {
            (int vbo, int ebo) = _meshBuffers[mesh];

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            ConfidurateShaderForPoints();

            SetColorToShader(shader, _scene.Materials[mesh.MaterialIndex].ColorAmbient, "ambientColor");
            SetColorToShader(shader, _scene.Materials[mesh.MaterialIndex].ColorDiffuse, "diffuseColor");

            GL.DrawElements(PrimitiveType.Triangles, mesh.GetIndices().Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }

    private void ConfidurateShaderForPoints()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);
    }
    private void SetColorToShader(Shader shader, Color4D color, string name)
    {
        float r = _isInverse ? 1 - color.R : color.R;
        float g = _isInverse ? 1 - color.G : color.G;
        float b = _isInverse ? 1 - color.B : color.B;

        shader.SetVector3(name, new Vector3(r, g, b));
    }
}
