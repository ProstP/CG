using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task1.Figure;

namespace Task1;

public partial class Form1 : Form
{
    float _xRotation = 0f;
    float _yRotation = 0f;

    bool _isDragging = false;
    Point _lastMousePos = Point.Empty;

    Vector3 _lightDirection = new Vector3(-10f, 1.5f, 2f);
    float _ambientStrength = 0.3f;
    float _diffuseStrength = 0.7f;

    List<IFigure> _figures = new();

    [Obsolete]
    public Form1()
    {
        InitializeComponent();
    }

    public void AddFigure(IFigure figure)
    {
        _figures.Add(figure);
    }

    [Obsolete]
    private void GlControlLoad(object sender, EventArgs e)
    {
        GL.ClearColor(0.6f, 0.6f, 0.6f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.Lighting);
        GL.Enable(EnableCap.Light0);
        GL.Enable(EnableCap.Normalize);

        GL.Enable(EnableCap.ColorMaterial);
        GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Ambient);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        ResizeViewport();
    }

    [Obsolete]
    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 modelview = Matrix4.LookAt(Vector3.UnitZ * 3, Vector3.Zero, Vector3.UnitY);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelview);

        GL.Rotate(_xRotation, Vector3.UnitX);
        GL.Rotate(_yRotation, Vector3.UnitY);

        SetLightProperties();

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Front);

        DrawFigures();

        GL.CullFace(CullFaceMode.Back);

        DrawFigures();

        glControl1.SwapBuffers();
    }

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

    [Obsolete]
    private void SetLightProperties()
    {
        float[] lightDir = { _lightDirection.X, _lightDirection.Y, _lightDirection.Z, 1.0f };
        GL.Light(LightName.Light0, LightParameter.Position, lightDir);

        float[] lightColorAmbient = { _ambientStrength, _ambientStrength, _ambientStrength, 1.0f };
        float[] lightColorDiffuse = { _diffuseStrength, _diffuseStrength, _diffuseStrength, 1.0f };
        float[] lightColorSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };

        GL.Light(LightName.Light0, LightParameter.Ambient, lightColorAmbient);
        GL.Light(LightName.Light0, LightParameter.Diffuse, lightColorDiffuse);
        GL.Light(LightName.Light0, LightParameter.Specular, lightColorSpecular);

        float[] materialColorAmbient = { 0.2f, 0.2f, 0.2f, 1.0f };
        float[] materialColorDiffuse = { 0.8f, 0.8f, 0.8f, 1.0f };
        float[] materialColorSpecular = { 0.5f, 0.5f, 0.5f, 1.0f };
        float materialShininess = 32.0f;

        GL.Material(MaterialFace.Front, MaterialParameter.Ambient, materialColorAmbient);
        GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, materialColorDiffuse);
        GL.Material(MaterialFace.Front, MaterialParameter.Specular, materialColorSpecular);
        GL.Material(MaterialFace.Front, MaterialParameter.Shininess, materialShininess);
    }

    private void DrawFigures()
    {
        foreach (IFigure figure in _figures)
        {
            foreach (Face face in figure.GetFaces())
            {
                DrawPolygon(face.Vertexes.ToArray(), face.Color, figure.GetAlpha());
            }
        }
    }

    private void DrawPolygon(Point3F[] points, Color color, float alpha)
    {
        GL.Begin(PrimitiveType.Polygon);

        //GL.Color3(color.R, color.G, color.B);
        GL.Color4(color.R, color.G, color.B, alpha);

        Vector3 normal = Vector3.Zero;

        for (int i = 0; i < points.Length; i++)
        {
            normal.X += points[i].X;
            normal.Y += points[i].Y;
            normal.Z += points[i].Z;
        }

        GL.Normal3(normal);

        for (int i = 0; i < points.Length; i++)
        {
            GL.Vertex3(points[i].X, points[i].Y, points[i].Z);
        }

        GL.End();

        DrawBorder(points, alpha);
    }
    private void DrawBorder(Point3F[] points, float alpha)
    {
        GL.LineWidth(2.5f);
        GL.Begin(PrimitiveType.LineLoop);

        GL.Color4(0, 0, 0, alpha);
        for (int i = 0; i < points.Length; i++)
        {
            GL.Vertex3(points[i].X, points[i].Y, points[i].Z);
        }
        GL.End();
    }
}
