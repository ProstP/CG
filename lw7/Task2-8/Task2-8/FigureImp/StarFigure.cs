using OpenTK.Graphics.OpenGL;

namespace Task2_8.FigureImp;

public class StarFigure : Figure.Figure
{
    public override void CreatePoints()
    {
        CreateStar(0.03f, -0.7f, 0.4f, 1f, 0f, 0f);
        CreateStar(0.045f, -0.7f, 0.4f, 1f, 1f, 0f);
    }

    private void CreateStar(float radius, float cx, float cy, float r, float g, float b)
    {
        PointF center = new(cx, cy);
        List<PointF> points = CalculateStarVertexes(radius, cx, cy);

        if (points.Count != 5)
        {
            throw new ArgumentException("Must be 5 vertex on star");
        }

        CreateStarPart(center, points[0], points[2], r, g, b);
        CreateStarPart(center, points[2], points[4], r, g, b);
        CreateStarPart(center, points[4], points[1], r, g, b);
        CreateStarPart(center, points[1], points[3], r, g, b);
        CreateStarPart(center, points[3], points[0], r, g, b);
    }
    private void CreateStarPart(PointF v1, PointF v2, PointF v3, float r, float g, float b)
    {
        float[] points =
        [
            v1.X, v1.Y, 0f, r, g, b,
            v2.X, v2.Y, 0f, r, g, b,
            v3.X, v3.Y, 0f, r, g, b,
        ];

        CreateAndSetUpBuffer(points, PrimitiveType.Triangles);
    }

    private List<PointF> CalculateStarVertexes(float r, float cx, float cy)
    {
        List<PointF> points = [];

        double delta = Math.PI * 2 / 5;

        for (int i = 0; i < 5; i++)
        {
            double angle = Math.PI / 2 + delta * i;

            float x = cx + r * (float)Math.Cos(angle);
            float y = cy + r * (float)Math.Sin(angle);

            points.Add(new PointF(x, y));
        }

        return points;
    }
}
