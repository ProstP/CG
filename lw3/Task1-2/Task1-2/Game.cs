using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Shaders;

namespace Window;

public class Game : GameWindow
{
    const float STEP = 0.01f;

    float _minValue;
    float _maxValue;
    Func<float, float> _function;
    Shader _shader;
    private Matrix4 _projection;

    public Game(float minValue, float maxValue, Func<float, float> fn, int width, int height, string title)
        : base(GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                ClientSize = (width, height),
                Title = title,
            })
    {
        if (minValue > maxValue)
        {
            _minValue = maxValue;
            _maxValue = minValue;
        }
        else
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        _function = fn;
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
        DrawCoordinateAxes();
        DrawFunction();

        SwapBuffers();
    }
    private void ConfigurateShadersForPoints()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
    }
    private void DrawCoordinateAxes()
    {
        float color = 0.5f;
        float colorR = color;
        float colorG = color;
        float colorB = color;

        List<float> axesList = new()
        {
            0f, -1f, 0f, colorR, colorG, colorB,
            0f, 1f, 0f, colorR, colorG, colorB,
            -1f, 0f, 0f, colorR, colorG, colorB,
            1f, 0f, 0f, colorR, colorG, colorB,
        };

        AddStrokesToAxes(axesList, colorR, colorG, colorB);

        AddArrayToArrayBuffer(axesList.ToArray());
        GL.DrawArrays(PrimitiveType.Lines, 0, axesList.Count / 6);
    }
    private void AddStrokesToAxes(List<float> axesList, float colorR, float colorG, float colorB)
    {
        float strokeWidth = 0.01f;
        float startPoint = 0.1f;
        for (int i = 1; i <= 10; i++)
        {
            axesList.AddRange([startPoint * i, strokeWidth, 0f, colorR, colorG, colorB]);
            axesList.AddRange([startPoint * i, -strokeWidth, 0f, colorR, colorG, colorB]);

            axesList.AddRange([-startPoint * i, strokeWidth, 0f, colorR, colorG, colorB]);
            axesList.AddRange([-startPoint * i, -strokeWidth, 0f, colorR, colorG, colorB]);

            axesList.AddRange([strokeWidth, startPoint * i, 0f, colorR, colorG, colorB]);
            axesList.AddRange([-strokeWidth, startPoint * i, 0f, colorR, colorG, colorB]);

            axesList.AddRange([strokeWidth, -startPoint * i, 0f, colorR, colorG, colorB]);
            axesList.AddRange([-strokeWidth, -startPoint * i, 0f, colorR, colorG, colorB]);
        }
    }
    private void DrawFunction()
    {
        float colorR = 1.0f;
        float colorG = 0.5f;
        float colorB = 0.2f;

        List<float> points = new();
        for (float x = _minValue / 10; x <= _maxValue / 10; x += STEP)
        {
            points.AddRange([x, _function(x * 10) / 10, 0f, colorR, colorG, colorB]);
        }

        AddArrayToArrayBuffer(points.ToArray());
        GL.DrawArrays(PrimitiveType.LineStrip, 0, points.Count / 6);
    }
    private void AddArrayToArrayBuffer(float[] array)
    {
        GL.BufferData(BufferTarget.ArrayBuffer, array.Length * sizeof(float), array, BufferUsageHint.StaticDraw);
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
