using static SimpleTrafficSim.DataStructures;

namespace SimpleTrafficSim;

public class Areas {
    /// <summary>
    /// stores an array of <see cref="SpawnArea"/>
    /// </summary>
    private readonly SpawnArea[] _areas;

    public Areas(int n) {
        var values = Enum.GetValues<AreaLocation>();
        // maximum 4 locations allowed (NE, NW, SE, SE)
        if (n > values.Length) throw new Exception($"There can't be more areas than {values.Length}");
        
        var random = new Random();
        // track used location areas
        var usedAreas = new List<AreaLocation>();
        
        _areas = new SpawnArea[n];
        for (var i = 0; i < _areas.Length; i++) {
            AreaLocation randomLocation;
            
            // determine a random location that isn't already used
            do {
                randomLocation = values[random.Next(values.Length)];
            } while (usedAreas.Contains(randomLocation));
            
            // create an area and pass the location
            _areas[i] = new SpawnArea(randomLocation, 10);
            // prevent the said location to be used again
            usedAreas.Add(randomLocation);
        }
    }

    public void Draw() {
        foreach (var area in _areas) {
            area.Draw();
        }
    }
}