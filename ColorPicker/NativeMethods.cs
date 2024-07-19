using System.Runtime.InteropServices;

namespace ColorPicker;

public class NativeMethods
{
    public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hmod, IntPtr dwThreadId);

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr UnhookWindowsHookEx(IntPtr hhk);

    // The GetPixel method is a native function that returns an unsigned integer (uint).
    // The bytes in the returned uint represent color channels in the following order:
    // - First byte: Red channel
    // - Second byte: Green channel
    // - Third byte: Blue channel
    // - Fourth byte: Alpha channel

    [DllImport("Gdi32.dll", SetLastError = true)]
    public static extern uint GetPixel(IntPtr hdc, int x, int y);

    //GetDC and ReleaseDC are native methods used to acquire and release a device context,
    //which is commonly used for screen capture.

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    //Native method used to get cursor's position. I am using my own POINT structure because System.Windows.Point does not work in this context.

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref POINT pPoint);

    public struct POINT
    {
        public int X;
        public int Y;
    }

    //internal struct MSLLHOOKSTRUCT
    //{
    //    public POINT pt;
    //    public uint mouseData;
    //    public uint flags;
    //    public uint time;
    //    public IntPtr dwExtraInfo;
    //}

}
