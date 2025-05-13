using OpenTK.Graphics.OpenGL;

namespace Task2_8.FigureImp;

public class CanvasFigure : Figure.Figure
{
    public override void CreatePoints()
    {
        float[] points =
        [
            -1f, 0.5f, 0f,
            1f, 0.5f, 0f,
            1f, -0.5f, 0f,
            -1f, -0.5f, 0f,
        ];

        CreateAndSetUpBuffer(points, PrimitiveType.TriangleFan);
    }
}
