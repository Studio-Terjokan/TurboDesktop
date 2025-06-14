using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TurboDesktop.Glfw;

namespace TurboDesktop
{
    public static class ImGuiController 
    {
        

        public static void LoadFont(string filepath, float Size, bool icon = false)
        {

            try
            {
                unsafe
                {
                    IntPtr strPtr = Marshal.StringToHGlobalAnsi(filepath); // Convert string to IntPtr

                    if (icon)
                        LoadIcons(strPtr, Size);
                    else
                        LoadFont(strPtr, Size);

                    Marshal.FreeHGlobal(strPtr);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(TurboDesktop.C_PREFIX + "Error while loading Imgui font: " + ex.Message);
            }
        }

        [DllImport("Resources\\TurboCore.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void InitImGui(IntPtr window);

        [DllImport("Resources\\TurboCore.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void NewFrame();

        [DllImport("Resources\\TurboCore.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Render();

        [DllImport("Resources\\TurboCore.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Shutdown();

        [DllImport("Resources\\TurboCore.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void LoadFont(IntPtr strPtr, float size);
        [DllImport("Resources\\TurboCore.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void LoadIcons(IntPtr strPtr, float size);

    }
}
