using static ColorPicker.NativeMethods;

namespace ColorPicker.Mouse;
internal class Hook
{
    private const int WH_MOUSE_LL = 14;
    private const IntPtr WM_LBUTTONDOWN = 0x0201; // (512)
    private readonly IntPtr _hookId;
    private readonly LowLevelMouseProc _procedure;

    private readonly Action _onMouseClick;

    // Sets up a global mouse hook to detect clicks
    public Hook(Action onMouseClick)
    {
        // Initialize hook to capture mouse clicks
        _onMouseClick = onMouseClick;
        _procedure = HookCallBack;
        _hookId = SetWindowsHookEx(WH_MOUSE_LL, _procedure, IntPtr.Zero, 0);
    }

    private IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // Check if left mouse button is clicked
        if (nCode >= 0 && wParam == WM_LBUTTONDOWN)
        {
            _onMouseClick();
            UnhookWindowsHookEx(_hookId);
            return new IntPtr(-1);
        }
        else
        {
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }
}
