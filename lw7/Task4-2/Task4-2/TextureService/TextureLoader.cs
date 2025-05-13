using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace Task4_2.TextureService;

public static class TextureLoader
{
    public static int Load(string file)
    {
        Bitmap bitmap = new(file);
        int textureID;
        GL.GenTextures(1, out textureID);
        GL.BindTexture(TextureTarget.Texture2D, textureID);

        BitmapData data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
            PixelType.UnsignedByte, data.Scan0);

        bitmap.UnlockBits(data);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return textureID;
    }
}
