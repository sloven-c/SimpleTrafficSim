using System.Numerics;
using PrometSim.AreaStructures;
using PrometSim.Structures;
using Raylib_cs;

namespace PrometSim.CarStructures;

public class Car(SpawnArea parent, (int w, int h) cell, (int i, int j) slot)
    : CarData {
    private SpawnArea? _destination; // the destination car wishes to reach
    private int _fuel = 85; // depletes as we drive
    private int _speed = 0; // dynamically controlled - physics engine

    /// <summary>
    ///     Draws the car - todo make it an actual sprite
    /// </summary>
    public void Draw() {
        var x = parent.Location.X + cell.w * slot.j + CarBuffer * GameData.Scale;
        var y = parent.Location.Y + cell.h * slot.i + CarBuffer * GameData.Scale;
        var calculatedLocation = new Vector2(x, y);

        var rect = new Rectangle(calculatedLocation, CarWidth * GameData.Scale, CarHeight * GameData.Scale);
        Raylib.DrawRectangleRec(rect, Color.DarkBlue);
        // Raylib.DrawRectangle((int)parent.X, (int)parent.Y, Width * Scale, Height * Scale, Color.DarkBlue);
    }
}