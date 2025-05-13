using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task4_2.Shaders;
using Task4_2.TextureService;

namespace Task4_2;

public partial class Form1 : Form
{
    Shader _shader;

    Matrix4 _model = Matrix4.Identity;
    Matrix4 _view = Matrix4.Identity;
    Matrix4 _projection = Matrix4.Identity;

    Vector3 _cameraPos = new(0, 0f, 3f);

    int _canvasVao;
    int _canvasVertexCount;

    int _textureFrom;
    int _textureTo;

    float _time = 0f;
    Vector2 _clickPosition = new();

    System.Windows.Forms.Timer _timer;

    public Form1()
    {
        InitializeComponent();

        _timer = new()
        {
            Interval = 16
        };
        _timer.Tick += OnTimerTick;
    }

    private void GlControlLoad(object sender, EventArgs e)
    {
        GL.ClearColor(0.2f, 0.5f, 0.7f, 1f);
        GL.Enable(EnableCap.DepthTest);

        _shader = new();

        CalculateViewMatrix();
        CalculateProjectionMatrix();

        CreateCanvas();

        _textureFrom = TextureLoader.Load("Images/from.jpg");
        _textureTo = TextureLoader.Load("Images/to.jpg");
    }
    private void GlControlDisposed(object sender, EventArgs e)
    {
        _shader.Dispose();
    }

    private void GLControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.Use();
        _shader.SetMatrix4("model", _model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);
        _shader.SetFloat("Time", _time);
        _shader.SetVector2("ClickPos", _clickPosition);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _textureFrom);
        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, _textureTo);

        _shader.SetInt("textureFrom", 0);
        _shader.SetInt("textureTo", 1);

        PaintCanvas();

        glControl1.SwapBuffers();
    }
    private void GLControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, Width, Height);
        CalculateProjectionMatrix();
    }

    // Mouse click
    private void GlControlMouseDown(object sender, MouseEventArgs e)
    {
        _clickPosition.X = 0.5f;
        _clickPosition.Y = 0.5f;

        _time = 0f;
        _timer.Start();
    }

    // Canvas
    private void CreateCanvas()
    {
        float[] points =
        [
            -1f,  1f, 0f, 0f, 0f,
             1f,  1f, 0f, 1f, 0f,
             1f, -1f, 0f, 1f, 1f,
            -1f, -1f, 0f, 0f, 1f
        ];

        _canvasVertexCount = points.Length / 3;
        _canvasVao = GL.GenVertexArray();
        GL.BindVertexArray(_canvasVao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        ConfigurateShaders();

        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(0);
    }
    private void PaintCanvas()
    {
        GL.BindVertexArray(_canvasVao);
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, _canvasVertexCount);
        GL.BindVertexArray(0);
    }

    // Congigurate shaders
    private void ConfigurateShaders()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
    }

    // Update view matrix
    private void CalculateViewMatrix()
    {
        _view = Matrix4.LookAt(_cameraPos, Vector3.Zero, Vector3.UnitY);

        glControl1.Invalidate();
    }

    // Calculate projection matrix on resize
    private void CalculateProjectionMatrix()
    {
        float aspectRatio = (float)glControl1.Width / glControl1.Height;
        _projection = Matrix4.CreateOrthographicOffCenter(
            -aspectRatio,
            aspectRatio,
            -1, 1,
            0.1f,
            100f);
    }

    // Timer tick
    private void OnTimerTick(object sender, EventArgs e)
    {
        _time += 0.02f;

        if (_time > 1.5f)
        {
            _timer.Stop();
        }

        glControl1.Invalidate();
    }
}
