using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Task25;

public partial class Form1 : Form
{
    float _xRotation = 0f;
    float _yRotation = 0f;

    bool _isDragging = false;
    Point _lastMousePos = Point.Empty;

    public Form1()
    {
        InitializeComponent();
    }

    // Load
    private void GlControlLoad(object sender, EventArgs e)
    {
        GL.ClearColor(0.7f, 0.7f, 0.7f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        ResizeViewport();
    }

    // Mouse dragging  

    private void GLControlMouseDown(object sender, MouseEventArgs args)
    {
        if (_isDragging)
        {
            return;
        }

        _lastMousePos = args.Location;
        _isDragging = true;
    }
    private void GLControlMouseUp(object sender, MouseEventArgs args)
    {
        if (!_isDragging)
        {
            return;
        }

        _lastMousePos = Point.Empty;
        _isDragging = false;
    }
    private void GLControlMouseMove(object sender, MouseEventArgs args)
    {
        if (!_isDragging)
        {
            return;
        }

        float dx = _lastMousePos.X - args.Location.X;
        float dy = _lastMousePos.Y - args.Location.Y;

        _lastMousePos = args.Location;

        _xRotation = ConvertAngle(_xRotation - dy);
        _yRotation = ConvertAngle(_yRotation - (Math.Abs(_xRotation) < 90 || 270 < Math.Abs(_xRotation) ? dx : -dx));

        glControl1.Invalidate();
    }
    private static float ConvertAngle(float angle)
    {
        if (Math.Abs(angle) >= 360)
        {
            angle %= 360;
        }

        return angle;
    }

    // Resize
    private void GlControlResize(object sender, EventArgs e)
    {
        ResizeViewport();
    }

    private void ResizeViewport()
    {
        GL.Viewport(0, 0, Width, Height);
        float aspectRatio = (float)Width / (float)Height;
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45.0f),
            aspectRatio,
            0.1f,
            100.0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref perspective);
    }

    // Paint
    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 modelview = Matrix4.LookAt(Vector3.UnitZ * 5, Vector3.Zero, Vector3.UnitY);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelview);

        GL.Rotate(_xRotation, Vector3.UnitX);
        GL.Rotate(_yRotation, Vector3.UnitY);

        DrawFigure();

        glControl1.SwapBuffers();
    }
    private void DrawFigure()
    {
        GL.Begin(PrimitiveType.Quads);

        double uStep = 0.1;
        double vStep = 0.1;

        for (double u = 0; u < 2 * Math.PI; u += uStep)
        {
            u = Math.Round(u, 5);
            double nextU = Math.Round(u + uStep, 5);

            for (double v = -1; v < 1; v += vStep)
            {
                v = Math.Round(v, 5);
                double nextV = Math.Round(v + vStep, 5);

                (double x, double y, double z) values1 = CalculateCoodinatesForMobiusStrip(u, v);
                (double x, double y, double z) values2 = CalculateCoodinatesForMobiusStrip(u, nextV);
                (double x, double y, double z) values3 = CalculateCoodinatesForMobiusStrip(nextU, nextV);
                (double x, double y, double z) values4 = CalculateCoodinatesForMobiusStrip(nextU, v);

                GL.Color3(values1.x, values1.y, values1.z + 0.4);
                GL.Vertex3(values1.x, values1.z, values1.y);

                GL.Color3(values2.x, values2.y, values2.z + 0.4);
                GL.Vertex3(values2.x, values2.z, values2.y);

                GL.Color3(values3.x, values3.y, values3.z + 0.4);
                GL.Vertex3(values3.x, values3.z, values3.y);

                GL.Color3(values4.x, values4.y, values4.z + 0.4);
                GL.Vertex3(values4.x, values4.z, values4.y);
            }
        }

        GL.End();
    }


    private (double x, double y, double z) CalculateCoodinatesForMobiusStrip(double u, double v)
    {
        double x = (1 + v / 2 * Math.Cos(u / 2)) * Math.Cos(u);
        double y = (1 + v / 2 * Math.Cos(u / 2)) * Math.Sin(u);
        double z = v / 2 * Math.Sin(u / 2);

        return (x, y, z);
    }
}
