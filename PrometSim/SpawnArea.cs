using Raylib_cs;

namespace PrometSim;

/// <summary>
/// A class for SpawnArea an area at the edge(s) of the map where cars will spawn and drive off/on to/from the roads
/// </summary>
public class SpawnArea : GameData {
    private const int Width = 100;
    private const int Height = 50;
    private const int SWidth = Width * Scale;
    private const int SHeight = Height * Scale;
    
    private int _x, _y;
    private DataStructures.AreaLocation Location { get; }
    private List<Car> _cars;

    public SpawnArea(DataStructures.AreaLocation location, int maxCars) {
        Location = location;
        _cars = [];
        SetLocation();
        SpawnCars(maxCars);
        
        Console.WriteLine($"Area: ({_x}, {_y}) - {Location}");
    }

    /// <summary>
    /// Sets coordinates 
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">if location doesn't match any AreaLocation enum values</exception>
    private void SetLocation() {
        switch (Location) {
            case DataStructures.AreaLocation.TopLeft:
                _x = 0;
                _y = 0;
                break;
            case DataStructures.AreaLocation.TopRight:
                _x = Size.width - SWidth;
                _y = 0;
                break;
            case DataStructures.AreaLocation.BottomLeft:
                _x = 0;
                _y = Size.height - SHeight;
                break;
            case DataStructures.AreaLocation.BottomRight:
                _x = Size.width - SWidth;
                _y = Size.height - SHeight;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Method that attempts to spawn cars
    /// </summary>
    /// <param name="max">the upper limit for the amount of cars to be spawned on the spawn area</param>
    private void SpawnCars(int max) {
        // calculating car slots
        var carTemplate = new Car(0, 0);

        var hCarSlot = 2 * carTemplate.Buffer + carTemplate.Width;
        var vCarSlot = 2 * carTemplate.Buffer + carTemplate.Height;

        var hSlots = Width / hCarSlot;
        var vSlots = Height / vCarSlot;

        var totalSlots = hSlots * vSlots;
        if (max > totalSlots) max = totalSlots;
        
        Console.WriteLine($"Max available slots for cars: {totalSlots}");
        
        // todo
    }

    /// <summary>
    /// Draws that specific area
    /// </summary>
    public void Draw() {
        Raylib.DrawRectangle(_x, _y, SWidth, SHeight, Color.Gray);
    }
}