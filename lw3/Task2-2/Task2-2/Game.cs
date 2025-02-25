using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Painter;
using Shaders;

namespace MainWindow;
public class Game : GameWindow
{
    Shader _shader;
    Matrix4 _projection;
    List<IPainter> _painters = new();

    public Game(int width, int height, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            ClientSize = (width, height),
            Title = title
        })
    {
    }

    public void AddPainter(IPainter painter)
    {
        _painters.Add(painter);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

        int vertexBufferObj = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObj);
        int vertexArrayObj = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObj);

        _shader = new Shader("../../../shader.vert", "../../../shader.frag");

        CreateOrthographicMatrix();
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.Use();
        _shader.SetMatrix4("projection", _projection);
        ConfigurateShadersForPoints();
        _painters.ForEach(p => p.Paint());

        SwapBuffers();
    }
    private void ConfigurateShadersForPoints()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        CreateOrthographicMatrix();
    }
    private void CreateOrthographicMatrix()
    {
        float aspectRatio = (float)Size.X / Size.Y;

        if (aspectRatio > 1)
        {
            _projection = Matrix4.CreateOrthographic(aspectRatio * 2, 2, -1, 1);
        }
        else
        {
            _projection = Matrix4.CreateOrthographic(2, 2 / aspectRatio, -1, 1);
        }
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _shader.Dispose();
    }
}
