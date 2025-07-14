using System.Numerics;
using Raylib_cs;
using static PrometSim.GameData;

namespace PrometSim;

/// <summary>
///     A class for SpawnArea an area at the edge(s) of the map where cars will spawn and drive off/on to/from the roads
/// </summary>
public class SpawnArea : CarData, IDisposable {
    private const int Width = 100;
    private const int Height = 50;
    private const int SWidth = Width * Scale;
    private const int SHeight = Height * Scale;
    private readonly List<Car> _cars;
    private DataStructures.CarSlot[,]? _carsData;

    public SpawnArea(DataStructures.AreaLocation areaLoc, int maxCars) {
        AreaLoc = areaLoc;
        _cars = [];
        SetLocation();
        SpawnCars(maxCars);
        SizeChanged += OnSizeChanged;

        Console.WriteLine($"Area: {Location} - {AreaLoc}");
    }

    public Vector2 Location { get; private set; }

    private DataStructures.AreaLocation AreaLoc { get; }

    public void Dispose() {
        SizeChanged -= OnSizeChanged;
    }

    private void OnSizeChanged((int width, int height) obj) {
        SetLocation();
    }

    /// <summary>
    ///     Sets coordinates for spawn area
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">if location doesn't match any AreaLocation enum values</exception>
    private void SetLocation() {
        switch (AreaLoc) {
            case DataStructures.AreaLocation.TopLeft:
                Location = new Vector2(0, 0);
                break;
            case DataStructures.AreaLocation.TopRight:
                Location = new Vector2(Size.width - SWidth, 0);
                break;
            case DataStructures.AreaLocation.BottomLeft:
                Location = new Vector2(0, Size.height - SHeight);
                break;
            case DataStructures.AreaLocation.BottomRight:
                Location = new Vector2(Size.width - SWidth, Size.height - SHeight);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    ///     Method that attempts to spawn cars
    /// </summary>
    /// <param name="max">the upper limit for the amount of cars to be spawned on the spawn area</param>
    private void SpawnCars(int max) {
        // calculating car slots
        var hCarSlot = 2 * CarBuffer + CarWidth;
        var vCarSlot = 2 * CarBuffer + CarHeight;

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

            (int w, int h) cell = (Width / hSlots * Scale, Height / vSlots * Scale);

            // old implementation
            /*var c = new Car(new Vector2(Location.X + cellW * slot.Value.j + carTemplate.Buffer * Scale,
                Y + cellH * slot.Value.i + carTemplate.Buffer * Scale));*/

            var c = new Car(this, cell, slot.Value);
            _cars.Add(c);
            _carsData[slot.Value.i, slot.Value.j].Occupied = true;
        }
    }

    /// <summary>
    ///     Tries to find the best rated parking spot
    /// </summary>
    /// <returns>coordinates of a parking spot, null if no spot is available</returns>
    private (int i, int j)? FindCarSlot() {
        int? priority = null;
        (int i, int j)? index = null;

        for (var i = 0; i < _carsData!.GetLength(0); i++)
        for (var j = 0; j < _carsData.GetLength(1); j++) {
            var slot = _carsData[i, j];

            if (slot.Occupied) continue;

            // skip checking if our priority is bigger than slot's
            if (!priority.HasValue || slot.Priority < priority || slot.Priority == 0) {
                priority = slot.Priority;
                index = (i, j);

                // additional checks are made for 0 priority because it makes no sense to check other priorities if we found the best rated parking spot already
                if (slot.Priority == 0) return index;
            }
        }

        return index;
    }

    /// <summary>
    ///     Method sets car priorities for parking
    /// </summary>
    private void SetPriorities() {
        if (_carsData == null) return;

        // todo this will have to be changed in the future to account for road connections
        for (var i = 0; i < _carsData.GetLength(0); i++)
        for (var j = 0; j < _carsData.GetLength(1); j++)
            _carsData[i, j].Priority = CalculateManhattan(i, j);
    }

    /// <summary>
    ///     Method calculates the priority for each car slot via Manhattan
    /// </summary>
    /// <param name="i">vertical position</param>
    /// <param name="j">horizontal position</param>
    /// <returns>car parking priority</returns>
    /// <exception cref="ArgumentOutOfRangeException">Location is not valid</exception>
    private int CalculateManhattan(int i, int j) {
        // we disable null check because this function should only be called from SetPriorities function which explicitly checks that _carsData is not null!
        var hMax = _carsData!.GetLength(1) - 1;
        var vMax = _carsData!.GetLength(0) - 1;

        return AreaLoc switch {
            DataStructures.AreaLocation.TopLeft => vMax - i + (hMax - j),
            DataStructures.AreaLocation.TopRight => vMax - i + j,
            DataStructures.AreaLocation.BottomLeft => i + (hMax - j),
            DataStructures.AreaLocation.BottomRight => i + j,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    ///     Draws that specific area
    /// </summary>
    public void Draw() {
        var rect = new Rectangle(Location, SWidth, SHeight);
        Raylib.DrawRectangleRec(rect, Color.Gray);
        DrawCars();
    }

    private void DrawCars() {
        foreach (var car in _cars) car.Draw();
    }
}