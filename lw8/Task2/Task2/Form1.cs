using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task2.Shaders;
using Task2.Shapes;

namespace Task2;

public partial class Form1 : Form
{
    Shader _shader;

    Matrix4 _model = Matrix4.Identity;
    Matrix4 _view = Matrix4.Identity;
    Matrix4 _projection = Matrix4.Identity;

    Vector3 _cameraPos = new(0f, 2f, 5f);
    Vector3 _lightPos = new(1f, 5f, -5f);

    Vector3 _lightColor = new(1f, 1f, 1f);

    ShapeStore _shapeStore = new();

    bool _isMousePressed = false;
    Point _lastMousePos;
    float _sensitivity = 0.2f;

    float _verticalAngle = 0f;
    float _horizontalAngle = -90f;
    float _cameraDistance = 5f;

    float _ambientStrength = 0.3f;
    float _specularStrength = 0.5f;
    float _shininess = 32;


    public Form1()
    {
        InitializeComponent();
    }

    private void GlControlLoad(object sender, EventArgs args)
    {
        GL.ClearColor(Color.Aqua);
        GL.Enable(EnableCap.DepthTest);

        _shader = new();

        CalculateViewMatrix();
        CalculateProjectionMatrix();

        _shapeStore.CreatePoints();
    }
    private void GlControlDisposed(object sender, EventArgs args)
    {
        _shader.Dispose();
    }

    private void GLControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.Use();
        SetMatrixToShader();
        SetLightToShader();

        PaintCubes();

        glControl1.SwapBuffers();
    }
    private void GLControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, Width, Height);
        CalculateProjectionMatrix();
    }

    // Paint
    private void SetMatrixToShader()
    {
        _shader.SetMatrix4("model", _model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);
    }
    private void SetLightToShader()
    {
        _shader.SetVector3("lightPos", _lightPos);
        _shader.SetVector3("lightColor", _lightColor);
        _shader.SetFloat("ambientStrength", _ambientStrength);
        _shader.SetFloat("specularStrength", _specularStrength);
        _shader.SetFloat("shininess", _shininess);
        _shader.SetVector3("viewPos", _cameraPos);
    }
    private void PaintCubes()
    {
        _shader.SetInt("cubeCount", _shapeStore.GetCubeCount());

        Vector3[] centres = _shapeStore.GetCentres();
        float[] sizes = _shapeStore.GetSizes();
        for (int i = 0; i < _shapeStore.GetCubeCount(); i++)
        {
            _shader.SetVector3($"cubeCentres[{i}]", centres[i]);
            _shader.SetFloat($"cubeSizes[{i}]", sizes[i]);
        }

        _shapeStore.Paint();
    }

    // Update view matrix
    private void CalculateViewMatrix()
    {
        UpdateCameraPosition();
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

    // Mouse actions
    private void GLControlMouseDown(object sender, MouseEventArgs e)
    {
        if (!_isMousePressed)
        {
            _isMousePressed = true;
            _lastMousePos = e.Location;
        }
    }
    private void GLControlMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isMousePressed)
        {
            return;
        }

        float deltaX = e.X - _lastMousePos.X;
        float deltaY = e.Y - _lastMousePos.Y;
        _lastMousePos = e.Location;

        _horizontalAngle += deltaX * _sensitivity;
        _verticalAngle += deltaY * _sensitivity;

        _verticalAngle = Math.Clamp(_verticalAngle, -89f, 89f);

        UpdateCameraPosition();
    }
    private void GlControlMouseUp(object sender, MouseEventArgs e)
    {
        if (_isMousePressed)
        {
            _isMousePressed = false;
        }
    }


    // Camera position
    private void UpdateCameraPosition()
    {
        float horizontalAngleRad = MathHelper.DegreesToRadians(_horizontalAngle);
        float verticalAngleRad = MathHelper.DegreesToRadians(_verticalAngle);

        float x = _cameraDistance * (float)(Math.Cos(verticalAngleRad) * Math.Cos(horizontalAngleRad));
        float y = _cameraDistance * (float)(Math.Sin(verticalAngleRad));
        float z = _cameraDistance * (float)(Math.Cos(verticalAngleRad) * Math.Sin(horizontalAngleRad));

        _cameraPos = new Vector3(x, y, z);

        _view = Matrix4.LookAt(_cameraPos, Vector3.Zero, Vector3.UnitY);

        glControl1.Invalidate();
    }
}
