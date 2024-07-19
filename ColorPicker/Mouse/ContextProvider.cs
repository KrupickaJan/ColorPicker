using System.Windows.Media;
using System.Windows.Threading;
using static ColorPicker.NativeMethods;

namespace ColorPicker.Mouse;

// Handles cursor position and color picking logic
internal class ContextProvider
{

    POINT _cursorPosition = new();
    private DispatcherTimer _timer;
    private Color _color;
    private string _rgbCode;
    private string _hexCode;
    private string _hsvCode;
    private Hook _hook;
    private Action<Color> _onMouseClick;

    public ContextProvider(Action<Color> onMouseClick)
    {
        // Initialize and start a timer to update color under cursor
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(0.05);
        _timer.Tick += OnTimerTick;
        _timer.Start();
        _onMouseClick = onMouseClick;
        _hook = new Hook(OnMouseClick);
        _rgbCode = "0, 0, 0";
        _hexCode = "#000000";
        _hsvCode = "0, 0%, 0%";
    }

    public Color Color { get => _color; set => _color = value; }
    public string RgbCode { get => _rgbCode; }
    public string HexCode { get => _hexCode; }
    public string HsvCode { get => _hsvCode; }

    public event EventHandler? CursorContextChanged;
    
    public void OnMouseClick()
    {
        // Handle mouse click event to pick a color
        _onMouseClick(_color);
        _timer.Stop();
    }

    public void OnTimerTick(object? sender, EventArgs e)
    {
        // Update the color and raise the cursor context changed event
        UpdateColor();
        CursorContextChanged?.Invoke(this, e);
    }

    public void UpdateColor()
    {
        // Get the color under the cursor
        uint pixel;

        GetCursorPos(ref _cursorPosition);
        IntPtr screenCapture = GetDC(IntPtr.Zero);

        if(screenCapture != IntPtr.Zero)
        {
            pixel = GetPixel(screenCapture, _cursorPosition.X, _cursorPosition.Y);
        }
        else
        {
            pixel = 0;
        }

        ReleaseDC(IntPtr.Zero, screenCapture);
        _color = Color.FromArgb
            (
            (byte)(255 - ((pixel & 0xFF000000) >> 24)),
            (byte)(pixel & 0x000000FF), 
            (byte)((pixel & 0x0000FF00) >> 8), 
            (byte)((pixel & 0x00FF0000) >> 16)
            );
        
        UpdateHSV();
        UpdateRGB();
        UpdateHEX();
    }

    public void UpdateRGB()
    {
        _rgbCode = $"{_color.R}, {_color.G}, {_color.B}";
    }
    public void UpdateHEX()
    {
        _hexCode = $"#{_color.R:X2}{_color.G:X2}{_color.B:X2}";
    }
    public void UpdateHSV()
    {
        double r, g, b;
        double h, s, v;
        double min;
        double max;
        double dif;

        r = _color.R / 255.0;
        g = _color.G / 255.0;
        b = _color.B / 255.0;

        max = Math.Max(Math.Max(r, g), b);
        min = Math.Min(Math.Min(r, g), b);
        dif = max - min;

        v = max;

        if (max != 0)
        {
            s = dif / max;
        }
        else
        {
            s = 0;
        }

        if(dif == 0)
        {
            h = 0;
        }
        else if ( max == r)
        {
            h = 60 * (g - b) / dif;
        }
        else if (max == g)
        {
            h = 120 + (60 * (b - r) / dif);
        }
        else
        {
            h = 240 + (60 * (r - g) / dif);
        }

        if(h < 0)
        {
            h += 360;
        }
        _hsvCode = $"{h:##0}, {s*100:##0}%, {v*100:##0}%"; ;
    }
}
