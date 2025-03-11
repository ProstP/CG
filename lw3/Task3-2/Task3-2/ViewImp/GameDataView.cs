using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Task3_2.Model.Tetraminoes;
using Task3_2.ViewImp.TextDrawing;

namespace Task3_2.ViewImp;

public class GameDataView
{
    TextRenderer _scoreRender;
    TextRenderer _levelRender;
    RectangleF _rendererRect;
    Font _mono = new Font(FontFamily.GenericMonospace, 16);

    int _score;
    int _level;

    bool _isUpdatingLevel = false;
    bool _isUpdatingScore = false;

    public TetraminoType TetraminoType { get; set; }
    public bool IsPlaying { get; set; }

    public int Score
    {
        get => _score;
        set => UpdateScoreValue(value);
    }
    public int Level
    {
        get => _level;
        set => UpdateLevelValue(value);
    }

    public GameDataView(int width, int height)
    {
        _rendererRect = new(0, 0, width / 2, height / 10);
        _scoreRender = new(width / 2, height / 10);
        _levelRender = new(width / 2, height / 10);

        Score = 0;
        Level = 1;
    }

    // Reset
    public void Reset()
    {
        Score = 0;
        Level = 1;
    }

    // Display data
    public void Display()
    {
        if (IsPlaying)
        {
            DrawNextTetramino();
        }
        if (!_isUpdatingScore)
        {
            DrawScoreValue();
        }
        if (!_isUpdatingLevel)
        {
            DrawLevelValue();
        }
    }

    // Update data
    private void UpdateScoreValue(int value)
    {
        _isUpdatingScore = true;
        _score = value;
        UpdateTextRenderer(_scoreRender, $"Score: {_score}");
        _isUpdatingScore = false;
    }
    private void UpdateLevelValue(int value)
    {
        _isUpdatingLevel = true;
        _level = value;
        UpdateTextRenderer(_levelRender, $"Level: {_level}");
        _isUpdatingLevel = false;
    }
    private void UpdateTextRenderer(TextRenderer textRenderer, string text)
    {
        textRenderer.Clear(Color.White);
        textRenderer.DrawString(text, _mono, Brushes.Black, _rendererRect);
    }

    // Draw next tetramino
    private void DrawNextTetramino()
    {
        (float left, float top)[] points = CreatePointsToNextTetramino(TetraminoType);

        float offsetX = 0.4f;
        float offsetY = 0.6f;

        float height = 2f / 20;
        float width = height;

#pragma warning disable CS0618
        GL.Begin(BeginMode.Quads);
#pragma warning restore CS0618 

        GL.Color3(1.0f, 0.0f, 0.0f);

        for (int i = 0; i < points.Length; i++)
        {
            float left = points[i].left * width;
            float top = points[i].top * height;

            GL.Vertex2(offsetX + left, offsetY - top);
            GL.Vertex2(offsetX + left + width, offsetY - top);
            GL.Vertex2(offsetX + left + width, offsetY - top - height);
            GL.Vertex2(offsetX + left, offsetY - top - height);
        }

        GL.End();
    }
    private (float left, float top)[] CreatePointsToNextTetramino(TetraminoType type)
    {
        return type switch
        {
            TetraminoType.I => [
                                (0,0),
                    (0,1),
                    (0,2),
                    (0,3),
                ],
            TetraminoType.J => [
                    (1,0),
                    (1,1),
                    (1,2),
                    (0,2),
                ],
            TetraminoType.L => [
                    (0,0),
                    (0,1),
                    (0,2),
                    (1,2),
                ],
            TetraminoType.S => [
                    (0,1),
                    (1,1),
                    (1,0),
                    (2,0),
                ],
            TetraminoType.T => [
                    (1,0),
                    (1,1),
                    (1,2),
                    (0,1),
                ],
            TetraminoType.Z => [
                    (0,0),
                    (1,0),
                    (1,1),
                    (2,1),
                ],
            _ => [
                    (0,0),
                    (0,1),
                    (1,0),
                    (1,1),
                ],
        };
    }

    // Draw datas
    private void DrawScoreValue()
    {
        DrawData(_scoreRender, -0.2f);
    }
    private void DrawLevelValue()
    {
        DrawData(_levelRender, -0.6f);
    }
    private void DrawData(TextRenderer textRenderer, float top)
    {
        GL.BindTexture(TextureTarget.Texture2D, textRenderer.Texture);

        GL.Begin(PrimitiveType.Quads);
        GL.Color3(Color.White);

        float x = 0.25f;
        float width = 0.5f;
        float y = top;
        float height = 0.2f;

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

    ~GameDataView()
    {
        _levelRender.Dispose();
        _scoreRender.Dispose();
    }
}
