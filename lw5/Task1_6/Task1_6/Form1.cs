using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task1_6.Camera;
using Task1_6.Map;

namespace Task1_6;

public partial class Form1 : Form
{
    GameMap _gameMap = new([
                            [2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2],
                            [0,0,2,1,0,0,0,0,0,0,0,0,0,0,0,7],
                            [3,0,2,0,0,0,0,0,0,0,5,5,5,5,0,7],
                            [3,0,4,4,4,4,4,4,0,0,5,0,0,5,0,7],
                            [3,0,0,0,0,0,0,4,0,0,5,0,0,5,0,7],
                            [3,0,0,0,0,0,0,4,0,0,5,0,0,5,0,7],
                            [3,0,4,4,4,4,4,4,0,0,5,0,0,0,0,7],
                            [3,0,6,0,0,0,0,0,0,0,5,5,5,5,5,7],
                            [3,0,6,0,0,0,0,0,0,0,0,0,0,0,0,7],
                            [3,0,6,0,6,0,0,0,0,0,0,0,0,0,0,7],
                            [3,0,6,0,6,0,0,0,0,0,0,0,0,0,0,7],
                            [3,0,6,0,6,0,6,6,6,6,0,7,7,7,7,7],
                            [3,0,6,6,6,0,6,0,0,6,0,7,0,0,0,7],
                            [3,0,0,0,0,0,6,0,6,6,0,7,0,0,0,7],
                            [3,0,0,0,6,0,6,0,0,0,0,0,0,0,0,7],
                            [2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2],
                           ]);

    Vector3 _cameraPos = Vector3.Zero;
    Vector3 _cameraRot = Vector3.Zero;

    CameraController _cameraController = new();

    bool _isFocused = false;

    public Form1()
    {
        InitializeComponent();

        KeyPreview = true;

        KeyDown += FormKeyDown;
        KeyUp += FormKeyUp;

        _cameraController.OnMoveObject += AddCameraPosition;
        _cameraController.OnRotateObject += AddCameraRotation;
    }

    private void GLControlLoad(object sender, EventArgs args)
    {
        GL.ClearColor(Color.Aqua);
        GL.Enable(EnableCap.DepthTest);

        InitCamera();

        ResizeViewport();

        _gameMap.InitTextures();
    }

    private void GLControlResize(object sender, EventArgs args)
    {
        ResizeViewport();
    }

    private void GLControlPaint(object sender, PaintEventArgs e)
    {
        if (!_isFocused)
        {
            Focus();
            _isFocused = true;
        }

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 modelView = Matrix4.LookAt(_cameraPos, _cameraPos + _cameraRot, Vector3.UnitY);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelView);

        GL.Enable(EnableCap.Texture2D);

        _gameMap.PaintMap();

        glControl1.SwapBuffers();
    }
    private void ResizeViewport()
    {
        GL.Viewport(0, 0, Width, Height);
        float aspectRatio = (float)Width / (float)Height;
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45.0f),
            aspectRatio,
            0.1f,
            100.0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref perspective);
    }

    // Key handle
    private void FormKeyDown(object sender, KeyEventArgs args)
    {
        if (args.KeyCode == Keys.W)
        {
            _cameraController.BeginMove(MoveDirection.Forward);
        }
        else if (args.KeyCode == Keys.S)
        {
            _cameraController.BeginMove(MoveDirection.Backward);
        }
        else if (args.KeyCode == Keys.A)
        {
            _cameraController.BeginRotate(RotationDirection.Left);
        }
        else if (args.KeyCode == Keys.D)
        {
            _cameraController.BeginRotate(RotationDirection.Right);
        }
    }

    private void FormKeyUp(object sender, KeyEventArgs args)
    {
        if (args.KeyCode == Keys.W || args.KeyCode == Keys.S)
        {
            _cameraController.EndMove();
        }
        else if (args.KeyCode == Keys.A || args.KeyCode == Keys.D)
        {
            _cameraController.EndRotate();
        }
    }

    // Camera
    private void InitCamera()
    {
        _gameMap.FindCameraPosition(out int x, out int z);
        _cameraPos = new Vector3(-GameMap.CUBE_SIZE * 8 + x * GameMap.CUBE_SIZE + GameMap.CUBE_SIZE / 2, 0.4f,
            -GameMap.CUBE_SIZE * 8 + z * GameMap.CUBE_SIZE + GameMap.CUBE_SIZE / 2);
        _cameraRot = new Vector3(0, 0, -1f);

        UpdateCamera();
    }
    private void AddCameraPosition(float speed)
    {
        Vector3 cameraPos = _cameraPos + _cameraRot * speed;
        if (_gameMap.CorrectPosition(cameraPos.X, cameraPos.Z))
        {
            _cameraPos = cameraPos;
        }
        UpdateCamera();
    }
    private void AddCameraRotation(float angle)
    {
        _cameraRot = RotateXZ(_cameraRot, angle);
        UpdateCamera();
    }
    private static Vector3 RotateXZ(Vector3 vector, float angle)
    {
        float x = vector.X;
        float z = vector.Z;

        float sin = (float)Math.Sin(angle);
        float cos = (float)Math.Cos(angle);

        return new Vector3(x * cos - z * sin, 0f, x * sin + z * cos);
    }
    private void UpdateCamera()
    {
        glControl1.Invalidate();
    }
}
