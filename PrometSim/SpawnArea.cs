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
    private DataStructures.CarSlot[,]? _carsData;

    public SpawnArea(DataStructures.AreaLocation location, int maxCars) {
        Location = location;
        _cars = [];
        SetLocation();
        SpawnCars(maxCars);
        
        Console.WriteLine($"Area: ({_x}, {_y}) - {Location}");
    }

    /// <summary>
    /// Sets coordinates for spawn area
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
        
        // todo calculate priority
        _carsData = new DataStructures.CarSlot[vSlots, hSlots];
        SetPriorities();

        for (var i = 0; i < max; i++) {
            // find a free slot for a car
            var slot = FindCarSlot();
            if (!slot.HasValue) throw new Exception("Failed to find a free slot");

            var c = new Car(slot.Value.i, slot.Value.j);
            _cars.Add(c);
            _carsData[slot.Value.i, slot.Value.j].Occupied = true;
        }
    }

    private (int i, int j)? FindCarSlot() {
        int? priority = null;
        (int i, int j)? index = null;
        
        for (var i = 0; i < _carsData.GetLength(0); i++) {
            for (var j = 0; j < _carsData.GetLength(0); j++) {
                var slot = _carsData[i, j];
                
                if (slot.Occupied) continue;
                // skip checking if our priority is bigger than slot's
                if (priority.HasValue && !(priority < slot.Priority)) continue;
                
                priority = slot.Priority;
                index = (i, j);
                    
                // if we found the highest (unoccupied) priority then we needn't dig anymore
                if (priority == 0) return index;
            }
        }

        return index;
    }

    /// <summary>
    /// Method sets car priorities for parking
    /// </summary>
    private void SetPriorities() {
        if (_carsData == null) return;
        
        // todo this will have to be changed in the future to account for road connections
        for (var i = 0; i < _carsData.GetLength(0); i++) {
            for (var j = 0; j < _carsData.GetLength(1); j++) {
                _carsData[i, j].Priority = CalculateManhattan(i, j);
            }
        }
    }

    /// <summary>
    /// Method calculates the priority for each car slot via Manhattan
    /// </summary>
    /// <param name="i">vertical position</param>
    /// <param name="j">horizontal position</param>
    /// <returns>car parking priority</returns>
    /// <exception cref="ArgumentOutOfRangeException">Location is not valid</exception>
    private int CalculateManhattan(int i, int j) {
        // we disable null check because this function should only be called from SetPriorities function which explicitly checks that _carsData is not null!
        var hMax = _carsData!.GetLength(1) - 1;
        var vMax = _carsData!.GetLength(0) - 1;

        return Location switch {
            DataStructures.AreaLocation.TopLeft => i + j,
            DataStructures.AreaLocation.TopRight => i + (hMax - j),
            DataStructures.AreaLocation.BottomLeft => (vMax - i) + j,
            DataStructures.AreaLocation.BottomRight => (vMax - i) + (hMax - j),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Draws that specific area
    /// </summary>
    public void Draw() {
        Raylib.DrawRectangle(_x, _y, SWidth, SHeight, Color.Gray);
        DrawCars();
    }

    private void DrawCars() {
        foreach (var car in _cars) {
            car.Draw();
        }
    }
}