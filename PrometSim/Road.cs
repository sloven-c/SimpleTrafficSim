using System.Numerics;
using Raylib_cs;

namespace PrometSim;

public class Road(Vector2 startPoint) {
    // get thickness based on car size - height * x
    private const float RoadThickness = 10f;
    private Vector2? _endPoint;

    public bool Confirmed() {
        return _endPoint.HasValue;
    }

    public void ConfirmRoad(Vector2 endPoint) {
        _endPoint = endPoint;
    }

    /// <summary>
    ///     Draws the road - if the road is not completed then the 2nd coordinate is mouse cursor
    /// </summary>
    public void Draw() {
        // todo resizing will have to be implemented here as well
        var pos = Raylib.GetMousePosition();
        Raylib.DrawLineEx(startPoint, _endPoint ?? pos, RoadThickness * GameData.Scale, Color.Black);
    }
}