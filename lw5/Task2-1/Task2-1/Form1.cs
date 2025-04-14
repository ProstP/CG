using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task2_1.Area;
using Task2_1.Model;
using Task2_1.View;
using static Task2_1.View.IView;

namespace Task2_1;

public partial class Form1 : Form, IView
{
    private Matrix4 _projectionMatrix;
    private Matrix4 _viewMatrix;
    private Matrix4 _modelMatrix;

    Vector3 _lightDirection = new Vector3(-1f, 0f, 4f);
    float _ambientStrength = 0.3f;
    float _diffuseStrength = 0.5f;

    bool _isShowMenu;

    GameArea _area;

    public event ClickToCellEventHandler OnClickToCell
    {
        add => _area.OnClickToCell += value;
        remove => _area.OnClickToCell -= value;
    }
    public event IntEventHandler OnStartNewGame;

    [Obsolete]
    public Form1()
    {
        InitializeComponent();

        _area = new();

        _isShowMenu = true;
        mainMenu.Visible = true;
        glControl1.Visible = false;
    }

    // GlControl
    [Obsolete]
    private void GLControlLoad(object sender, EventArgs args)
    {
        GL.ClearColor(Color.Aqua);
        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.Lighting);
        GL.Enable(EnableCap.Light0);
        GL.Enable(EnableCap.Normalize);

        GL.Enable(EnableCap.ColorMaterial);
        GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

        _modelMatrix = Matrix4.Identity;
        _viewMatrix = Matrix4.LookAt(new Vector3(0f, -1f, 3f), Vector3.Zero, Vector3.UnitY);

        ResizeViewport();

        _area.LoadTextures();
        _area.OnInvalidated += OnAreaInvalidate;
    }
    private void GLControlResize(object sender, EventArgs args)
    {
        ResizeViewport();
    }

    [Obsolete]
    private void GLControlPaint(object sender, PaintEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref _viewMatrix);

        SetLightProperties();

        GL.Enable(EnableCap.Texture2D);

        _area.Paint();

        glControl1.SwapBuffers();
    }
    private void ResizeViewport()
    {
        GL.Viewport(0, 0, Width, Height);
        float aspectRatio = (float)Width / (float)Height;
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45.0f),
            aspectRatio,
            0.1f,
            100.0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref _projectionMatrix);
    }

    // Light
    [Obsolete]
    private void SetLightProperties()
    {
        float[] lightDir = { _lightDirection.X, _lightDirection.Y, _lightDirection.Z, 1.0f };
        GL.Light(LightName.Light0, LightParameter.Position, lightDir);

        float[] lightColorAmbient = { _ambientStrength, _ambientStrength, _ambientStrength, 1.0f };
        float[] lightColorDiffuse = { _diffuseStrength, _diffuseStrength, _diffuseStrength, 1.0f };
        float[] lightColorSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };

        GL.Light(LightName.Light0, LightParameter.Ambient, lightColorAmbient);
        GL.Light(LightName.Light0, LightParameter.Diffuse, lightColorDiffuse);
        GL.Light(LightName.Light0, LightParameter.Specular, lightColorSpecular);

        float[] materialColorAmbient = { 0.2f, 0.2f, 0.2f, 1.0f };
        float[] materialColorDiffuse = { 0.8f, 0.8f, 0.8f, 1.0f };
        float[] materialColorSpecular = { 0.5f, 0.5f, 0.5f, 1.0f };
        float materialShininess = 32.0f;

        GL.Material(MaterialFace.Front, MaterialParameter.Ambient, materialColorAmbient);
        GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, materialColorDiffuse);
        GL.Material(MaterialFace.Front, MaterialParameter.Specular, materialColorSpecular);
        GL.Material(MaterialFace.Front, MaterialParameter.Shininess, materialShininess);
    }

    // Mouse handler
    private void GLControlMouseDown(object sender, MouseEventArgs args)
    {
        _area.ClickToArea(
            new Vector3(args.X, args.Y, GetDepthValue(args.X, args.Y)),
            _projectionMatrix,
            _viewMatrix,
            _modelMatrix,
            glControl1.Width,
            glControl1.Height
        );
    }
    private float GetDepthValue(int x, int y)
    {
        float depth = 0.0f;
        int winY = glControl1.Height - y;
        GL.ReadPixels(x, winY, 1, 1, PixelFormat.DepthComponent, PixelType.Float, ref depth);
        return depth;
    }

    // Toggle view
    private void ToggleView()
    {
        if (_isShowMenu)
        {
            SetVisibleMenu();
        }
        else
        {
            SetVisibleCanvas();
        }
    }
    private void SetVisibleMenu()
    {
        _isShowMenu = true;
        mainMenu.Visible = true;
        glControl1.Visible = false;
    }
    private void SetVisibleCanvas()
    {
        _isShowMenu = false;
        mainMenu.Visible = false;
        glControl1.Visible = true;
    }

    // Difficult btn down
    private void OnDifficultBtnDown(object sender, EventArgs args)
    {
        if (sender is Button button)
        {
            if (int.TryParse(button.Text, out int value))
            {
                OnStartNewGame.Invoke(value);

                SetVisibleCanvas();
            }
        }
    }

    // IView methods
    public void Init(int width, int height)
    {
        SetVisibleCanvas();
        _area.Init(width, height);
    }
    public void OpenCell(Cell cell, string value)
    {
        _area.OpenCell(cell, value);
    }
    public void CloseCell(Cell cell)
    {
        _area.AddToClose(cell);
        _area.BeginCloseCell();
    }
    public void CloseCell(Cell first, Cell second)
    {
        _area.AddToClose(first);
        _area.AddToClose(second);
        _area.BeginCloseCell();
    }
    public void ClearCell(Cell first, Cell second)
    {
        _area.AddToRemove(first);
        _area.AddToRemove(second);
        _area.BeginRemove();
    }
    public void Win()
    {
        SetVisibleMenu();
    }

    private void OnAreaInvalidate(object sender, EventArgs args)
    {
        glControl1.Invalidate();
    }
}
