
using GLFW;
using System.Numerics;
using System.Runtime.InteropServices;
using static GLFW.GLFW;

namespace TurboDesktop.Glfw
{
    public unsafe class Window
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct WindowIcon
        {
            public uint Width;
            public uint Height;
            public byte* Data;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Title {  get; private set; }

        public event Action<int, int> OnWindowSizeChanche;
        public event Action<bool> OnWindowFocusChanche;
        public event Action OnWindowClosed;


        private nint m_Window;


        private readonly GLFWwindowsizefun m_SizeCallback;
        private readonly GLFWwindowfocusfun m_FocusCallback;
        private readonly GLFWwindowposfun m_WindowPosCallback;

        public Vector2 WindowPosition { get; private set; }

        public Window(int width, int height, string title)
        {
            Width = width;
            Height = height;
            Title = title;

            glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
            glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 4);
            glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

            m_Window = glfwCreateWindow(Width, Height, Title, IntPtr.Zero, IntPtr.Zero);

            if (m_Window == IntPtr.Zero)
            {
                Console.WriteLine(TurboDesktop.C_PREFIX + "Cant Open a new Window");
                return;
            }

            m_SizeCallback = new GLFWwindowsizefun(WindowSizeCallback);
            m_FocusCallback = new GLFWwindowfocusfun(WindowFocusCallback);
            m_WindowPosCallback = new GLFWwindowposfun((nint winow, int x, int y) => { WindowPosition = new Vector2(x, y); });
            glfwSetWindowSizeCallback(m_Window, m_SizeCallback);
            glfwSetWindowFocusCallback(m_Window, m_FocusCallback);
            glfwSetWindowPosCallback(m_Window, m_WindowPosCallback);

            glfwSwapInterval(1); // TODO: Enable again, set to 1

            glfwMakeContextCurrent(m_Window);

            GL.LoadEntryPoints();
        }
        public void SetTitle(string title)
        {
            glfwSetWindowTitle(m_Window, title);
        }

        public unsafe void SetIcon(int width, int height, byte[] bytes)
        {
            WindowIcon icon2 = new WindowIcon();
            icon2.Width = (uint)width;
            icon2.Height = (uint)height;

            byte* data = stackalloc byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                data[i] = bytes[i];
            }

            icon2.Data = data;

            glfwSetWindowIcon(m_Window, 1, new IntPtr(&icon2));
        }

        private void WindowSizeCallback(nint window, int width, int height)
        {
            OnWindowSizeChanche?.Invoke(width, height);
        }

        private void WindowFocusCallback(nint window, int focus)
        {
            OnWindowFocusChanche?.Invoke(focus == 1);
        }

        public void Close()
        {
            if (m_Window != IntPtr.Zero)
                glfwDestroyWindow(m_Window);
        }

        public void Update()
        {
            glfwPollEvents();
            if (glfwWindowShouldClose(m_Window) == 1)
                OnWindowClosed?.Invoke();


        }

        public void SwapBuffers()
        {
            glfwSwapBuffers(m_Window);
        }

        public nint GetNativePtr() => m_Window;
    }
}
