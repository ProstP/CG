using OpenTK.Graphics.OpenGL;
using Painter;

namespace PainterImp;
public class FigurePainter : IPainter
{
    float _leftOffset;
    float _topOffset;
    float _widthScale;
    float _heightScale;

    public FigurePainter()
        : this(0.0f, 0.0f, 1.0f, 1.0f)
    { }

    public FigurePainter(float leftOffset, float topOffset, float widthScale, float heightScale)
    {
        _leftOffset = leftOffset * 2;
        _topOffset = topOffset * 2;
        _widthScale = leftOffset + widthScale > 1.0f ? 1.0f - leftOffset : widthScale;
        _heightScale = topOffset + heightScale > 1.0f ? 1.0f - topOffset : heightScale;
    }

    public void Paint()
    {
        PaintBackground();
        DrawClouds();
        DrawSun();
    }
    private void PaintBackground()
    {
        float meadowColorR = 0.4f;
        float meadowColorG = 1f;
        float meadowColorB = 0.4f;

        float skyColorR = 0.3f;
        float skyColorG = 0.3f;
        float skyColorB = 1f;

        float weadowHeight = 0.6f;
        float[] points =
        {
            0f, -2.0f + weadowHeight, 0f, meadowColorR, meadowColorG, meadowColorB,
            2.0f, -2.0f + weadowHeight, 0f, meadowColorR, meadowColorG, meadowColorB,
            2.0f, -2.0f , 0f, meadowColorR, meadowColorG, meadowColorB,
            0f, -2.0f , 0f, meadowColorR, meadowColorG, meadowColorB,
            0f, -2.0f + weadowHeight , 0f, meadowColorR, meadowColorG, meadowColorB,
            0f, 0f, 0f, skyColorR, skyColorG, skyColorB,
            2.0f, 0f, 0f, skyColorR , skyColorG, skyColorB,
            2.0f, -2.0f + weadowHeight, 0f, skyColorR, skyColorG, skyColorB,
            0f, -2.0f + weadowHeight, 0f, skyColorR, skyColorG, skyColorB,
            0f, 0f, 0f, skyColorR , skyColorG, skyColorB,
        };

        points = AddOffsetToElements(points);

        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        GL.DrawArrays(PrimitiveType.Triangles, 2, 3);
        GL.DrawArrays(PrimitiveType.Triangles, 5, 3);
        GL.DrawArrays(PrimitiveType.Triangles, 7, 3);
    }
    private void DrawClouds()
    {
        float colorR = 1f;
        float colorG = 1f;
        float colorB = 1f;

        DrawCirle(0.1f, 0.1f, 0.15f, colorR, colorG, colorB);
        DrawCirle(0.15f, 0.2f, 0.1f, colorR, colorG, colorB);
        DrawCirle(0.11f, 0.7f, 0.15f, colorR, colorG, colorB);
        DrawCirle(0.15f, 0.85f, 0.11f, colorR, colorG, colorB);
        DrawCirle(0.1f, 1.05f, 0.14f, colorR, colorG, colorB);
    }
    private void DrawSun()
    {
        float colorR = 1.0f;
        float colorG = 1.0f;
        float colorB = 0.0f;

        DrawCirle(0.2f, 1.45f, 0.2f, colorR, colorG, colorB);
    }
    private void DrawGrass()
    {
        float colorR = 0.0f;
        float colorG = 1.0f;
        float colorB = 0.0f;


    }
    private void DrawCirle(float r, float offsetX, float offsetY, float colorR, float colorG, float colorB)
    {
        List<float> points = CreateCircleCoordinates(r, offsetX, offsetY);

        List<float> cloud = new();
        for (int i = 0; i < points.Count; i += 2)
        {
            if (i + 2 >= points.Count)
            {
                continue;
            }

            cloud.AddRange([points[i], -points[i + 1], 0f, colorR, colorG, colorB]);
        }

        float[] array = AddOffsetToElements(cloud.ToArray());

        GL.BufferData(BufferTarget.ArrayBuffer, array.Length * sizeof(float), array, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, array.Length / 6);
    }
    private List<float> CreateCircleCoordinates(float r, float offsetX, float offsetY)
    {
        List<float> points = new();

        for (int i = 0; i < 360; i++)
        {
            double rad = i + Math.PI / 180;
            float x = (float)Math.Cos(rad) * r;
            float y = (float)Math.Sin(rad) * r;

            points.Add(x + offsetX + r);
            points.Add(y + offsetY + r);
        }

        return points;
    }

    private float[] AddOffsetToElements(float[] array)
    {
        for (int i = 0; i < array.Length; i += 6)
        {
            if (i + 2 >= array.Length)
            {
                continue;
            }

            array[i] *= _widthScale;
            array[i + 1] *= _heightScale;

            array[i] += -1 + _leftOffset;
            array[i + 1] += 1 - _topOffset;
        }

        return array;
    }
}
