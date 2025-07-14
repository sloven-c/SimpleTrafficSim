using System.Numerics;
using Raylib_cs;

namespace PrometSim;

public class RoadManager : RoadData {
    // todo given so many classes use draw ponder if we could use something with interfaces
    private readonly List<Road> _roads = [];
    private bool _canAddRoad = true;

    private void AddRoad(Vector2 point) {
        if (!_canAddRoad) return;

        _canAddRoad = false;
        // we check if the last road needs to be finished (end point needing to be set)
        if (GetLastIndex() is int index && !_roads[index].EndPoint.HasValue) _roads[index].ConfirmRoad(point);
        _roads.Add(new Road(point));

        _canAddRoad = true;
    }

    private void DeleteRoad(Vector2 pos) {
        for (var i = 0; i < _roads.Count; i++) {
            if (!_roads[i].EndPoint.HasValue) continue;

            if (CursorOnRoad(_roads[i].StartPoint, pos, _roads[i].EndPoint!.Value)) {
                _roads.RemoveAt(i);
                break;
            }
        }
    }

    private bool CursorOnRoad(Vector2 start, Vector2 cursor, Vector2 end) {
        var sc = new Vector2(start.X - cursor.X, start.Y - cursor.Y);
        var se = new Vector2(start.X - end.X, start.Y - end.Y);

        var prod = Math.Abs(sc.X * sc.Y - se.Y * se.X);
        return prod <= RoadThickness / 2f;
    }

    public void Draw() {
        foreach (var road in _roads) road.Draw();
    }

    public void InputHandler() {
        var mousePos = Raylib.GetMousePosition();

        if (Raylib.IsKeyDown(KeyboardKey.C))
            _roads.Clear();
        else if (Raylib.IsKeyDown(KeyboardKey.D))
            DeleteRoad(mousePos);

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            AddRoad(mousePos);
        else if (Raylib.IsMouseButtonReleased(MouseButton.Right) && GetLastIndex() is int index) _roads.RemoveAt(index);
    }

    private int? GetLastIndex() {
        if (_roads.Count >= 1) return _roads.Count - 1;

        return null;
    }
}