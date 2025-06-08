
using ImGuiNET;

namespace TurboDesktop.Example
{
    public class TestProg : TurboDesktop
    {
        public static void Main(string[] args)
        {
            new TestProg().Run();
        }
        public TestProg() : base("Test Program")
        {
        }

        public override void Draw()
        {
            ImGui.Begin("TestWIndow");

            ImGui.Text("text");
            ImGui.Text("text");
            ImGui.Text("text");
            ImGui.Text("text");
            ImGui.Text("text");
            ImGui.Text("text");
            ImGui.Text("text");
            ImGui.End();

            ImGui.ShowDemoWindow();
        }

        public override void Init()
        {

        }

        public override void Shutdown()
        {

        }
    }
}