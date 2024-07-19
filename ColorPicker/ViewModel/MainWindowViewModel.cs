using ColorPicker.Mouse;
using ColorPicker.MVVM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace ColorPicker.ViewModel;

class MainWindowViewModel : BaseViewModel
{
    private Color _colorItem = new Color();

    private ContextProvider _cursorContext;

    private SolidColorBrush? _colorRec;

    private string _RGB, _HEX, _HSV;

    public MainWindowViewModel()
    {
        // Initialize properties and subscribe to cursor context changes
        ColorRec = new();
        ColorsHistory = new();
        _cursorContext = new ContextProvider(OnMouseButtonDown);
        _cursorContext.CursorContextChanged += OnCursorContextChanged;
        _RGB = _cursorContext.RgbCode;
        _HEX = _cursorContext.HexCode;
        _HSV = _cursorContext.HsvCode;
    }

    public SolidColorBrush? ColorRec { get => _colorRec;  set => _colorRec = value; }
    public ObservableCollection<Color> ColorsHistory { get; set; }
    public Color ColorItem
    {
        get
        {
            return _colorItem;
        }
        set
        {
            // Update the context and notify UI about property changes
            _cursorContext.Color = value;
            _cursorContext.UpdateRGB();
            _cursorContext.UpdateHEX();
            _cursorContext.UpdateHSV();
            ColorRec = new SolidColorBrush(_cursorContext.Color);
            RGB = _cursorContext.RgbCode;
            HEX = _cursorContext.HexCode;
            HSV = _cursorContext.HsvCode;
            OnPropertyChanged(nameof(ColorRec));
            OnPropertyChanged(nameof(RGB));
            OnPropertyChanged(nameof(HEX));
            OnPropertyChanged(nameof(HSV));
            _colorItem = value;
        }
    }

    public string RGB { get => _RGB; set => _RGB = value; }
    public string HEX { get => _HEX; set => _HEX = value; }
    public string HSV { get => _HSV; set => _HSV = value; }

    public RelayCommand DeleteItem => new RelayCommand(obj => RemoveColor());
    public RelayCommand PickColor => new RelayCommand(obj => OnPickButtonClick());
    public RelayCommand CloseCommand => new RelayCommand(obj => Application.Current.Shutdown());
    public RelayCommand MinimizeCommand => new RelayCommand(obj => Application.Current.Windows[0].WindowState = WindowState.Minimized);

    public void OnMouseButtonDown(Color color)
    {
        // Add color to history and update the current color
        ColorsHistory.Insert(0, color);
        ColorItem = color;
        OnPropertyChanged(nameof(ColorItem));
    }

    public void OnPickButtonClick()
    {
        // Reset cursor context to pick a new color
        _cursorContext = new ContextProvider(OnMouseButtonDown);
        _cursorContext.CursorContextChanged += OnCursorContextChanged;
    }

    public void OnCursorContextChanged(object? sender, EventArgs e)
    {
        // Update color properties when the cursor context changes
        ColorRec = new SolidColorBrush(_cursorContext.Color);
        RGB = _cursorContext.RgbCode;
        HEX = _cursorContext.HexCode;
        HSV = _cursorContext.HsvCode;
        OnPropertyChanged(nameof(ColorRec));
        OnPropertyChanged(nameof(RGB));
        OnPropertyChanged(nameof(HEX));
        OnPropertyChanged(nameof(HSV));
    }

    public void RemoveColor()
    {
        // Remove the current color from history and update the color item
        int index = ColorsHistory.IndexOf(ColorItem);
        ColorsHistory.Remove(ColorItem);

        if (ColorsHistory.Count == 0)
        {
            ColorItem = new Color();
        }
        else if(ColorsHistory.Count > index)
        {
            ColorItem = ColorsHistory[index];
        }
        else if (ColorsHistory.Count > index - 1)
        {
            ColorItem = ColorsHistory[index - 1];
        }
        OnPropertyChanged(nameof(ColorItem));
        ColorRec = new SolidColorBrush(ColorItem);
        OnPropertyChanged(nameof(ColorRec));
    }
}


