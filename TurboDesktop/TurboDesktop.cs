using ImGuiNET;
using TurboDesktop.Glfw;

namespace TurboDesktop
{
    public abstract class TurboDesktop
    {
        internal const string C_PREFIX = "[TurboDesktop] ";

        public readonly string Name;
        public Window Window { get; private set; }


        private bool m_IsRunning;
        public TurboDesktop(string name) { Name = name; }


        public void Run()
        {
            if(m_IsRunning)
            {
                Console.WriteLine(C_PREFIX + "Cant start DesktopProgram Twice");
                return;
            }
            m_IsRunning = true;

            if (GLFW.GLFW.glfwInit() == 0)
            {
                Console.WriteLine(C_PREFIX + "Cant Open a new Window -> glfw Init Error");
                return;
            }

            Window = new Window(800, 700, "TurboDesktop: " + Name);
            Window.OnWindowClosed += () => m_IsRunning = false;

            ImGuiController.InitImGui(Window.GetNativePtr());
            Init();

            while(m_IsRunning)
            {
                Window.Update();
                GLFW.GL.glClear(GLFW.GL.GL_COLOR_BUFFER_BIT);

                ImGuiController.NewFrame();

                Draw();

                ImGuiController.Render();

                Window.SwapBuffers();
            }

            Shutdown();
            ImGuiController.Shutdown();
            Window.Close();
            GLFW.GLFW.glfwTerminate();
        }


        public abstract void Init();
        public abstract void Draw();
        public abstract void Shutdown();
    }
}
