using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using Task3_2.Model.Tetraminoes;
using Task3_2.View;
using Task3_2.ViewImp.TextDrawing;
using static Task3_2.View.IView;

namespace Task3_2.ViewImp;

// Сделать движение вниз отзывчивой, чтобы сразу

public class GameView : GameWindow, IView
{
    TextRenderer _menuRenderer;
    GameAreaView _areaView;
    GameDataView _dataView;
    bool _isPlaying;
    bool _isPaused;

    public event WithoutArgsEventHandler OnBeginMoveFaster;
    public event WithoutArgsEventHandler OnEndMoveFaster;
    public event WithoutArgsEventHandler OnRotateClicked;
    public event WithoutArgsEventHandler OnStartNewGame;
    public event MoveByXEventHandler OnMoveByX;
    public event WithoutArgsEventHandler OnPauseClicked;

    public GameView(int width, int height, string title)
        : base(GameWindowSettings.Default,
            new()
            {
                ClientSize = (width, height),
                Title = title,
                APIVersion = new(3, 3),
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Compatability
            })
    {
        _areaView = new();
        _dataView = new(width / 2, height);
    }

    // Func from GameWindow
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.9f, 0.9f, 0.9f, 1.0f);

        SetOrtographicMatrix();

        GL.Enable(EnableCap.Texture2D);

        _menuRenderer = new(Size.X / 2, Size.Y / 4);

        _menuRenderer.Clear(Color.White);
        Font mono = new Font(FontFamily.GenericMonospace, 24);
        RectangleF rect = new(0, 0, Size.X / 2, Size.Y / 4 - mono.Height);

        _menuRenderer.DrawString("Нажмите пробел", mono, Brushes.Black, rect);
        rect.Y += mono.Height;
        _menuRenderer.DrawString("для начала игры", mono, Brushes.Black, rect);
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _areaView.Display();
        _dataView.Display();
        if (!_isPlaying)
        {
            ShowMenu();
        }

        SwapBuffers();
    }
    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
        SetOrtographicMatrix();
    }
    protected override void OnUnload()
    {
        base.OnUnload();

        _menuRenderer.Dispose();
    }

    // Show start menu
    private void ShowMenu()
    {
        GL.BindTexture(TextureTarget.Texture2D, _menuRenderer.Texture);

        GL.Begin(PrimitiveType.Quads);
        GL.Color3(Color.White);

        float x = -0.5f;
        float width = 1.0f;
        float y = -0.25f;
        float height = 0.5f;

        GL.TexCoord2(0.0f, 1.0f);
        GL.Vertex2(x, y);
        GL.TexCoord2(1.0f, 1.0f);
        GL.Vertex2(x + width, y);
        GL.TexCoord2(1.0f, 0.0f);
        GL.Vertex2(x + width, y + height);
        GL.TexCoord2(0.0f, 0.0f);
        GL.Vertex2(x, y + height);

        GL.End();
    }

    // OrthographicMatrix

    private void SetOrtographicMatrix()
    {
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();

        float aspectRatio = (float)Size.X / Size.Y;
        if (aspectRatio > 1)
        {
            GL.Ortho(-aspectRatio, aspectRatio, -1, 1, -1, 1);
        }
        else
        {
            GL.Ortho(-1, 1, -1 / aspectRatio, 1 / aspectRatio, -1, 1);
        }

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }

    // Input Handler
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        if (KeyboardState.IsKeyPressed(Keys.Space))
        {
            _areaView.Clear();
            OnStartNewGame.Invoke();
            _isPlaying = true;
            _dataView.IsPlaying = true;
            _dataView.Reset();
        }
        if (!_isPlaying)
        {
            return;
        }
        if (KeyboardState.IsKeyPressed(Keys.P))
        {
            TogglePause();
        }
        if (_isPaused)
        {
            return;
        }
        if (KeyboardState.IsKeyPressed(Keys.Up))
        {
            OnRotateClicked.Invoke();
        }
        if (KeyboardState.IsKeyPressed(Keys.Down))
        {
            OnBeginMoveFaster.Invoke();
        }
        if (KeyboardState.IsKeyReleased(Keys.Down))
        {
            OnEndMoveFaster.Invoke();
        }
        if (KeyboardState.IsKeyPressed(Keys.Left))
        {
            OnMoveByX.Invoke(false);
        }
        if (KeyboardState.IsKeyPressed(Keys.Right))
        {
            OnMoveByX.Invoke(true);
        }
    }

    // Func from IView
    public void UpdateTetramino(Tetramino tetramino)
    {
        _areaView.UpdateTetramino(tetramino);
    }

    public void BlockTetramino(Tetramino tetramino)
    {
        _areaView.BlockTetramino();
    }

    public void ClearRows(List<int> rows)
    {
        _areaView.ClearRows(rows);
    }

    public void SetNextTetramino(TetraminoType type)
    {
        _dataView.TetraminoType = type;
    }

    public void UpdateScore(int score)
    {
        _dataView.Score = score;
    }

    public void UpdateLevel(int level)
    {
        _dataView.Level = level;
    }

    public void Lost()
    {
        _isPlaying = false;
        _dataView.IsPlaying = false;
    }

    // Pause
    private void TogglePause()
    {
        _isPaused = !_isPaused;

        OnPauseClicked.Invoke();
    }
}
