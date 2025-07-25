using System.Numerics;
using Raylib_cs;
using RoadBuilderPrototype.Structures;

namespace RoadBuilderPrototype.RoadStructures;

public class Road(Vector2 startPoint) : RoadData, IDrawable {
    private Color _color = Color.Black;

    // todo get thickness based on car size - height * x
    public Vector2 StartPoint { get; } = startPoint;
    public Vector2? EndPoint { get; private set; }

    /// <summary>
    ///     Draws the road - if the road is not completed then the 2nd coordinate is mouse cursor
    /// </summary>
    public void Draw() {
        // todo resizing will have to be implemented here as well
        var pos = Raylib.GetMousePosition();
        Raylib.DrawLineEx(StartPoint, EndPoint ?? pos, RoadThickness * GameData.Scale, _color);
    }

    public void ConfirmRoad(Vector2 endPoint) {
        EndPoint = endPoint;
        // Console.WriteLine($"Road built: {StartPoint}, {EndPoint}");
    }

    public void SetColor(DataStructures.RoadColor color) {
        _color = color == DataStructures.RoadColor.Default ? Color.Black : Color.Green;
    }
}