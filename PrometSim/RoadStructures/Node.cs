using System.Numerics;
using PrometSim.Structures;
using Raylib_cs;

namespace PrometSim.RoadStructures;

/// <summary>
///     Class structure that is present in the intersections and in the roads ends
/// </summary>
/// <param name="location"></param>
public class Node(Vector2 location) : RoadData, IDrawable {
    // track all road parts!
    private List<Vector2> _roadSections = [];
    public Vector2 Location = location;
    public DataStructures.NodeValidation? validation;

    public void Draw() {
        if (!validation.HasValue) return;

        // draws the node
        // how to determine node color dynamically
        Raylib.DrawCircleV(Location, (float)(GameData.Scale * RoadThickness / Math.Sqrt(2)),
            validation.Value == DataStructures.NodeValidation.Valid ? Color.Green : Color.Red);

        validation = null;
    }
}