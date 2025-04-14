namespace Task2_1.Primitives;

public struct Point3F
{
    public float X;
    public float Y;
    public float Z;

    public Point3F(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Point3F Create(float x, float y, float z)
    {
        return new Point3F(x, y, z);
    }
}
