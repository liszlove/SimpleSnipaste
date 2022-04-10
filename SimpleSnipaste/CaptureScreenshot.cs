using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleSnipaste
{
    public class CaptureScreenshot
    {
        private enum DeviceCap
        {
            VERTRES = 10,
            PHYSICALWIDTH = 110,
            SCALINGFACTORX = 114,
            DESKTOPVERTRES = 117
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        public static void HideConsole()
        {
            ShowWindow(GetConsoleWindow(), 0u);
        }

        private static double GetScreenScalingFactor()
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hdc = graphics.GetHdc();
            int deviceCaps = GetDeviceCaps(hdc, 117);
            return (double)deviceCaps / (double)Screen.PrimaryScreen.Bounds.Height;
        }

        public static Bitmap GetScreen()
        {
            double screenScalingFactor = GetScreenScalingFactor();
            int width = (int)((double)SystemInformation.PrimaryMonitorSize.Width * screenScalingFactor);
            int height = (int)((double)SystemInformation.PrimaryMonitorSize.Height * screenScalingFactor);
            Size blockRegionSize = new Size(width, height);
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, blockRegionSize);
            return bitmap;
        }
    }
}