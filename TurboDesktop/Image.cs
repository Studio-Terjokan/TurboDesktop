using StbImageSharp;
using static GLFW.GL;

namespace TurboDesktop
{
    public class Image : IDisposable
    {
        private uint m_RendererId;
        public readonly int Width;
        public readonly int Height;
        private static int ImageCount;
        private unsafe Image(string path)
        {
            glGenTextures(1, out m_RendererId);
            if (m_RendererId == 0)
                throw new Exception("Failed to generate OpenGL texture.");

            glBindTexture(GL_TEXTURE_2D, m_RendererId);

            ImageResult result = ImageResult.FromStream(File.OpenRead(path));

            Width = result.Width; Height = result.Height;

            uint format = result.Comp switch
            {
                ColorComponents.Grey => GL_RED,
                ColorComponents.GreyAlpha => GL_RG,
                ColorComponents.RedGreenBlue => GL_RGB,
                ColorComponents.RedGreenBlueAlpha => GL_RGBA,
                _ => throw new Exception("Unsupported image format: " + result.Comp)
            };

            fixed (byte* ptr = result.Data)
            {
                glTexImage2D(GL_TEXTURE_2D, 0, (int)format, Width, Height, 0, format, GL_UNSIGNED_BYTE, new IntPtr(ptr));
            }

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);



            glBindTexture(GL_TEXTURE_2D, 0);
        }
        public static Image Load(string path)
        {
            Image image = new Image(path);

            return image;

        }

        public int GetNativePtr() => (int)m_RendererId;

        public void Dispose()
        {
            glDeleteTextures(1, m_RendererId);
        }
    }
}
