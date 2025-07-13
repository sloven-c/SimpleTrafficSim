using Raylib_cs;

namespace PrometSim;

public class Car(int x, int y) : GameData {
    public int Width { get; } = 13;
    public int Height { get; } = 6;
    public int Buffer { get; } = 5;
    private int _speed = 0; // dynamically controlled - physics engine
    private int _fuel = 85; // depletes as we drive
    private SpawnArea? _destination; // the destination car wishes to reach

    /// <summary>
    /// Draws the car - todo make it an actual sprite
    /// </summary>
    public void Draw() {
        Raylib.DrawRectangle(x, y, Width * Scale, Height * Scale, Color.Red);
        // Raylib.DrawRectangle(x-Buffer*Scale, y, Buffer * Scale, Height * Scale, Color.DarkBlue);
        // Raylib.DrawRectangle(x+Width*Scale, y, Buffer*Scale, Height*Scale, Color.DarkBlue);
    }
}