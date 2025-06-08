using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GLFW
{
    internal static class NativeFuncExecuturer
    {
        private const int RtldLazy = 0x0001;

        private static class Windows
        {
            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadLibrary(string lpszLib);
        }

        private static class Linux
        {
            [DllImport("libdl")]
            public static extern IntPtr dlopen(string path, int flags);
            [DllImport("libdl")]
            public static extern IntPtr dlsym(IntPtr handle, string symbol);
        }

        public static IntPtr LoadLibraryExt(string libName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(baseDirectory, "Resources", libName);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Windows.LoadLibrary(fullPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Linux.dlopen(fullPath, RtldLazy);
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported platform.");
            }
        }

        public static T LoadFunction<T>(IntPtr library, string function)
        {
            IntPtr ptr;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ptr = Windows.GetProcAddress(library, function);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ptr = Linux.dlsym(library, function);
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported platform.");
            }

            if (ptr == IntPtr.Zero)
            {
                throw new EntryPointNotFoundException($"Function {function} not found in the library.");
            }

            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }
    }
}