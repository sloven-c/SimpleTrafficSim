using System.Numerics;
using PrometSim.Structures;
using Raylib_cs;

namespace PrometSim.RoadStructures;

public class RoadManager : RoadData, IDrawable {
    private const float Buffer = RoadThickness * GameData.Scale / 2f;
    private readonly List<Node> _nodes = [];

    // todo given so many classes use draw ponder if we could use something with interfaces
    private readonly List<Road> _roads = [];

    private int? _selectedRoad;
    private bool _trackMode;
    private static float MinDistance => GameData.Size.width / 25f;

    public void Draw() {
        foreach (var node in _nodes) {
            node.validation = CanCreateNode(node.Location, node)
                ? DataStructures.NodeValidation.Valid
                : DataStructures.NodeValidation.Invalid;
            node.Draw();
        }

        foreach (var road in _roads) road.Draw();
    }

    private void AddRoad(Vector2 point) {
        // we check if the last road needs to be finished (end point needing to be set)
        if (GetLastIndex() is int index && !_roads[index].EndPoint.HasValue) {
            if (Vector2.Distance(_roads[index].StartPoint, point) >= MinDistance) {
                if (!CanCreateNode(point, null)) return;
                // todo improve it by creating visual feedback if node can be added
                _nodes.Add(new Node(point));

                // what about spawn area? is it a node?
                // todo think ^
                _roads[index].ConfirmRoad(point);
            }
            // if the road can't be finished new road can not be built
            else {
                return;
            }
        }

        // immediately add new road
        _roads.Add(new Road(point));
    }

    private void TrackRoad(Vector2 pos) {
        if (!_trackMode) return;

        DefaultRoadColor();
        _selectedRoad = null;

        for (var i = 0; i < _roads.Count; i++) {
            if (!_roads[i].EndPoint.HasValue) continue;

            if (CursorOnRoad(_roads[i].StartPoint, pos, _roads[i].EndPoint!.Value)) {
                _roads[i].SetColor(DataStructures.RoadColor.Selected);
                _selectedRoad = i;
                break;
            }
        }
    }

    private bool CanCreateNode(Vector2 location, Node? nodeToSkip) {
        foreach (var node in _nodes) {
            if (nodeToSkip != null && nodeToSkip == node) continue;

            if (Vector2.Distance(node.Location, location) < MinDistance)
                return false;
        }

        return true;
    }

    private void DefaultRoadColor() {
        foreach (var road in _roads) road.SetColor(DataStructures.RoadColor.Default);
    }

    private static bool CursorOnRoad(Vector2 start, Vector2 cursor, Vector2 end) {
        var minX = Math.Min(start.X, end.X) - Buffer;
        var maxX = Math.Max(start.X, end.X) + Buffer;
        var minY = Math.Min(start.Y, end.Y) - Buffer;
        var maxY = Math.Max(start.Y, end.Y) + Buffer;

        if (!(cursor.X >= minX && cursor.X <= maxX && cursor.Y >= minY && cursor.Y <= maxY)) return false;

        var sc = new Vector2(start.X - cursor.X, start.Y - cursor.Y);
        var se = new Vector2(start.X - end.X, start.Y - end.Y);

        var prod = Math.Abs(sc.X * se.Y - sc.Y * se.X);
        var roadlength = Math.Sqrt((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y));
        var threshold = roadlength * Buffer;

        return prod <= threshold;
    }

    public void InputHandler() {
        var mousePos = Raylib.GetMousePosition();
        TrackRoad(mousePos);

        if (Raylib.IsKeyPressed(KeyboardKey.C)) {
            _roads.Clear();
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.D)) {
            _trackMode = !_trackMode;
            if (!_trackMode) {
                DefaultRoadColor();
                _selectedRoad = null;
            }
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.Left)) {
            if (_trackMode) {
                if (_selectedRoad == null) return;
                _roads.RemoveAt(_selectedRoad.Value);
                _selectedRoad = null;
            }
            else {
                AddRoad(mousePos);
            }
        }
        else if (Raylib.IsMouseButtonReleased(MouseButton.Right) && GetLastIndex() is int index) {
            _roads.RemoveAt(index);
        }
    }

    private int? GetLastIndex() {
        if (_roads.Count >= 1) return _roads.Count - 1;

        return null;
    }
}