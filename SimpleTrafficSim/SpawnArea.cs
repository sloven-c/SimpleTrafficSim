using Raylib_cs;
using static SimpleTrafficSim.DataStructures;

namespace SimpleTrafficSim;

/// <summary>
/// A class for SpawnArea an area at the edge(s) of the map where cars will spawn and drive off/on to/from the roads
/// </summary>
public class SpawnArea : GameData {
    private const int Width = 100 * Scale;
    private const int Height = 50 * Scale;
    private int _x, _y;
    private AreaLocation Location { get; }
    private List<Car> cars;

    public SpawnArea(AreaLocation location) {
        Location = location;
        cars = [];
        SetLocation();
        
        Console.WriteLine($"Area: ({_x}, {_y}) - {Location}");
    }
    
    /// <summary>
    /// Sets coordinates 
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">if location doesn't match any AreaLocation enum values</exception>
    private void SetLocation() {
        switch (Location) {
            case AreaLocation.TopLeft:
                _x = 0;
                _y = 0;
                break;
            case AreaLocation.TopRight:
                _x = Resolution.width - Width;
                _y = 0;
                break;
            case AreaLocation.BottomLeft:
                _x = 0;
                _y = Resolution.height - Height;
                break;
            case AreaLocation.BottomRight:
                _x = Resolution.width - Width;
                _y = Resolution.height - Height;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Draw() {
        Raylib.DrawRectangle(_x, _y, Width, Height, Color.Gray);
    }
}