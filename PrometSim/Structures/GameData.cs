using Raylib_cs;

namespace PrometSim.Structures;

public static class GameData {
    public const int Scale = 6;
    private static (int width, int height) _size = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

    public static (int width, int height) Size {
        get => _size;
        set {
            // if Size (width or height) changes fire the SizeChanged event
            if (_size != value) {
                _size = value;
                SizeChanged?.Invoke(value);
            }
        }
    }

    public static event Action<(int width, int height)>? SizeChanged;
}