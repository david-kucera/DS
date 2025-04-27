using System.Runtime.InteropServices;
using System;

namespace DSAgentSimulationVisualization
{
    static unsafe class WinApi
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static unsafe extern bool DestroyWindow(IntPtr hwnd);
    }
}
