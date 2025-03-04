using OpenTK.Graphics.OpenGL;
using Painter;

namespace PainterImp;
public class FigurePainter : IPainterByBuffer
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
        DrawGrasses();
        DrawFlowers();
        DrawButterflies();
    }
    private void PaintBackground()
    {
        float meadowColorR = 0.4f;
        float meadowColorG = 0.8f;
        float meadowColorB = 0.4f;

        float skyColorR = 0.5f;
        float skyColorG = 0.6f;
        float skyColorB = 1f;

        float weadowHeight = 0.6f;
        float[] points =
        {
            0f, -2.0f + weadowHeight, 0f, meadowColorR, meadowColorG, meadowColorB,
            2.0f, -2.0f + weadowHeight, 0f, meadowColorR, meadowColorG, meadowColorB,
            2.0f, -2.0f , 0f, meadowColorR, meadowColorG, meadowColorB,
            0f, -2.0f , 0f, meadowColorR, meadowColorG, meadowColorB,
            0f, 0f, 0f, skyColorR, skyColorG, skyColorB,
            2.0f, 0f, 0f, skyColorR , skyColorG, skyColorB,
            2.0f, -2.0f + weadowHeight, 0f, skyColorR, skyColorG, skyColorB,
            0f, -2.0f + weadowHeight, 0f, skyColorR, skyColorG, skyColorB,
        };

        points = AddOffsetToElements(points);

        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
        GL.DrawArrays(PrimitiveType.TriangleFan, 4, 4);
    }
    private void DrawClouds()
    {
        float colorR = 1f;
        float colorG = 1f;
        float colorB = 1f;

        DrawCirle(0.12f, 0.35f, 0.15f, colorR, colorG, colorB);
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

    private void DrawGrasses()
    {
        DrawGrass1(0.4f, -1.3f);
        DrawGrass2(0.8f, -1.7f);
        DrawGrass3(1.6f, -1.55f);
    }
    private void DrawGrass1(float x, float y)
    {
        float colorR = 0.2f;
        float colorG = 0.9f;
        float colorB = 0.2f;

        float[] points =
        {
            x + 0.05f, y - 0.3f, 0f, colorR, colorG, colorB,
            x, y - 0.2f, 0f, colorR, colorG, colorB,
            x + 0.08f, y - 0.25f, 0f, colorR, colorG, colorB,
            x + 0.1f, y, 0f, colorR, colorG, colorB,
            x + 0.13f, y - 0.26f, 0f, colorR, colorG, colorB,
            x + 0.2f, y - 0.19f, 0f, colorR, colorG, colorB,
            x + 0.17f, y - 0.3f, 0f, colorR, colorG, colorB,
        };

        DrawTraingleFan(points);
    }
    private void DrawGrass2(float x, float y)
    {
        float colorR = 0.2f;
        float colorG = 0.9f;
        float colorB = 0.2f;

        float[] points =
        {
            x + 0.05f, y - 0.25f, 0f, colorR, colorG, colorB,
            x, y - 0.15f, 0f, colorR, colorG, colorB,
            x + 0.05f, y - 0.17f, 0f, colorR, colorG, colorB,
            x + 0.07f, y - 0.02f, 0f, colorR, colorG, colorB,
            x + 0.10f, y - 0.16f, 0f, colorR, colorG, colorB,
            x + 0.14f, y, 0f, colorR, colorG, colorB,
            x + 0.18f, y - 0.161f, 0f, colorR, colorG, colorB,
            x + 0.24f, y - 0.14f, 0f, colorR, colorG, colorB,
            x + 0.18f, y - 0.25f, 0f, colorR, colorG, colorB,
        };

        DrawTraingleFan(points);
    }
    private void DrawGrass3(float x, float y)
    {
        float colorR = 0.2f;
        float colorG = 0.9f;
        float colorB = 0.2f;

        float[] part1 =
        {
            x + 0.03f, y - 0.25f, 0f, colorR, colorG, colorB,
            x, y - 0.17f, 0f, colorR, colorG, colorB,
            x + 0.05f, y - 0.19f, 0f, colorR, colorG, colorB,
            x + 0.07f, y - 0.01f, 0f, colorR, colorG, colorB,
            x + 0.10f, y - 0.16f, 0f, colorR, colorG, colorB,
            x + 0.14f, y - 0.08f, 0f, colorR, colorG, colorB,
            x + 0.18f, y - 0.161f, 0f, colorR, colorG, colorB,
        };
        float[] part2 =
        {
            x + 0.18f, y - 0.161f, 0f, colorR, colorG, colorB,
            x + 0.23f, y - 0.015f, 0f, colorR, colorG, colorB,
            x + 0.23f, y - 0.18f, 0f, colorR, colorG, colorB,
            x + 0.28f, y - 0.155f, 0f, colorR, colorG, colorB,
            x + 0.24f, y - 0.25f, 0f, colorR, colorG, colorB,
            x + 0.03f, y - 0.25f, 0f, colorR, colorG, colorB,
        };

        DrawTraingleFan(part1);
        DrawTraingleFan(part2);
    }

    private void DrawFlowers()
    {
        DrawFlower1(0.2f, -1.5f);
        DrawFlower2(1f, -1.3f);
        DrawFlower3(1.5f, -1.4f);
    }
    private void DrawFlower1(float x, float y)
    {
        float stemColorR = 0.5f;
        float stemColorG = 1f;
        float stemColorB = 0.5f;

        float[] points =
        {
            x + 0.07f, y - 0.07f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.07f, y - 0.34f, 0f, stemColorR, stemColorG, stemColorB,
        };
        points = AddOffsetToElements(points);
        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.Lines, 0, points.Length / 6);

        float petalColorR = 1f;
        float petalColorG = 1f;
        float petalColorB = 1f;

        DrawCirle(0.03f, x + 0.02f, -y, petalColorR, petalColorG, petalColorB);
        DrawCirle(0.03f, x + 0.06f, -y, petalColorR, petalColorG, petalColorB);
        DrawCirle(0.03f, x, -(y - 0.04f), petalColorR, petalColorG, petalColorB);
        DrawCirle(0.03f, x + 0.08f, -(y - 0.04f), petalColorR, petalColorG, petalColorB);
        DrawCirle(0.03f, x + 0.02f, -(y - 0.08f), petalColorR, petalColorG, petalColorB);
        DrawCirle(0.03f, x + 0.06f, -(y - 0.08f), petalColorR, petalColorG, petalColorB);

        DrawCirle(0.02f, x + 0.05f, -(y - 0.05f), 1.0f, 1.0f, 0.0f);
    }
    private void DrawFlower2(float x, float y)
    {
        float stemColorR = 0.5f;
        float stemColorG = 1f;
        float stemColorB = 0.5f;

        float[] stem =
        {
            x + 0.04f, y - 0.07f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.0f, y - 0.14f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.0f, y - 0.34f, 0f, stemColorR, stemColorG, stemColorB,
        };
        stem = AddOffsetToElements(stem);
        GL.BufferData(BufferTarget.ArrayBuffer, stem.Length * sizeof(float), stem, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.LineStrip, 0, stem.Length / 6);

        float petalColorR = 1f;
        float petalColorG = 0f;
        float petalColorB = 0f;

        float[] petal =
        {
            x + 0.07f, y - 0.04f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.06f, y - 0.0f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.03f, y - 0.04f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.04f, y - 0.07f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.07f, y - 0.08f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.11f, y - 0.05f, 0f, petalColorR, petalColorG, petalColorB,
        };
        DrawTraingleFan(petal);
    }
    private void DrawFlower3(float x, float y)
    {
        float stemColorR = 0.5f;
        float stemColorG = 1f;
        float stemColorB = 0.5f;

        float[] stem =
        {
            x + 0.05f, y - 0.05f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.1f, y - 0.0f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.2f, y - 0.1f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.2f, y - 0.2f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.2f, y - 0.1f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.3f, y - 0.0f, 0f, stemColorR, stemColorG, stemColorB,
            x + 0.35f, y - 0.05f, 0f, stemColorR, stemColorG, stemColorB,
        };
        stem = AddOffsetToElements(stem);
        GL.BufferData(BufferTarget.ArrayBuffer, stem.Length * sizeof(float), stem, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.LineStrip, 0, stem.Length / 6);

        float petalColorR = 0.2f;
        float petalColorG = 0.2f;
        float petalColorB = 1f;

        float[] petal1 =
        {
            x + 0.05f, y - 0.05f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.07f, y - 0.1f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.03f, y - 0.1f, 0f, petalColorR, petalColorG, petalColorB,
        };
        petal1 = AddOffsetToElements(petal1);
        GL.BufferData(BufferTarget.ArrayBuffer, petal1.Length * sizeof(float), petal1, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.Triangles, 0, petal1.Length / 6);

        float[] petal2 =
        {
            x + 0.35f, y - 0.05f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.37f, y - 0.1f, 0f, petalColorR, petalColorG, petalColorB,
            x + 0.33f, y - 0.1f, 0f, petalColorR, petalColorG, petalColorB,
        };
        petal2 = AddOffsetToElements(petal2);
        GL.BufferData(BufferTarget.ArrayBuffer, petal2.Length * sizeof(float), petal2, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.Triangles, 0, petal2.Length / 6);
    }

    private void DrawButterflies()
    {
        DrawButterfly1(0.2f, -1.3f);
        DrawButterfly2(0.5f, -1f);
        DrawButterfly3(1.4f, -1f);
    }

    private void DrawButterfly1(float x, float y)
    {
        float wingColorR = 1f;
        float wingColorG = 0.2f;
        float wingColorB = 0.2f;

        float[] wing1 =
        {
            x + 0.06f, y - 0.04f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.0f, y - 0.0f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.0f, y - 0.08f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.06f, y - 0.12f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.12f, y - 0.08f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.12f, y - 0.00f, 0.0f, wingColorR, wingColorG, wingColorB,
        };
        DrawTraingleFan(wing1);

        float[] wing2 =
        {
            x + 0.06f, y - 0.12f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.02f, y - 0.14f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.02f, y - 0.18f, 0.0f, wingColorR, wingColorG, wingColorB,
        };
        DrawTraingleFan(wing2);
        float[] wing3 =
        {
            x + 0.06f, y - 0.12f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.1f, y - 0.14f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.1f, y - 0.18f, 0.0f, wingColorR, wingColorG, wingColorB,
        };
        DrawTraingleFan(wing3);

        float bodyColorR = 0.1f;
        float bodyColorG = 0.1f;
        float bodyColorB = 0.1f;

        float[] body =
        {
            x + 0.06f, y - 0.04f, 0f, bodyColorR, bodyColorG, bodyColorB,
            x + 0.06f, y - 0.14f, 0f, bodyColorR, bodyColorG, bodyColorB,
        };

        body = AddOffsetToElements(body);
        GL.BufferData(BufferTarget.ArrayBuffer, body.Length * sizeof(float), body, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.Lines, 0, body.Length / 6);
    }
    private void DrawButterfly2(float x, float y)
    {
        float wing1ColorR = 0.1f;
        float wing1ColorG = 0.1f;
        float wing1ColorB = 0.1f;

        float[] wing1 =
        {
            x + 0.04f, y - 0.03f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.0f, y - 0.0f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.0f, y - 0.06f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.04f, y - 0.09f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.08f, y - 0.06f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.08f, y - 0.00f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
        };
        DrawTraingleFan(wing1);
        float[] wing2 =
        {
            x + 0.04f, y - 0.09f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.01f, y - 0.12f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.01f, y - 0.15f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.04f, y - 0.13f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.07f, y - 0.15f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
            x + 0.07f, y - 0.12f, 0.0f, wing1ColorR, wing1ColorG, wing1ColorB,
        };
        DrawTraingleFan(wing2);

        float wing2ColorR = 1.0f;
        float wing2ColorG = 0.41f;
        float wing2ColorB = 0.07f;

        float[] wing1_1 =
        {
            x + 0.04f, y - 0.04f, 0.0f, wing2ColorR, wing2ColorG, wing2ColorB,
            x + 0.01f, y - 0.02f, 0.0f, wing2ColorR, wing2ColorG, wing2ColorB,
            x + 0.01f, y - 0.05f, 0.0f, wing2ColorR, wing2ColorG, wing2ColorB,
            x + 0.04f, y - 0.08f, 0.0f, wing2ColorR, wing2ColorG, wing2ColorB,
            x + 0.07f, y - 0.05f, 0.0f, wing2ColorR, wing2ColorG, wing2ColorB,
            x + 0.07f, y - 0.02f, 0.0f, wing2ColorR, wing2ColorG, wing2ColorB,
        };
        DrawTraingleFan(wing1_1);

        float bodyColorR = 0.4f;
        float bodyColorG = 0.4f;
        float bodyColorB = 0.4f;

        float[] body =
        {
            x + 0.04f, y - 0.03f, 0f, bodyColorR, bodyColorG, bodyColorB,
            x + 0.04f, y - 0.12f, 0f, bodyColorR, bodyColorG, bodyColorB,
        };

        body = AddOffsetToElements(body);
        GL.BufferData(BufferTarget.ArrayBuffer, body.Length * sizeof(float), body, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.Lines, 0, body.Length / 6);
    }
    private void DrawButterfly3(float x, float y)
    {
        float wingColorR = 0.99f;
        float wingColorG = 0.11f;
        float wingColorB = 0.7f;

        float wingBorderColorR = 0.7f;
        float wingBorderColorG = 0.0f;
        float wingBorderColorB = 0.47f;

        float[] wing2 =
        {
            x + 0.02f, y - 0.01f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.00f, y - 0.04f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.125f, y - 0.04f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.15f, y - 0.01f, 0.0f, wingColorR, wingColorG, wingColorB,
        };
        DrawTraingleFan(wing2);
        float[] wing2Border =
        {
            x + 0.02f, y - 0.01f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
            x + 0.00f, y - 0.04f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
            x + 0.125f, y - 0.04f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
            x + 0.15f, y - 0.01f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
        };
        wing2Border = AddOffsetToElements(wing2Border);
        GL.BufferData(BufferTarget.ArrayBuffer, wing2Border.Length * sizeof(float), wing2Border, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.LineLoop, 0, wing2Border.Length / 6);

        float[] wing1 =
        {
            x + 0.08f, y - 0.00f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.05f, y - 0.04f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.12f, y - 0.04f, 0.0f, wingColorR, wingColorG, wingColorB,
            x + 0.15f, y - 0.00f, 0.0f, wingColorR, wingColorG, wingColorB,
        };
        DrawTraingleFan(wing1);

        float[] wing1Border =
        {
            x + 0.08f, y - 0.00f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
            x + 0.05f, y - 0.04f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
            x + 0.12f, y - 0.04f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
            x + 0.15f, y - 0.00f, 0.0f, wingBorderColorR, wingBorderColorG, wingBorderColorB,
        };
        wing1Border = AddOffsetToElements(wing1Border);
        GL.BufferData(BufferTarget.ArrayBuffer, wing1Border.Length * sizeof(float), wing1Border, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.LineLoop, 0, wing1Border.Length / 6);

        float bodyColorR = 0.0f;
        float bodyColorG = 0.0f;
        float bodyColorB = 0.0f;

        float[] body =
        {
            x + 0.02f, y - 0.04f, 0f, bodyColorR, bodyColorG, bodyColorB,
            x + 0.14f, y - 0.04f, 0f, bodyColorR, bodyColorG, bodyColorB,
        };

        body = AddOffsetToElements(body);
        GL.BufferData(BufferTarget.ArrayBuffer, body.Length * sizeof(float), body, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.Lines, 0, body.Length / 6);
    }

    private void DrawTraingleFan(float[] array)
    {
        array = AddOffsetToElements(array);
        GL.BufferData(BufferTarget.ArrayBuffer, array.Length * sizeof(float), array, BufferUsageHint.StaticDraw);
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, array.Length / 6);
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
