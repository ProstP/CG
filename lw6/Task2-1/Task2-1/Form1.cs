using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Task2_1.ObjectHandler;
using Task2_1.Shaders;

namespace Task2_1;

public partial class Form1 : Form
{
    Shader _shader;

    Matrix4 _model = Matrix4.Identity;
    Matrix4 _view = Matrix4.Identity;
    Matrix4 _projection = Matrix4.Identity;

    Vector3 _cameraPos = new(0, 2f, 5f);

    Vector3 _lightPos = new(0.0f, 1.0f, -5.0f);
    Vector3 _lightColor = new(1.0f, 1.0f, 1.0f);
    float _ambientStrength = 0.7f;

    ObjView _obj;

    public Form1()
    {
        InitializeComponent();
    }
    private void GLControlLoad(object sender, EventArgs args)
    {
        GL.ClearColor(0.8f, 1.0f, 1.0f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        _shader = new();
        _obj = new("Ship1.obj", -1f, 0, 0, 0.5f);

        UpdateViewMatrix();
        CalculateProjectionMatrix();
    }
    private void GLControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.Use();
        _shader.SetMatrix4("model", _model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);
        _shader.SetVector3("lightPos", _lightPos);
        _shader.SetVector3("lightColor", _lightColor);
        _shader.SetFloat("ambientStrength", _ambientStrength);

        _obj.Paint();

        glControl1.SwapBuffers();
    }
    private void GLControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, Width, Height);
        CalculateProjectionMatrix();
    }

    // Update view matrix
    private void UpdateViewMatrix()
    {
        _view = Matrix4.LookAt(_cameraPos, Vector3.Zero, Vector3.UnitY);

        glControl1.Invalidate();
    }

    // Calculate projection matrix on resize
    private void CalculateProjectionMatrix()
    {
        _projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45f),
            (float)glControl1.Width / (float)glControl1.Height,
            0.1f,
            100f);
    }
}
