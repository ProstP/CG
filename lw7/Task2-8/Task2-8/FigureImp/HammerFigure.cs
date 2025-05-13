using OpenTK.Graphics.OpenGL;

namespace Task2_8.FigureImp;

public class HammerFigure : Figure.Figure
{
    public override void CreatePoints()
    {
        float r = 1f;
        float g = 1f;
        float b = 0f;

        CreateHammerHead(r, g, b);
        CreateHammerHandle(r, g, b);
    }

    private void CreateHammerHead(float r, float g, float b)
    {
        float[] points =
        [
            -0.812f, 0.287f, 0f, r, g, b,
            -0.790f, 0.265f, 0f, r, g, b,
            -0.710f, 0.327f, 0f, r, g, b,
            -0.750f, 0.335f, 0f, r, g, b,
        ];

        CreateAndSetUpBuffer(points, PrimitiveType.TriangleFan);
    }

    private void CreateHammerHandle(float r, float g, float b)
    {
        float[] points =
        [
            -0.580f, 0.150f, 0f, r, g, b,
            -0.595f, 0.135f, 0f, r, g, b,
            -0.760f, 0.295f, 0f, r, g, b,
            -0.750f, 0.307f, 0f, r, g, b,
        ];

        CreateAndSetUpBuffer(points, PrimitiveType.TriangleFan);
    }
}
