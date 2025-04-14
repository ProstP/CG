using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task2_1.Model;
using Task2_1.Primitives;

namespace Task2_1.Area;

public class CellView
{
    const float HEIGHT = 0.05f;

    public int? TextureId { get; set; }

    public Cell Cell { get; private set; }
    public float Size { get; private set; }
    public float Angle { get; private set; }

    float _startX;
    float _startY;

    public CellView(Cell cell, float size, float startX, float startY)
    {
        Cell = cell;
        Size = size;
        _startX = startX;
        _startY = startY;
    }

    // Paint
    public void Paint()
    {
        Point3F[] points = FindPoints();

        PaintFace([
             points[0],
             points[1],
             points[2],
             points[3],
         ]);
        PaintFace([
            points[4],
             points[5],
             points[1],
             points[0],
         ]);
        PaintFace([
            points[3],
             points[2],
             points[6],
             points[7],
         ]);
        PaintFace([
            points[4],
             points[0],
             points[3],
             points[7],
         ]);
        PaintFace([
            points[1],
             points[5],
             points[6],
             points[2],
         ]);
        PaintFaceWithTexture([
            points[7],
            points[6],
            points[5],
            points[4],
        ], TextureId ?? 0);
    }
    private Point3F[] FindPoints()
    {
        (float x, float y) = CalculateCoordinates();
        (float offsetX, float offsetY) = CalculateOffset();
        float globalX = _startX + Size * Cell.X + Size / 2;
        float globalY = _startY + Size * Cell.Y + Size / 2;

        float halfOfRealSize = Size * 0.9f / 2;

        return [
            Point3F.Create(globalX - x + offsetX, globalY + halfOfRealSize, -y + offsetY),
            Point3F.Create(globalX - x + offsetX, globalY - halfOfRealSize, -y + offsetY),
            Point3F.Create(globalX + x + offsetX, globalY - halfOfRealSize, y + offsetY),
            Point3F.Create(globalX + x + offsetX, globalY + halfOfRealSize, y + offsetY),
            Point3F.Create(globalX - x - offsetX, globalY + halfOfRealSize, -y - offsetY),
            Point3F.Create(globalX - x - offsetX, globalY - halfOfRealSize, -y - offsetY),
            Point3F.Create(globalX + x - offsetX, globalY - halfOfRealSize, y - offsetY),
            Point3F.Create(globalX + x - offsetX, globalY + halfOfRealSize, y - offsetY),
        ];
    }
    private void PaintFace(Point3F[] points)
    {
        GL.Begin(PrimitiveType.Polygon);
        GL.Color3(Color.Gray);
        GL.Normal3(CalculateNormal(points));

        for (int i = 0; i < points.Length; i++)
        {
            GL.Vertex3(points[i].X, points[i].Y, points[i].Z);
        }

        GL.End();
    }
    private Vector3 CalculateNormal(Point3F[] points)
    {
        Vector3 first = new(
            points[1].X - points[0].X,
            points[1].Y - points[0].Y,
            points[1].Z - points[0].Z);
        Vector3 second = new(
            points[2].X - points[1].X,
            points[2].Y - points[1].Y,
            points[2].Z - points[1].Z);

        Vector3 normal = Vector3.Cross(first, second);
        normal.Normalize();

        return normal;
    }
    private void PaintFaceWithTexture(Point3F[] points, int textureId)
    {
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        GL.Begin(PrimitiveType.Quads);
        GL.Color3(Color.White);
        GL.Normal3(CalculateNormal(points));

        GL.TexCoord2(0f, 0f);
        GL.Vertex3(points[0].X, points[0].Y, points[0].Z);
        GL.TexCoord2(0f, 1f);
        GL.Vertex3(points[1].X, points[1].Y, points[1].Z);
        GL.TexCoord2(1f, 1f);
        GL.Vertex3(points[2].X, points[2].Y, points[2].Z);
        GL.TexCoord2(1f, 0f);
        GL.Vertex3(points[3].X, points[3].Y, points[3].Z);

        GL.End();
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    // Rotate
    public void Rotate(float angle)
    {
        Angle += angle;

        if (Angle > 2 * (float)Math.PI)
        {
            Angle = Angle - 2 * (float)Math.PI;
        }
    }

    // Points by angle
    private (float x, float y) CalculateCoordinates()
    {
        float x = (float)Math.Cos(Angle) * (Size * 0.9f) / 2;
        float y = (float)Math.Sin(Angle) * (Size * 0.9f) / 2;

        return (x, y);
    }
    private (float offsetX, float offsetY) CalculateOffset()
    {
        float x = (float)Math.Cos(Angle + Math.PI / 2) * HEIGHT / 2;
        float y = (float)Math.Sin(Angle + Math.PI / 2) * HEIGHT / 2;

        return (x, y);
    }
}
