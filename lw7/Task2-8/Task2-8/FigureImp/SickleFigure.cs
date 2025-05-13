using OpenTK.Graphics.OpenGL;

namespace Task2_8.FigureImp;

public class SickleFigure : Figure.Figure
{
    public override void CreatePoints()
    {
        float r = 1f;
        float g = 1f;
        float b = 0f;

        CreateBlade(r, g, b);
        CreateSickleHandle(r, g, b);
    }

    private void CreateBlade(float r, float g, float b)
    {
        float cx = -0.70f;
        float cy = 0.25f;
        float radius = 0.1f;
        double startAngle = 225;
        double endAngle = 450;
        double angleWithMaxWidth = 270;
        List<float> points = [];

        for (double angle = startAngle; angle <= endAngle; angle++)
        {
            float offset = (float)(1 - Math.Abs(angleWithMaxWidth - angle) / 180) * 0.01f;
            float angleRad = (float)(Math.Round(angle) * Math.PI / 180);

            float x1 = cx + (radius + offset) * (float)Math.Cos(angleRad);
            float y1 = cy + (radius + offset) * (float)Math.Sin(angleRad);

            points.AddRange([x1, y1, 0f, r, g, b]);

            float x2 = cx + (radius - offset) * (float)Math.Cos(angleRad);
            float y2 = cy + (radius - offset) * (float)Math.Sin(angleRad);

            points.AddRange([x2, y2, 0f, r, g, b]);
        }

        CreateAndSetUpBuffer(points.ToArray(), PrimitiveType.LineStrip);
    }
    private void CreateSickleHandle(float r, float g, float b)
    {
        float[] points =
        [
            -0.775f, 0.175f, 0f, r, g, b,
            -0.765f, 0.165f, 0f, r, g, b,
            -0.807f, 0.115f, 0f, r, g, b,
            -0.825f, 0.13f, 0f, r, g, b,
        ];

        CreateAndSetUpBuffer(points, PrimitiveType.TriangleFan);
    }
}
