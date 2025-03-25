using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task3_2.Map;
using Task3_2.Shaders;

namespace Task3_2;

public partial class Form1 : Form
{
    // Вынести управление в другой класс, добавить виды кубов, валидация количества, камера видит сквозь объекта
    // Освещение в системе коодринат наблюдателя
    // предать матрцу нормали через uniform
    // Передать ambient
    Shader _shader;

    bool _isFocused = false;

    Vector3 _cameraPos = Vector3.Zero;
    Vector3 _cameraRot = Vector3.Zero;

    Matrix4 _model = Matrix4.Identity;
    Matrix4 _view = Matrix4.Identity;
    Matrix4 _projection = Matrix4.Identity;

    Vector3 _lightPos = new Vector3(1.0f, 0.0f, 1.0f);
    Vector3 _lightColor = new Vector3(1.0f, 1.0f, 1.0f);

    System.Windows.Forms.Timer _rotTimer;
    System.Windows.Forms.Timer _posTimer;

    bool _isMoving = false;
    bool _isRotating = false;

    float _posSpeed = 0.05f;
    float _rotSpeed = (float)Math.PI / 120;

    GameMap _gameMap = new([
                            [1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],
                            [1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1],
                            [1,0,1,0,0,0,0,0,0,0,1,1,1,1,0,1],
                            [1,0,1,1,1,1,1,1,0,0,1,0,0,1,0,1],
                            [1,0,0,0,0,0,0,1,0,0,1,0,0,1,0,1],
                            [1,0,0,0,0,0,0,1,0,0,1,0,0,1,0,1],
                            [1,0,1,1,1,1,1,1,0,0,1,0,0,0,0,1],
                            [1,0,1,0,0,0,0,0,0,0,1,1,1,1,1,1],
                            [1,0,1,0,0,0,0,2,0,0,0,0,0,0,0,1],
                            [1,0,1,0,1,0,0,0,0,0,0,0,0,0,0,1],
                            [1,0,1,0,1,0,0,0,0,0,0,0,0,0,0,1],
                            [1,0,1,0,1,0,1,1,1,1,0,1,1,1,1,1],
                            [1,0,1,1,1,0,1,0,0,1,0,1,0,0,0,1],
                            [1,0,0,0,0,0,1,0,1,1,0,1,0,0,0,1],
                            [1,0,0,0,1,0,1,0,0,0,0,0,0,0,0,1],
                            [1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],
                           ]);

    public Form1()
    {
        InitializeComponent();

        KeyPreview = true;

        KeyDown += Form1_KeyDown;
        KeyUp += Form1_KeyUp;

        _rotTimer = new System.Windows.Forms.Timer();
        _rotTimer.Tick += RotTimerTimeout;
        _rotTimer.Interval = 1;

        _posTimer = new System.Windows.Forms.Timer();
        _posTimer.Tick += PosTimerTimeout;
        _posTimer.Interval = 1;
    }
    private void GLControlLoad(object sender, EventArgs args)
    {
        GL.ClearColor(0.8f, 1.0f, 1.0f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        _shader = new();

        int vertexBufferObj = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObj);
        int vertexArrayObj = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObj);
        InitCamera();
        CalculateProjectionMatrix();
    }
    private void GLControlDisposed(object sender, EventArgs e)
    {
        _shader.Dispose();
        _posTimer.Dispose();
        _rotTimer.Dispose();
    }
    private void GLControlPaint(object sender, PaintEventArgs e)
    {
        if (!_isFocused)
        {
            Focus();
            _isFocused = true;
        }

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


        _shader.Use();
        _shader.SetMatrix4("model", _model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);
        _shader.SetVector3("lightPos", _cameraPos);
        _shader.SetVector3("lightColor", _lightColor);
        ConfigurateShadersForPoints();

        GL.Enable(EnableCap.CullFace);
        GL.FrontFace(FrontFaceDirection.Ccw);

        GL.CullFace(TriangleFace.Back);

        _gameMap.PaintMap();

        glControl1.SwapBuffers();
    }
    private void GLControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, Width, Height);
        CalculateProjectionMatrix();
    }

    // Keyboard handlers
    private void Form1_KeyDown(object sender, KeyEventArgs args)
    {
        if (!_isMoving)
        {
            if (args.KeyCode == Keys.W)
            {
                _isMoving = true;
                _posSpeed = Math.Abs(_posSpeed);

                _posTimer.Start();
            }
            else if (args.KeyCode == Keys.S)
            {
                _isMoving = true;
                _posSpeed = -Math.Abs(_posSpeed);

                _posTimer.Start();
            }
        }
        if (!_isRotating)
        {
            if (args.KeyCode == Keys.A)
            {
                _isRotating = true;
                _rotSpeed = -Math.Abs(_rotSpeed);

                _rotTimer.Start();
            }
            else if (args.KeyCode == Keys.D)
            {
                _isRotating = true;
                _rotSpeed = Math.Abs(_rotSpeed);

                _rotTimer.Start();
            }
        }
    }
    private void Form1_KeyUp(object sender, KeyEventArgs args)
    {
        if (_isMoving && (args.KeyCode == Keys.W || args.KeyCode == Keys.Up || args.KeyCode == Keys.S || args.KeyCode == Keys.Down))
        {
            _posTimer.Stop();

            _isMoving = false;
        }

        if (_isRotating && (args.KeyCode == Keys.A || args.KeyCode == Keys.Left || args.KeyCode == Keys.D || args.KeyCode == Keys.Right))
        {
            _rotTimer.Stop();

            _isRotating = false;
        }
    }

    // Camera
    private void InitCamera()
    {
        _gameMap.FindCameraPosition(out int x, out int y);
        UpdateCameraPosition(new Vector3(-GameMap.CUBE_SIZE * 8 + x * GameMap.CUBE_SIZE + GameMap.CUBE_SIZE / 2, 0.4f,
            -GameMap.CUBE_SIZE * 8 + y * GameMap.CUBE_SIZE + GameMap.CUBE_SIZE / 2));
        UpdateCameraRotation(new Vector3(0, 0, -1f));
    }
    private void PosTimerTimeout(object sender, EventArgs args)
    {
        if (!_isMoving)
        {
            _posTimer.Stop();
        }

        Vector3 cameraPos = _cameraPos + _cameraRot * _posSpeed;
        if (_gameMap.IsPositionFree(cameraPos.X + _cameraRot.X * _posSpeed * 10, cameraPos.Z + _cameraRot.Z * _posSpeed * 10))
        {
            UpdateCameraPosition(cameraPos);
        }
    }
    private void RotTimerTimeout(object sender, EventArgs args)
    {
        if (!_isRotating)
        {
            _rotTimer.Stop();
        }

        UpdateCameraRotation(RotateXZ(_cameraRot, _rotSpeed));
    }
    private static Vector3 RotateXZ(Vector3 vector, float angle)
    {
        float x = vector.X;
        float z = vector.Z;

        float sin = (float)Math.Sin(angle);
        float cos = (float)Math.Cos(angle);

        return new Vector3(x * cos - z * sin, 0f, x * sin + z * cos);
    }
    private void UpdateCameraPosition(Vector3 newPos)
    {
        _cameraPos = newPos;
        _view = Matrix4.LookAt(_cameraPos, _cameraPos + _cameraRot, Vector3.UnitY);

        glControl1.Invalidate();
    }
    private void UpdateCameraRotation(Vector3 newRot)
    {
        _cameraRot = newRot;
        _view = Matrix4.LookAt(_cameraPos, _cameraPos + _cameraRot, Vector3.UnitY);

        glControl1.Invalidate();
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

    // Calculate projection matrix on resize
    private void CalculateProjectionMatrix()
    {
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f),
            (float)glControl1.Width / (float)glControl1.Height, 0.1f, 100f);
    }
}
