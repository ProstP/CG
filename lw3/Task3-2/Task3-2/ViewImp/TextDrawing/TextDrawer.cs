using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Task3_2.ViewImp.TextDrawing;

public class TextRenderer : IDisposable
{
    Bitmap _bmp;
    Graphics _gfx;
    int _texture;
    Rectangle _rectGFX;
    bool _disposed;

    public TextRenderer(int width, int height)
    {
        _bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        _gfx = Graphics.FromImage(_bmp);

        _gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        _texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _texture);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
    }

    public void Clear(Color color)
    {
        _gfx.Clear(color);
        _rectGFX = new Rectangle(0, 0, _bmp.Width, _bmp.Height);
    }

    public void DrawString(string text, Font font, Brush brush, RectangleF rect)
    {
        SizeF textSize = _gfx.MeasureString(text, font);
        PointF point = new PointF(
            rect.X + (rect.Width - textSize.Width) / 2,
            rect.Y + (rect.Height - textSize.Height) / 2);

        _gfx.DrawString(text, font, brush, point);
    }

    public int Texture
    {
        get
        {
            UploadBitmap();
            return _texture;
        }
    }

    void UploadBitmap()
    {
        if (_rectGFX != Rectangle.Empty)
        {
            System.Drawing.Imaging.BitmapData data = _bmp.LockBits(_rectGFX,
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                _rectGFX.X, _rectGFX.Y, _rectGFX.Width, _rectGFX.Height,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            _bmp.UnlockBits(data);
            _rectGFX = Rectangle.Empty;
        }
        else
        {
            System.Drawing.Imaging.BitmapData data = _bmp.LockBits(
                new Rectangle(0, 0, _bmp.Width, _bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                _bmp.Width, _bmp.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            _bmp.UnlockBits(data);
        }
    }
    void Dispose(bool manual)
    {
        if (!_disposed)
        {
            if (manual)
            {
                _bmp.Dispose();
                _gfx.Dispose();
            }
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~TextRenderer()
    {
        Console.WriteLine("[Предупреждение] Есть проблеммы: {0}.", typeof(TextRenderer));
    }
}
