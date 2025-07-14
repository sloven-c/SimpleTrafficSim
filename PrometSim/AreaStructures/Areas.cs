using PrometSim.Structures;

namespace PrometSim.AreaStructures;

public class Areas {
    private const int CarLimit = 2;

    /// <summary>
    ///     stores an array of <see cref="SpawnArea" />
    /// </summary>
    private readonly SpawnArea[] _areas;

    /// <summary>
    ///     Constructor for setting up areas
    /// </summary>
    /// <param name="n">number of areas to spawn</param>
    /// <exception cref="Exception">
    ///     if more areas are passed than there are defined in AreaLocation in
    ///     <see cref="DataStructures" />
    /// </exception>
    public Areas(int n) {
        var values = Enum.GetValues<DataStructures.AreaLocation>();
        // maximum 4 locations allowed (NE, NW, SE, SE)
        if (n > values.Length) throw new Exception($"There can't be more areas than {values.Length}");

        var random = new Random();
        // track used location areas
        var usedAreas = new List<DataStructures.AreaLocation>();

        _areas = new SpawnArea[n];
        for (var i = 0; i < _areas.Length; i++) {
            var allowedSet = values.Except(usedAreas).ToArray();
            var randomLocation = allowedSet[random.Next(allowedSet.Length)];

            _areas[i] = new SpawnArea(randomLocation, CarLimit);
            // prevent the said location to be used again
            usedAreas.Add(randomLocation);
        }
    }

    /// <summary>
    ///     Draw all the areas
    /// </summary>
    public void Draw() {
        foreach (var area in _areas) area.Draw();
    }
}