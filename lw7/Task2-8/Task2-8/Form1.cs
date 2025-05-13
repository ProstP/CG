using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task2_8.Shaders;

namespace Task2_8;

public partial class Form1 : Form
{
    Shader _shader;

    Matrix4 _model = Matrix4.Identity;
    Matrix4 _view = Matrix4.Identity;
    Matrix4 _projection = Matrix4.Identity;

    Vector3 _cameraPos = new(0, 0f, 3f);

    const float InternalRadius = 0.2f;
    const float OuterRadius = 0.25f;
    Vector3 BackroundColor = new Vector3(1);
    Vector3 CircleColor = new Vector3(0);
    Vector2 Center = new(0, 0);

    List<Figure.Figure> _figures;

    public Form1(List<Figure.Figure> figures)
    {
        _figures = figures;
        InitializeComponent();
    }

    private void GlControlLoad(object sender, EventArgs args)
    {
        GL.ClearColor(0.2f, 0.5f, 0.7f, 1f);
        GL.Enable(EnableCap.DepthTest);

        _shader = new();

        CalculateViewMatrix();
        CalculateProjectionMatrix();

        _figures.ForEach(f => f.CreatePoints());
    }
    private void GlControlDisposed(object sender, EventArgs args)
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
        _shader.SetFloat("OuterRadius", OuterRadius);
        _shader.SetFloat("InternalRadius", InternalRadius);
        _shader.SetVector2("Center", Center);
        _shader.SetVector3("BackroundColor", BackroundColor);
        _shader.SetVector3("CircleColor", CircleColor);

        _figures.ForEach(f => f.Paint());

        glControl1.SwapBuffers();
    }
    private void GLControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, Width, Height);
        CalculateProjectionMatrix();
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
}
