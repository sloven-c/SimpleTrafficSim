using System.Numerics;
using Raylib_cs;

namespace PrometSim;

public class Road(Vector2 startPoint) {
    private Vector2? _endPoint;

    public bool Confirmed() {
        return _endPoint.HasValue;
    }

    public void ConfirmRoad(Vector2 endPoint) {
        _endPoint = endPoint;
    }

    public void Draw() {
        var pos = Raylib.GetMousePosition();
        Raylib.DrawLine((int)startPoint.X, (int)startPoint.Y, (int)(_endPoint?.X ?? pos.X),
            (int)(_endPoint?.Y ?? pos.Y), Color.Black);
    }
}